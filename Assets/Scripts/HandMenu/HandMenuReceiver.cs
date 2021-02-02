// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// Modified by Steven Lay 2021

using UnityEngine;
using UnityEngine.Events;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

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
                clearMenuPanels(menupanels);
                menupanels.transform.Find("TFMenuPanel").gameObject.SetActive(true);
                break;
            case "VizButton":
                clearMenuPanels(menupanels);
                menupanels.transform.Find("TestMenuPanel").gameObject.SetActive(true);
                break;
            case "PoseButton":
                break;
            case "PointButton":
                break;
            // Tools sub-menu
            case "TestButton":
                break;
            default:
                break;
        }
    }

    private void clearMenuPanels(GameObject imgt)
    {
        for (int i = 0; i < imgt.transform.Find("MenuPanels").transform.childCount; i++)
        {
            if (imgt.transform.GetChild(i).CompareTag("MenuPanel"))
            {
                imgt.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
