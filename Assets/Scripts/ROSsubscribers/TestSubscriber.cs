using RosSharp.RosBridgeClient.MessageTypes.Tf2;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TestSubscriber : UnitySubscriber<MessageTypes.Tf2.TFMessage>
    {
        public MessageTypes.Geometry.TransformStamped[] PublishedTransforms;
        protected override void Start()
        {
            base.Start();
            PublishedTransforms = new MessageTypes.Geometry.TransformStamped[0];
        }
        protected override void ReceiveMessage(MessageTypes.Tf2.TFMessage message)
        {
            PublishedTransforms = message.transforms;
        }
    }
}