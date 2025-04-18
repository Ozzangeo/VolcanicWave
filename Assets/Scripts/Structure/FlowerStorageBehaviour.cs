using Manager;
using Structure.Infomation.Structure;
using Structure.Information;
using Structure.Interface;
using UnityEngine;

namespace Structure {
    public class FlowerStorageBehaviour : StorageBehaviour {
        [SerializeField] protected StorageInfo[] _infos;

        protected override void Awake() {
            _info = _infos[PlayerManager.Instance.Level];
            Initialize();

            PlayerManager.Instance.OnLevelUp += o => {
                _info = _infos[PlayerManager.Instance.Level];
                Initialize();
            };
        }

        protected override void Update() {
            base.Update();

            if (IsFull) {
                PlayerManager.LevelUp();
            }
        }

        public override void OnReceived(StorageResource storage, IStructure from = null) {
            PlayerManager.Instance.Exp = Mathf.Clamp(PlayerManager.Instance.Exp + Info.Price, 0, PlayerManager.MAX_EXP);
        }
    }
}