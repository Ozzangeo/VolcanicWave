namespace Utility {
    [System.Serializable]
    public class Range {
        public float Max { get; private set; }
        public float Min { get; private set; }

        public float value;

        public float Value { 
            get => value; 
            set => this.value = System.Math.Clamp(value, Min, Max); 
        }

        public Range(float max, float min, float init_value = 0.0f) {
            Max = max; 
            Min = min;

            Value = init_value;
        }
    }
}