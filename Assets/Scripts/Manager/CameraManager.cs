using Define;
using UnityEngine;

namespace Manager {
    public class CameraManager : BasicManager<CameraManager> {
        public override string Name => "Camera Manager";
        
        [SerializeField] private Camera _camera;

        public Camera Camera {
            get {
                if (_camera == null) {
                    _camera = Camera.main;
                }

                return _camera;
            }
        }

        private bool IsPointInCornerLocal(Vector3 screen_point, float sensitivity) {
            var camera = Camera;

            var width = camera.pixelWidth - sensitivity;
            var height = camera.pixelHeight - sensitivity;

            bool is_under_corner_x = screen_point.x <= sensitivity;
            bool is_under_corner_y = screen_point.y <= sensitivity;
            bool is_over_corner_x = screen_point.x >= width;
            bool is_over_corner_y = screen_point.y >= height;

            return is_under_corner_x || is_under_corner_y || is_over_corner_x || is_over_corner_y;
        } 
        private Vector3 GetPointDirectionLocal(Vector3 screen_point) {
            var camera = Camera;

            var resolution = new Vector2(camera.pixelWidth, camera.pixelHeight);
            var half_resolution = resolution * 0.5f;
            var distance = screen_point - (Vector3)half_resolution;

            return distance.normalized;
        }
        private Vector3 ScreenRayToWorldPointLocal(Vector3 screen_point, int layer_mask, float max_length) {
            var camera = Camera;

            var ray = camera.ScreenPointToRay(screen_point);
            if (Physics.Raycast(ray, out var hit, max_length, layer_mask)) {
                var point = hit.point;

                if (Mathf.Abs(point.y) <= ConstValue.EPSILON) {
                    point.y = 0.0f;
                }

                return point;
            }

            return ConstValue.MaxPosition;
        }

        public static bool IsPointInCorner(Vector3 screen_point, float sensitivity) => Instance.IsPointInCornerLocal(screen_point, sensitivity);
        public static Vector3 GetPointDirection(Vector3 screen_point) => Instance.GetPointDirectionLocal(screen_point); 
        public static Vector3 ScreenRayToWorldPoint(Vector3 screen_point, int layer_mask = -1, float max_length = Mathf.Infinity) => Instance.ScreenRayToWorldPointLocal(screen_point, layer_mask, max_length);
    }
}