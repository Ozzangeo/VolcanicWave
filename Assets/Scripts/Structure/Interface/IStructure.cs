using Interface;
using Resource.GameData;

namespace Structure.Interface {
    public interface IStructure : ISelectable, IConnectable {
        public StructureDirection Direction { get; set; }
    }
}