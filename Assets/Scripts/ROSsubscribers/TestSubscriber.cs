using RosSharp.RosBridgeClient.MessageTypes.Std;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TestSubscriber : UnitySubscriber<MessageTypes.Std.String>
    {
        private string data;
        protected override void Start()
        {
            base.Start();
        }
        protected override void ReceiveMessage(String message)
        {
            Debug.Log("Hello World");
        }

    }
}