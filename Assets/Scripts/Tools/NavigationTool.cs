/*
 * Written by Steven Hoang 2021
 * Navigation Tool, mimics what has been offered in Rviz, using arrow to set a Pose for the robot to navigate to
 */
using Microsoft.MixedReality.Toolkit.Input;
using RosSharp;
using RosSharp.RosBridgeClient;
using UnityEngine;

public class NavigationTool : MonoBehaviour, IMixedRealityPointerHandler
{
    public GameObject PrefabToSpawn;
    const int POSITION_STATE = 0;
    const int ORIENTATION_STATE = 1;
    private int state = 0;
    private GameObject arrow_clone;
    [SerializeField]
    private GameObject referenceFrame;
    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        switch (state)
        {
            case POSITION_STATE:
                if (PrefabToSpawn != null)
                {
                    var result = eventData.Pointer.Result;
                    arrow_clone = Instantiate(PrefabToSpawn, Vector3.zero, Quaternion.identity);
                    arrow_clone.GetComponent<ArrowManipulation>().SetArrow(Vector3.zero, Vector3.forward);
                    arrow_clone.transform.parent = transform.parent; // Parent of GridDisplay aka ImageTarget
                    arrow_clone.transform.position = result.Details.Point;
                    arrow_clone.GetComponent<ArrowManipulation>().SetFollowCursor(true);
                }
                state = ORIENTATION_STATE;
                break;
            case ORIENTATION_STATE:
                RosSharp.RosBridgeClient.MessageTypes.Geometry.PoseStamped goal = new RosSharp.RosBridgeClient.MessageTypes.Geometry.PoseStamped();
                goal.header.Update();
                goal.header.frame_id = "map"; //Hardcoded as map for now, change later
                Debug.Log(referenceFrame.transform.InverseTransformPoint(arrow_clone.transform.position).Unity2Ros());
                Debug.Log(arrow_clone.transform.rotation.Unity2Ros().unity2RosQuaternionMsg());
                RosSharp.RosBridgeClient.MessageTypes.Geometry.Point position = referenceFrame.transform.InverseTransformPoint(arrow_clone.transform.position).Unity2Ros().unity2RosPointMsg();
                position.z = 0;
                RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion orientation = (arrow_clone.transform.rotation*Quaternion.Inverse(GameObject.Find("ImageTarget").transform.localRotation)).Unity2Ros().unity2RosQuaternionMsg();
                orientation.x = 0;
                orientation.y = 0;
                orientation.z = 0;
                orientation.w = 1;
                goal.pose = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose(position, orientation);
                GameObject.Find("/ROS Connector").GetComponent<NavigationPublisher>().SendGoal(goal);
                Destroy(arrow_clone);
                state = POSITION_STATE;
                break;
        }
    }
    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {

    }
    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        
    }
    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
    }
}
