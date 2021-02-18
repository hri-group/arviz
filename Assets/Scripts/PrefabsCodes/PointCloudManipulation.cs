using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp;

// TODO: Reuse this for PointCloud
public class PointCloudManipulation : MonoBehaviour
{
    [SerializeField]
    Transform cubePrefab;
    [SerializeField]
    Transform spherePrefab;
    private Vector3 objectDimension;
    int objectType;
    Color objectColour;
    public void SetColour(RosSharp.RosBridgeClient.MessageTypes.Std.ColorRGBA colour)
    {
        objectColour = colour.rosMsg2Unity();
    }
    public void SetType(int markerType)
    {
        objectType = markerType;
    } 
    public void SetDimenstion(RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3 scale)
    {
        objectDimension = scale.rosMsg2Unity().Ros2UnityScale();
    }
    public void SetPoints(RosSharp.RosBridgeClient.MessageTypes.Geometry.Point[] points)
    {
        foreach(var point in points)
        {
            Transform InstatiatedObject = null;
            switch (objectType)
            {
                case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.POINTS:
                    InstatiatedObject = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
                    InstatiatedObject.localScale = objectDimension + new Vector3(0, 0, 0.001f); // Create thin square
                    InstatiatedObject.parent = transform;
                    InstatiatedObject.GetComponent<SquareEffect>().SetSquareEffect(true);
                    break;
                case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.CUBE_LIST:
                    InstatiatedObject = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
                    InstatiatedObject.localScale = objectDimension;
                    InstatiatedObject.parent = transform;
                    InstatiatedObject.GetComponent<SquareEffect>().SetSquareEffect(false);
                    break;
                case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.SPHERE_LIST:
                    InstatiatedObject = Instantiate(spherePrefab, Vector3.zero, Quaternion.identity);
                    InstatiatedObject.localScale = objectDimension;
                    InstatiatedObject.parent = transform;
                    break;
            }
            InstatiatedObject.GetComponent<MeshRenderer>().material.color = objectColour;
            InstatiatedObject.localPosition = point.rosMsg2Unity().Ros2Unity();
        }
    }
}
