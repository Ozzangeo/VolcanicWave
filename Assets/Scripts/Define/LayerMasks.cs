using UnityEngine;

namespace Define {
    public static class LayerMasks {
        public static readonly int StructureMask = LayerMask.GetMask(LayerNames.STRUCTURE_LAYER);
        public static readonly int ResourceMask = LayerMask.GetMask(LayerNames.RESOURCE_LAYER);
        public static readonly int GroundMask = LayerMask.GetMask(LayerNames.GROUND_LAYER);
        public static readonly int BlueprintMask = LayerMask.GetMask(LayerNames.BLUEPRINT_LAYER);
    }
}