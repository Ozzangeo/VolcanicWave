using System;
using UnityEngine;

namespace Manager {
    public class PlayerManager : BasicManager<PlayerManager> {
        public const int MAX_LEVEL = 3;
        public const int MAX_EXP = 999_999;

        public override string Name => "Player Manager";

        public override bool IsIndestructible => false;

        [field: SerializeField] public int Level { get; set; } = 0;
        [field: SerializeField] public int Exp { get; set; } = 10000;

        public event Action<int> OnLevelUp;

        private void LevelUpLocal() {
            Level++;

            if (Level > MAX_LEVEL) {
                GameManager.GameEnding();

                Level = 0;
            }

            OnLevelUp?.Invoke(Level);
        }

        public static void LevelUp() => Instance.LevelUpLocal();
    }
}