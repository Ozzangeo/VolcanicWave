using UnityEngine;

namespace Structure.Information {
    [CreateAssetMenu(fileName = "Structure Info", menuName = "Structure/Info/Structure")]
    public class StructureInfo : ScriptableObject {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
    }
}