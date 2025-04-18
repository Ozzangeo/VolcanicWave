using Structure;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Information {
    [CreateAssetMenu(fileName = "Structure Select Info", menuName = "Custom/Structure Select Info")]
    public class StructureSelectInfo : ScriptableObject {
        [field: SerializeField] public Sprite Preview { get; private set; }
        [field: SerializeField] public StructureBehaviour Structure { get; private set; }

        [field: SerializeField] public KeyCode ShortKey { get; private set; }
        [field: SerializeField] public List<KeyCode> SubShortKeys { get; private set; } = new();
    }
}