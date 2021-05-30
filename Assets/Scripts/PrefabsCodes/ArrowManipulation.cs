using UnityEngine;
using RosSharp;
// Written by Steven Hoang 2021
public class ArrowManipulation : MonoBehaviour
{
    private Transform ArrowTail;
    private Transform ArrowTip;
    private Transform TipCap;
    private bool followCursor = false;
    private GameObject pointerPos;
    private GameObject referenceFrame;
    private void Start()
    {
        pointerPos = GameObject.Find("pointerPos");
        referenceFrame = GameObject.Find("/ImageTarget/GridDisplay");
    }
    private void Update()
    {
        if (followCursor)
        {
            Vector3 goal_point = referenceFrame.transform.InverseTransformPoint(pointerPos.transform.position);
            goal_point = new Vector3(goal_point.x, 0, goal_point.z);
            transform.LookAt(referenceFrame.transform.TransformPoint(goal_point));
        }
    }
    public void SetFollowCursor(bool arg)
    {
        followCursor = arg;
    }
    public void SetColor(Color tailColor, Color tipColor)
    {
        ArrowTail = transform.GetChild(0);
        ArrowTip = transform.GetChild(1);
        TipCap = transform.GetChild(2);
        ArrowTail.GetComponent<MeshRenderer>().material.color = tailColor;
        ArrowTip.GetComponent<MeshRenderer>().material.color = tipColor;
        TipCap.GetComponent<MeshRenderer>().material.color = tipColor;
    }
    // Specify tip and tail point of the arrow, arrow length will be calculated accordingly
    public void SetArrow(UnityEngine.Vector3 start, UnityEngine.Vector3 end)
    {
        ArrowTail = transform.GetChild(0);
        ArrowTip = transform.GetChild(1);
        TipCap = transform.GetChild(2); 
        transform.localPosition = start;
        float arrow_length = Vector3.Distance(start, end);
        float arrow_width = arrow_length / 20.0f; // Arrow Tail radius in metres
        if (arrow_width > 0.05f)
        {
            arrow_width = 0.05f; // Largest width
        }
        ArrowTail.localScale = new UnityEngine.Vector3(arrow_width, (arrow_length * 0.77f) / 2, arrow_width);
        ArrowTip.localScale = new UnityEngine.Vector3(arrow_width * 2f, arrow_width * 2f, arrow_length * 0.23f);
        TipCap.localScale = new UnityEngine.Vector3(arrow_width * 4f, 0.001f, arrow_width * 4f);
        ArrowTail.localPosition = new UnityEngine.Vector3(0, 0, (arrow_length * 0.77f) / 2);
        ArrowTip.localPosition = new UnityEngine.Vector3(0, 0, arrow_length);
        TipCap.localPosition = new UnityEngine.Vector3(0, 0, arrow_length * 0.77f);
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
        ArrowTail = transform.GetChild(0);
        ArrowTip = transform.GetChild(1);
        TipCap = transform.GetChild(2);
        if (arrow.points.Length == 2)
        {
            Vector3 start = new UnityEngine.Vector3((float)arrow.points[0].x, (float)arrow.points[0].y, (float)arrow.points[0].z).Ros2Unity();
            Vector3 end = new UnityEngine.Vector3((float)arrow.points[1].x, (float)arrow.points[1].y, (float)arrow.points[1].z).Ros2Unity();
            SetArrow(start, end);
            // This scale adjust code below can be commented out since the proportion is adjusted in SetArrow already, put here for code completion purposes
            ArrowTail.localScale = new UnityEngine.Vector3((float)arrow.scale.x, ArrowTail.transform.localScale.y, (float)arrow.scale.x);
            ArrowTip.localScale = new UnityEngine.Vector3((float)arrow.scale.y/2, (float)arrow.scale.y/2, ArrowTip.transform.localScale.z);
            TipCap.localScale = new UnityEngine.Vector3((float)arrow.scale.y, 0.001f, (float)arrow.scale.y);
            // Ignore the z scale (head length) here as the proprotion has been predetermined as it looks pretty good lol
        }
        else
        {
            // Arrow Size
            // Ref from Rviz source code
            // float shaft_length=0.77, float shaft_diameter=1, float head_length=0.23, float head_diameter=2;
            ArrowTail.localScale = new UnityEngine.Vector3((float)arrow.scale.y, ((float)arrow.scale.x * 0.77f) / 2, (float)arrow.scale.z);
            ArrowTip.localScale = new UnityEngine.Vector3((float)arrow.scale.y, (float)arrow.scale.z, (float)arrow.scale.x * 0.23f);
            TipCap.localScale = new UnityEngine.Vector3((float)arrow.scale.y * 2f, 0.001f, (float)arrow.scale.z * 2f);
            ArrowTail.localPosition = new UnityEngine.Vector3(0, 0, ((float)arrow.scale.x * 0.77f) / 2);
            ArrowTip.localPosition = new UnityEngine.Vector3(0, 0, (float)arrow.scale.x);
            TipCap.localPosition = new UnityEngine.Vector3(0, 0, (float)arrow.scale.x * 0.77f);

            // Arrow Position
            transform.localPosition = arrow.pose.position.rosMsg2Unity().Ros2Unity();
            // Arrow Rotation
            transform.localRotation = arrow.pose.orientation.rosMsg2Unity().Ros2Unity();
        }
        // Arrow Colour
        ArrowTail.GetComponent<MeshRenderer>().material.color = arrow.color.rosMsg2Unity();
        ArrowTip.GetComponent<MeshRenderer>().material.color = arrow.color.rosMsg2Unity();
        TipCap.GetComponent<MeshRenderer>().material.color = arrow.color.rosMsg2Unity();
    }
}
