#define FORCE_LOCAL_NOTIFICATIONS

#if (UNITY_ANDROID && !UNITY_EDITOR) || FORCE_LOCAL_NOTIFICATIONS
#define USE_LOCAL_NOTIFICATIONS
#endif

using UnityEngine;
using System;

namespace Net.Agasper.UnityNotifications
{
    class LocalNotification
    {
        public static readonly Color32 noColor = new Color32(0, 0, 0, 0);

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
        private static string fullClassName = "net.agasper.unitynotification.UnityNotificationManager";
#endif

        public static void SendNotification
            ( int id
            , DateTime when
            , string title
            , string message
            , Color32 bgColor
            , Color32 lightsColor
            , string group = null
            , bool cancelPrevious = false
            , bool showTime = true
            , bool sound = true
            , long[] vibrationPattern = null
            , int lightsOnMs = 1000
            , int lightsOffMs = 3000
            , string smallIcon = "notify_icon_small"
            , string bigIcon = ""
            , ScheduleMode scheduleMode = ScheduleMode.None)
        {
#if USE_LOCAL_NOTIFICATIONS
            AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
            if (pluginClass != null)
            {
                long timeDeltaMs = (long)(when.ToUniversalTime() - DateTime.UtcNow).TotalMilliseconds;
                if (timeDeltaMs >= 0)
                {
                    pluginClass.CallStatic
                        ( "SetNotification"
                        , id
                        , group
                        , timeDeltaMs
                        , title
                        , message
                        , message
                        , 0
                        , showTime ? 1 : 0
                        , sound ? 1 : 0
                        , vibrationPattern
                        , (lightsColor.a << 24) | (lightsColor.r << 16) | (lightsColor.g << 8) | lightsColor.b
                        , lightsOnMs
                        , lightsOffMs
                        , bigIcon
                        , smallIcon
                        , (bgColor.a << 24) | (bgColor.r << 16) | (bgColor.g << 8) | bgColor.b
                        , (int)scheduleMode);
                }
                else
                {
                    Debug.LogWarningFormat("Passed notification date is in past ({0}), ignored.", when);
                }
            }
            else
            {
                ReportNoJavaClass();
            }
#endif
        }

        public static void SendRepeatingNotification
            ( int id
            , DateTime when
            , TimeSpan repeatInterval
            , string title
            , string message
            , Color32 bgColor
            , Color32 lightsColor
            , string group = null
            , bool cancelPrevious = false
            , bool showTime = true
            , bool sound = true
            , long[] vibrationPattern = null
            , int lightsOnMs = 1000
            , int lightsOffMs = 3000
            , string smallIcon = "notify_icon_small"
            , string bigIcon = ""
            , ScheduleMode scheduleMode = ScheduleMode.None)
        {
#if USE_LOCAL_NOTIFICATIONS
            AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
            if (pluginClass != null)
            {
                long timeDeltaMs = (long)(when.ToUniversalTime() - DateTime.UtcNow).TotalMilliseconds;
                if (timeDeltaMs >= 0)
                {
                    pluginClass.CallStatic
                        ( "SetNotification"
                        , id
                        , group
                        , timeDeltaMs
                        , title
                        , message
                        , message
                        , (long)repeatInterval.TotalMilliseconds
                        , showTime ? 1 : 0
                        , sound ? 1 : 0
                        , vibrationPattern
                        , (lightsColor.a << 24) | (lightsColor.r << 16) | (lightsColor.g << 8) | lightsColor.b
                        , lightsOnMs
                        , lightsOffMs
                        , bigIcon
                        , smallIcon
                        , (bgColor.a << 24) | (bgColor.r << 16) | (bgColor.g << 8) | bgColor.b
                        , (int)scheduleMode);
                }
                else
                {
                    Debug.LogWarningFormat("Passed notification date is in past ({0}), ignored.", when);
                }
            }
            else
            {
                ReportNoJavaClass();
            }
#endif
        }

        public static void CancelNotification (string id, bool cancelPending = true, bool cancelShown = true)
        {
#if USE_LOCAL_NOTIFICATIONS
            AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
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
                ReportNoJavaClass();
            }
#endif
        }

        public static void CancelAllShownNotifications ()
        {
#if USE_LOCAL_NOTIFICATIONS
            AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
            if (pluginClass != null)
            {
                pluginClass.CallStatic("CancelAllShownNotifications");
            }
            else
            {
                ReportNoJavaClass();
            }
#endif
        }

        private static void ReportNoJavaClass ()
        {
            Debug.LogErrorFormat("There's no Java class with a name '{0}' in a project. Local notifications will not work.", fullClassName);
        }
    }
}