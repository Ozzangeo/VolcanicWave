using Resource.Infomation;
using UnityEngine;

namespace Structure.Information {
    [CreateAssetMenu(fileName = "Converyor Belt Info", menuName = "Structure/Info/Converyor Belt")]
    public class ConveryorBeltInfo : StructureInfo {
        [field: SerializeField] public ResourceType Type { get; private set; } = ResourceType.None;
        [field: SerializeField] public float Speed { get; private set; } = 1.0f;
        [field: SerializeField] public float MaxWeight { get; private set; } = 30.0f;
    }
}