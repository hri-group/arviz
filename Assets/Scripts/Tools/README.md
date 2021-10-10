# ROS Tool

ROS tool collects the user's input via interactive marker (e.g. arrows, bounding boxes) to communicate with ROS nodes. 

## Implementation

Depends on the tool, the implementation would be a little bit different. However the general idea must be maintained:  

- Must be able to dynamically spawn in runtime  
- Must be toggleable using hand menu 
- Ideally only one tool can be used at a time

## Navigation Tool

NavigationTool (2DNav) is an example of ROS tool implemented with ARviz. This tool mimics the tool with the same name in RViz, which is used with the Navigation stack.   
The tool is implemented in conjuction with Grid Display. When a hand ray pointer is pointed to the grid, users can use a click gesture to spawn an arrow which can be rotated with their hand as shown below. After choosing the direction, user can perform another click to send the goal pose (position and orientation) to the Navigation stack.  

![alt text](Images/ArrowTool.gif "ArrowTool")  

### The implementation:  

NavigationTool is implemented as a Component of Grid Display. It extends the PointerHandler provided by the Mixed Reality Toolkit. The functionality of the tool is implemented within `OnPointerClicked`. This means the tool can only be used when user perform a click gesture with the pointer pointed on the grid.  



