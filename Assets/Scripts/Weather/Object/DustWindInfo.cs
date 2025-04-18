using System;
using System.Linq;
using UnityEngine;

namespace Weather.Object {
    [CreateAssetMenu(fileName = "Dust Wind Info", menuName = "Weather/Info/DustWind")]
    public class DustWindInfo : ScriptableObject {
        [field: Tooltip("Unit: Minutes")]
        [field: SerializeField] public float[] RemainTimes { get; private set; } = new float[] {
            10.0f,
            10.0f,
            10.0f,
            10.0f,
        };
        [field: SerializeField] public float[] GroundRemoveRates { get; private set; } = new float[] {
            5.0f,
            10.0f,
            20.0f,
            25.0f,
        };
        [field: SerializeField] public float FadeTime { get; private set; } = 1.5f;

        public TimeSpan[] RemainSpans => RemainTimes.Select(o => TimeSpan.FromMinutes(o)).ToArray();
        public TimeSpan FadeSpan => TimeSpan.FromSeconds(FadeTime);
    }
}