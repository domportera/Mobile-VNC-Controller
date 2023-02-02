# Mobile-VNC-Controller
An android application to control a device hosting a VNC server. It does not mirror the display - it simply provides convenient mobile control for controlling your PC without needing to sit at your desk. Work in progress.
Made in Godot 3.5.1.

Currently, the only builds available live in the Releases section.

Special thanks to [Godot Touch Input Manager](https://github.com/Federico-Ciuffardi/GodotTouchInputManager)

## Planned Features
o = in progress
- [ ] gyro mouse movement
  - [ ] screen off functionality w/ volume buttons for clicks
- [ ] mouse sensitivity options
- [ ] mouse acceleration
- [ ] list of saved VNC servers + passwords (encrypted)
- [ ] vnc server discovery
- [o] full-keyboard emulation (platform restrictions aside, this is complete)
- [o] compatibility with android soft-keyboards and hardware keyboards (platform restrictions apply)
- [o] simplified text entry with auto-correct support
- [ ] keyboard type selection (godot restriction afaik)
- [x] System key buttons for every possible system key with multi-key shortcuts
- [ ] system key command editable list 
- [ ] system key button multi-touch support
- [ ] system key button realtime press-up and press-down

## UI To-dos
- [ ] proper font scaling and multi-resolution support
- [ ] improved system key layout
- [ ] an options menu

## Library Development
- [o] genericization of tools and libraries developed for this for use in other projects
    - [ ] ui-control-constrained gestures (from the Trackpad)
    - [ ] multi-touch and press-up-and-down godot UI
    - [ ] runtime debug log console
    - [ ] general .NET and Godot .NET libraries
        - [ ] fuse with DomsUnityHelper and make many functions platform-agnostic
    - [ ] Android wireless debugging .bat script

## Build Instructions
Standard Godot-for-android build procedure applies, with only a couple of permissions checked:
  1. Internet
  2. Vibrate

You may also want to uncheck `Classify As Game`.
