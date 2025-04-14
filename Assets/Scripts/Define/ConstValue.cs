using UnityEngine;

namespace Define {
    public static class ConstValue {
        public const float EPSILON = 1E-06f;

        public static readonly Vector3 MaxPosition = Vector3.positiveInfinity;
        public static readonly Quaternion Rotate45 = Quaternion.Euler(0.0f, 0.0f, -45.0f);
    }
}