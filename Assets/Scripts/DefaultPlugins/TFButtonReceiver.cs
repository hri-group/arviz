// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// Modified by Steven Lay 2021

using UnityEngine;
using UnityEngine.Events;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

public class TFButtonReceiver : ReceiverBase
{
    public override bool HideUnityEvents => true;

    private State lastState;

    public TFButtonReceiver(UnityEvent ev) : base(ev, "CustomEvent")
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
        GameObject[] TFobj = GameObject.FindGameObjectsWithTag("TF");

        base.OnClick(state, source);

        if (source.transform.name == "ShowNameCheckbox")
        {
            foreach (var tf in TFobj)
            {
                if (tf.transform.GetChild(1).localScale.x != 0)
                    tf.transform.GetChild(1).localScale = new Vector3(0, 0, 0);
                else
                    tf.transform.GetChild(1).localScale = new Vector3(1, 1, 1);
            }   
        } 
        else if (source.transform.name == "ShowTFCheckbox")
        {
            foreach (var tf in TFobj)
            {
                if (tf.transform.GetChild(0).localScale.x != 0)
                    tf.transform.GetChild(0).localScale = new Vector3(0, 0, 0);
                else
                    tf.transform.GetChild(0).localScale = new Vector3(1, 1, 1);
            }
        } 
        else
        {
            string tf_name = source.transform.name.Remove(source.transform.name.Length - 9);
            GameObject target_tf = GameObject.Find(tf_name);

            target_tf.transform.GetChild(1).gameObject.SetActive(!target_tf.transform.GetChild(1).gameObject.activeInHierarchy);
            target_tf.transform.GetChild(0).gameObject.SetActive(!target_tf.transform.GetChild(0).gameObject.activeInHierarchy);
        }
    }
}
