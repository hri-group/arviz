using UnityEngine;
using RosSharp;

public class TestArrow : MonoBehaviour
{
    [SerializeField]
    Transform arrow_prefab;
    [SerializeField]
    Transform RosAxes;
    Transform test_arrow;
    bool done = false;
    GameObject sphere;
    RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker ros_arrow;
    // Start is called before the first frame update
    void Start()
    {
        test_arrow = Instantiate(arrow_prefab, new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
        test_arrow.transform.parent = transform;
        test_arrow.localPosition = new Vector3(0, 0, 0);
        Transform axes = Instantiate(RosAxes, new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
        axes.transform.parent = transform;
        axes.transform.localPosition = new Vector3(0, 0, 0);
        axes.transform.localRotation = UnityEngine.Quaternion.identity.Ros2Unity();
        ros_arrow = new RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker();
        ros_arrow.ns = "basic_shapes";
        ros_arrow.id = 0;
        ros_arrow.type = RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.ARROW;
        ros_arrow.action = RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker.ADD;
        ros_arrow.pose.position.x = -10.7f;
        ros_arrow.pose.position.y = -5.3f;
        ros_arrow.pose.position.z = 0.85f;
        ros_arrow.pose.orientation.x = 0;
        ros_arrow.pose.orientation.y = 0;
        ros_arrow.pose.orientation.z = -0.7f;
        ros_arrow.pose.orientation.w = -0.7f;
        ros_arrow.scale.x = 0.5;
        ros_arrow.scale.y = 0.5;
        ros_arrow.scale.z = 0.5;
        ros_arrow.color.a = 1;
        ros_arrow.color.g = 1;
    }
    // Update is called once per frame
    void Update()
    {
        test_arrow.GetComponent<ArrowManipulation>().SetArrow(ros_arrow);
    }
}