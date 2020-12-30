using UnityEngine;
namespace RosSharp.RosBridgeClient
{
    public class TestPublisher : UnityPublisher<MessageTypes.Std.String>
    {
        private MessageTypes.Std.String message;
        protected override void Start()
        {
            base.Start();
            message = new MessageTypes.Std.String();
        }
        private void Update()
        {
            message.data = "Hello World";
            Publish(message);
        }
    }
}
