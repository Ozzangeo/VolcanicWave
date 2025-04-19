using Manager;
using UI.Information;
using UnityEngine;
using UnityEngine.UI;

public class StructureBuildSelectUI : MonoBehaviour {
    [field: SerializeField] public StructureSelectInfo Info { get; private set; }

    [field: Header("Require")]
    [field: SerializeField] public Image PreviewImage { get; private set; }
    [field: SerializeField] public Text Text { get; private set; }
    [field: SerializeField] public GameObject Highlight { get; private set; }

    private void Update() {
        PreviewImage.sprite = Info.Preview;
        Text.text = $"{Info.Structure.Info.Name} ({Info.Structure.Info.Price:###,###0})";

        Highlight.SetActive(StructureBuildManager.Instance.Structure == Info.Structure);

        foreach (var key in Info.SubShortKeys) {
            if (!Input.GetKey(key)) {
                return;
            }
        }

        if (Input.GetKeyDown(Info.ShortKey)) {
            Select();
        }
    }

    public void Select() {
        if (StructureBuildManager.Instance.Structure == Info.Structure) {
            StructureBuildManager.SetStructure(null);
        }
        else {
            StructureBuildManager.SetStructure(Info.Structure);
        }
    }

    private string GetShortKeyText() {
        string short_key_text = "";

        foreach (var sub_short_key in Info.SubShortKeys) {
            short_key_text += $"{sub_short_key} + ";
        }

        short_key_text += $"{Info.ShortKey}";

        return short_key_text;
    }
}
