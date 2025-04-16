using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace UnityEditor.Brush {
    [CreateAssetMenu(fileName = "Prefab Brush", menuName = "Brushes/Prefab Brush")]
    [CustomGridBrush(false, true, false, "Prefab Brush")]
    public class PrefabBrush : GameObjectBrush {
        private static bool TryGetGameObjectInCell(GridLayout grid_layout, Transform parent, Vector3Int position, out GameObject game_object) {
            game_object = null;

            var min = grid_layout.LocalToWorld(grid_layout.CellToLocalInterpolated(position));
            var max = grid_layout.LocalToWorld(grid_layout.CellToLocalInterpolated(position + Vector3Int.one));
            
            var bounds = new Bounds((max + min) * 0.5f, max - min);
            
            int child_count = parent.childCount;
            for (int i = 0; i < child_count; ++i) {
                var child = parent.GetChild(i);

                if (bounds.Contains(child.position)) {
                    game_object = child.gameObject;

                    return true;
                }
            }

            return false;
        }

        public override void Erase(GridLayout grid_layout, GameObject brush_target, Vector3Int position) {
            if (brush_target.layer == 31) {
                return;
            }

            position.z = 0;
            if (TryGetGameObjectInCell(grid_layout, brush_target.transform, position, out var erased)) {
                Undo.DestroyObjectImmediate(erased);
            }
        }
    }
}