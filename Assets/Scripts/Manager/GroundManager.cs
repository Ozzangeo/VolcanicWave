using Structure;
using System.Collections.Generic;
using UnityEngine;

namespace Manager {
    public class GroundManager : BasicManager<GroundManager> {
        public override string Name => "Ground Manager";

        [field: SerializeField] public List<GroundBehaviour> Grounds { get; set; } = new();

        private void RegisterGroundLocal(GroundBehaviour ground) {
            Grounds.Add(ground);
        }
        public static void RegisterGround(GroundBehaviour ground) => Instance.RegisterGroundLocal(ground);
    }
}