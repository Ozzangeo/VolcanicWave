using UnityEngine;

namespace Structure.Interface {
    public interface IConnectable {
        public LayerMask ConnectLayer { get; set; }

        public void Connect();
        public void Disconnect();

        public void ConnectAround();
    }
}