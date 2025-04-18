using Define;
using Interaface;
using Resource.GameData;
using Structure.GameData;
using Structure.Information;
using Structure.Interface;
using UI.Interface;
using UnityEngine;

namespace Structure {
    public class StructureBehaviour : MonoBehaviour, IStructure, IReleaseable {
        [field: SerializeField] public virtual StructureInfo Info { get; protected set; }
        [field: SerializeField] public virtual StructureData Data { get; protected set; } = new();

        public StructureDirection Direction {
            get => Data.direction;
            set => Data.direction = value;
        }
        public Vector3 Position {
            get => transform.position;
            set => transform.position = value;
        }
        public virtual string Title => Info.Name;
        public virtual string Description => Info.Description;

        public int Price => Info.Price;

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

            if (direction.x > ConstValue.EPSILON) {
                return StructureDirection.Right;
            }

            if (direction.x < -ConstValue.EPSILON) {
                return StructureDirection.Left;
            }

            if (direction.z > ConstValue.EPSILON) {
                return StructureDirection.Up;
            }

            if (direction.z < -ConstValue.EPSILON) {
                return StructureDirection.Down;
            }

            return StructureDirection.None;
        }

        public static StructureDirection GetSafeDirection(IStructure structure, Vector3 target) {
            if (structure.Direction == StructureDirection.None) {
                return VectorToDirection(structure.Position, target);
            }

            return structure.Direction;
        }
        #endregion

        public virtual void OnConfirm() { }
        public virtual void OnPreview() { }

        #region Implement for IConnectable
        public static readonly Vector3[] ConnectAround4Ways = new Vector3[] { Vector3.left, Vector3.right, Vector3.forward, Vector3.back, Vector3.down };
       
        [field: SerializeField] public virtual LayerMask ConnectLayer { get; set; }

        public virtual void Connect() {}
        public virtual void Disconnect() {}

        public virtual void ConnectAround() {
            foreach (var direction in ConnectAround4Ways) {
                if (Physics.Raycast(transform.position, direction, out var hit, 1.0f, ConnectLayer)) {
                    if (hit.transform.TryGetComponent<IConnectable>(out var connectable)) {
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