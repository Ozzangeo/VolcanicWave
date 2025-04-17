using Define;
using Resource;
using Resource.GameData;
using Resource.Infomation.Structure;
using Structure.Infomation;
using Structure.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace Structure {
    public class StorageBehaviour : StructureBehaviour, IResourceReceivable {
        [SerializeField] private StorageResource _tempStorage = null;
        [SerializeField] private StorageData _data = new();

        [field: SerializeField] public StorageInfo Info { get; protected set; } = new();
        public override StructureData Data => _data;

        [field: SerializeField] public List<ConveryorBelt> ConveryorBelts { get; protected set; } = new();

        private void Update() {
            foreach (var converyor_belt in ConveryorBelts) { 
                foreach (var storage in _data.storages) {
                    if (storage.count > 0) {
                        if (converyor_belt.IsResourceReceivable(storage.target)) {
                            var resource = Instantiate(storage.target);

                            Direction = VectorToDirection(transform.position, converyor_belt.transform.position);

                            converyor_belt.ReceiveResource(resource, this);

                            storage.count--;

                            break;
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
                if (Physics.Raycast(transform.position, direction, out var hit, 1.0f, LayerMasks.StructureMask)) {
                    if (hit.transform.TryGetComponent<ConveryorBelt>(out var belt)) {
                        var belt_direction = GetStructureDirectionVector(belt);

                        var distance = (belt.transform.position + belt_direction) - transform.position;
                        distance.y = 0.0f;

                        if (distance.magnitude > ConstValue.EPSILON) {
                            ConveryorBelts.Add(belt);
                        }
                    }
                }
            }
        }
        [ContextMenu("Disconnect")]
        public override void Disconnect() {
            ConveryorBelts.Clear();
        }
        #endregion

        public bool IsResourceReceivable(BasicResource resource) {
            if (!Info.isReceivable) {
                return false; 
            }

            foreach (var storage in _data.storages) {
                if (storage.target.Type == resource.Type) {
                    _tempStorage = storage;

                    return true;
                }
            }

            return false;
        }
        public void ReceiveResource(BasicResource resource, IStructure from = null) {
            _tempStorage.count++;

            resource.Release();
        }
    }
}