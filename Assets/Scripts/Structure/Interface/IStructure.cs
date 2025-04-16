using Interface;
using Resource.GameData;

namespace Structure.Interface {
    public interface IStructure : ISelectable, ILinkable {
        public StructureDirection Direction { get; set; }
    }
}