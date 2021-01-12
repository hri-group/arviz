# AR-Project Template
This repo is the template for any future AR Project
## Requirements
In order to use the template, you would need:
- Unity 2019.4.4f1
- Visual Studio 2019
- Microsoft SDK 18632 or above
## Included
- ROS#
- MixedRealityToolkit 2.4.0
- Vuforia 9.28
- TextMeshPro
## Usage
- Create a copy of sample scene
- Modifiy the ROS Connector's ROSConnector property with the IP address of your ROS machine
- For HoloLens (UWP) projects, make sure to keep the "Protocol" field to be "WebSocketUWP"
- Add your elements and enjoy.
## Deploy your app
Instructions on to how to deploy your app is shown [here](https://hri-wiki.erc.monash.edu/index.php/How_to_pages/How_to_deploy_Unity_applications_onto_the_Hololens)
## Subscribers and Publishers
- Remember to follow the template TestSubscriber and TestPublisher scripts that being provided in Scripts folder
- Add the subscribers and publishers as properties of ROS Connector GameObject
## Debugging on Simulation
- You can debug your project with Unity Editor simulator and a Virtual Machine with ROS installed.
- Instructions to prepare Virtual Machine and ROS is shown in our HRI wiki [here](https://hri-wiki.erc.monash.edu/index.php/How_to_pages/How_to_set_up_Virtual_Machine). If you don't have access to the page, please contact the Monash HRI research group for access rights.
- In order to connect to your ROS machine, retrieve the IP address starts with 192.168 on the ROS machine and follow the "Usage" instructions above
