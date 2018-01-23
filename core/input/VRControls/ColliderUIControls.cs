﻿using UnityEngine;
using UnityEngine.UI;
using worldWizards.core.input.Tools;
using worldWizards.core.input.VRControls;
using worldWizardsCore.core.input.Tools;
using worldWizardsCore.core.manager;
using WorldWizards.core.manager;

namespace worldWizardsCore.core.input.VRControls
{
    /**
     * ColliderUIControls is attached to the right hand controller to allow collision with the Arm Menu.
     * 
     */
    public class ColliderUIControls : MonoBehaviour {
    
        public GameObject popupArmMenu;
        private SteamVR_TrackedController controller;
        public Button objPlaceButton;
        public Button objEditButton;

        void Awake()
        {
            if (ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().GetMenuExists("PopupArmMenu"))
            {
                popupArmMenu = ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().GetMenuReference("PopupArmMenu");
            }
            
            controller = GetComponent<SteamVR_TrackedController>();

        }
    
        /**
         * Checks for collision with Arm Menu.
         * 
         */
        void OnTriggerEnter (Collider col)
        {
            Debug.Log("Collision");

            switch (col.gameObject.name)
            {
                case "OpenPopupButton":
                    if (popupArmMenu.activeSelf)
                    {
                        popupArmMenu.SetActive(false);
                    }
                    else
                    {
                        popupArmMenu.SetActive(true);
                    }
                    break;
                case "ObjPlaceButton":
                    OnClickObjectPlacement();
                    break;
                case "ObjEditButton":
                    OnClickObjectEdit();
                    break;
            }
        }

        /**
         * Called when the Object Placement button is hit.
         * Changes the tool on the controller to CreateObjectTool.
         */
        public void OnClickObjectPlacement()
        {
            controller.GetComponent<VRListener>().ChangeTool(typeof(CreateObjectTool));
            // TODO: Check that you actually have to select (maybe use click listener instead??)
            objPlaceButton.Select();
        }

        /**
         * Called when the Object Edit button is hit.
         * Changes the tool on the controller to EditObjectTool.
         */
        public void OnClickObjectEdit()
        {
            controller.GetComponent<VRListener>().ChangeTool(typeof(EditObjectTool));
            // TODO: Check that you actually have to select (maybe use click listener instead??)
            objEditButton.Select();
        }
    }
}
