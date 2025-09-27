using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class MapMetrics
    {
        public const float CoordinateUnitX = 2f;
        public const float CoordinateUnitY = 1f;

        public static Vector3 MapCoordinateToUnityWorldPos(Vector2Int mapCoordinate)
        {
            return new Vector3(mapCoordinate.x * CoordinateUnitX,
                mapCoordinate.y * CoordinateUnitY,
                0);
        }


    }

}