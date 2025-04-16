using Manager;
using UnityEngine;

public class TestGroundManager : MonoBehaviour {
    public void Confirm() {
        GroundManager.ConfirmAll();
    }
    public void Cancel() {
        GroundManager.CancelAll();
    }
}
