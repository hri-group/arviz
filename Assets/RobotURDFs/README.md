# Robot URDF Import

This will help you to import any URDF-defined robots into ARviz Scene.

## Import your robot into Unity
- Create a folder for your URDF file and the meshes of the robot model. 

![alt text](/Images/URDFBuild1.PNG "URDF Import 1")

- Copy the URDF file of the robot into this folder. 
> **IMPORTANT NOTE**: If your robot uses XACRO for definition, please stop here and move on to the **Convert XACRO files to a single URDF file** to get your URDF file. After that, you can come back here and continue.

Edit the robot name to the one that you want

![alt text](/Images/URDFBuild5.PNG "URDF Import 2")

- Copy the folder of the robot descriptions into the same folder as the URDF file. Pay attention to the location of the meshs being defined in the URDF file. You need to arrange the folders so that it is exactly the same as what being defined here. In this example, the meshes are located in the directory *franka_descriton/meshes/visual/*, therefore, you need the folder franka_description on the same directory as the URDF file.

![alt text](/Images/URDFBuild6.PNG "URDF Import 3")

- In Unity, go **GameObject > 3D > URDF Model (import)** to import the URDF robot into the scene. You should end up with something looking like this

![alt text](/Images/URDFBuild7.PNG "URDF Import 4")

- In ObjectProperty as shown below, select **Enable, Disable, Enable, Enable**

![alt text](/Images/URDFBuild8.PNG "URDF Import 5")

- As you can see, the robot is not at its ready state, you need to manually adjust the rotation of each link to match with the actual joint states of the real robot. You can do so by expanding the robot in the GameObject window and carefully modify the rotation parameters (need to check which axis each mesh should be rotated by and by how much. To check for these, you can have a joint state subscriber with the robot attached to it. Check at the end of this tutorial). 
> **IMPORTANT NOTE**: After fixing the robot, if remove the Joint State Subscriber, otherwise, the robot model will be distorted.

![alt text](/Images/URDFBuild9.PNG "URDF Import 5")

- You should end up with something like this.

![alt text](/Images/URDFBuild10.PNG "URDF Import 6")

## Convert XACRO files to a single URDF file
If your URDF robot has Xacro in it (i.e. XML macros) than you need to convert it to URDF as the URDF import function can only take URDF file. This can be done in ROS Ubuntu.

Follow these steps to generate the URDF file for your robot:

- Prepare your xacro file. Sometimes a robot description can be a combination of multiple xacro files which there is one xacro file that will link the other xacro files together. Make sure you have that xacro file ready in a new folder.

![alt text](/Images/URDFBuild2.PNG "XACRO Converter 1")  

![alt text](/Images/URDFBuild3.PNG "XACRO Converter 2")  

- In the new folder, open a terminal and run xacro, make sure roscore is launched first.
```
	rosrun xacro xacro -o [YOUR_OUTPUT_FILENAME].urdf [URDF_ROBOT_XACRO].xacro
```

- The output would look something like this, examine your URDF file to make sure all the robot links are included.

![alt text](/Images/URDFBuild4.PNG "XACRO Converter 3")

## Joint State Patcher
- In **ROSConnector** GameObject's Property, add **Joint State Patcher** Script. Drag the robot model created in the scene into the field **Urdf Robot**

![alt text](/Images/URDFBuild11.PNG "Joint State Patcher 1")

- In Joint State Patcher, click "Enable" Joint State Subscriber and "Disable" Joint State Publisher. You should end up with a Joint State Subscriber script being added.

- In the **Topic** field, specify the join states topic name, normally "/joint_states"

![alt text](/Images/URDFBuild12.PNG "Joint State Patcher 2")

- Now when you simulate the scene, the robot should mimic the real robot. If you don't see anything, walk around the scene, maybe the camera is placed such that you can't see the robot.




