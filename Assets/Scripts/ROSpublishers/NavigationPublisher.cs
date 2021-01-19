using System.Collections.Generic;

namespace RosSharp.RosBridgeClient {
    public class NavigationPublisher : UnityPublisher<MessageTypes.Geometry.PoseStamped>
    {
        private List<MessageTypes.Geometry.PoseStamped> message_queue;
        protected override void Start()
        {
            base.Start();
            InitialisedMessage();
        }
        private void InitialisedMessage()
        {
            message_queue = new List<MessageTypes.Geometry.PoseStamped>();
        }
        private void FixedUpdate()
        {
            // If there is message in the queue
            if (message_queue.Count > 0)
            {
                // Publish the first message from queue
                Publish(message_queue[0]);
                // Then remove it
                message_queue.RemoveAt(0);
            }
        }
        public void SendGoal(MessageTypes.Geometry.PoseStamped goal)
        {
            message_queue.Add(goal);
        }
    }
}
