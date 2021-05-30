/*
 * Written by Steven Hoang 2021
 * VisualisationMarkerDisplay using MarkerArray message
 * Developer's Note: This visualisation tool can't handle marker with the same name but different type.
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
    // List of Marker tags
    const string markerArrowTag = "_Marker_Arrow";
    const string markerCubeTag = "_Marker_Cube";
    const string markerSphereTag = "_Marker_Sphere";
    const string markerCylinderTag = "_Marker_Cylinder";
    const string markerLineStripTag = "_Marker_LineStrip";
    const string markerLineListTag = "_Marker_LineList";
    const string markerCubeListTag = "_Marker_CubeList";
    const string markerSphereListTag = "_Marker_SphereList";
    const string markerPointsTag = "_Marker_Points";
    const string markerTextViewFacingTag = "_Marker_TextViewFacing";
    const string markerMeshResourceTag = "_Marker_MeshResource";
    const string markerTriangleListTag = "_Marker_TriangleList";
    string suffixSpacing = "_";
    string markerSuffix;

    [SerializeField]
    GameObject rosConnector;
    [SerializeField]
    GameObject arrowPrefab;
    [SerializeField]
    GameObject linePrefab;
    [SerializeField]
    GameObject pointPrefab;
    [SerializeField]
    GameObject cubePrefab;
    [SerializeField]
    GameObject spherePrefab;
    [SerializeField]
    GameObject cylinderPrefab;
    [SerializeField]
    GameObject textPrefab;
    RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker[] publishedMarkers;
    GameObject displayMarker;
    bool isExisted = false;
    WaitForSeconds updateInterval = new WaitForSeconds(0.075f);
    List<GameObject> renderredDisplayMarkers;
    List<GameObject> unusedDisplayMarkers;
    PointCloudManipulation modifier;
    LineRenderer lineRenderer;
    TextMeshPro textTool;
    GameObject target;
    // TF Data
    TFListener tfListener;
    List<GameObject> publishedTFTree;
    GameObject headerFrameObj;
    Transform hearderFrame;
    private void OnEnable()
    {
        publishedMarkers = new RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker[0];
        tfListener = GameObject.Find("TFListener").GetComponent<TFListener>();
        renderredDisplayMarkers = new List<GameObject>();
        unusedDisplayMarkers = new List<GameObject>();
        StartCoroutine(MarkersRenderer());
    }
    IEnumerator MarkersRenderer()
    {
        while (true)
        {
            publishedMarkers = rosConnector.GetComponent<MarkerArraySubscriber>().GetPublishedMarkers();
            if (publishedMarkers != null)
            {
                foreach (RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker marker in publishedMarkers)
                {
                    switch (marker.type)
                    {
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.ARROW:
                            markerSuffix = markerArrowTag;
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.CUBE:
                            markerSuffix = markerCubeTag;
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.SPHERE:
                            markerSuffix = markerSphereTag;
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.CYLINDER:
                            markerSuffix = markerCylinderTag;
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.LINE_STRIP:
                            markerSuffix = markerLineStripTag;
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.LINE_LIST:
                            markerSuffix = markerLineListTag;
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.CUBE_LIST:
                            markerSuffix = markerCubeListTag;
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.SPHERE_LIST:
                            markerSuffix = markerSphereListTag;
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.POINTS:
                            markerSuffix = markerPointsTag;
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.TEXT_VIEW_FACING:
                            markerSuffix = markerTextViewFacingTag;
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.MESH_RESOURCE:
                            markerSuffix = markerMeshResourceTag;
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.TRIANGLE_LIST:
                            markerSuffix = markerTriangleListTag;
                            break;
                        default:
                            break;
                    }
                    switch (marker.action)
                    {
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.ADD: // MODIFY and ADD share the same case
                            target = renderredDisplayMarkers.Find(savedMarker => savedMarker.name == marker.ns + marker.id + markerSuffix);
                            isExisted = target != null;
                            if (isExisted)
                            {
                                unusedDisplayMarkers = renderredDisplayMarkers.FindAll(savedMarker => savedMarker.name.Contains(marker.ns + marker.id + suffixSpacing));
                                foreach(GameObject unused in unusedDisplayMarkers)
                                {
                                    if (unused.name != target.name)
                                    {
                                        unused.SetActive(false);
                                    }
                                    else
                                    {
                                        unused.SetActive(true);
                                    }
                                }
                            }
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.DELETE:
                            target = renderredDisplayMarkers.Find(savedMarker => savedMarker.name == marker.ns + marker.id + markerSuffix);
                            if (target != null)
                            {
                                Destroy(target.gameObject);
                            }
                            renderredDisplayMarkers.Remove(target);
                            continue;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.DELETEALL:
                            foreach (Transform child in transform)
                            {
                                Destroy(child.gameObject);
                            }
                            renderredDisplayMarkers.RemoveAll(savedMarkers => savedMarkers == null);
                            continue;
                        default:
                            Debug.LogWarning("MarkerArray: Action type not found");
                            continue;
                    }
                    // Update the TFTree
                    publishedTFTree = tfListener.GetTFTree();
                    // Find the parent frame of the marker from the TFTree retrieved
                    if (publishedTFTree != null)
                    {
                        headerFrameObj = publishedTFTree.Find(frame => frame.name == marker.header.frame_id);
                        if (headerFrameObj != null)
                        {
                            hearderFrame = headerFrameObj.transform;
                            Debug.LogWarning("Used correct header frame");
                        }
                        else
                        {
                            hearderFrame = transform;
                            Debug.LogWarning("Could not find the parent frame. Using VisualisationMarkerDisplay frame instead");
                        }
                    }
                    else
                    {
                        hearderFrame = transform;
                        Debug.LogWarning("publishedTFTree not found. Using VisualisationMarkerDisplay frame instead");
                    }
                    switch (marker.type)
                    {
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.ARROW:
                            if (!isExisted)
                            {
                                displayMarker = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
                            }
                            else
                            {
                                displayMarker = target;
                            }
                            // Modify Arrow
                            displayMarker.transform.parent = hearderFrame;
                            displayMarker.name = marker.ns + marker.id + markerArrowTag;
                            displayMarker.GetComponent<ArrowManipulation>().SetArrow(marker);
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.CUBE:
                            if (!isExisted)
                            {
                                displayMarker = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
                            }
                            else
                            {
                                displayMarker = target;
                            }
                            // Modify cube
                            displayMarker.transform.parent = hearderFrame;
                            displayMarker.name = marker.ns + marker.id + markerCubeTag;
                            displayMarker.transform.localScale = marker.scale.rosMsg2Unity().Ros2UnityScale();
                            displayMarker.transform.localRotation = marker.pose.orientation.rosMsg2Unity().Ros2Unity();
                            displayMarker.transform.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                            displayMarker.GetComponent<MeshRenderer>().material.color = marker.color.rosMsg2Unity();
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.SPHERE:
                            if (!isExisted)
                            {
                                displayMarker = Instantiate(spherePrefab, Vector3.zero, Quaternion.identity);
                            }
                            else
                            {
                                displayMarker = target;
                            }
                            // Modify Sphere
                            displayMarker.transform.parent = transform;
                            displayMarker.name = marker.ns + marker.id + markerSphereTag;
                            displayMarker.transform.localScale = marker.scale.rosMsg2Unity().Ros2UnityScale();
                            displayMarker.transform.localRotation = marker.pose.orientation.rosMsg2Unity().Ros2Unity();
                            displayMarker.transform.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                            displayMarker.GetComponent<MeshRenderer>().material.color = marker.color.rosMsg2Unity();
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.CYLINDER:
                            if (!isExisted)
                            {
                                displayMarker = Instantiate(cylinderPrefab, Vector3.zero, Quaternion.identity);
                            }
                            else
                            {
                                displayMarker = target;
                            }
                            // Modify Cylinder
                            displayMarker.transform.parent = transform;
                            displayMarker.name = marker.ns + marker.id + markerCylinderTag;
                            displayMarker.transform.localScale = new Vector3((float)marker.scale.y, (float)marker.scale.z / 2, (float)marker.scale.x);
                            displayMarker.transform.localRotation = marker.pose.orientation.rosMsg2Unity().Ros2Unity();
                            displayMarker.transform.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                            displayMarker.GetComponent<MeshRenderer>().material.color = marker.color.rosMsg2Unity();
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.LINE_STRIP:
                            if (!isExisted)
                            {
                                displayMarker = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
                            }
                            else
                            {
                                displayMarker = target;
                            }
                            // Modify Line Strip
                            displayMarker.transform.parent = hearderFrame;
                            displayMarker.transform.localPosition = Vector3.zero;
                            displayMarker.transform.localRotation = Quaternion.identity;
                            displayMarker.name = marker.ns + marker.id + markerLineStripTag;
                            lineRenderer = displayMarker.GetComponent<LineRenderer>();
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
                            if (!isExisted)
                            {
                                displayMarker = Instantiate(pointPrefab, Vector3.zero, Quaternion.identity);
                            }
                            else
                            {
                                displayMarker = target;
                            }
                            // Modify Cube List
                            displayMarker.transform.parent = hearderFrame;
                            displayMarker.transform.localPosition = Vector3.zero;
                            displayMarker.transform.localRotation = Quaternion.identity;
                            displayMarker.name = marker.ns + marker.id + markerCubeListTag;
                            modifier = displayMarker.GetComponent<PointCloudManipulation>();
                            modifier.SetType(marker.type);
                            modifier.SetDimenstion(marker.scale);
                            modifier.SetColour(marker.color);
                            modifier.SetPoints(marker.points);
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.SPHERE_LIST:
                            if (!isExisted)
                            {
                                displayMarker = Instantiate(pointPrefab, Vector3.zero, Quaternion.identity);
                            }
                            else
                            {
                                displayMarker = target;
                            }
                            // Modify Sphere List
                            displayMarker.transform.parent = hearderFrame;
                            displayMarker.transform.localPosition = Vector3.zero;
                            displayMarker.name = marker.ns + marker.id + markerSphereListTag;
                            modifier = displayMarker.GetComponent<PointCloudManipulation>();
                            modifier.SetType(marker.type);
                            modifier.SetDimenstion(marker.scale);
                            modifier.SetColour(marker.color);
                            modifier.SetPoints(marker.points);
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.POINTS:
                            if (!isExisted)
                            {
                                displayMarker = Instantiate(pointPrefab, Vector3.zero, Quaternion.identity);
                            }
                            else
                            {
                                displayMarker = target;
                            }
                            // Modify Point List
                            displayMarker.transform.parent = hearderFrame;
                            displayMarker.transform.localPosition = Vector3.zero;
                            displayMarker.name = marker.ns + marker.id + markerPointsTag;
                            modifier = displayMarker.GetComponent<PointCloudManipulation>();
                            modifier.SetType(marker.type);
                            modifier.SetDimenstion(marker.scale);
                            modifier.SetColour(marker.color);
                            modifier.SetPoints(marker.points);
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.TEXT_VIEW_FACING:
                            if (!isExisted)
                            {
                                displayMarker = Instantiate(textPrefab, Vector3.zero, Quaternion.identity);
                            }
                            else
                            {
                                displayMarker = target;
                            }
                            // Modifiy Text
                            displayMarker.transform.parent = hearderFrame;
                            displayMarker.transform.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                            displayMarker.name = marker.ns + marker.id + markerTextViewFacingTag;
                            textTool = displayMarker.transform.GetChild(0).GetComponent<TextMeshPro>();
                            textTool.text = marker.text;
                            textTool.fontSize = (float)(14 * marker.scale.z);
                            textTool.color = marker.color.rosMsg2Unity();
                            displayMarker.GetComponent<SquareEffect>().SetSquareEffect(true);
                            break;
                        case RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.MESH_RESOURCE:
                            if (!isExisted)
                            {
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
                                displayMarker = (GameObject)Instantiate(Resources.Load(result), Vector3.zero, Quaternion.identity);
                            }
                            else
                            {
                                displayMarker = target;
                            }
                            // Modify the mesh
                            displayMarker.transform.parent = hearderFrame;
                            displayMarker.transform.localPosition = marker.pose.position.rosMsg2Unity().Ros2Unity();
                            displayMarker.transform.localRotation = marker.pose.orientation.rosMsg2Unity().Ros2Unity();
                            displayMarker.transform.localScale = marker.scale.rosMsg2Unity().Ros2UnityScale();
                            displayMarker.name = marker.ns + marker.id + markerMeshResourceTag;
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
                    displayMarker.transform.parent = transform;
                    if (!isExisted)
                    {
                        renderredDisplayMarkers.Add(displayMarker);
                    }
                }
            }
            yield return updateInterval;
        }
    }
}
