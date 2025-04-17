using Interaface;
using Resource.GameData;
using Structure.Interface;
using UnityEngine;

namespace Structure {
    public class StructureBehaviour : MonoBehaviour, IStructure, IReleaseable {
        [field: SerializeField] public virtual StructureData Data { get; protected set; } = new();

        public StructureDirection Direction {
            get => Data.direction;
            set => Data.direction = value;
        }

        #region Utility
        public static Vector3 GetStructureDirectionVector(IStructure structure)
            => DirectionToVector(structure.Direction);
        public static Vector3 DirectionToVector(StructureDirection direction) =>
            direction switch {
                StructureDirection.Up => Vector3.forward,
                StructureDirection.Down => Vector3.back,
                StructureDirection.Left => Vector3.left,
                StructureDirection.Right => Vector3.right,
                _ => Vector2.zero
            };

        public static StructureDirection VectorToDirection(Vector3 pivot, Vector3 target) {
            var direction = (target - pivot);

            if (direction.x > 0.0f) {
                return StructureDirection.Right;
            }

            if (direction.x < 0.0f) {
                return StructureDirection.Left;
            }

            if (direction.z > 0.0f) {
                return StructureDirection.Up;
            }

            if (direction.z < 0.0f) {
                return StructureDirection.Down;
            }

            return StructureDirection.None;
        }
        #endregion

        #region Implement for IConnectable
        public static readonly Vector3[] ConnectAround4Ways = new Vector3[] { Vector3.left, Vector3.right, Vector3.forward, Vector3.back, Vector3.down };
       
        [field: SerializeField] public virtual LayerMask ConnectLayer { get; set; }

        public virtual void Connect() {}
        public virtual void Disconnect() {}

        public virtual void ConnectAround() {
            foreach (var direction in ConnectAround4Ways) {
                if (Physics.Raycast(transform.position, direction, out var hit, 1.0f, ConnectLayer)) {
                    if (hit.transform.TryGetComponent<IConnectable>(out var connectable)) {
                        if (connectable is MonoBehaviour mono) {
                            Debug.Log($"[ Connect Around ] {transform.position} connect notify to {mono.transform.position}");
                        }

                        connectable.Connect();
                    }
                }
            }
        }
        #endregion
        #region Implement for IReleaseable
        public virtual void Release() {
            Destroy(gameObject);
        }
        #endregion
    }
}