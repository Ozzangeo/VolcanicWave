using Define;
using Resource;
using Resource.Infomation;
using Structure.GameData;
using Structure.Infomation.Structure;
using Structure.Information;
using Structure.Interface;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Structure {
    public class StorageBehaviour : StructureBehaviour, IResourceReceivable {
        [SerializeField] protected StorageResource _currentStorage = null;
        
        [SerializeField] protected StorageInfo _info;
        [SerializeField] protected StorageData _data = new();

        public override string Description {
            get {
                string storages = "";

                foreach (var storage in _data.storages) {
                    var count = SharedStorages.Sum(o => o._data.storages.FirstOrDefault(x => x.target.Type == storage.target.Type).count) + storage.count;
                    var max_count = SharedStorages.Sum(o => o._data.storages.FirstOrDefault(x => x.target.Type == storage.target.Type).maxCount) + storage.maxCount;

                    storages += $"{storage.target.Info.name} [{count:###,###,###,###0}/{max_count:###,###,###,###0}], ";
                }

                return $"적재된 자원: {storages[..^2]}";
            }
        }

        public StorageInfo StorageInfo => _info;
        public StorageData StorageData => _data;

        public override StructureInfo Info => _info;
        public override StructureData Data => _data;

        [field: SerializeField] public List<ConveryorBeltBehaviour> ReceiveConveryorBelt { get; protected set; } = new();
        [field: SerializeField] public List<StorageBehaviour> SharedStorages { get; protected set; } = new();

        public bool IsFull => _data.storages.All(o => !o.IsInsertable);

        protected void Initialize() {
            _data.storages.Clear();
            foreach (var storage in _info.InitStorages) {
                _data.storages.Add(storage.Copy());
            }
        }

        protected virtual void Awake() {
            Initialize();

            Connect();
        }

        protected virtual void Update() {
            SharedStorages = SharedStorages.Where(o => o != null).ToList();

            if (_info.IsSendable) {
                foreach (var converyor_belt in ReceiveConveryorBelt) {
                    foreach (var storage in _data.storages) {
                        if (storage.IsEmpty) {
                            foreach(var shared in SharedStorages) {
                                var shared_storages = shared._data.storages.Where(o => o.target.Type == storage.target.Type && !o.IsEmpty);

                                bool is_inserted = false;
                                foreach (var shared_storage in shared_storages) {
                                    storage.count++;
                                    shared_storage.count--;

                                    is_inserted = true;

                                    break;
                                }

                                if (is_inserted) {
                                    break;
                                }
                            }
                        }

                        if (!storage.IsEmpty) {
                            if (converyor_belt.IsResourceReceivable(storage.target)) {
                                var resource = Instantiate(storage.target);

                                converyor_belt.ReceiveResource(resource, this);

                                storage.count--;

                                OnSended(storage, converyor_belt);

                                break;
                            }
                        }
                    }
                }
            }

        }

        public void Share(StorageBehaviour storage) {
            var added_storages = new List<StorageBehaviour>();

            if (!SharedStorages.Contains(storage) && storage != this) {
                SharedStorages.Add(storage);

                added_storages.Add(storage);
            }

            foreach (var shared_storage in storage.SharedStorages) {
                if (!SharedStorages.Contains(shared_storage) && shared_storage != this) {
                    SharedStorages.Add(shared_storage);

                    added_storages.Add(shared_storage);
                }
            }

            foreach (var shared_storage in added_storages) {
                shared_storage.Share(this);
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
                    else if (hit.transform.TryGetComponent<StorageBehaviour>(out var storage_behaviour)) {
                        if (SharedStorages.Contains(storage_behaviour)) {
                            continue;
                        }

                        if (!StorageData.storages.All(o => storage_behaviour.StorageData.storages.Any(x => o.target.Type == x.target.Type))) {
                            continue;
                        }

                        Share(storage_behaviour);
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
            if (!_info.IsReceivable) {
                return false; 
            }

            foreach (var storage in _data.storages) {
                if (storage.target.Type == resource.Type) {
                    if (!storage.IsInsertable) {
                        continue;
                    }

                    _currentStorage = storage;
                    
                    return true;
                }
            }

            return false;
        }
        public void ReceiveResource(ResourceBehaviour resource, IStructure from = null) {
            _currentStorage.count++;

            resource.Release();

            OnReceived(_currentStorage, from);
        }

        public virtual void OnSended(StorageResource storage, ConveryorBeltBehaviour converyor_belt) {}
        public virtual void OnReceived(StorageResource storage, IStructure from = null) {}
    }
}