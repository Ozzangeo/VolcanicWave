using Resource.GameData;
using UnityEngine;

namespace Structure.Renderer {
    public class ConveryorBeltRenderer : MonoBehaviour {
        public const int DIRECTION_ALL = (int)StructureDirection.Right + (int)StructureDirection.Up + (int)StructureDirection.Left + (int)StructureDirection.Down;

        [field: SerializeField] public ConveryorBelt ConveryorBelt { get; private set; }

        [Header("Require")]
        [SerializeField] private GameObject _end;
        [SerializeField] private GameObject _straight;
        [SerializeField] private GameObject _coner;
        [SerializeField] private GameObject _3ways;
        [SerializeField] private GameObject _4ways;

        [Header("Debug")]
        [SerializeField] private GameObject _display;

        private void Awake() {
            ConveryorBelt.OnDependencyChanged += OnDependencyChanged;
        }

        [ContextMenu("Update")]
        private void OnDependencyChanged() {
            if (ConveryorBelt == null) {
                return;
            }

            if (_display != null) {
                Destroy(_display);
                _display = null;

            }

            var previous_count = ConveryorBelt.Previous.Count;

            switch (previous_count) {
                case 0:
                    _display = Instantiate(_end, transform);

                    _display.transform.rotation = GetRotation(ConveryorBelt.Direction);

                    break;
                case 1:
                    var previous = ConveryorBelt.Previous[0];
                    if (previous.Direction == ConveryorBelt.Direction) {
                        _display = Instantiate(_straight, transform);

                        _display.transform.rotation = GetRotation(ConveryorBelt.Direction);
                    } else {
                        _display = Instantiate(_coner, transform);

                        _display.transform.rotation = GetRotation(ConveryorBelt.Direction, previous.Direction);
                    }

                    break;
                case 2:
                    _display = Instantiate(_3ways, transform);

                    var previous1 = ConveryorBelt.Previous[0];
                    var previous2 = ConveryorBelt.Previous[1];

                    _display.transform.rotation = GetRotation(ConveryorBelt.Direction, previous1.Direction, previous2.Direction);

                    break;
                case 3:
                    _display = Instantiate(_4ways, transform);

                    _display.transform.rotation = GetRotation(ConveryorBelt.Direction);

                    break;
            }
        }

        private static Quaternion GetRotation(int rotate) => Quaternion.Euler(90.0f, 0.0f, rotate * 90.0f);
        private static Quaternion GetRotation(StructureDirection direction) => GetRotation((int)direction);
        private static Quaternion GetRotation(StructureDirection direction, StructureDirection previous) {
            if (direction == StructureDirection.Down) {
                if (previous == StructureDirection.Right) {
                    return GetRotation(direction + 1);
                }
            }
            
            if (previous == StructureDirection.Down) {
                if (direction == StructureDirection.Right) {
                    return GetRotation(direction + 2);
                }
            }

            if (direction > previous) {
                return GetRotation(direction + 2);
            }

            return GetRotation(direction + 1);
        }
        private static Quaternion GetRotation(StructureDirection direction, StructureDirection previous1, StructureDirection previous2) {
            var other = (DIRECTION_ALL - direction - previous1 - previous2);

            return GetRotation(other + 1);
        }
    }
}