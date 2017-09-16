﻿using System;
using worldWizards.core.entity.gameObject;

namespace worldWizards.core.entity.common
{
    /// <summary>
    /// A enumeration of all possible World Wizards object types.
    /// </summary>
    public enum WWType {Tile, Prop, Interactable};

    /// <summary>
    /// A helper class containing a number of static functions to help with using the WWType enum.
    /// </summary>
    public static class WWTypeHelper
    {
        public static Type ConvertToSysType(WWType type)
        {
            switch (type)
            {
                case WWType.Interactable:
                    return typeof(Interactable);
                case WWType.Prop:
                    return typeof(Prop);
                case WWType.Tile:
                    return typeof(Tile);
                default:
                    return typeof(Tile);
            }
        }
    }
}
