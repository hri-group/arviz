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
        base.OnClick(state, source);

        var imgtarg = GameObject.Find("ImageTarget");

        for (int i = 0; i < imgtarg.transform.childCount; i++)
        {
            if (imgtarg.transform.GetChild(i).CompareTag("MenuPanel"))
            {
                imgtarg.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        switch (source.transform.name)
        {
            case "TFButton":
                imgtarg.transform.Find("TFMenuPanel").gameObject.SetActive(true);
                break;
            case "TestButton":
                imgtarg.transform.Find("TestMenuPanel").gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
