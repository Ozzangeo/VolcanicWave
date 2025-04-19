using Define;
using Structure;
using Structure.GameData;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System.Linq;
using UnityEngine.EventSystems;

namespace Manager {
    public class StructureBuildManager : BasicManager<StructureBuildManager> {
        public static readonly Vector3 StandardSize = new(0.8f, 1.0f, 0.8f);

        public override string Name => "Structure Build Manager";

        [field: SerializeField] public Vector3 OnDownGroundPoint { get; private set; }
        [field: SerializeField] public Vector3 OnHoldGroundPoint { get; private set; }

        [field: SerializeField] public StructureBehaviour Structure { get; private set; }
        [field: SerializeField] public StructureDirection Direction { get; set; }

        [field: SerializeField] public List<GroundBehaviour> SelectGroundAlls { get; set; } = new();
        [field: SerializeField] public List<GroundBehaviour> SelectGrounds { get; set; } = new();
        [field: SerializeField] public List<GroundBehaviour> BlueprintGrounds { get; set; } = new();

        [SerializeField] private Vector3 _center;
        [SerializeField] private Vector3 _size;
        [SerializeField] private StructureBehaviour _preview;
        [SerializeField] private bool _isCanceled = false;

        protected override void OnAwake() {
            if (Structure != null) {
                SetStructureLocal(Structure);
            }
        }

        private void Update() {
            if (Structure == null) {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                ConfirmAll();

                return;
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                CancelAll();

                return;
            }

            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            var mouse_position = Input.mousePosition;

            var ground_point = CameraManager.ScreenRayToWorldPoint(mouse_position, LayerMasks.GroundMask);
            ground_point.y = 0.0f;

            if (!ground_point.IsPositiveInfinityAny()) {
                if (Input.GetMouseButtonDown(MouseButton.LEFT)) {
                    OnDownGroundPoint = Vector3Int.RoundToInt(ground_point);

                    _preview.gameObject.SetActive(false);
                }

                if (Input.GetMouseButton(MouseButton.LEFT) && !_isCanceled) {
                    if (OnDownGroundPoint.IsPositiveInfinityAny()) {
                        OnDownGroundPoint = Vector3Int.RoundToInt(ground_point);
                    }

                    OnHoldGroundPoint = Vector3Int.RoundToInt(ground_point);

                    SelectGround();

                    if (Input.GetMouseButtonDown(MouseButton.RIGHT)) {
                        foreach (var ground in SelectGroundAlls) {
                            ground.ReleaseGuide();
                        }
                        SelectGrounds.Clear();
                        SelectGroundAlls.Clear();

                        _isCanceled = true;
                    }
                }

                var unit = Vector3Int.RoundToInt(ground_point) + Vector3.up;

                _preview.transform.position = unit;
                _preview.Direction = Direction;
            }

            if (!Input.GetMouseButton(MouseButton.LEFT) && !Input.GetMouseButton(MouseButton.RIGHT)) {
                _isCanceled = false;
            }

            if (Input.GetMouseButtonUp(MouseButton.LEFT)) {
                AddSelectGround();

                _preview.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                SetStructure(null);
            }
        }

        private void SelectGround() {
            var distance = OnHoldGroundPoint - OnDownGroundPoint;
            StructureDirection direction;
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.z)) {
                distance.z = 0.0f;

                if (distance.x > 0.0f) {
                    direction = StructureDirection.Right;
                }
                else {
                    direction = StructureDirection.Left;
                }
            }
            else {
                distance.x = 0.0f;

                if (distance.z > 0.0f) {
                    direction = StructureDirection.Up;
                }
                else {
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
                if (overlap.TryGetComponent<GroundBehaviour>(out var ground)) {
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
        private void AddSelectGround() {
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
        }

        #region Local Function
        public static int TotalBlueprintPrice => Instance.BlueprintGrounds.Sum(o => o.Preview.structure.Price);
        public static int TotalSelectPrice => Instance.SelectGrounds.Sum(o => o.Guide.structure.Price);

        private void SetStructureLocal(StructureBehaviour structure) {
            if (_preview != null) {
                Destroy(_preview.gameObject);
            }

            Structure = structure;

            if (structure != null) {
                _preview = Instantiate(structure);
            } else {
                _preview = null;
            }
        }

        [ContextMenu("Confirm All")]
        private void ConfirmAllLocal() {
            if (PlayerManager.Instance.Exp >= TotalBlueprintPrice) {
                PlayerManager.Instance.Exp -= TotalBlueprintPrice;

                foreach (var ground in BlueprintGrounds) {
                    if (ground != null) {
                        ground.Confirm();
                    }
                }

                BlueprintGrounds.Clear();
            }
        }
        [ContextMenu("Cancel All")]
        private void CancelAllLocal() {
            foreach (var ground in BlueprintGrounds) {
                if (ground != null) {
                    ground.Cancel();
                }
            }

            BlueprintGrounds.Clear();
        }
        #endregion

        public static void SetStructure(StructureBehaviour structure) => Instance.SetStructureLocal(structure);
        public static void ConfirmAll() => Instance.ConfirmAllLocal();
        public static void CancelAll() => Instance.CancelAllLocal();

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(_center, _size);

            Gizmos.color = Color.white;
        }
    }
}