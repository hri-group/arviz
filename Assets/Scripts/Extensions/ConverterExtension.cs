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
    public static RosSharp.RosBridgeClient.MessageTypes.Geometry.Point unity2RosPointMsg(this Vector3 point)
    {
        return new RosSharp.RosBridgeClient.MessageTypes.Geometry.Point(point.x, point.y, point.z);
    }
    public static RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3 unity2RosVector3Msg(this Vector3 vector3)
    {
        return new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(vector3.x, vector3.y, vector3.z);
    }
    public static RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion unity2RosQuaternionMsg(this Quaternion quartenion)
    {
        return new RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion(quartenion.x, quartenion.y, quartenion.z, quartenion.w);
    }
}
