Unity3D Android notification plugin
=====
License: MIT
Features:
* Set delayed notification
* Set delayed repeatable notification
* Supports custom icon and large icon
* Fully supports Android 6.0

### FAQ

**How to get exact notification in Android M ?**

It's not recommended by Google since notifications are batching when phone in IDLE_MAINTENANCE state. Now you'll receive notification with additional delay. You can use NotificationExecuteMode.ExactAndAllowWhileIdle mode to force system show your notification with maximum priority. [Read more] (https://developer.android.com/intl/ru/reference/android/app/AlarmManager.html#setExact(int, long, android.app.PendingIntent))

**How to set up big icon ?**

Just put it into drawable directories *\UnityProject\Assets\Plugins\Android\res\* with name *notify_icon_big.png*. And in the SendNotification method set bigIcon to *"notify_icon_big"*

**How to get rid of obsolete warnings like "*OBSOLETE - Providing Android resources in Assets/Plugins/Android/res is deprecated, please move your resources to an Android Library. See "Building Plugins for Android" section of the Manual.*"**

You can avoid this by recompiling plugin with icons inside.

### Screenshot
![Screenshot](https://github.com/Agasper/unity-android-notifications/blob/master/screenshot.png?raw=true "Screenshot")
