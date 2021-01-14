using RosSharp.RosBridgeClient.MessageTypes.Visualization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class MarkerArraySubscriber : UnitySubscriber<MessageTypes.Visualization.MarkerArray>
    {
        private Marker[] PublishedMarkers;
        private bool isNewMessage = false;
        protected override void Start()
        {
            base.Start();
            PublishedMarkers = new Marker[0];
        }
        protected override void ReceiveMessage(MarkerArray message)
        {
            PublishedMarkers = message.markers;
            isNewMessage = true;
        }
        public Marker[] GetPublishedMarkers()
        {
            if (isNewMessage)
            {
                isNewMessage = false;
                return PublishedMarkers;
            }
            else
            {
                return null;
            }
        }
    }
}
