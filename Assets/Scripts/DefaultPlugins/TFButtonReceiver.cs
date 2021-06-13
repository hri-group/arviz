// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// Modified by Steven Lay 2021

using UnityEngine;
using UnityEngine.Events;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections.Generic;
using System.Linq;

public class TFButtonReceiver : ReceiverBase
{
    public override bool HideUnityEvents => true;

    private State lastState;

    public TFButtonReceiver() : this(new UnityEvent())
    {
    }

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
        List<GameObject> tfAxesAndName = GameObject.FindGameObjectsWithTag("TF").ToList();
        List<GameObject> tfArrow = GameObject.FindGameObjectsWithTag("TFArrow").ToList();

        base.OnClick(state, source);

        if (source.transform.name == "ShowNameCheckbox")
        {
            foreach (var tf in tfAxesAndName)
            {
                if (tf.transform.GetChild(1).gameObject.activeInHierarchy)
                    tf.transform.GetChild(1).gameObject.SetActive(false);
                else
                    tf.transform.GetChild(1).gameObject.SetActive(true);
            }
        } 
        else if (source.transform.name == "ShowTFCheckbox")
        {
            foreach (var tf in tfAxesAndName)
            {
                if (tf.transform.GetChild(0).gameObject.activeInHierarchy)
                    tf.transform.GetChild(0).gameObject.SetActive(false);
                else
                    tf.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else if (source.transform.name == "ShowArrowCheckbox")
        {
            foreach (var tf in tfArrow)
            {
                if (tf.transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    tf.transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    tf.transform.GetChild(0).gameObject.SetActive(true);
                }
                if (tf.transform.GetChild(1).gameObject.activeInHierarchy)
                {
                    tf.transform.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    tf.transform.GetChild(1).gameObject.SetActive(true);
                }
                if (tf.transform.GetChild(2).gameObject.activeInHierarchy)
                {
                    tf.transform.GetChild(2).gameObject.SetActive(false);
                }
                else
                {
                    tf.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            string tf_name = source.transform.name.Remove(source.transform.name.Length - 9);
            GameObject target_tf = tfAxesAndName.Find(res => res.name == tf_name + "_tf");
            GameObject target_arrow = tfArrow.Find(res => res.name == tf_name + "_arrow");
            target_tf.transform.GetChild(1).gameObject.SetActive(!target_tf.transform.GetChild(1).gameObject.activeInHierarchy);
            target_tf.transform.GetChild(0).gameObject.SetActive(!target_tf.transform.GetChild(0).gameObject.activeInHierarchy);
            target_arrow.transform.GetChild(0).gameObject.SetActive(!target_arrow.transform.GetChild(0).gameObject.activeInHierarchy);
            target_arrow.transform.GetChild(1).gameObject.SetActive(!target_arrow.transform.GetChild(1).gameObject.activeInHierarchy);
            target_arrow.transform.GetChild(2).gameObject.SetActive(!target_arrow.transform.GetChild(2).gameObject.activeInHierarchy);
        }
    }
}
