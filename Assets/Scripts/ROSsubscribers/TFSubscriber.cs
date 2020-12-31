/*
© Siemens AG, 2017-2019
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TFSubscriber : UnitySubscriber<MessageTypes.Tf2.TFMessage>
    {
        public static List<MessageTypes.Geometry.TransformStamped> transforms { get; private set; }
        protected override void Start()
        {
            base.Start();
            transforms = new List<MessageTypes.Geometry.TransformStamped>();
        }
        protected override void ReceiveMessage(MessageTypes.Tf2.TFMessage message)
        {
            transforms = message.transforms.ToList();
        }
    }
}

