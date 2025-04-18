using Structure.Infomation.Structure;
using System.Collections.Generic;

namespace Structure.GameData {
    [System.Serializable]
    public class StorageData : StructureData {
        public List<StorageResource> storages = new();
    }
}