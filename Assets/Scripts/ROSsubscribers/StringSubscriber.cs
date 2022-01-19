using RosSharp.RosBridgeClient.MessageTypes.Std;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class StringSubscriber : UnitySubscriber<MessageTypes.Std.String>
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(String message)
        {
            throw new System.NotImplementedException();
        }
    }

}
