using Manager;
using Resource.Infomation;
using Structure;
using UnityEngine;

namespace UI {
    public class PlayerStorageUI : MonoBehaviour {
        [SerializeField] private FlowerStorageBehaviour _flower;

        [SerializeField] private ElementSlider _waterSlider;
        [SerializeField] private ElementSlider _sunSlider;
        [SerializeField] private ElementSlider _dirtSlider;
        [SerializeField] private ElementSlider _expSlider;

        private void Awake() {
            if (_flower == null) {
                _flower = GameObject.FindAnyObjectByType<FlowerStorageBehaviour>();
            }
        }

        private void Update() {
            foreach (var storage in _flower.StorageData.storages) {
                switch (storage.target.Type) {
                    case ResourceType.Dirt:
                        _dirtSlider.SetValue(storage.count, storage.maxCount);
                        break;
                    case ResourceType.Sun:
                        _sunSlider.SetValue(storage.count, storage.maxCount);
                        break;
                    case ResourceType.Water:
                        _waterSlider.SetValue(storage.count, storage.maxCount);
                        break;
                }
            }

            _expSlider.SetValue(PlayerManager.Instance.Exp, PlayerManager.MAX_EXP);
        }
    }
}