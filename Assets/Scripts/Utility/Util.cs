using Unity.VisualScripting;
using UnityEngine;

namespace Utility {
    public static class Util {
        public static bool IsPositiveInfinity(this Vector3 vector) =>
            float.IsPositiveInfinity(vector.x) &&
            float.IsPositiveInfinity(vector.y) &&
            float.IsPositiveInfinity(vector.z);
        public static bool IsPositiveInfinityAny(this Vector3 vector) =>
            float.IsPositiveInfinity(vector.x) ||
            float.IsPositiveInfinity(vector.y) ||
            float.IsPositiveInfinity(vector.z);

        public static bool IsPositiveInfinity(this Vector3Int vector) =>
            int.MinValue == vector.x &&
            int.MinValue == vector.y &&
            int.MinValue == vector.z;
        public static bool IsPositiveInfinityAny(this Vector3Int vector) =>
            int.MinValue == vector.x ||
            int.MinValue == vector.y ||
            int.MinValue == vector.z;
    }
}