# ARviz Core components
This folder contains most of the core components of ARviz.
## Default Plugins
All the display types are implemented and put in this folder. So far there are two display types being implemented, TF Display and VisualisationMarkerArray. [Link to folder](./DefaultPlugins)
## Tools
So far there's only one tool implemented for the system, 2D Navigation tool - an arrow-placing tool.
Link:
## ROS Publisher/Subscriber
ROS Publisher/Subscriber is developed in conjunction with the aforementioned Default Plugins to create a mean to translate messages received/sent from/to ROS side
Link:
##  Menu
Hand Menu-related codes are placed in here. These codes control the behavior of buttons appear on the hand menu
Link:
## Extension
Codes for helper functions are placed here, if you want to add additional helper functions, please implement them and put them here.
## Prefabs code
Prefab code creates a mean to manipulate the pre-defined prefab, for example, ArrowManipulation changes the dimensions of the arrow. It also provides a method to manipulate an arrow using a Marker ROS message. Link to ArrowManipulation:
Any codes that are attached to prefabs that are used to manipulate them are placed in this folder
## Utils
Folder for codes that don't fall into any of the categories above or codes that running in the background.
Link:
## Vuforia
CustomVuforiaBehavior that we implemented for our one-off calibration with QR code marker. Future localisation behavior will be put into this folder


