using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Tf2;

namespace RosSharp.RosBridgeClient
{
    public class ArvizCalibration : UnitySubscriber<MessageTypes.Geometry.TransformStamped>
    {
        private bool isMessageReceived = false;
        private UnityEngine.Vector3 PublishedPosition;
        private UnityEngine.Quaternion PublishedOrientation;
        private string childFrame;
        public GameObject TF;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            PublishedOrientation = new Quaternion();
            PublishedPosition = new Vector3();
        }

        // Update is called once per frame
        void Update()
        {
            if (isMessageReceived)
            {
                ProcessMessage();
            }
        }
        protected override void ReceiveMessage(MessageTypes.Geometry.TransformStamped message)
        {
            childFrame = message.child_frame_id;
            PublishedPosition = message.transform.translation.rosMsg2Unity().Ros2Unity();
            PublishedOrientation = message.transform.rotation.rosMsg2Unity().Ros2Unity();
            isMessageReceived = true;
        }
        void ProcessMessage()
        {
            if(TF != null)
            {
                if (TF.transform.localPosition != PublishedPosition && TF.transform.localRotation != PublishedOrientation)
                {
                    TF.transform.localPosition = PublishedPosition;
                    TF.transform.localRotation = PublishedOrientation;
                }
                isMessageReceived = false;
            }
        }
    }
}
