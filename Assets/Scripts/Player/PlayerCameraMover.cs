using Define;
using Manager;
using Player.GameData;
using UnityEngine;

namespace Player {
    public class PlayerCameraMover : MonoBehaviour {
        public const float CORNER_SENSITIVITY = 20.0f;

        public const float STANDARD_ZOOM = -8.0f;
        public static readonly Vector3 StandardZoomPoint = new(0.0f, 0.0f, STANDARD_ZOOM);

        [field: SerializeField] public CameraData Data { get; private set; } = new();

        [field: SerializeField] public float HeightRatio { get; private set; } = 0.0f;

        private void Awake() {
            UpdateHeightRatio(CameraManager.Instance.Camera.transform.eulerAngles);
        }

        private void Start() {
            MouseManager.Instance.OnScrollWheel += CameraZoom;
        }

        private void Update() {
            if (MouseManager.IsMouseInCorner(CORNER_SENSITIVITY)) {
                var mouse_direction = MouseManager.GetMouseDirection();

                mouse_direction.z = mouse_direction.y;
                mouse_direction.y = 0.0f;

                var tick = Data.speed.value * Time.deltaTime;
                transform.localPosition += mouse_direction * tick;
            }

            ApplyCameraZoom();
        }

        private void CameraZoom(float wheel_scroll) {
            var sign = -Mathf.Sign(wheel_scroll);
            var zoom = (sign * Data.zoomSpeed.value) * Time.deltaTime;

            Data.zoomRate.Value += zoom;
        }

        [ContextMenu("Update Height Ratio")]
        private void UpdateHeightRatio(Vector3 eular_angles) => HeightRatio = Mathf.Tan(eular_angles.x * Mathf.Deg2Rad);

        public void ApplyCameraZoom() {
            var position = StandardZoomPoint;
            position.y = -STANDARD_ZOOM * HeightRatio;

            CameraManager.Instance.Camera.transform.localPosition = position * Data.zoomRate.value;
        }
    }
}