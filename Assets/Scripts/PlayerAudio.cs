using Manager;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    [SerializeField] private AudioClip _levelUp;
    [SerializeField] private AudioClip _lastLevelUp;
    
    [SerializeField] private AudioClip _confirmClip;
    [SerializeField] private AudioClip _cancelClip;
    
    private void Awake() {
        PlayerManager.Instance.OnLevelUp += o => {
            if (o >= PlayerManager.MAX_LEVEL) {
                AudioManager.Play(_lastLevelUp, Manager.AudioType.SFX);
            } else {
                AudioManager.Play(_levelUp, Manager.AudioType.SFX);
            }
        };

        var instance = StructureBuildManager.Instance;
        if (instance.ConfirmClip == null) {
            instance.ConfirmClip = _confirmClip;
        }

        if (instance.CancelClip == null) {
            instance.CancelClip = _cancelClip;
        }
    }
}