using UnityEngine;

namespace Define {
    public static class LayerIndexs {
        public static readonly int StructureLayer    = LayerMask.NameToLayer(LayerNames.STRUCTURE_LAYER);
        public static readonly int ResourceLayer     = LayerMask.NameToLayer(LayerNames.RESOURCE_LAYER);
        public static readonly int GroundLayer       = LayerMask.NameToLayer(LayerNames.GROUND_LAYER);
        public static readonly int BlueprintLayer    = LayerMask.NameToLayer(LayerNames.BLUEPRINT_LAYER);
    }
}