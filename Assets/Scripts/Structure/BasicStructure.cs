using Define;
using Interaface;
using Resource.GameData;
using Structure.Interface;
using UnityEngine;

namespace Structure {
    public class BasicStructure : MonoBehaviour, IStructure, IReleaseable {
        public static readonly Vector3[] ChainLink4Ways = new Vector3[] { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

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

        public virtual void Link() {}
        public virtual void ChainLink() {
            foreach (var direction in ChainLink4Ways) {
                if (Physics.Raycast(transform.position, direction, out var hit, 1.0f, LayerMasks.StructureMask)) {
                    if (hit.transform.TryGetComponent<ILinkable>(out var linkable)) {
                        linkable.Link();

                        if (linkable is MonoBehaviour mono) {
                            Debug.Log($"{name} link! -> {mono.name}");
                        }
                    }
                }
            }
        }
        public virtual void LinkClear() {}

        public virtual void Release() {
            Destroy(gameObject);
        }
    }
}