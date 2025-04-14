using Resource.GameData;
using UnityEngine;

namespace Resource {
    public class BasicResource : MonoBehaviour {
        [field: SerializeField] public virtual ResourceData Data { get; protected set; } = new();
        [field: SerializeField] public virtual ResourceInfo Info { get; protected set; } = new();

        public float Progress => Data.progress;
        public float Weight => Info.weight;

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
    }
}