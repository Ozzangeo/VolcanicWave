using Util;

namespace Player.GameData {
    [System.Serializable]
    public class CameraData {
        public const float MAX_SPEED = 20.0f;
        public const float MIN_SPEED = 1.0f;
        public const float MAX_ZOOM_SPEED = 20.0f;
        public const float MIN_ZOOM_SPEED = 1.0f;
        public const float MAX_ZOOM_RATE = 1.0f;
        public const float MIN_ZOOM_RATE = 0.25f;

        public Range speed = new(MAX_SPEED, MIN_SPEED, 10.0f);
        public Range zoomSpeed = new(MAX_ZOOM_SPEED, MIN_ZOOM_SPEED, 10.0f);
        public Range zoomRate = new(MAX_ZOOM_RATE, MIN_ZOOM_RATE, 1.0f);
    }
}