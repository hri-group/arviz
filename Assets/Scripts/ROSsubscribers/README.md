# ROS Subscribers - Data from ROS -> Unity

ROS Subscribers are used as a mean of communication with ROS nodes. Data received from ROS nodes is processed by this unit into the form so that Coroutines in [DefaultPlugins](../DefaultPlugins) can manipulate the visual elements based on the received data.
ROS Subscriber class is an abstract class, defined by `rossharp`- a Unity package that was developed by Siemens and modified that allows the system to run on HoloLens 2.  

## ROSSubscriber class

There are three main components in this class:

### Start()

Similar to most Csharp classes, variables need to be initialised in Start(). 
Initialise all your variables here.  

### ReceiveMessage()

ReceiveMessage is the callback function which is called everytime there is a new message received from the topic which the ROSSubscriber subcribes to.
This is the most crucial element of the subcribers. It is recommended to keep this function as simple as possible, i.e. mimics what has been done with the already-implemented subscriber classes.

### Getter()

Getter method is used by other components in the code to access the newly acquired messages from ReceiveMessage.

### (Optional) ProcessMessage + Update

In the situation where the message needs further processing in order to be used by the others, ProcessMessage() method is implemented. It is used in conjunction with Update(), where it checks for new message every frame and process it if a new message has arrived.

## Usage

Prepare your script with all the elements mentioned above implemented. 
In order to add the implemented subscriber, click on the ROSConnector GameObject.

![alt text](/Images/ROSConnector.PNG "ROSConnector")  

Underneath `ROSConnector`, click on `Add Component` and add script with the name that you give your script and class.  
Specify the topic that you want this subcriber to subscribe to.

Have fun!  

