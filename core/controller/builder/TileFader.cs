﻿using System.Collections.Generic;
using UnityEngine;

namespace WorldWizards.core.controller.builder
{
    public class TileFader
    {
        private readonly Material[] fadeMatList;

        private readonly List<Material[]> meshMaterials;
        private readonly MeshRenderer[] meshRenderers;
        private readonly List<Material[]> skinnedMaterials;
        private readonly SkinnedMeshRenderer[] skinnedRenderers;

        public TileFader(GameObject gameObject)
        {
            fadeMatList = new Material[1];
            fadeMatList[0] = Resources.Load("Materials/TileFadeMat") as Material;
            ;
            meshMaterials = new List<Material[]>();
            skinnedMaterials = new List<Material[]>();

            meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mesh in meshRenderers)
                meshMaterials.Add(mesh.materials);
            skinnedRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer skin in skinnedRenderers)
                skinnedMaterials.Add(skin.materials);
        }

        public void On()
        {
            for (var i = 0; i < meshRenderers.Length; i++)
                meshRenderers[i].materials = meshMaterials[i];
            for (var i = 0; i < skinnedRenderers.Length; i++)
                skinnedRenderers[i].materials = skinnedMaterials[i];
        }

        public void Off()
        {
            for (var i = 0; i < meshRenderers.Length; i++)
                meshRenderers[i].materials = fadeMatList;
            for (var i = 0; i < skinnedRenderers.Length; i++)
                skinnedRenderers[i].materials = fadeMatList;
        }
    }
}