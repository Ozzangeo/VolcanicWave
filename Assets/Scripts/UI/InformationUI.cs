using Define;
using Manager;
using Structure;
using UI.Interface;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI {
    public class InformationUI : MonoBehaviour {
        [SerializeField] private GameObject _cursor;
        [SerializeField] private GameObject _group;
        [SerializeField] private Text _title;
        [SerializeField] private Text _description;

        [SerializeField] private Vector3 _clickPosition;
        private ISelectable _selectable;

        private void Awake() {
            _group.SetActive(false);
        }

        private void Update() {
            if (Input.GetMouseButtonDown(MouseButton.LEFT)) {
                var mouse_position = Input.mousePosition;

                var ground_point = CameraManager.ScreenRayToWorldPoint(mouse_position, LayerMasks.GroundMask);
                ground_point.y = 0.0f;

                _clickPosition = Vector3Int.RoundToInt(ground_point);

                var is_active = !_clickPosition.IsPositiveInfinityAny() && StructureBuildManager.Instance.Structure == null;
                _group.SetActive(is_active);
                _cursor.SetActive(is_active);

                _selectable = null;
                _cursor.transform.position = _clickPosition;
                if (Physics.Raycast(_clickPosition, Vector3.down, out var hit, 1.0f, LayerMasks.GroundMask)) {
                    if (hit.transform.TryGetComponent<GroundBehaviour>(out var ground)) {
                        if (ground.Structure is ISelectable selectable) {
                            _selectable = selectable;
                        }
                    }
                }
            }

            if (_selectable != null) {
                _title.text = _selectable.Title;
                _description.text = _selectable.Description;
            } else {
                _group.SetActive(false);
                _cursor.SetActive(false);
            }                
        }
    }
}