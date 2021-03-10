/*
Written by Steven Hoang 2021
*/

using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TFSubscriber : UnitySubscriber<MessageTypes.Tf2.TFMessage>
    {
        private List<MessageTypes.Geometry.TransformStamped> PublishedTransforms;
        private MessageTypes.Geometry.TransformStamped[] ReceivedTransforms;
        [SerializeField]
        public int TFTimeOutInSeconds = 5;
        private uint currTime;
        private bool isMessageReceived;
        protected override void Start()
        {
            base.Start();
            PublishedTransforms = new List<MessageTypes.Geometry.TransformStamped>();
            PublishedTransforms.Add(new MessageTypes.Geometry.TransformStamped()); // Add a dummy variable to start the loop
            ReceivedTransforms = new MessageTypes.Geometry.TransformStamped[0];
            isMessageReceived = false;
        }
        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
        }
        protected override void ReceiveMessage(MessageTypes.Tf2.TFMessage message)
        {
            if (!isMessageReceived)
            {
                ReceivedTransforms = message.transforms;
                isMessageReceived = true;
            }
        }
        private void ProcessMessage()
        {
            // Add/Update the array PublishedTransforms with new TF frames received from the message
            for (int i = 0; i < ReceivedTransforms.Length; i++)
            {
                int j = 0;
                while (ReceivedTransforms[i].child_frame_id != PublishedTransforms[j].child_frame_id || ReceivedTransforms[i].header.frame_id != PublishedTransforms[j].header.frame_id)
                {
                    j++;
                    if (j == PublishedTransforms.Count)
                    {
                        break;
                    }
                }
                if (j < PublishedTransforms.Count)
                {
                    // If the frame is found in the PublishedTransforms array
                    PublishedTransforms[j] = ReceivedTransforms[i];
                }
                else
                {
                    // If it is not found, then add it in
                    PublishedTransforms.Add(ReceivedTransforms[i]);
                }
                currTime = PublishedTransforms[j].header.stamp.secs;
            }
            // Delete outdated TF frames
            PublishedTransforms.RemoveAll(TFFrames => TFFrames.header.stamp.secs < currTime - TFTimeOutInSeconds);
            isMessageReceived = false;
        }
        public List<MessageTypes.Geometry.TransformStamped> GetPublishedTransforms()
        {
            // Getters can only return values when message is processed
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

