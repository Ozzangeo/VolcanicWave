using Manager;
using UnityEngine;

namespace Structure.Renderer {
    public class FlowerRenderer : MonoBehaviour {
        [SerializeField] private FlowerStorageBehaviour _flower;

        [SerializeField] private GameObject[] _flowers;

        private void Awake() {
            PlayerManager.Instance.OnLevelUp += o => {
                foreach (var flower in _flowers) {
                    flower.SetActive(false);
                }

                _flowers[o].SetActive(true);
            };

            foreach (var flower in _flowers) {
                flower.SetActive(false);
            }

            _flowers[PlayerManager.Instance.Level].SetActive(true);
        }
    }
}