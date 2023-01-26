# Mobile-VNC-Controller
An android application to control a device hosting a VNC server. It does not mirror the display - it simply provides convenient mobile control for controlling your PC without needing to sit at your desk. Work in progress.
Made in Godot 3.5.1.

Special thanks to [Godot Touch Input Manager](https://github.com/Federico-Ciuffardi/GodotTouchInputManager)

## Planned Features
- [ ] gyro mouse movement
  - [ ] screen off functionality w/ volume buttons for clicks
- [ ] mouse sensitivity options
- [ ] mouse acceleration
- [ ] list of saved VNC servers + passwords (encrypted)
- [ ] full-keyboard emulation
- [ ] compatibility with android soft-keyboards and hardware keyboards
- [ ] simplified text entry with auto-correct support
- [ ] keyboard type selection
- [ ] System keys popup as an editable list, which can expand for every possible system key
- [ ] genericization of tools and libraries developed for this for use in other projects
    - [ ] ui-control-constrained gestures (from the Trackpad)
    - [ ] runtime debug log console
    - [ ] general .NET and Godot .NET libraries
        - [ ] fuse with DomsUnityHelper and make many functions platform-agnostic
    - [ ] Android wireless debugging .bat script


## Build Instructions
Standard Godot-for-android build procedure applies, with only a couple of permissions checked:
  1. Internet
  2. Vibrate

You may also want to uncheck `Classify As Game`.