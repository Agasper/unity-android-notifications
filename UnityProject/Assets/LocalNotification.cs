#define FORCE_LOCAL_NOTIFICATIONS

#if (UNITY_ANDROID && !UNITY_EDITOR) || FORCE_LOCAL_NOTIFICATIONS
#define USE_LOCAL_NOTIFICATIONS
#endif

using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace Net.Agasper.UnityNotifications
{
    public class NotificationData
    {
        public static readonly Color32 noColor = new Color32(0, 0, 0, 0); // Means: no explicit color to set.

        public string   title               { get; private set; }
        public string   message             { get; private set; }
        public string   ticker              { get; private set; }

        public string   group               { get; private set; }

        public bool     showTime            { get; private set; }
        public bool     sound               { get; private set; }

        public Color32  color               { get; private set; }

        public int[]    vibrationPattern    { get; private set; }

        public Color32  lightsColor         { get; private set; }
        public int      lightsOn            { get; private set; } // In milliseconds.
        public int      lightsOff           { get; private set; } // In milliseconds.

        public string   smallIconResource   { get; private set; }
        public string   largeIconResource   { get; private set; }

        public NotificationData (string title, string message)
        {
            this.title = title;
            this.message = message;
            this.group = null;
            this.showTime = true;
            this.sound = true;
            this.color = noColor;
            this.vibrationPattern = null;
            this.lightsColor = noColor;
            this.lightsOn = 1000;
            this.lightsOff = 3000;
            this.smallIconResource = "notify_icon_small";
            this.largeIconResource = null;
        }

        public NotificationData SetTicker(string ticker)
        {
            this.ticker = ticker;
            return this;
        }

        public NotificationData SetGroup(string group)
        {
            this.group = group;
            return this;
        }

        public NotificationData SetShowTime(bool showTime)
        {
            this.showTime = showTime;
            return this;
        }

        public NotificationData SetSound(bool sound)
        {
            this.sound = sound;
            return this;
        }

        public NotificationData SetColor(Color32 color)
        {
            this.color = color;
            return this;
        }

        public NotificationData SetVibrationPattern(int[] vibrationPattern)
        {
            this.vibrationPattern = vibrationPattern;
            return this;
        }

        public NotificationData SetLightsColor(Color32 lightsColor)
        {
            this.lightsColor = lightsColor;
            return this;
        }

        public NotificationData SetLightsOn(int lightsOn)
        {
            this.lightsOn = lightsOn;
            return this;
        }

        public NotificationData SetLightsOff(int lightsOff)
        {
            this.lightsOff = lightsOff;
            return this;
        }

        public NotificationData SetSmallIconResource(string smallIconResource)
        {
            this.smallIconResource = smallIconResource;
            return this;
        }

        public NotificationData SetLargeIconResource(string largeIconResource)
        {
            this.largeIconResource = largeIconResource;
            return this;
        }
    }

    class LocalNotification
    {
        /// <summary>
        /// Mode of scheduling notification to Android Alarm Manager.
        /// 
        /// Exact uses `setExact` method (API 19+, Android 4.4 or higher) or 'setExactAndAllowWhileIdle' method (API 23+, Android 6.0 or higher). Inexact alternatives will be used on older Android versions.
        /// AllowWhileIdle uses `setAndAllowWhileIdle` method or `setExactAndAllowWhileIdle` method (API 23+, Android 6.0 or higher). Normal versions will be used on older Android versions.
        /// 
        /// Documentation: https://developer.android.com/intl/ru/reference/android/app/AlarmManager.html
        /// </summary>
        [Flags]
        public enum ScheduleMode
        {
            None = 0,
            Exact = 1,
            AllowWhileIdle = 2,
        }

#if USE_LOCAL_NOTIFICATIONS
        private static string managerClassName = "net.agasper.unitynotification.UnityNotificationManager";
        private static string notificationDataClassName = "net.agasper.unitynotification.NotificationData";
#endif

        public static void SendNotification
            ( string id
            , DateTime when
            , NotificationData data
            , bool cancelPrevious = false
            , ScheduleMode scheduleMode = ScheduleMode.None)
        {
            SendNotification (id, when, TimeSpan.Zero, data, cancelPrevious, scheduleMode);
        }

        public static void SendNotification
            ( string id
            , DateTime when
            , TimeSpan repeatInterval
            , NotificationData data
            , bool cancelPrevious = false
            , ScheduleMode scheduleMode = ScheduleMode.None)
        {
            Assert.IsNotNull(id);
            Assert.IsTrue(id.Length > 0);
            Assert.IsNotNull(data);

#if USE_LOCAL_NOTIFICATIONS
            AndroidJavaClass pluginClass = new AndroidJavaClass(managerClassName);
            if (pluginClass == null)
            {
                ReportNoJavaClass(managerClassName);
                return;
            }

            AndroidJavaObject dataObject = PrepareNotificationData(data);
            if (dataObject == null)
            {
                return;
            }

            long timeDeltaMs = (long)(when.ToUniversalTime() - DateTime.UtcNow).TotalMilliseconds;
            if (timeDeltaMs >= 0)
            {
                    pluginClass.CallStatic
                        ( "SetNotification"
                        , id
                        , timeDeltaMs
                        , (repeatInterval != TimeSpan.Zero) ? (long)repeatInterval.TotalMilliseconds : 0
                        , cancelPrevious
                        , dataObject
                        , (int)scheduleMode);
            }
            else
            {
                Debug.LogWarningFormat("Passed notification date is in past ({0}), ignored.", when);
            }
#endif
        }

        public static void CancelNotification (string id, bool cancelPending = true, bool cancelShown = true)
        {
#if USE_LOCAL_NOTIFICATIONS
            AndroidJavaClass pluginClass = new AndroidJavaClass(managerClassName);
            if (pluginClass != null)
            {
                pluginClass.CallStatic
                    ("CancelNotification"
                    , id
                    , cancelPending ? 1 : 0
                    , cancelShown ? 1 : 0);
            }
            else
            {
                ReportNoJavaClass(managerClassName);
            }
#endif
        }

        public static void CancelAllShownNotifications ()
        {
#if USE_LOCAL_NOTIFICATIONS
            AndroidJavaClass pluginClass = new AndroidJavaClass(managerClassName);
            if (pluginClass != null)
            {
                pluginClass.CallStatic("CancelAllShownNotifications");
            }
            else
            {
                ReportNoJavaClass(managerClassName);
            }
#endif
        }

#if USE_LOCAL_NOTIFICATIONS
        private static void ReportNoJavaClass (string className)
        {
            Debug.LogErrorFormat("There's no Java class with a name '{0}' in a project. Local notifications will not work.", className);
        }

        private static AndroidJavaObject PrepareNotificationData (NotificationData data)
        {
            AndroidJavaObject dataObject = new AndroidJavaObject(notificationDataClassName);
            if (dataObject != null)
            {
                dataObject.Set("title", data.title);
                dataObject.Set("message", data.message);
                dataObject.Set("ticker", data.ticker);

                dataObject.Set("group", data.group);

                dataObject.Set("showTime", data.showTime);
                dataObject.Set("sound", data.sound);

                dataObject.Set("color", (data.color.a << 24) | (data.color.r << 16) | (data.color.g << 8) | data.color.b);

                if (data.vibrationPattern != null)
                    dataObject.Call("SetVibrationPattern", data.vibrationPattern);

                dataObject.Set("lightsColor", (data.lightsColor.a << 24) | (data.lightsColor.r << 16) | (data.lightsColor.g << 8) | data.lightsColor.b);
                dataObject.Set("lightsOn", data.lightsOn);
                dataObject.Set("lightsOff", data.lightsOff);

                dataObject.Set("smallIconResource", data.smallIconResource);
                dataObject.Set("largeIconResource", data.largeIconResource);
            }
            else
            {
                ReportNoJavaClass(notificationDataClassName);
            }
            return dataObject;
        }
#endif
    }
}