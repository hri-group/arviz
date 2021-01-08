/*
Written by Steven Hoang 2021
*/

using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    // Subscriber Implemetation when a publisher with latched messages
    public class TFStaticSubscriber : UnitySubscriber<MessageTypes.Tf2.TFMessage>
    {
        private bool isMessageReceived;
        private List<MessageTypes.Geometry.TransformStamped> PublishedTransforms;
        protected override void Start()
        {
            base.Start();
            PublishedTransforms = new List<MessageTypes.Geometry.TransformStamped>();
            isMessageReceived = false;
        }
        protected override void ReceiveMessage(MessageTypes.Tf2.TFMessage message)
        {
            isMessageReceived = true;
            int msg_length = message.transforms.Length;
            for (int i = 0; i < msg_length; i++)
            {
                PublishedTransforms.Add(message.transforms[i]);
            }
            isMessageReceived = false;
        }
        public List<MessageTypes.Geometry.TransformStamped> GetPublishedTransforms()
        {
            if (!isMessageReceived)
            {
                return PublishedTransforms;
            }
            else
            {
                return null;
            }
        }
    }
}

