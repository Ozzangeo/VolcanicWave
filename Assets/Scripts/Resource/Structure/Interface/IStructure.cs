using Resource.GameData;
using Resource.Interface;

namespace Resource.Structure.Interface {
    public interface IStructure : ISelectable {
        public StructureDirection Direction { get; set; }
    }
}