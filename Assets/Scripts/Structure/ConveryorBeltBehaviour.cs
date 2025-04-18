using Manager;
using Resource;
using Structure.GameData;
using Structure.Information;
using Structure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Structure {
    public class ConveryorBeltBehaviour : StructureBehaviour, IResourceReceivable, IConnectable {
        public static readonly Vector3 RESOURCE_VISUAL_OFFSET = Vector3.zero;

        [SerializeField] private ConveryorBeltInfo[] BeltInfos;
        public ConveryorBeltInfo BeltInfo => BeltInfos[PlayerManager.Instance.Level];

        public override StructureInfo Info => BeltInfo;

        public override string Description => $"수용 가능한 무게: {TotalWeight}/{BeltInfo.MaxWeight}";

        public virtual IResourceReceivable Next { get; set; }
        public virtual List<IStructure> Previous { get; protected set; } = new();
        public event Action OnDependencyChanged;

        [field: Header("Resources")]
        [field: SerializeField] public virtual List<ResourceBehaviour> Resources { get; protected set; } = new();
        [field: SerializeField] public virtual List<ResourceBehaviour> RestResources { get; protected set; } = new();

        [field: SerializeField] protected virtual List<ResourceBehaviour> ThrowResources { get; set; } = new();

        public IEnumerable<ResourceBehaviour> AllResources => Resources.Concat(RestResources);
        public ResourceBehaviour LastResource => AllResources.OrderBy(o => o.Progress).FirstOrDefault();

        public float TotalRestWeight => RestResources.Sum(o => o.Info.weight);
        public float TotalWeight => TotalRestWeight + Resources.Sum(o => o.Info.weight);

        public bool IsBeltEnd => Next == null;
        public bool IsBeltStart => Previous.Count <= 0;

        protected virtual void Update() {
            if (!IsBeltEnd && RestResources.Any()) {
                var first = RestResources.First();
                if (Next.IsResourceReceivable(first)) {
                    foreach (var resource in RestResources) {
                        UnRestResource(resource);
                    }
                    RestResources.Clear();
                }
            }

            foreach (var resource in Resources) {
                if (RestResources.Any()) {
                    var need_progress = 1.0f - (TotalRestWeight / BeltInfo.MaxWeight);

                    if (resource.Progress >= need_progress) {
                        resource.Progress = need_progress;

                        RestResource(resource);
                    }
                } 
                else if (resource.IsProgressDone) {
                    if (!IsBeltEnd && Next.IsResourceReceivable(resource)) {
                        Next.ReceiveResource(resource, this);

                        ThrowResource(resource);

                        continue;
                    }
                    else {
                        resource.Progress = 1.0f;

                        RestResource(resource);
                    }
                }

                ResourceDisplayUpdate(resource);
            }

            // Throw Resources
            foreach (var resource in ThrowResources) {
                Resources.Remove(resource);
            }
            ThrowResources.Clear();
        }
        
        public bool IsResourceReceivable(ResourceBehaviour resource) {
            if (!BeltInfo.Type.HasFlag(resource.Type)) {
                return false;
            }

            // Total Weight Check
            if (TotalWeight + resource.Weight > BeltInfo.MaxWeight) {
                return false;
            }

            // Is There Any Free Space Check
            var last_resource = LastResource;

            if (last_resource != null) {
                var weight = (BeltInfo.MaxWeight * last_resource.Progress) - (last_resource.Weight * 0.5f);

                if ((resource.Weight * 0.5f) > weight) {
                    return false;
                }
            }

            return true;
        }
        public void ReceiveResource(ResourceBehaviour resource, IStructure from) {
            resource.Data.Reset();
            resource.Data.tickRate += BeltInfo.Speed;
            resource.LastDirection = GetSafeDirection(from, Position);

            Resources.Add(resource);
        }

        protected void ResourceDisplayUpdate(ResourceBehaviour resource) {
            var offset = GetOffsetByProgress(resource);

            resource.transform.position = transform.position + offset;
        }

        public void ThrowResource(ResourceBehaviour resource) {
            resource.Data.tickRate -= BeltInfo.Speed;

            ThrowResources.Add(resource);
        }
        public void RestResource(ResourceBehaviour resource) {
            resource.Data.isRest = true;

            RestResources.Add(resource);

            ThrowResources.Add(resource);
        }
        public void UnRestResource(ResourceBehaviour resource) {
            resource.Data.isRest = false;

            Resources.Add(resource);
        }

        #region Implement for IConnectable
        [ContextMenu("Connect")]
        public override void Connect() {
            NotNotifyDisconnect();

            var direction = GetStructureDirectionVector(this);

            if (Physics.Raycast(transform.position, direction, out var hit, 1.0f, ConnectLayer)) {
                if (hit.transform.TryGetComponent<IResourceReceivable>(out var receivable)) {
                    if (receivable is IStructure structure) {
                        if (Mathf.Abs(Direction - structure.Direction) == 2) {
                            return;
                        }
                    }

                    Next = receivable;

                    if (Next is ConveryorBeltBehaviour next_belt) {
                        if (BeltInfo.Type == next_belt.BeltInfo.Type) {
                            next_belt.AddPrevious(this);
                        } else {
                            Next = null;
                        }
                        
                        next_belt.OnDependencyChanged?.Invoke();

                        transform.name = $"{transform.position}";
                        next_belt.transform.name = $"{next_belt.transform.position}";
                    }
                }
            }

            OnDependencyChanged?.Invoke();
        }
        [ContextMenu("Disconnect")]
        public override void Disconnect() {
            NotNotifyDisconnect();

            OnDependencyChanged?.Invoke();
        }
        public void NotNotifyDisconnect() {
            if (Next is ConveryorBeltBehaviour next_belt) {
                next_belt.RemoveAtPrevious(this);
            }

            Next = null;
        }

        [ContextMenu("Connect Around")]
        public override void ConnectAround() {
            base.ConnectAround();

            OnDependencyChanged?.Invoke();
        }
        #endregion

        [ContextMenu("Print Dependency")]
        public void PrintDependency() {
            Debug.Log($"Next: {Next != null}, Previous: {Previous.Count}");
        }

        public override void Release() {
            base.Release();

            foreach (var previous in Previous) {
                if (previous is ConveryorBeltBehaviour previous_belt) {
                    previous_belt.Next = null;
                }
            }
            Previous.Clear();

            if (Next is ConveryorBeltBehaviour next_belt) {
                next_belt.RemoveAtPrevious(this);

                next_belt.OnDependencyChanged?.Invoke();
            }
            Next = null;

            foreach (var resource in AllResources) {
                resource.Release();
            }
        }

        private void AddPrevious(ConveryorBeltBehaviour belt) {
            if (!Previous.Contains(belt)) {
                Previous.Add(belt);
            }

            belt.Next = this;

            OnDependencyChanged?.Invoke();
        }

        private void RemoveAtPrevious(ConveryorBeltBehaviour belt) {
            Previous.Remove(belt);
        }

        protected Vector3 GetOffsetByProgress(ResourceBehaviour resource) {
            var direction = GetStructureDirectionVector(this);

            if (resource.Progress >= 0.5f || resource.LastDirection == StructureDirection.None) {
                return (direction * resource.Progress) - (direction * 0.5f) + RESOURCE_VISUAL_OFFSET;
            }

            var previous_direction = DirectionToVector(resource.LastDirection);

            return (previous_direction * resource.Progress) - (previous_direction * 0.5f) + RESOURCE_VISUAL_OFFSET;
        }

    }
}