# Hand Menu  

With HoloLens 2, hand-tracking capabilities are provided. With that, we implemented an extendable/configurable hand menu that provides convenient control of the display types and tools that are implemented in the system.

Hand menu in use:

![alt text](/Images/HandMenu.png "HandMenu")  

## Implementation

If you want to add additional entries on Hand Menu, it needs to be done in both Unity Editor and code.

In Unity, look for HandMenu GameObject, as shown below:

![alt text](/Images/HandMenuinUnity.PNG "HandMenuUnity")  

There are three components on the hand menu, only one of them is shown at a time.  

### Main Menu

Users would see this menu first when their hand is raised in front of their eyes, clicking either of the buttons would hide the Main Menu and make the chosen one visible. 

This should be keep as is.

![alt text](/Images/MainMenu.PNG "MainMenu")  

### Display Menu & Tools menu

This menu allows users to toggle the implemented displays or tools, as well as to bring up the specialized menu for the display of choice (no specialized menu for tool).

![alt text](/Images/DisplayMenu.PNG "DisplayMenu")  

To extend this menu, simply extend the blue background board to add more buttons. Duplicate the already existing button, modify the GameObject name, and modify the name in `ButtonConfigHelper` as shown:  

![alt text](/Images/DisplayMenu2.PNG "DisplayMenu2")
![alt text](/Images/DisplayMenu3.PNG "DisplayMenu3")  

For sanity checks, make sure the component `Interactable`->`Event` has a `HandMenuReceiver` EventReceiverType component.
After this, go to the script `HandMenuReceiver.cs` and modify the switch statement with your button(s).

Have fun!  
