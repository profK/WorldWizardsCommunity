﻿using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.coordinate.utils
{
    /// <summary>
    ///     A utility that converts the Unity Coordinate System to World
    ///     Wizards Coordinate System.
    /// </summary>
    public static class CoordinateHelper
    {
        public static float baseTileLength = 10f; // the base size of what a tile should be
        public static float tileLengthScale = 1f; // how much to scale up the base size

        /// <summary>
        ///     Gets the tile scale. Takes into account original scale of tiles.
        /// </summary>
        /// <returns>The tile scale.</returns>
        public static float GetTileScale()
        {
            return baseTileLength * tileLengthScale;
        }

        public static Coordinate ConvertUnityCoordinateToWWCoordinate(Vector3 coordinate)
        {
            return ConvertUnityCoordinateToWWCoordinate(coordinate, 0);
        }

        public static Coordinate ConvertUnityCoordinateToWWCoordinate(Vector3 coordinate, int rotation)
        {
            Vector3 full = coordinate / GetTileScale();
            Vector3 fraction = full - new Vector3(Mathf.Floor(full.x), Mathf.Floor(full.y), Mathf.Floor(full.z));

            // Convert the coordinate origin to be in the center of tile. Origin is in the middle of the Tile Cube.
            fraction -= new Vector3(0.5f, 0.5f, 0.5f); // convert to center
            fraction *= 2f; // convert to center

            var offset = new Vector3(fraction.x, -1, fraction.z);
            return new Coordinate(new IntVector3(full), offset, rotation);
        }

        public static Vector3 convertWWCoordinateToUnityCoordinate(Coordinate coordinate)
        {
            float offsetX = coordinate.offset.x * GetTileScale() * 0.5f;
            float offsetY = coordinate.offset.y * GetTileScale() * 0.5f;
            float offsetZ = coordinate.offset.z * GetTileScale() * 0.5f;
            var offset = new Vector3(offsetX, offsetY, offsetZ);
            Vector3 c = new Vector3(coordinate.index.x, coordinate.index.y, coordinate.index.z) * GetTileScale();
            return c + offset;
        }
    }
}