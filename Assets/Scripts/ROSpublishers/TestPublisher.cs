using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.Tf2;
namespace RosSharp.RosBridgeClient
{
    public class TestPublisher : UnityPublisher<MessageTypes.Tf2.TFMessage>
    {
        MessageTypes.Tf2.TFMessage message;
        MessageTypes.Geometry.TransformStamped[] tf_test_arr;
        protected override void Start()
        {
            base.Start();
            message = new TFMessage();
            tf_test_arr = new MessageTypes.Geometry.TransformStamped[1];
            tf_test_arr[0] = new MessageTypes.Geometry.TransformStamped();
            tf_test_arr[0].child_frame_id = "test_frame";
        }
        private void Update()
        {
            message.transforms = tf_test_arr;
            Publish(message);
        }
    }
}
