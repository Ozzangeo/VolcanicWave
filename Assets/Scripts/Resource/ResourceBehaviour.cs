using Interaface;
using Resource.GameData;
using Resource.Infomation;
using Structure.GameData;
using UnityEngine;

namespace Resource {
    public class ResourceBehaviour : MonoBehaviour, IReleaseable {
        [field: SerializeField] public virtual ResourceInfo Info { get; protected set; } = new();
        [field: SerializeField] public virtual ResourceData Data { get; protected set; } = new();

        public ResourceType Type => Info.type;
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
                Data.progress += Data.tickRate * Time.deltaTime;
            }
        }

        public void Release() {
            // After Use Object Pooling
            Destroy(gameObject);
        }
    }
}