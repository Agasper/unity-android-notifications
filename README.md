Unity3D Android notification plugin
=====
License: *MIT*

Features:
* Set delayed notification
* Set delayed repeatable notification
* Supports custom icon and large icon
* Fully supports Unity 4.x, 5.x
* Fully supports Android 4.0.3 - 7.1

### FAQ

**How to set up big icon ?**

Just put it into drawable directories *\UnityProject\Assets\Plugins\Android\res\* with name *notify_icon_big.png*. And in the SendNotification method set bigIcon to **"notify_icon_big"**.

If you want to use app icon just set bigIcon = **"app_icon"**.

**How to recompile plugin ?**

There is special gradle task called **exportJar**, just run it and grab plugin's jar in app/release/notification.jar

**How to get rid of obsolete warnings like "*OBSOLETE - Providing Android resources in Assets/Plugins/Android/res is deprecated, please move your resources to an Android Library. See "Building Plugins for Android" section of the Manual.*" ?**

You can avoid this by recompiling plugin with icons inside.

### Screenshot
![Screenshot](https://github.com/Agasper/unity-android-notifications/blob/master/screenshot.png?raw=true "Screenshot")
