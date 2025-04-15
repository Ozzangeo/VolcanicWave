using Resource.GameData;
using Resource.Infomation;
using Resource.Interface;
using UnityEngine;

namespace Resource {
    public class BasicResource : MonoBehaviour, ISelectable {
        [field: SerializeField] public virtual ResourceInfo Info { get; protected set; } = new();
        [field: SerializeField] public virtual ResourceData Data { get; protected set; } = new();

        public int Type => Info.type;
        public float Progress {
            get => Data.progress;
            set => Data.progress = value;
        }
        public float Weight {
            get => Info.weight;
            set => Info.weight = value;
        }

        [field: SerializeField] public virtual StructureDirection LastDirection { get; set; } = StructureDirection.None;

        public bool IsProgressHalfDone => Progress >= 0.5f;
        public bool IsProgressDone => Progress >= 1.0f;

        protected virtual void Update() {
            if (Info is null) {
                return;
            }

            if (!Data.isRest) {
                float weight_rate = Info.weight / 10.0f;

                Data.progress += Data.tickRate * weight_rate * Time.deltaTime;
            }
        }

        public void Release() {
            // After Use Object Pooling
            Destroy(gameObject);
        }
    }
}