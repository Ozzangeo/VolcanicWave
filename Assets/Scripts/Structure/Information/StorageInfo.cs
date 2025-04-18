using Structure.Infomation.Structure;
using System.Collections.Generic;
using UnityEngine;

namespace Structure.Information {
    [CreateAssetMenu(fileName = "Storage Info", menuName = "Structure/Info/Storage")]
    public class StorageInfo : StructureInfo {
        [field: SerializeField] public bool IsSendable { get; private set; } = true;
        [field: SerializeField] public bool IsReceivable { get; private set; } = true;

        [field: SerializeField] public List<StorageResource> InitStorages { get; private set; } = new();

    }
}