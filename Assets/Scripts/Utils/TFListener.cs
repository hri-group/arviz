using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using Microsoft.MixedReality.Toolkit.UI;
using RosSharp;
using TMPro;
using UnityEngine.Events;
using System.Linq;

public class TFListener : MonoBehaviour
{
    [SerializeField]
    GameObject rosConnector;

    List<TransformStamped> tfDynamic;
    List<TransformStamped> tfStatic;
    TFSubscriber tfDynamicListener;
    TFStaticSubscriber tfStaticListener;
    List<string> frameNames;
    List<string> parentNames;
    List<RosSharp.RosBridgeClient.MessageTypes.Geometry.Transform> parentToChildTF;
    List<GameObject> tfTree;
    bool isTFUpdating = false;
    WaitForSeconds updateInterval = new WaitForSeconds(0.02f);

    // Start is called before the first frame update
    void Start()
    {
        tfStaticListener = rosConnector.GetComponent<TFStaticSubscriber>();
        tfDynamicListener = rosConnector.GetComponent<TFSubscriber>();
        tfDynamic = new List<TransformStamped>();
        tfStatic = new List<TransformStamped>();
        frameNames = new List<string>();
        parentNames = new List<string>();
        parentToChildTF = new List<RosSharp.RosBridgeClient.MessageTypes.Geometry.Transform>();
        tfTree = new List<GameObject>();

        StartCoroutine(TFUpdate());
    }
    IEnumerator TFUpdate()
    {
        while (true)
        {
            // Update the list of TF frames
            tfStatic = tfStaticListener.GetPublishedTransforms();
            tfDynamic = tfDynamicListener.GetPublishedTransforms();
            if (tfDynamic != null && tfStatic != null)
            {
                isTFUpdating = true;
                frameNames.Clear();
                parentNames.Clear();
                parentToChildTF.Clear();

                foreach (TransformStamped parent_transform in tfStatic.ToList())
                {
                    frameNames.Add(parent_transform.child_frame_id);
                    parentNames.Add(parent_transform.header.frame_id);
                    parentToChildTF.Add(parent_transform.transform);
                }
                foreach (TransformStamped parent_transform in tfDynamic.ToList())
                {
                    frameNames.Add(parent_transform.child_frame_id);
                    parentNames.Add(parent_transform.header.frame_id);
                    parentToChildTF.Add(parent_transform.transform);
                }
                // Delete the TF frames that no longer exist in the newly received list
                foreach (GameObject frame in tfTree)
                {
                    if (frame)
                    {
                        // if the frame name is not found in the new list of frames and parent frames
                        if (frameNames.IndexOf(frame.name) == -1 && parentNames.IndexOf(frame.name) == -1)
                        {
                            Destroy(frame);
                        }
                    }
                }
                tfTree.RemoveAll(frame => frame == null);
                // Create TF frames that have not been added
                foreach (string new_frame in frameNames)
                {
                    if (!tfTree.Find(frame => frame.name == new_frame))
                    {
                        var tfClone = new GameObject();
                        tfClone.name = new_frame;
                        tfClone.transform.parent = transform;
                        tfTree.Add(tfClone);
                    }
                }
                // Loop through the parent frames to create frames that are not in the frame list
                foreach (string parent_frame in parentNames)
                {
                    if (!tfTree.Find(frame => frame.name == parent_frame))
                    {
                        var tfClone = new GameObject();
                        tfClone.name = parent_frame;
                        tfClone.transform.parent = transform;
                        tfClone.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
                        tfClone.transform.localRotation = new UnityEngine.Quaternion(0, 0, 0, 1);
                        tfTree.Add(tfClone);
                    }
                }
                // Create the TF tree
                foreach (GameObject frame in tfTree)
                {
                    int parentIdx = frameNames.IndexOf(frame.name);
                    // If the parent is found, then setParent appropriately, otherwise, just set parent to TFDisplay
                    if (parentIdx > -1)
                    {
                        frame.transform.parent = tfTree.Find(t => t.name == parentNames[parentIdx]).transform;
                        frame.transform.localPosition = parentToChildTF[parentIdx].translation.rosMsg2Unity().Ros2Unity();
                        frame.transform.localRotation = parentToChildTF[parentIdx].rotation.rosMsg2Unity().Ros2Unity();
                    }
                }
                isTFUpdating = false;
            }
            yield return updateInterval;
        }
    }
    public List<GameObject> GetTFTree()
    {
        if (isTFUpdating)
        {
            return null;
        }
        else
        {
            return tfTree.ToList();
        }
    }
}

