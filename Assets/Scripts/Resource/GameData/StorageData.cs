using Resource.Infomation.Structure;
using System.Collections.Generic;

namespace Resource.GameData {
    [System.Serializable]
    public class StorageData : StructureData {
        public List<StorageResource> storages = new();
    }
}