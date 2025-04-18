using Manager;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class StructureBuildUI : MonoBehaviour {
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Text _priceText;
        
        private void Update() {
            var exsit_blueprint = StructureBuildManager.Instance.BlueprintGrounds.Any() || StructureBuildManager.Instance.SelectGrounds.Any();

            _confirmButton.gameObject.SetActive(exsit_blueprint);
            _cancelButton.gameObject.SetActive(exsit_blueprint);
            _priceText.gameObject.SetActive(exsit_blueprint);

            _priceText.text = $"{StructureBuildManager.TotalBlueprintPrice + StructureBuildManager.TotalSelectPrice:###,###,###,###}";
        }

        public void Confirm() {
            StructureBuildManager.ConfirmAll();
        }
        public void Cancel() {
            StructureBuildManager.CancelAll();
        }
    }
}