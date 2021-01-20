/*
 * Written by Steven Hoang 2021
 * VisualisationMarkerDisplay using MarkerArray message
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using RosSharp;
using System.Linq;
using TMPro;

public class VisualisationMarkersDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject rosConnector;
    [SerializeField]
    Transform arrowPrefab;
    [SerializeField]
    Transform linePrefab;
    [SerializeField]
    Transform pointPrefab;
    [SerializeField]
    Transform cubePrefab;
    [SerializeField]
    Transform spherePrefab;
    [SerializeField]
    Transform cylinderPrefab;
    [SerializeField]
    Transform textPrefab;
    RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker[] PublishedMarkers;
    Transform DisplayMarker;
    PointCloudManipulation modifier;
    LineRenderer lineRenderer;
    TextMeshPro textTool;
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        PublishedMarkers = new RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker[0];
    }
    // Update is called once per frame
    void Update()
    {
        PublishedMarkers = rosConnector.GetComponent<MarkerArraySubscriber>().GetPublishedMarkers();
        if (PublishedMarkers != null)
        {
            foreach (RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker marker in PublishedMarkers)
            {
                switch (marker.action)
                {
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.ADD: // MODIFY and ADD share the same case
                        target = transform.Find(marker.ns + marker.id);
                        if (target != null)
                        {
                            Destroy(target.gameObject);
                        }
                        break;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.DELETE:
                        target = transform.Find(marker.ns + marker.id);
                        if (target != null)
                        {
                            Destroy(target.gameObject);
                        }
                        continue;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.DELETEALL:
                        foreach(Transform child in transform)
                        {
                            Destroy(child.gameObject);
                        }
                        continue;
                }
                // Assume the reference frame is always going to be the world frame. TODO: fix this so that it could be any frame
                // This property is reflected in header.frame_id;
                switch (marker.type)
                {
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.ARROW:
                        DisplayMarker = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
                        DisplayMarker.parent = transform; 
                        DisplayMarker.localPosition = Vector3.zero;
                        DisplayMarker.GetComponent<ArrowManipulation>().SetArrow(marker);
                        DisplayMarker.name = marker.ns + marker.id;
                        break;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.CUBE:
                        DisplayMarker = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
                        DisplayMarker.parent = transform;
                        DisplayMarker.localPosition = Vector3.zero;
                        DisplayMarker.name = marker.ns + marker.id;
                        // Modify cube
                        DisplayMarker.localScale = marker.scale.rosMsg2Unity();
                        DisplayMarker.rotation = marker.pose.orientation.rosMsg2Unity().Ros2Unity();
                        DisplayMarker.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                        DisplayMarker.GetComponent<MeshRenderer>().material.color = marker.color.rosMsg2Unity();
                        break;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.SPHERE:
                        DisplayMarker = Instantiate(spherePrefab, Vector3.zero, Quaternion.identity);
                        DisplayMarker.parent = transform;
                        DisplayMarker.localPosition = Vector3.zero;
                        DisplayMarker.name = marker.ns + marker.id;
                        // Modify Sphere
                        DisplayMarker.localScale = marker.scale.rosMsg2Unity();
                        DisplayMarker.rotation = marker.pose.orientation.rosMsg2Unity().Ros2Unity();
                        DisplayMarker.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                        DisplayMarker.GetComponent<MeshRenderer>().material.color = marker.color.rosMsg2Unity();
                        break;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.CYLINDER:
                        DisplayMarker = Instantiate(cylinderPrefab, Vector3.zero, Quaternion.identity);
                        DisplayMarker.parent = transform;
                        DisplayMarker.localPosition = Vector3.zero;
                        DisplayMarker.name = marker.ns + marker.id;
                        // Modify Cylinder
                        DisplayMarker.transform.localScale = new Vector3((float)marker.scale.y, (float)marker.scale.z / 2, (float)marker.scale.x);
                        DisplayMarker.transform.rotation = marker.pose.orientation.rosMsg2Unity().Ros2Unity();
                        DisplayMarker.transform.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                        DisplayMarker.GetComponent<MeshRenderer>().material.color = marker.color.rosMsg2Unity();
                        break;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.LINE_STRIP:
                        DisplayMarker = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
                        DisplayMarker.parent = transform;
                        DisplayMarker.localPosition = Vector3.zero;
                        DisplayMarker.name = marker.ns + marker.id;
                        // Modify Line Strip
                        lineRenderer = DisplayMarker.GetComponent<LineRenderer>();
                        lineRenderer.material.color = marker.color.rosMsg2Unity();
                        lineRenderer.widthMultiplier = (float)marker.scale.x;
                        lineRenderer.useWorldSpace = false;
                        List<Vector3> vertices = new List<Vector3>();
                        foreach (RosSharp.RosBridgeClient.MessageTypes.Geometry.Point point in marker.points)
                        {
                            vertices.Add(point.rosMsg2Unity().Ros2Unity());
                        }
                        lineRenderer.positionCount = vertices.Count;
                        lineRenderer.SetPositions(vertices.ToArray());
                        break;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.LINE_LIST:
                        /*
                         * =================================================================================
                         * =================================================================================
                         * ===========================YOUR CODE GOES HERE===================================
                         * =================================================================================
                         * =================================================================================
                         */
                        break;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.CUBE_LIST:
                        DisplayMarker = Instantiate(pointPrefab, Vector3.zero, Quaternion.identity);
                        DisplayMarker.parent = transform;
                        DisplayMarker.localPosition = Vector3.zero;
                        DisplayMarker.name = marker.ns + marker.id;
                        // Modify Cube List
                        modifier = DisplayMarker.GetComponent<PointCloudManipulation>();
                        modifier.SetType(marker.type);
                        modifier.SetDimenstion(marker.scale);
                        modifier.SetColour(marker.color);
                        modifier.SetPoints(marker.points);
                        break;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.SPHERE_LIST:
                        DisplayMarker = Instantiate(pointPrefab, Vector3.zero, Quaternion.identity);
                        DisplayMarker.parent = transform;
                        DisplayMarker.localPosition = Vector3.zero;
                        DisplayMarker.name = marker.ns + marker.id;
                        // Modify Sphere List
                        modifier = DisplayMarker.GetComponent<PointCloudManipulation>();
                        modifier.SetType(marker.type);
                        modifier.SetDimenstion(marker.scale);
                        modifier.SetColour(marker.color);
                        modifier.SetPoints(marker.points);
                        break;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.POINTS:
                        DisplayMarker = Instantiate(pointPrefab, Vector3.zero, Quaternion.identity);
                        DisplayMarker.parent = transform;
                        DisplayMarker.localPosition = Vector3.zero;
                        DisplayMarker.name = marker.ns + marker.id;
                        // Modify Point List
                        modifier = DisplayMarker.GetComponent<PointCloudManipulation>();
                        modifier.SetType(marker.type);
                        modifier.SetDimenstion(marker.scale);
                        modifier.SetColour(marker.color);
                        modifier.SetPoints(marker.points);
                        break;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.TEXT_VIEW_FACING:
                        DisplayMarker = Instantiate(textPrefab, Vector3.zero, Quaternion.identity);
                        DisplayMarker.parent = transform;
                        DisplayMarker.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                        DisplayMarker.name = marker.ns + marker.id;
                        // Modifiy Text
                        textTool = DisplayMarker.GetChild(0).GetComponent<TextMeshPro>();
                        textTool.text = marker.text;
                        textTool.fontSize = (float)(14 * marker.scale.z);
                        textTool.color = marker.color.rosMsg2Unity();
                        DisplayMarker.GetComponent<SquareEffect>().SetSquareEffect(true);
                        break;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.MESH_RESOURCE:
                        /*
                         * =================================================================================
                         * =================================================================================
                         * ===========================YOUR CODE GOES HERE===================================
                         * =================================================================================
                         * =================================================================================
                         */
                        break;
                    case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.TRIANGLE_LIST:
                        /*
                         * =================================================================================
                         * =================================================================================
                         * ===========================YOUR CODE GOES HERE===================================
                         * =================================================================================
                         * =================================================================================
                         */
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
