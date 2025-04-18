using Structure.GameData;
using UI.Interface;
using UnityEngine;

namespace Structure.Interface {
    public interface IStructure : ISelectable, IConnectable {
        public StructureDirection Direction { get; set; }
        public Vector3 Position { get; set; }
    }
}