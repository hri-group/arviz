# Default Plugins
Default Plugins contains code which manipulate the visual elements received from ROS side
For a display, two scripts are needed:
- Script for manipulating the display with the data received from ROS
- Script for the behavior of buttons on its dedicated menu aka control the visibility of the visual elements
An example of this is the TFDisplay.
## TF Display
TF Display has two components
### TFDisplay
The main component of TFDisplay is the Coroutine TFFrameRenderer that spawns/deactives and controls the behavior of the visual elements which relate to TF
TFFrameRenderer()
TFFrameRenderer retrieves the TF tree from TFListener. TF tree is a List of all the GameObjects that have been arranged in a hierachy which based on the TF messages that were published by ROS. This step is needed for all others display types as TF tree provides the origin of where the visual elements need to be placed
TFFrameRenderer then loops through all the GameObjects in the TF tree and manipulate them depending on their existence in the list.
### TFButtonReceiver
TFButtonReceiver is used to manipulate the behavior of the visual elements related to TF when checkboxes on TF menu are ticked/unticked aka toggled.
The menu is shown below:
Images of the menu
(Before)[Images/TFMenuBefore.PNG]
(After)[Images/TFMenu.PGN]
As shown in the second image, the menu is consisted of many different checkboxes. These checkboxes are spawn from a prefab by a Coroutine PopulateMenu() in TFDisplay. 
(Checkbox Prefab)[Images/Checkbox.PNG]
The checkbox is created from a Prefab called CheckboxPrefab. The behavior of the checkbox is controlled via Interactable Receiver as shown below.
(Checkbox's Interactable Property)[Images/TFCheckbox.PNG]
## Implementation for other display type with menu
Prepare two scripts:
### Manipulating the display with Coroutines
Similarly to TFDisplay, initialise all elements with OnEnable instead of Start()
With the Coroutine, retrieve data from ROS Subscriber script, explain here "link" and update the visual elements accordingly
Another Coroutine should be added to create the menu interface, similar to PopulateMenu()
### Changing the display behavior with display menu
Similarly to TFButtonReceiver, create a script which defines the behavior of the checkboxes on your display menu
Create a copy of the checkbox prefab, rename it and go to Interactable properties as shown above to add your custom behavior event for your checkbox (as created in the previous step)
Assign this prefab to your DisplayManipulating script (the previous script that you made which has the couroutines) so that PopulateMenu() can spawn the right checkbox with your customised behavior
Have fun!
Developer's notes (Steven Hoang): I'm in the process of figuring out how to assign the event aka assign the receiver during run-time with script. I'm looking at <Interactable>.AddReceiver<T> function but looks like it doesn't work properly. If you guys can figure out how we can make it to work, please let me know.


