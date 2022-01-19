using RosSharp.RosBridgeClient.MessageTypes.Std;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class Int32Subscriber : UnitySubscriber<MessageTypes.Std.Int32>
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Std.Int32 message)
        {
            throw new System.NotImplementedException();
        }
    }

}
