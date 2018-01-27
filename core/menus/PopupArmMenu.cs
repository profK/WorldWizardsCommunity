﻿using UnityEngine;
using WorldWizards.core.manager;

namespace WorldWizards.core.menus
{
    public class PopupArmMenu : WWMenu
    {
        private GameObject armMenu;
        
        private void Start()
        {
            Debug.Log("PopupArmMenu Start");

            armMenu = ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().GetMenuReference("ArmMenu");

            // TODO: Change this because it sucks
            if (armMenu)
            {
                Setup();
            }
        }

        protected override void Setup()
        {
            base.Setup();
            
            // Put the popup menu in the right place and parent it to the arm menu
            transform.rotation = transform.rotation;
            transform.position = transform.position;
            transform.Rotate(0, 0, 90);
            transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
            transform.parent = transform;
        }
    }
}