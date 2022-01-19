using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class PoseSubscriber : UnitySubscriber<MessageTypes.Geometry.Pose>
    {
        protected override void Start()
        {
            base.Start();
        }
        protected override void ReceiveMessage(MessageTypes.Geometry.Pose message)
        {
            throw new System.NotImplementedException();
        }
    }

}
