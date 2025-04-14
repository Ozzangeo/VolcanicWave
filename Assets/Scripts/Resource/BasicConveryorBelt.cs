using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Resource {
    public class BasicConveryorBelt : MonoBehaviour {
        public static readonly Vector3 RESOURCE_VISUAL_OFFSET = Vector3.up * 0.5f;

        public const byte UP_FLAG       = 0b0000_0001;
        public const byte DOWN_FLAG     = 0b0000_0010;
        public const byte LEFT_FLAG     = 0b0000_0100;
        public const byte RIGHT_FLAG    = 0b0000_1000;

        [field: SerializeField] public virtual BasicConveryorBelt Next { get; set; }
        [field: SerializeField] public virtual List<BasicConveryorBelt> Previous { get; set; }

        [field: SerializeField] public virtual List<BasicResource> Resources { get; protected set; } = new();
        [field: SerializeField] public virtual List<BasicResource> RestResources { get; protected set; } = new();

        [field: SerializeField] protected virtual List<BasicResource> ThrowResources { get; set; } = new();

        [field: SerializeField] public virtual ConveryorBeltInfo Info { get; protected set; } = new();

        public float TotalRestWeight => RestResources.Sum(o => o.Info.weight);
        public float TotalWeight => TotalRestWeight + Resources.Sum(o => o.Info.weight);

        public bool IsBeltEnd => Next == null;

        [field: SerializeField] public byte Direction { get; protected set; } = RIGHT_FLAG;

        protected virtual void Update() {
            if (IsBeltEnd) {
                foreach (var resource in Resources) {
                    if (resource.IsProgressHalfDone) {
                        resource.Data.progress = 0.5f;

                        RestResource(resource);

                        ResourceDisplayUpdate(resource);

                        continue;
                    }

                    if (RestResources.Any()) {
                        var rest_weight = TotalRestWeight * 2.0f;
                        var need_progress = 1.0f - (rest_weight / Info.maxWeight);

                        if (resource.Data.progress >= need_progress) {
                            resource.Data.progress = need_progress;

                            ResourceDisplayUpdate(resource);

                            RestResource(resource);

                            continue;
                        }
                    }

                    ResourceDisplayUpdate(resource);
                }
            }
            else {
                if (RestResources.Any()) {
                    foreach (var resource in RestResources) {
                        UnRestResource(resource);
                    }

                    RestResources.Clear();
                }

                foreach (var resource in Resources) {
                    if (resource.IsProgressDone) {
                        if (Next.IsResourceTakeable(resource)) {
                            Next.TakeResource(resource);

                            ThrowResource(resource);

                            Next.ResourceDisplayUpdate(resource);
                        }
                        else {
                            resource.Data.progress = 1.0f;

                            RestResource(resource);

                            ResourceDisplayUpdate(resource);
                        }

                        continue;
                    }

                    if (RestResources.Any()) {
                        var need_progress = 1.0f - (TotalRestWeight / Info.maxWeight);

                        if (resource.Data.progress >= need_progress) {
                            resource.Data.progress = need_progress;

                            ResourceDisplayUpdate(resource);

                            RestResource(resource);

                            continue;
                        }
                    }

                    ResourceDisplayUpdate(resource);
                }
            }

            // Throw Resources
            foreach (var resource in ThrowResources) {
                Resources.Remove(resource);
            }
            ThrowResources.Clear();
        }

        private void ResourceDisplayUpdate(BasicResource resource) {
            var point = GetPointByProgress(resource.Data.progress);

            resource.transform.position = transform.position + point;
        }
        public bool IsResourceTakeable(BasicResource resource) {
            var weight_rate = IsBeltEnd ? 2.0f : 1.0f;

            var future_weight = (TotalWeight + resource.Weight) * weight_rate;
            var is_full_weight = future_weight > Info.maxWeight;

            

            return !is_full_weight;
        }

        public void TakeResource(BasicResource resource) {
            resource.Data.Reset();

            Resources.Add(resource);
            resource.Data.tickRate += Info.speed;
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

        protected Vector3 GetPointByProgress(float progress) {
            var direction = GetDirectionVector();

            return (direction * progress) + RESOURCE_VISUAL_OFFSET;
        }
        protected Vector3 GetDirectionVector() => 
            Direction switch {
                UP_FLAG     => Vector3.forward,
                DOWN_FLAG   => Vector3.back,
                LEFT_FLAG   => Vector3.left,
                RIGHT_FLAG  => Vector3.right,
                _ => Vector2.zero
            };
    }
}