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
    Transform imageTargetTransform;
    GameObject TFDisplayObj;
    List<GameObject> PublishedTFTree;
    // Start is called before the first frame update
    void Start()
    {
        PublishedMarkers = new RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker[0];
        imageTargetTransform = transform.parent.transform;
        TFDisplayObj = GameObject.Find("TF Display");
        StartCoroutine(MarkersRenderer());
    }
    // Update is called once per frame
    IEnumerator MarkersRenderer()
    {
        while (true)
        {
            PublishedMarkers = rosConnector.GetComponent<MarkerArraySubscriber>().GetPublishedMarkers();
            if (PublishedMarkers != null)
            {
                // Update the TFTree
                PublishedTFTree = TFDisplayObj.GetComponent<TFDisplay>().GetTFTree();
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
                            foreach (Transform child in transform)
                            {
                                Destroy(child.gameObject);
                            }
                            continue;
                    }
                    // Find the parent frame of the marker from the TFTree retrieved
                    GameObject header_frame_obj = PublishedTFTree.Find(frame => frame.name == marker.header.frame_id + "_tf");
                    Transform header_frame;
                    if (header_frame_obj == null)
                    {
                        header_frame = transform;
                        Debug.LogWarning("Could not find the parent frame. Using VisualisationMarkerDisplay frame instead");
                    }
                    else
                    {
                        header_frame = header_frame_obj.transform;
                        Debug.LogWarning("Used correct header frame");
                    }
                    switch (marker.type)
                    {
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.ARROW:
                            DisplayMarker = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
                            DisplayMarker.parent = header_frame;
                            DisplayMarker.localPosition = Vector3.zero;
                            DisplayMarker.GetComponent<ArrowManipulation>().SetArrow(marker);
                            DisplayMarker.name = marker.ns + marker.id;
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.CUBE:
                            DisplayMarker = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
                            DisplayMarker.parent = header_frame;
                            DisplayMarker.localPosition = Vector3.zero;
                            DisplayMarker.name = marker.ns + marker.id;
                            // Modify cube
                            DisplayMarker.localScale = marker.scale.rosMsg2Unity().Ros2UnityScale();
                            DisplayMarker.localRotation = marker.pose.orientation.rosMsg2Unity().Ros2Unity();
                            DisplayMarker.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                            DisplayMarker.GetComponent<MeshRenderer>().material.color = marker.color.rosMsg2Unity();
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.SPHERE:
                            DisplayMarker = Instantiate(spherePrefab, Vector3.zero, Quaternion.identity);
                            DisplayMarker.parent = transform;
                            DisplayMarker.localPosition = Vector3.zero;
                            DisplayMarker.name = marker.ns + marker.id;
                            // Modify Sphere
                            DisplayMarker.localScale = marker.scale.rosMsg2Unity().Ros2UnityScale();
                            DisplayMarker.localRotation = marker.pose.orientation.rosMsg2Unity().Ros2Unity();
                            DisplayMarker.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                            DisplayMarker.GetComponent<MeshRenderer>().material.color = marker.color.rosMsg2Unity();
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.CYLINDER:
                            DisplayMarker = Instantiate(cylinderPrefab, Vector3.zero, Quaternion.identity);
                            DisplayMarker.parent = transform;
                            DisplayMarker.localPosition = Vector3.zero;
                            DisplayMarker.name = marker.ns + marker.id;
                            // Modify Cylinder
                            DisplayMarker.localScale = new Vector3((float)marker.scale.y, (float)marker.scale.z / 2, (float)marker.scale.x);
                            DisplayMarker.localRotation = marker.pose.orientation.rosMsg2Unity().Ros2Unity();
                            DisplayMarker.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                            DisplayMarker.GetComponent<MeshRenderer>().material.color = marker.color.rosMsg2Unity();
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.LINE_STRIP:
                            DisplayMarker = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
                            DisplayMarker.parent = header_frame;
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
                            DisplayMarker.parent = header_frame;
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
                            DisplayMarker.parent = header_frame;
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
                            DisplayMarker.parent = header_frame;
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
                            DisplayMarker.parent = header_frame;
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
                             * The mesh with its full directory must be placed in the Resources folder in Assets
                             * For example: if the mesh location is package://pr2_description/meshes/base_v0/base.dae
                             * Then the directory that base.dae needs to be in is Resources/pr2_description/meshes/base_v0
                            */
                            // Remove the package header in dir and extension
                            string toRemove = "package://";
                            string filePath = string.Empty;
                            int i = marker.mesh_resource.IndexOf(toRemove);
                            if (i >= 0)
                            {
                                filePath = marker.mesh_resource.Remove(i, toRemove.Length);
                            }
                            string result = System.IO.Path.ChangeExtension(filePath, null);
                            // Create the mesh
                            DisplayMarker = (Transform)Instantiate(Resources.Load(result), Vector3.zero, Quaternion.identity);
                            DisplayMarker.parent = header_frame;
                            DisplayMarker.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                            DisplayMarker.localRotation = marker.pose.orientation.rosMsg2Unity().Ros2Unity();
                            DisplayMarker.localScale = marker.scale.rosMsg2Unity().Ros2UnityScale();
                            DisplayMarker.name = marker.ns + marker.id;
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
                    // After performing the transformations, return the marker back to VisualisationMarker for easy management
                    DisplayMarker.parent = transform;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
