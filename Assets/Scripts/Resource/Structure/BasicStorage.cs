using Define;
using Resource.GameData;
using Resource.Infomation;
using Resource.Infomation.Structure;
using Resource.Structure.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace Resource.Structure {
    public class BasicStorage : BasicStructure, IResourceReceivable, ILinkable {
        public static readonly Vector3[] LinkDirections = new Vector3[] { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        [SerializeField] private StorageResource _tempStorage = null;
        [SerializeField] private StorageData _data = new();

        [field: SerializeField] public StorageInfo Info { get; protected set; } = new();
        public override StructureData Data => _data;

        [field: SerializeField] public List<BasicConveryorBelt> ConveryorBelts { get; protected set; } = new();

        private void Awake() {
            Link();
        }

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

        public void Link() {
            LinkClear();

            foreach (var direction in LinkDirections) {
                if (Physics.Raycast(transform.position, direction, out var hit, 1.0f, LayerMasks.StructureMask)) {
                    if (hit.transform.TryGetComponent<BasicConveryorBelt>(out var belt)) {
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
        public void LinkClear() {
            ConveryorBelts.Clear();
        }

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