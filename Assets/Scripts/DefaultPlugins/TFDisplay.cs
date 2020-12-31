using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using System.Threading;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp;

public class TFDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject tf_prefab;
    GameObject rosConnector;
    List<TransformStamped> tf_dynamic;
    List<TransformStamped> tf_static;
    List<string> frame_name;
    List<string> parent_name;
    List<RosSharp.RosBridgeClient.MessageTypes.Geometry.Transform> parent_to_child_tf;
    List<GameObject> tf_tree;

    // Start is called before the first frame update
    void Start()
    {
        rosConnector = GameObject.Find("ROS Connector");
        tf_dynamic = new List<TransformStamped>();
        tf_static = new List<TransformStamped>();
        frame_name = new List<string>();
        parent_name = new List<string>();
        parent_to_child_tf = new List<RosSharp.RosBridgeClient.MessageTypes.Geometry.Transform>();
        tf_tree = new List<GameObject>();
        var world_frame = Instantiate(tf_prefab, transform.position, transform.rotation);
        world_frame.name = "world_tf";
        world_frame.transform.parent = transform;
        world_frame.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
        world_frame.transform.localRotation = UnityEngine.Quaternion.identity;
        tf_tree.Add(world_frame);
    }
    private void Update()
    {
        // Update the list of TF frames
        tf_static = TFStaticSubscriber.transforms;
        tf_dynamic = TFSubscriber.transforms;

        frame_name.Clear();
        parent_name.Clear();
        parent_to_child_tf.Clear();

        foreach (TransformStamped parent_transform in tf_static)
        {
            frame_name.Add(parent_transform.child_frame_id + "_tf");
            parent_name.Add(parent_transform.header.frame_id + "_tf");
            parent_to_child_tf.Add(parent_transform.transform);
        }
        foreach (TransformStamped parent_transform in tf_dynamic)
        {
            frame_name.Add(parent_transform.child_frame_id + "_tf");
            parent_name.Add(parent_transform.header.frame_id + "_tf");
            parent_to_child_tf.Add(parent_transform.transform);
        }
        // Delete the TF frames that no longer exist in the newly received message
        foreach (GameObject frame in tf_tree)
        {
            if (frame)
            {
                // if the frame name is not found in the new list
                if (frame_name.IndexOf(frame.name) == -1)
                {
                    if (frame.name != "world_tf")
                    {
                        Destroy(frame);
                    }
                }
            }
        }
        tf_tree.RemoveAll(frame => frame == null);

        // Create TF frames that have not been added
        foreach (string new_frame in frame_name)
        {
            if (!tf_tree.Find(frame => frame.name == new_frame))
            {
                var tf_clone = Instantiate(tf_prefab, transform.position, UnityEngine.Quaternion.identity);
                tf_clone.name = new_frame;
                tf_clone.transform.parent = transform;
                tf_tree.Add(tf_clone);
            }
        }
        // Create the TF tree
        foreach (GameObject frame in tf_tree)
        {
            if (frame.name != "world_tf")
            {
                int parent_idx = frame_name.IndexOf(frame.name);
                frame.transform.parent = tf_tree.Find(t => t.name == parent_name[parent_idx]).transform;
                frame.transform.localPosition = parent_to_child_tf[parent_idx].translation.rosMsg2Unity().Ros2Unity();
                frame.transform.localRotation = parent_to_child_tf[parent_idx].rotation.rosMsg2Unity().Ros2Unity();
            }
        }
    }
}

