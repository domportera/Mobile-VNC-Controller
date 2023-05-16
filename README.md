# This project is on hold until Godot 4 can export C# to Android

# Mobile-VNC-Controller
An android application to control a device hosting a VNC server. It does not mirror the display - it simply provides convenient mobile control for controlling your PC without needing to sit at your desk. Work in progress.
Made in Godot 3.5.1.

Currently, the only builds available live in the Releases section.

This project has been a workbench for several tools I plan on releasing separately. As this is my first real Godot project, I've run into several limitations that I otherwise would have taken for granted that I think others would too. So if all goes well, the following will be byproducts of this project:

1. A Nice Touch ™️ system built from scratch that makes multitouch and gestures easily accessible, allowing nodes to claim ownership over touch events for simplified game and application logic.
2. A small UI library to allow for multitouch and button up/down events
3. An in-game console log to view the outputs of a debug system (if anyone knows of a way too hook into GD.Print natively please lmk)
4. A Drag-down split container (used in my console) - currently limited by Godot 3.5 but I may work around that if I can
5. 

C# takes priority for these systems, however I would like to expose as much of it as possible to GDScript. 


Special thanks to [Godot Touch Input Manager](https://github.com/Federico-Ciuffardi/GodotTouchInputManager) for providing the touch foundation for the first iterations of builds.
## Planned Features
o = in progress
- [ ] gyro mouse movement
  - [ ] screen off functionality w/ volume buttons for clicks
- [ ] mouse sensitivity options
- [o] mouse acceleration
- [ ] list of saved VNC servers + passwords (encrypted)
- [ ] vnc server discovery
- [o] full-keyboard emulation (platform restrictions aside, this is complete)
- [o] compatibility with android soft-keyboards and hardware keyboards (platform restrictions apply)
- [o] simplified text entry with auto-correct support
- [ ] keyboard type selection (godot restriction afaik)
- [x] System key buttons for every possible system key with multi-key shortcuts
- [ ] system key command editable list 
- [ot] system key button multi-touch support
- [x] system key button realtime press-up and press-down

## UI To-dos
- [ ] proper font scaling and multi-resolution support
- [ ] improved system key layout
- [ ] an options menu
- [ ] oled theme
- [ ] landscape support
- [ ] minimalist layout
- [ ] gamepad layout 
- [ ] button icons

 ## Technical To-dos
- [ ] framerate limit option
- [ ] upgrade to more capable VNC library (requires Godot 4 once it supports .NET android compilation)
- [ ] upgrade to Godot 4
- [ ] refactor with .NET Core goodness

## Unlikely Ideas
- [ ] Rustdesk support
- [ ] HID emulation
- [ ] Bluetooth support
- [ ] stream zoomed in VNC video feed under trackpad
- [ ] full traditional video support

## Build Instructions
Standard Godot-for-android build procedure applies, with only a couple of permissions checked:
  1. Internet
  2. Vibrate

You may also want to uncheck `Classify As Game`.
