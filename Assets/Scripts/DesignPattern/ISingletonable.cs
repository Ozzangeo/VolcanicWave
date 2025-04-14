namespace DesignPattern {
    public interface ISingletonable {
        public string Name { get; }
        public bool IsIndestructible { get; }

        public bool IsDebugBeforeName { get; }
    }
}