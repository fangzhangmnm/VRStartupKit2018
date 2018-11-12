# VR Startup Kit for Unity2018

by fangzhangmnm

Ready-to-use VR Startup Kit to boost up your development!

Tested on HTC Vive and WindowsMR.

# Features:

- Teleporting
- Rigidbody-based trackpad movement
- ArmSwinger
- Prevent Falling and Head Collision
- Adjust player collider when crouch
- Climbing
- Pickups

# Setup:

To enable VR for your project, go to menu Edit/Project Settings/Player. Check on "Virtual Reality Supported" in XR Settings. And make sure "OpenVR" SDK is enabled.

Setting up input axis: You can copy the InputManager.assert in InputManager.zip to the ProjectSettings folder under your project folder. (NOT the Assets folder!) Alternatively, you can manually set up the Input by going to Edit/Project Settings/Inputs and referring to https://docs.unity3d.com/Manual/OpenVRControllers.html

Open DemoScene and play, where instructions are included.

This package is lightweight designed. Just drag the Player prefab into the new scene to make your own game.

# How to use:

The locomotion functions are organized in individual scripts under the locomotion gameobject in player prefab. You can disable any function by simply uncheck the corresponding script.

## Teleporting

Press the left trackpad, and point the destination you want to go with the parabola. Then release the trackpad.

Teleporting point selection is based on Physics.Raycast API, which means, the target must have a collider in order to let the player teleport on it. You can filter the colliders by teleportLayer option

You can change the controller which triggers the teleporting by Trackpad Hand option.

You can also customize the teleporting indicator.

## Trackpad Movement

- Vive: touch the left trackpad in the direction you want to move. Do not press, only touch.
- WindowsMR/Oculus: push the left thumbstick

The direction of the consequently movement are decided by the orientation of the controller. Which means if you are holding the controller 90-degree right and pushing the thumbstick left, you would go forward!

You can change movement speed, control hand.

## ArmSwinger

To enable armswinger, check “Use Arm Swinger” in the “VR Trackpad Movement” script.

Similar to trackpad movement, but you have to at the same time swing your arms in order to move. This option would significantly reduce motion sickness. 

“Arm Swinger Multiplier” controls the amount of movement you gain from swinging your arm.

## Turning around

- Vive: To turn left, press the left side of the right trackpad. Right side for turning right.
- WindowsMR/Oculus: push the right thumbstick left or right.

You can also change the control hand.

## Climbing

To climb, touch your hand to a climbable surface, then grab the trigger. If you release the trigger, you fall.

To make an object climbable, make sure it has a collider and a VRClimbableSurface script. rigidbody is not required.

### Coughing, Collision resolving and Fall preventing

The height and position of player’s collider is automatically adjusted when the player is coughing.

Collision is carefully handled so the camera won’t penetrate into a mesh when player is navigating throughout the scene, even if the player suddenly stands up under an obstacle, his head will not penetrate. The only exception is while climbing.

If the player lean forward in front of a cliff, he will not fall. If the player lean forward in front of a table, he will not be “teleported” on the table. These are the common mistakes an ordinary vr locomotion system would take.

## Pickups

Attach VRPickup script to the gameobject you want to pick. To make the gameobject throwable, VelocityEstimator and Rigidbody are also needed.

Pull the trigger to pick up an item. Items can be configured whether it would detached from player’s hand. For throwable objects, it is recommended to set Unlock Type to “Holding”. The object will detach itself as soon as you release the trigger. For melee weapons, setting Unlock Type to “Grip and Trigger to Unlock“ prevents accidentally sliping out of hand.

# Compatibility:

Notice that for a standalone build (.exe), OpenVR sdk is used. Which means even if you were using WindowsMR devices, the input would be mapped by the SteamVR API to HTCVive controls. That means you should set the buttons according to HTCVive, not WindowsMR.

For WindowsMR, there is a minor problem that the analog input of touchpad is overridden by the thumb stick. For Oculus, there is no touchpad but only thumb stick. So different input schemes were devised for each kind of controllers.

If the input doesn’t responce and errors like “XRInputDeviceState: Failed to set the value at featureIndex [4].” occurs, this is a Unity bug. It has been fixed in the recent Unity versions.

