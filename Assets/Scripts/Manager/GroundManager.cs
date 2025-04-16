using Define;
using Ground;
using Resource.GameData;
using Structure;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Manager {
    public class GroundManager : BasicManager<GroundManager> {
        public static readonly Vector3 StandardSize = new(0.8f, 1.0f, 0.8f);

        public override string Name => "Ground Manager";

        [field: SerializeField] public Vector3 OnDownGroundPoint { get; private set; }
        [field: SerializeField] public Vector3 OnHoldGroundPoint { get; private set; }

        [field: SerializeField] public BasicStructure Structure { get; set; }
        [field: SerializeField] public StructureDirection Direction { get; set; }

        [field: SerializeField] public List<BasicGround> Selects { get; set; }
        [field: SerializeField] public List<BasicGround> Blueprints { get; set; }

        [SerializeField] private Vector3 _center;
        [SerializeField] private Vector3 _size;

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

                    foreach (var select in Selects) {
                        select.ExitGuide();
                    }
                    Selects.Clear();

                    var overlaps = Physics.OverlapBox(_center, _size * 0.5f, Quaternion.identity, LayerMasks.GroundMask);
                    foreach (var overlap in overlaps) {
                        if (overlap.TryGetComponent<BasicGround>(out var ground)) {
                            if (ground.IsConstrutible) {
                                Selects.Add(ground);

                                ground.StartGuide(Structure, direction);
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(MouseButton.LEFT)) {
                foreach (var select in Selects) {
                    select.ReleaseGuide();
                    select.Preview(select.Guide.instance, select.Guide.direction);
                    
                    if (!Blueprints.Contains(select)) {
                        Blueprints.Add(select);
                    }
                }
                Selects.Clear();
            }
        }

        [ContextMenu("Confirm All")]
        private void ConfirmAllLocal() {
            foreach (var blueprint in Blueprints) {
                blueprint.Confirm();
            }

            Blueprints.Clear();
        }
        [ContextMenu("Cancel All")]
        private void CancelAllLocal() {
            foreach (var blueprint in Blueprints) {
                blueprint.Cancel();
            }

            Blueprints.Clear();
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