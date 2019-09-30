## IMPORTANT
If you're using Unity 2018.3+, you can setup notifications throught built-in Unity package
https://docs.unity3d.com/Packages/com.unity.mobile.notifications@1.0/manual/index.html


Unity3D Android notification plugin
=====
License: *MIT*

Features:
* Set delayed notification
* Set delayed repeatable notification
* Supports custom icon and large icon
* Fully supports Unity 4.x, 5.x, 2017.x
* Fully supports Android 4.0.3 - 8.0
* Fully supports iOS 8.0 - 11.0

### FAQ

**How to set up small icon ?**

Use the Android Asset Studio notification icons generator (https://romannurik.github.io/AndroidAssetStudio/icons-notification.html) to prepare small icons pack and replace temporary icons named **notify_icon_small.png** with the new ones in *\UnityProject\Assets\Plugins\Android\res\*

**How to set up big icon ?**

Same as small icon, but use launcher icons generator (https://romannurik.github.io/AndroidAssetStudio/icons-launcher.html)
Just put the result into drawable directories *\UnityProject\Assets\Plugins\Android\res\* with name *notify_icon_big.png*. And in the SendNotification method set bigIcon to **"notify_icon_big"**.

If you want to use app icon just set bigIcon = **"app_icon"**.

**How to use a custom sound ?**

For Android, place an .mp3 or .ogg sound file under Assets/Plugins/Android/res/raw in your Unity project. For iOS, place a .wav, .caf or .aiff sound under Assets/StreamingAssets. Make sure your Unity script that creates the notification sets the `sound` parameter to `true` and the `soundName` parameter to the name of your sound file, without an extension.

**How to handle notification actions ?**

When you create your Action, pass in a component (Like NotificationTest) that's attached to a game object that's guaranteed to be on your stage when your app starts. Also make sure that component has a method named "OnAction", or customize the name of the `HandlerMethod` when you create your Action. Whatever the name, the method must accept a single string parameter, which is the identifier of the action that was chosen.

On iOS, if you have more than one kind of notification with actions, make sure you use a different `channel` value for each kind.

**How to create repeating notifications on iOS ?**

iOS only supports repeat intervals of a minute, hour, day, 2-5 days (for 'weekday'), week, month, quarter or year. Make sure your `timeout` is the correct number of milliseconds for one of those values, like `60 * 1000` for a minute.

**How to recompile Android plugin ?**

There is special gradle task called **exportJar**, just run it and grab plugin's jar in app/release/notification.jar

**How to get rid of obsolete warnings like "*OBSOLETE - Providing Android resources in Assets/Plugins/Android/res is deprecated, please move your resources to an Android Library. See "Building Plugins for Android" section of the Manual.*" ?**

You can avoid this by recompiling plugin with icons (and optionally sounds) inside.

### Screenshot
![Screenshot](https://github.com/Agasper/unity-android-notifications/blob/master/screenshot.png?raw=true "Screenshot")
