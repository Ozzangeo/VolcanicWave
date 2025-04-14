using UnityEngine;

namespace Define {
    public static class LayerMasks {
        public static readonly int ConveryorBeltMask = LayerMask.GetMask(LayerNames.CONVERYOR_BELT_LAYER);
        public static readonly int ResourceMask = LayerMask.GetMask(LayerNames.RESOURCE_LAYER);
        public static readonly int GroundMask = LayerMask.GetMask(LayerNames.GROUND_LAYER);
    }
}