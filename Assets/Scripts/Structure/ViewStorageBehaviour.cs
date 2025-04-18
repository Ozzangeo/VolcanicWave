using Define;
using Resource;
using Structure.GameData;
using Structure.Information;
using Structure.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace Structure {
    public class ViewStorageBehaviour : StructureBehaviour, IResourceReceivable {
        [field: SerializeField] public StorageBehaviour Storage { get; private set; }

        public bool IsShared => Storage != null;

        public override string Description => Storage.Description;

        public override StructureInfo Info => Storage.Info;
        public override StructureData Data => Storage.Data;

        [field: SerializeField] public List<ConveryorBeltBehaviour> ReceiveConveryorBelt { get; protected set; } = new();

        public bool IsFull => Storage.IsFull;

        protected virtual void Awake() {
            Connect();
        }

        protected virtual void Update() {
            if (IsShared) {
                if (Storage.StorageInfo.IsSendable) {
                    foreach (var converyor_belt in ReceiveConveryorBelt) {
                        foreach (var storage in Storage.StorageData.storages) {
                            if (storage.count > 0) {
                                if (converyor_belt.IsResourceReceivable(storage.target)) {
                                    var resource = Instantiate(storage.target);

                                    converyor_belt.ReceiveResource(resource, Storage);

                                    storage.count--;

                                    Storage.OnSended(storage, converyor_belt);

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        #region Implement for IConnectable
        [ContextMenu("Connect")]
        public override void Connect() {
            Disconnect();

            foreach (var direction in ConnectAround4Ways) {
                if (Physics.Raycast(transform.position, direction, out var hit, 1.0f, ConnectLayer)) {
                    if (hit.transform.TryGetComponent<ConveryorBeltBehaviour>(out var belt)) {
                        var belt_direction = GetStructureDirectionVector(belt);

                        var distance = (belt.transform.position + belt_direction) - transform.position;
                        distance.y = 0.0f;

                        if (distance.magnitude > ConstValue.EPSILON) {
                            ReceiveConveryorBelt.Add(belt);

                            belt.Previous.Add(this);
                        }
                    }
                    else if (hit.transform.TryGetComponent<StorageBehaviour>(out var storage)) {

                    }
                }
            }
        }

        [ContextMenu("Disconnect")]
        public override void Disconnect() {
            foreach (var belt in ReceiveConveryorBelt) {
                belt.Previous.Remove(this);
            }

            ReceiveConveryorBelt.Clear();
        }
        #endregion

        public bool IsResourceReceivable(ResourceBehaviour resource) {
            if (IsShared) {
                return Storage.IsResourceReceivable(resource);
            }

            return false;
        }
        public void ReceiveResource(ResourceBehaviour resource, IStructure from = null) {
            if (IsShared) {
                Storage.ReceiveResource(resource, from);
            }
        }
    }
}