// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// Modified by Steven Lay 2021

using UnityEngine;
using UnityEngine.Events;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using RosSharp;

public class HandMenuReceiver : ReceiverBase
{
    public override bool HideUnityEvents => true;

    private State lastState;

    public HandMenuReceiver(UnityEvent ev) : base(ev, "CustomEvent")
    {
    }

    public override void OnUpdate(InteractableStates state, Interactable source)
    {
        if (state.CurrentState() != lastState)
        {
            lastState = state.CurrentState();
        }
    }

    public override void OnClick(InteractableStates state, Interactable source, IMixedRealityPointer pointer = null)
    {
        GameObject handmenus = GameObject.Find("HandMenus");
        GameObject menupanels = GameObject.Find("MenuPanels");
        GameObject imagetarget = GameObject.Find("ImageTarget");
        base.OnClick(state, source);

        switch (source.transform.name)
        {
            // Main menu
            case "DisplaysButton":
                handmenus.transform.Find("MainMenu").gameObject.SetActive(false);
                handmenus.transform.Find("DisplaysMenu").gameObject.SetActive(true);
                break;
            case "ToolsButton":
                handmenus.transform.Find("MainMenu").gameObject.SetActive(false);
                handmenus.transform.Find("ToolsMenu").gameObject.SetActive(true);
                break;
            case "BackButton":
                handmenus.transform.Find("ToolsMenu").gameObject.SetActive(false);
                handmenus.transform.Find("DisplaysMenu").gameObject.SetActive(false);
                handmenus.transform.Find("MainMenu").gameObject.SetActive(true);
                break;
            // Displays sub-menu
            case "TFButton":
                if (menupanels.transform.Find("TFMenuPanel").gameObject.activeInHierarchy)
                {
                    clearMenuPanels(menupanels);
                }
                else
                {
                    clearMenuPanels(menupanels);
                    menupanels.transform.Find("Dragbar").gameObject.SetActive(true);
                    menupanels.transform.Find("TFMenuPanel").gameObject.SetActive(true);
                }
                break;
            case "VizButton":
                if (menupanels.transform.Find("VizMarkerMenuPanel").gameObject.activeInHierarchy)
                {
                    clearMenuPanels(menupanels);
                }
                else
                {
                    clearMenuPanels(menupanels);
                    menupanels.transform.Find("Dragbar").gameObject.SetActive(true);
                    menupanels.transform.Find("VizMarkerMenuPanel").gameObject.SetActive(true);
                }
                break;
            // Tools sub-menu
            case "Navigation2DButton":
                NavigationTool tool = imagetarget.transform.Find("GridDisplay").GetComponent<NavigationTool>();
                if (tool == null)
                    imagetarget.transform.Find("GridDisplay").gameObject.AddComponent(System.Type.GetType("NavigationTool"));               
                else 
                    imagetarget.transform.Find("GridDisplay").DestroyImmediateIfExists<NavigationTool>();          
                break;
            // Checkboxes
            case "TFCheckBox":
                if (source.GetComponent<Interactable>().IsToggled)
                    imagetarget.transform.Find("TFDisplay").gameObject.SetActive(true);
                else
                    imagetarget.transform.Find("TFDisplay").gameObject.SetActive(false);
                    
                break;
            
            case "VizCheckBox":
                if (source.GetComponent<Interactable>().IsToggled)
                    imagetarget.transform.Find("MarkerArrayDisplay").gameObject.SetActive(true);
                else
                    imagetarget.transform.Find("MarkerArrayDisplay").gameObject.SetActive(false);

                break;
            default:
                break;
        }
    }

    private void clearMenuPanels(GameObject imgt)
    {
        for (int i = 0; i < imgt.transform.childCount; i++)
        {
            if (imgt.transform.GetChild(i).CompareTag("MenuPanel"))
            {
                imgt.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
