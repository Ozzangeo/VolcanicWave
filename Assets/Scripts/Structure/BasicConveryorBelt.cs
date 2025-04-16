using Define;
using Resource;
using Resource.GameData;
using Resource.Infomation;
using Structure.Interface;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Structure {
    public class BasicConveryorBelt : BasicStructure, IResourceReceivable, ILinkable {
        public static readonly Vector3 RESOURCE_VISUAL_OFFSET = Vector3.up * 0.5f;

        [field: SerializeField] public virtual ConveryorBeltInfo Info { get; protected set; } = new();

        public virtual IResourceReceivable Next { get; set; }

        [field: Header("Resources")]
        [field: SerializeField] public virtual List<BasicResource> Resources { get; protected set; } = new();
        [field: SerializeField] public virtual List<BasicResource> RestResources { get; protected set; } = new();

        [field: SerializeField] protected virtual List<BasicResource> ThrowResources { get; set; } = new();

        public IEnumerable<BasicResource> AllResources => Resources.Concat(RestResources);
        public BasicResource LastResource => AllResources.OrderBy(o => o.Progress).FirstOrDefault();
        public BasicResource LastRestResource => RestResources.OrderBy(o => o.Progress).FirstOrDefault();

        public float TotalRestWeight => RestResources.Sum(o => o.Info.weight);
        public float TotalWeight => TotalRestWeight + Resources.Sum(o => o.Info.weight);

        public bool IsBeltEnd => Next == null;

        protected virtual void Update() {
            if (!IsBeltEnd && RestResources.Any()) {
                foreach (var resource in RestResources) {
                    UnRestResource(resource);
                }
                RestResources.Clear();
            }

            foreach (var resource in Resources) {
                if (RestResources.Any()) {
                    var need_progress = 1.0f - (TotalRestWeight / Info.maxWeight);

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
        public bool IsResourceReceivable(BasicResource resource) {
            if (!Info.type.HasFlag(resource.Type)) {
                return false;
            }

            // Total Weight Check
            if (TotalWeight + resource.Weight > Info.maxWeight) {
                return false;
            }

            // Is There Any Free Space Check
            var last_resource = LastResource;

            if (last_resource != null) {
                var weight = (Info.maxWeight * last_resource.Progress) - (last_resource.Weight * 0.5f);

                if ((resource.Weight * 0.5f) > weight) {
                    return false;
                }
            }

            return true;
        }
        public void ReceiveResource(BasicResource resource, IStructure from) {
            resource.Data.Reset();
            resource.Data.tickRate += Info.speed;
            resource.LastDirection = from.Direction;

            Resources.Add(resource);
        }

        protected void ResourceDisplayUpdate(BasicResource resource) {
            var offset = GetOffsetByProgress(resource);

            resource.transform.position = transform.position + offset;
        }

        public void ThrowResource(BasicResource resource) {
            resource.Data.tickRate -= Info.speed;

            ThrowResources.Add(resource);
        }
        public void RestResource(BasicResource resource) {
            resource.Data.isRest = true;

            RestResources.Add(resource);

            ThrowResources.Add(resource);
        }
        public void UnRestResource(BasicResource resource) {
            resource.Data.isRest = false;

            Resources.Add(resource);
        }

        [ContextMenu("Link")]
        public override void Link() {
            LinkClear();
            
            var direction = GetStructureDirectionVector(this);

            if (Physics.Raycast(transform.position, direction, out var hit, 1.0f, LayerMasks.StructureMask)) {
                if (hit.transform.TryGetComponent<IResourceReceivable>(out var receivable)) {
                    Next = receivable;
                }
            }
        }

        [ContextMenu("Link Clear")]
        public override void LinkClear() {
            Next = null;
        }

        public override void Release() {
            base.Release();

            foreach (var resource in AllResources) {
                resource.Release();
            }
        }

        protected Vector3 GetOffsetByProgress(BasicResource resource) {
            var direction = GetStructureDirectionVector(this);

            if (resource.Progress >= 0.5f || resource.LastDirection == StructureDirection.None) {
                return (direction * resource.Progress) - (direction * 0.5f) + RESOURCE_VISUAL_OFFSET;
            }

            var previous_direction = DirectionToVector(resource.LastDirection);

            return (previous_direction * resource.Progress) - (previous_direction * 0.5f) + RESOURCE_VISUAL_OFFSET;
        }

    }
}