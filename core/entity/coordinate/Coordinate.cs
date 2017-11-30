﻿using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.file.entity;

namespace WorldWizards.core.entity.coordinate
{
    public class Coordinate
    {
        public Coordinate(IntVector3 index, Vector3 offset, int rotation)
        {
            this.index = index;
            this.offset = offset;
            this.rotation = rotation;
        }

        public Coordinate(CoordinateJSONBlob b) : this(
            new IntVector3(b.indexX, b.indexY, b.indexZ),
            new Vector3(b.offsetX, b.offsetY, b.offsetZ), b.rotation)
        {
        }

        public Coordinate(IntVector3 index) : this(index, Vector3.zero, 0)
        {
        }

        public Coordinate(IntVector3 index, int rotation) : this(index, Vector3.zero, rotation)
        {
        }


        public Coordinate(int x, int y, int z) : this(new IntVector3(x, y, z))
        {
        }

        public IntVector3 index { get; private set; }
        public Vector3 offset { get; set; } // (-1,1)

        public int rotation { get; set; } // y rotation

        public void SnapToGrid()
        {
            offset = Vector3.zero;
        }
    }
}