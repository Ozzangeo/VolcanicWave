using System;
using UnityEngine;

namespace Manager {
    public class MouseManager : BasicManager<MouseManager> {
        public override string Name => "Mouse Manager";

        public event Action<float> OnScrollWheel;

        private void Update() {
            var mouse_wheel_scroll = Input.mouseScrollDelta;

            if (mouse_wheel_scroll.y != 0.0f) {
                OnScrollWheel?.Invoke(mouse_wheel_scroll.y);
            }
        }

        public static bool IsMouseInCorner(float sensitivity) => CameraManager.IsPointInCorner(Input.mousePosition, sensitivity);
        public static Vector3 GetMouseDirection() => CameraManager.GetPointDirection(Input.mousePosition);
    }
}