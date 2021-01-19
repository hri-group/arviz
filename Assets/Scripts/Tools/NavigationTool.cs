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
                    arrow_clone.transform.localPosition = new Vector3(arrow_clone.transform.localPosition.x, 0, arrow_clone.transform.localPosition.z);
                    arrow_clone.GetComponent<ArrowManipulation>().SetFollowCursor(true);
                }
                state = ORIENTATION_STATE;
                break;
            case ORIENTATION_STATE:
                RosSharp.RosBridgeClient.MessageTypes.Geometry.PoseStamped goal = new RosSharp.RosBridgeClient.MessageTypes.Geometry.PoseStamped();
                goal.header.Update();
                goal.header.frame_id = "map"; //Hardcoded as map for now, change later
                goal.pose = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose(
                                        arrow_clone.transform.localPosition.Unity2Ros().unity2RosPointMsg(),
                                        arrow_clone.transform.localRotation.Unity2Ros().unity2RosQuaternionMsg());
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
