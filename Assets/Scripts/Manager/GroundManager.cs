using Define;
using Structure;
using Resource.GameData;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Manager {
    public class GroundManager : BasicManager<GroundManager> {
        public static readonly Vector3 StandardSize = new(0.8f, 1.0f, 0.8f);

        public override string Name => "Ground Manager";

        [field: SerializeField] public Vector3 OnDownGroundPoint { get; private set; }
        [field: SerializeField] public Vector3 OnHoldGroundPoint { get; private set; }

        [field: SerializeField] public StructureBehaviour Structure { get; private set; }
        [field: SerializeField] public StructureDirection Direction { get; set; }

        [field: SerializeField] public List<StructureGround> SelectGroundAlls { get; set; } = new();
        [field: SerializeField] public List<StructureGround> SelectGrounds { get; set; } = new();
        [field: SerializeField] public List<StructureGround> BlueprintGrounds { get; set; } = new();

        [SerializeField] private Vector3 _center;
        [SerializeField] private Vector3 _size;
        [SerializeField] private StructureBehaviour _structure;

        protected override void OnAwake() {
            if (Structure != null) {
                SetStructureLocal(Structure);
            }
        }

        private void Update() {
            if (Structure == null) {
                return;
            }

            var mouse_position = Input.mousePosition;

            var ground_point = CameraManager.ScreenRayToWorldPoint(mouse_position, LayerMasks.GroundMask);
            ground_point.y = 0.0f;

            if (!ground_point.IsPositiveInfinityAny()) {
                if (Input.GetMouseButtonDown(MouseButton.LEFT)) {
                    OnDownGroundPoint = Vector3Int.RoundToInt(ground_point);

                    _structure.gameObject.SetActive(false);
                }

                if (Input.GetMouseButton(MouseButton.LEFT)) {
                    if (OnDownGroundPoint.IsPositiveInfinityAny()) {
                        OnDownGroundPoint = Vector3Int.RoundToInt(ground_point);
                    }

                    OnHoldGroundPoint = Vector3Int.RoundToInt(ground_point);

                    var distance = OnHoldGroundPoint - OnDownGroundPoint;
                    StructureDirection direction;
                    if (Mathf.Abs(distance.x) > Mathf.Abs(distance.z)) {
                        distance.z = 0.0f;

                        if (distance.x > 0.0f) {
                            direction = StructureDirection.Right;
                        } else {
                            direction = StructureDirection.Left;
                        }
                    } else {
                        distance.x = 0.0f;
                        
                        if (distance.z > 0.0f) {
                            direction = StructureDirection.Up;
                        } else {
                            direction = StructureDirection.Down;
                        }
                    }

                    _center = OnDownGroundPoint + (distance * 0.5f);

                    distance.x = Mathf.Abs(distance.x);
                    distance.z = Mathf.Abs(distance.z);

                    _size = StandardSize + distance;

                    foreach (var ground in SelectGrounds) {
                        ground.HideGuide();
                    }
                    SelectGrounds.Clear();

                    var overlaps = Physics.OverlapBox(_center, _size * 0.5f, Quaternion.identity, LayerMasks.GroundMask);
                    foreach (var overlap in overlaps) {
                        if (overlap.TryGetComponent<StructureGround>(out var ground)) {
                            if (ground.IsConstrutible) {
                                ground.ShowGuide(Structure, direction);
                                ground.Guide.ConnectAround();

                                SelectGrounds.Add(ground);
                                if (!SelectGroundAlls.Contains(ground)) {
                                    SelectGroundAlls.Add(ground);
                                }
                            }
                        }
                    }
                }
                else {
                    var unit = Vector3Int.RoundToInt(ground_point) + Vector3.up;

                    _structure.transform.position = unit;
                    _structure.Direction = Direction;
                }
            }

            if (Input.GetMouseButtonUp(MouseButton.LEFT)) {
                Debug.Log("Connect Start!");

                foreach (var ground in SelectGrounds) {
                    ground.HideGuide();

                    ground.ShowPreview();

                    SelectGroundAlls.Remove(ground);

                    if (!BlueprintGrounds.Contains(ground)) {
                        BlueprintGrounds.Add(ground);
                    }
                }

                foreach (var ground in SelectGroundAlls) {
                    ground.ReleaseGuide();
                }
                
                SelectGrounds.Clear();
                SelectGroundAlls.Clear();
                
                _structure.gameObject.SetActive(true);
            }
        }
        private void SetStructureLocal(StructureBehaviour structure) {
            if (_structure != null) {
                Destroy(_structure.gameObject);
            }

            Structure = structure;

            _structure = Instantiate(structure);
        }
        public static void SetStructure(StructureBehaviour structure) => Instance.SetStructureLocal(structure);

        [ContextMenu("Confirm All")]
        private void ConfirmAllLocal() {
            foreach (var ground in BlueprintGrounds) {
                ground.Confirm();
            }

            BlueprintGrounds.Clear();
        }
        [ContextMenu("Cancel All")]
        private void CancelAllLocal() {
            foreach (var blueprint in BlueprintGrounds) {
                blueprint.Cancel();
            }

            BlueprintGrounds.Clear();
        }

        public static void ConfirmAll() => Instance.ConfirmAllLocal();
        public static void CancelAll() => Instance.CancelAllLocal();

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(_center, _size);

            Gizmos.color = Color.white;
        }
    }
}