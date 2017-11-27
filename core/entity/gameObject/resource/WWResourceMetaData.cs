﻿using System;
using UnityEngine;
using UnityEditor;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.gameObject.resource
{
    [Serializable]
    public class WWResourceMetaData : MonoBehaviour
    {
        public WWObjectMetaData wwObjectMetaData;
//        public WWType type;
        public WWTileMetaData wwTileMetaData;
        public WWDoor door;
    }
    
    
    /// <summary>
    /// This class is a UI helper that makes it easier for artists to setup the doors in the Unity Inspector.
    /// </summary>
    [CustomEditor(typeof(WWResourceMetaData))]
    public class WWResourceMetaDataEditor : Editor
    {
        private static readonly string NORTH = "North";
        private static readonly string EAST = "East";
        private static readonly string SOUTH = "South";
        private static readonly string WEST = "West";
        private static readonly string TOP = "Top";
        private static readonly string BOTTOM = "Bottom";
        
        private static GameObject pivot;
        private static GameObject x1;
        private static GameObject x2;
        private static GameObject y;
        private static GameObject facingDirection;
        
        public override void OnInspectorGUI()
        {
            WWResourceMetaData script = target as WWResourceMetaData;
            script.wwObjectMetaData.type = (WWType)EditorGUILayout.EnumPopup("Asset Type", script.wwObjectMetaData.type);
           
            if (script.wwObjectMetaData.type == WWType.Tile)
            {
                DisplayCollisionsProperties(script);
                DisplayDoorHolderProperties(script.wwTileMetaData.northWwDoorHolder, NORTH, script.wwObjectMetaData.baseTileSize);
                DisplayDoorHolderProperties(script.wwTileMetaData.eastWwDoorHolder, EAST, script.wwObjectMetaData.baseTileSize);
                DisplayDoorHolderProperties(script.wwTileMetaData.southWwDoorHolder, SOUTH, script.wwObjectMetaData.baseTileSize);
                DisplayDoorHolderProperties(script.wwTileMetaData.westWwDoorHolder, WEST, script.wwObjectMetaData.baseTileSize);
            }
            else if (script.wwObjectMetaData.type == WWType.Door)
            {
               DisplayDoorProperties(script.door, script.wwObjectMetaData.baseTileSize);
            }
        }

        private void DisplayCollisionsProperties(WWResourceMetaData script )
        {
            GUILayout.Label("Collisions");
            DisplayWWCollisions(script);
        }

        private void DisplayWWCollisions(WWResourceMetaData script)
        {
            EditorGUILayout.BeginHorizontal();
            script.wwTileMetaData.wwOccupiedWalls.north = GUILayout.Toggle(
                script.wwTileMetaData.wwOccupiedWalls.north, "North");
            script.wwTileMetaData.wwOccupiedWalls.east = GUILayout.Toggle(
                script.wwTileMetaData.wwOccupiedWalls.east, "East");
            script.wwTileMetaData.wwOccupiedWalls.south = GUILayout.Toggle(
                script.wwTileMetaData.wwOccupiedWalls.south, "South");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            script.wwTileMetaData.wwOccupiedWalls.west = GUILayout.Toggle(
                script.wwTileMetaData.wwOccupiedWalls.west, "West");
            script.wwTileMetaData.wwOccupiedWalls.top = GUILayout.Toggle(
                script.wwTileMetaData.wwOccupiedWalls.top, "Top");
            script.wwTileMetaData.wwOccupiedWalls.bottom = GUILayout.Toggle(
                script.wwTileMetaData.wwOccupiedWalls.bottom, "Bottom");
            EditorGUILayout.EndHorizontal();            
        }

        private void DisplayDoorProperties(WWDoor door, int baseTileSize)
        {
            door.pivot = EditorGUILayout.Vector3Field("Door Pivot", door.pivot);
            door.facingDirection = EditorGUILayout.Vector3Field("Door Facing Direction", door.facingDirection);
            EditorGUILayout.BeginHorizontal();
            door.width = EditorGUILayout.FloatField("Door width", door.width);
            door.height = EditorGUILayout.FloatField("Door Height", door.height);
            EditorGUILayout.EndHorizontal();            
            
            door.openAnimation = EditorGUILayout.ObjectField("Open Animation",
                door.openAnimation, typeof(Animation), false) as Animation;
            door.closeAnimation = EditorGUILayout.ObjectField("Close Animation",
                door.closeAnimation, typeof(Animation), false) as Animation;
            if (GUILayout.Button("Create Helpers"))
            {
                CreateDoorHelpers();
            }
            if (GUILayout.Button("Get Door Dimensions and Pivot"))
            {
                GetDoorDimensions(door, baseTileSize);
            }
        }

        private void DisplayDoorHolderProperties(WWDoorHolder wwDoorHolder, String direction, int baseTileSize)
        {
            wwDoorHolder.hasDoorHolder = GUILayout.Toggle(
                wwDoorHolder.hasDoorHolder, string.Format(" Has {0} Door Holder", direction));
            if (wwDoorHolder.hasDoorHolder)
            {
                wwDoorHolder.pivot = EditorGUILayout.Vector3Field(
                    string.Format("{0} Door Pivot", direction), wwDoorHolder.pivot);
                wwDoorHolder.width = EditorGUILayout.FloatField(
                    string.Format("{0} Door Width", direction), wwDoorHolder.width);
                wwDoorHolder.height = EditorGUILayout.FloatField(
                    string.Format("{0} Door Height", direction), wwDoorHolder.height);
                if (GUILayout.Button("Create Helpers"))
                {
                    CreateDoorHolderHelpers(direction);
                }
                if (GUILayout.Button(direction +  "Get Door Dimensions and Pivot"))
                {
                    GetDoorHolderDimensions(wwDoorHolder, direction, baseTileSize);
                }
            }
        }

         private void CreateDoorHelpers()
         {
            WWResourceMetaData wwResourceMetaDataScript = target as WWResourceMetaData;
            Vector3 spawnPos = wwResourceMetaDataScript.transform.position;
            Vector3 widthOffset = Vector3.zero;
            var extents = CalculateLocalBounds(wwResourceMetaDataScript.gameObject);   
            spawnPos.y -= wwResourceMetaDataScript.wwObjectMetaData.baseTileSize * 0.5f;
          
            if (extents.z > extents.x)
            {
             spawnPos.x += wwResourceMetaDataScript.wwObjectMetaData.baseTileSize * 0.5f;
             widthOffset = Vector3.forward;
            }
            else
            {
             spawnPos.z += wwResourceMetaDataScript.wwObjectMetaData.baseTileSize * 0.5f;
             widthOffset = Vector3.right;
            }

             if (pivot != null)
            {
               DestroyImmediate(pivot);
            }
            if (x1 != null)    
            {
                DestroyImmediate(x1);
            }
            if (x2 != null)
            {
               DestroyImmediate(x2);
            }
            if (y != null)
            {
                DestroyImmediate(y);
            }
            if (facingDirection != null)
            {
                DestroyImmediate(facingDirection);
            }
            
            pivot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pivot.name = "pivot";
            pivot.transform.position = spawnPos;
            x1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            x1.name = "x1";
            x1.transform.position = spawnPos + (widthOffset * 0.3f * wwResourceMetaDataScript.wwObjectMetaData.baseTileSize);
            x2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            x2.name = "x2";
            x2.transform.position = spawnPos + (widthOffset * 0.3f * wwResourceMetaDataScript.wwObjectMetaData.baseTileSize * -1);
            y = GameObject.CreatePrimitive(PrimitiveType.Cube);
            y.name = "y";
            y.transform.position = new Vector3(spawnPos.x,
                spawnPos.y +wwResourceMetaDataScript.wwObjectMetaData.baseTileSize * 0.75f,
                spawnPos.z);
            facingDirection = GameObject.CreatePrimitive(PrimitiveType.Cube);
            facingDirection.name = "facing direction";
            facingDirection.transform.position = spawnPos + Vector3.forward;
        }
        
        private void CreateDoorHolderHelpers(string direction)
        {
            WWResourceMetaData wwResourceMetaDataScript = target as WWResourceMetaData;
            Vector3 spawnPos = wwResourceMetaDataScript.transform.position;
            Vector3 widthOffset = Vector3.zero;
            
            spawnPos.y -= wwResourceMetaDataScript.wwObjectMetaData.baseTileSize * 0.5f;
            if (direction.Equals(NORTH))
            {
                spawnPos.z += wwResourceMetaDataScript.wwObjectMetaData.baseTileSize * 0.5f;
                widthOffset = Vector3.right;
            }
            else if (direction.Equals(EAST))
            {
                spawnPos.x +=  wwResourceMetaDataScript.wwObjectMetaData.baseTileSize * 0.5f;
                widthOffset = Vector3.fwd;
            }
            else if (direction.Equals(SOUTH))
            {
                spawnPos.z -=  wwResourceMetaDataScript.wwObjectMetaData.baseTileSize * 0.5f;
                widthOffset = Vector3.right;
            }
            else if (direction.Equals(WEST))
            {
                spawnPos.x -=  wwResourceMetaDataScript.wwObjectMetaData.baseTileSize * 0.5f;
                widthOffset = Vector3.fwd;
            }
            if (pivot != null)
            {
               DestroyImmediate(pivot);
            }
            if (x1 != null)    
            {
                DestroyImmediate(x1);
            }
            if (x2 != null)
            {
               DestroyImmediate(x2);
            }
            if (y != null)
            {
                DestroyImmediate(y);
            }
            pivot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pivot.name = "pivot";
            pivot.transform.position = spawnPos;
            x1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            x1.name = "x1";
            x1.transform.position = spawnPos + (widthOffset * 0.3f * wwResourceMetaDataScript.wwObjectMetaData.baseTileSize);
            x2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            x2.name = "x2";
            x2.transform.position = spawnPos + (widthOffset * 0.3f * wwResourceMetaDataScript.wwObjectMetaData.baseTileSize * -1);
            y = GameObject.CreatePrimitive(PrimitiveType.Cube);
            y.name = "y";
            y.transform.position = new Vector3(spawnPos.x,
                spawnPos.y +wwResourceMetaDataScript.wwObjectMetaData.baseTileSize * 0.75f,
                spawnPos.z);
        }

        private void GetDoorHolderDimensions(WWDoorHolder wwDoorHolder, string direction, int baseTileSize)
        {
            // return if any of the helpers are null
            if (x1 == null || x2 == null || y == null || pivot == null)
            {
                Debug.Log("helpers are null, exiting early.");
                return;
            }
            var width = Vector3.Distance(x1.transform.position, x2.transform.position) / baseTileSize;
            var height = Math.Abs(y.transform.position.y - x1.transform.position.y) / baseTileSize;
            wwDoorHolder.width = width ;
            wwDoorHolder.height = height;
            wwDoorHolder.pivot = pivot.transform.position / baseTileSize;
            DestroyImmediate(pivot);
            DestroyImmediate(x1);
            DestroyImmediate(x2);
            DestroyImmediate(y);
        }
        
        private void GetDoorDimensions(WWDoor door, int baseTileSize)
        {
            // return if any of the helpers are null
            if (x1 == null || x2 == null || y == null || pivot == null || facingDirection == null)
            {
                Debug.Log("helpers are null, exiting early.");
                return;
            }
            var width = Vector3.Distance(x1.transform.position, x2.transform.position) / baseTileSize;
            var height = Math.Abs(y.transform.position.y - x1.transform.position.y) / baseTileSize;
            door.width = width ;
            door.height = height;
            door.pivot = pivot.transform.position / baseTileSize;
            door.facingDirection = (facingDirection.transform.position - pivot.transform.position).normalized;
            DestroyImmediate(pivot);
            DestroyImmediate(x1);
            DestroyImmediate(x2);
            DestroyImmediate(y);
            DestroyImmediate(facingDirection);
        }

        private Vector3 CalculateLocalBounds(GameObject gameObject)
        {
            Bounds bounds = new Bounds(gameObject.transform.position, Vector3.zero);
 
            foreach(Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(renderer.bounds);
            }
 
            Vector3 localCenter = bounds.center - gameObject.transform.position;
            bounds.center = localCenter;
            
            return new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);
        }
        
    }
}