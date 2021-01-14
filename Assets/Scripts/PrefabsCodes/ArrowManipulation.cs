using UnityEngine;
using RosSharp;
// Written by Steven Hoang 2021
public class ArrowManipulation : MonoBehaviour
{
    private GameObject ArrowTail;
    private GameObject ArrowTip;
    private GameObject TipCap;
    private void Start()
    {
        ArrowTail = GameObject.Find("Tail");
        ArrowTip = GameObject.Find("Cone");
        TipCap = GameObject.Find("ConeCap");
    }
    // Specify tip and tail point of the arrow, arrow length will be calculated accordingly
    public void SetArrow(UnityEngine.Vector3 start, UnityEngine.Vector3 end)
    {
        Start();
        transform.localPosition = start;
        float arrow_length = Vector3.Distance(start, end);
        float arrow_width = arrow_length / 20;
        ArrowTail.transform.localScale = new UnityEngine.Vector3(arrow_width, (arrow_length * 0.77f) / 2, arrow_width);
        ArrowTip.transform.localScale = new UnityEngine.Vector3(arrow_width * 2f, arrow_width * 2f, arrow_length * 0.23f);
        TipCap.transform.localScale = new UnityEngine.Vector3(arrow_width * 4f, 0.001f, arrow_width * 4f);
        ArrowTail.transform.localPosition = new UnityEngine.Vector3(0, 0, (arrow_length * 0.77f) / 2);
        ArrowTip.transform.localPosition = new UnityEngine.Vector3(0, 0, arrow_length);
        TipCap.transform.localPosition = new UnityEngine.Vector3(0, 0, arrow_length * 0.77f);
        if (transform.parent != null)
        {
            transform.LookAt(transform.parent.transform.TransformPoint(end));
        }
        else
        {
            // In the case where the arrow is created at the root of the scene, which rarely happens
            transform.LookAt(transform.TransformPoint(end));
        }
    }
    // Arrow with visualisation_msgs
    public void SetArrow(RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker arrow)
    {
        Start();
        if (arrow.points.Length == 2)
        {
            Vector3 start = new UnityEngine.Vector3((float)arrow.points[0].x, (float)arrow.points[0].y, (float)arrow.points[0].z).Ros2Unity();
            Vector3 end = new UnityEngine.Vector3((float)arrow.points[1].x, (float)arrow.points[1].y, (float)arrow.points[1].z).Ros2Unity();
            SetArrow(start, end);
            // This scale adjust code below can be commented out since the proportion is adjusted in SetArrow already, put here for code completion purposes
            ArrowTail.transform.localScale = new UnityEngine.Vector3((float)arrow.scale.x, ArrowTail.transform.localScale.y, (float)arrow.scale.x);
            ArrowTip.transform.localScale = new UnityEngine.Vector3((float)arrow.scale.y/2, (float)arrow.scale.y/2, ArrowTip.transform.localScale.z);
            TipCap.transform.localScale = new UnityEngine.Vector3((float)arrow.scale.y, 0.001f, (float)arrow.scale.y);
            // Ignore the z scale (head length) here as the proprotion has been predetermined as it looks pretty good lol
        }
        else
        {
            // Arrow Size
            // Ref from Rviz source code
            // float shaft_length=0.77, float shaft_diameter=1, float head_length=0.23, float head_diameter=2;
            ArrowTail.transform.localScale = new UnityEngine.Vector3((float)arrow.scale.y, ((float)arrow.scale.x * 0.77f) / 2, (float)arrow.scale.z);
            ArrowTip.transform.localScale = new UnityEngine.Vector3((float)arrow.scale.y, (float)arrow.scale.z, (float)arrow.scale.x * 0.23f);
            TipCap.transform.localScale = new UnityEngine.Vector3((float)arrow.scale.y * 2f, 0.001f, (float)arrow.scale.z * 2f);
            ArrowTail.transform.localPosition = new UnityEngine.Vector3(0, 0, ((float)arrow.scale.x * 0.77f) / 2);
            ArrowTip.transform.localPosition = new UnityEngine.Vector3(0, 0, (float)arrow.scale.x);
            TipCap.transform.localPosition = new UnityEngine.Vector3(0, 0, (float)arrow.scale.x * 0.77f);

            // Arrow Position
            transform.localPosition = arrow.pose.position.rosMsg2Unity().Ros2Unity();
            // Arrow Rotation
            transform.localRotation = arrow.pose.orientation.rosMsg2Unity().Ros2Unity();

            // Arrow Colour
            ArrowTail.GetComponent<MeshRenderer>().material.color = arrow.color.rosMsg2Unity();
            ArrowTip.GetComponent<MeshRenderer>().material.color = arrow.color.rosMsg2Unity();
            TipCap.GetComponent<MeshRenderer>().material.color = arrow.color.rosMsg2Unity();
        }
    }
}
