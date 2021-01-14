using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConverterExtension
{
    // ROS message converter
    public static UnityEngine.Vector3 rosMsg2Unity(this RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3 position)
    {
        return new UnityEngine.Vector3((float)position.x, (float)position.y, (float)position.z);
    }
    public static UnityEngine.Vector3 rosMsg2Unity(this RosSharp.RosBridgeClient.MessageTypes.Geometry.Point point)
    {
        return new UnityEngine.Vector3((float)point.x, (float)point.y, (float)point.z);
    }
    public static UnityEngine.Quaternion rosMsg2Unity(this RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion quaternion)
    {
        return new UnityEngine.Quaternion((float)quaternion.x, (float)quaternion.y, (float)quaternion.z, (float)quaternion.w);
    }
    public static UnityEngine.Color rosMsg2Unity(this RosSharp.RosBridgeClient.MessageTypes.Std.ColorRGBA colour)
    {
        return new Color(colour.r, colour.g, colour.b, colour.a);
    }
}
