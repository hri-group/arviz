# ROS Publishers - Data from Unity -> ROS

ROS Publishers are used as a mean of communication with ROS nodes. Data will be collected with user interface, then will be processed and send to ROS nodes.
The ROS Publisher class is an abstract class, defined by `rossharp` - a Unity package that was developed by Siemens and modified that allows the system to run on HoloLens 2.  

## ROSPublisher class

There are three main components in this class: 

### Start()

Similar to most C# classes, variables need to be initialised in Start().
Initialise all your variables here.

### Setter() 

Setter method is a public method which is used by other components in which they can add the message that needs to be published to the specified topic.  
The argument of this method should be a ROS message.  

### FixedUpdate() Loop

FixedUpdate is rendered at a Unity-defined rate which can be changed in Unity editor.
This method checks for the queue that is initialised at the beginning.  
If there is a message in the queue, the message would be published to the pre-defined topic using Publish() method, which has already been defined by the abstract class.  
It is suggested to keep the class design in this fashion to avoid any unecessary debugging coming from this class.  

## Usage

Prepare your script with all the elements mentioned above implemented.  
In order to add the implemented subscriber, click on the ROSConnector GameObject.  

![alt text](/Images/ROSConnector.PNG "ROSConnector")  

Underneath `ROSConnector`, click on `Add Component` and add script with the name that you give your script and class.  
Specify the topic that you want this publisher to publish to.

Have fun!  




