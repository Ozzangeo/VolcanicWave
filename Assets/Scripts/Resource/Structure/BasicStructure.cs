using Resource.GameData;
using Resource.Structure.Interface;
using UnityEngine;

namespace Resource.Structure {
    public class BasicStructure : MonoBehaviour, IStructure {
        [field: SerializeField] public virtual StructureData Data { get; protected set; } = new();

        public StructureDirection Direction {
            get => Data.direction;
            set => Data.direction = value;
        }

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
    }
}