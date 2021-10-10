# ARviz Core components

This folder contains most of the core components of ARviz.

## Default Plugins

All the display types are implemented and put in this folder. So far there are two display types being implemented, TF Display and VisualisationMarkerArray. 

[Link to folder](./DefaultPlugins)

## Tools

So far there's only one tool implemented for the system, 2D Navigation tool - an arrow-placing tool.

[Link to folder](./Tools)

## ROS Publisher/Subscriber

ROS Publisher/Subscriber is developed in conjunction with the aforementioned Default Plugins to create a mean to translate messages received/sent from/to ROS side.

> **IMPORTANT NOTE**: All publishers and subcribers must be added to the scene prior to compile, i.e. publishers and subscribers can't be added in runtime (or just that we haven't figured out a solution)  

[ROS Publisher](./ROSpublishers)  
[ROS Subscriber](./ROSsubscribers)  

##  Menu

Hand Menu-related codes are placed in here. These codes control the behavior of buttons appear on the hand menu.  

[Menu](./Menu)  

## Extension

Codes for helper functions are placed here, if you want to add additional helper functions, implement them and put them here.

## Prefabs code

Prefab code creates a mean to manipulate the pre-defined prefab, for example, ArrowManipulation changes the dimensions of the arrow. It also provides a method to manipulate an arrow using a Marker ROS message.  
Any code that is attached to prefabs that are used to manipulate them are placed in this folder.

## Utils

Folder for codes that don't fall into any of the categories above or codes that running in the background.

## Vuforia

CustomVuforiaBehavior that we implemented for our one-off calibration with QR code marker. Future localisation behavior will be put into this folder.  
