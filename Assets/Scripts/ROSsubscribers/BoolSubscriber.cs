using RosSharp.RosBridgeClient.MessageTypes.Std;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class BoolSubscriber : UnitySubscriber<MessageTypes.Std.Bool>
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(Bool message)
        {
            throw new System.NotImplementedException();
        }
    }

}
