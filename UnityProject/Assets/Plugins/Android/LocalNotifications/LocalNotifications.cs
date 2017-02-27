#define FORCE_LOCAL_NOTIFICATIONS

#if (UNITY_ANDROID && !UNITY_EDITOR) || FORCE_LOCAL_NOTIFICATIONS
    #define USE_LOCAL_NOTIFICATIONS
#endif

using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace Android.LocalNotifications
{
    public class LocalNotification
    {
        #region Types.
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
        #endregion

        #region Static readonly data.
#if USE_LOCAL_NOTIFICATIONS
        private static readonly string managerClassName = "net.agasper.unitynotification.UnityNotificationManager";
        private static readonly string notificationDataClassName = "net.agasper.unitynotification.NotificationData";
#endif
        #endregion

        #region Public static API.
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

            long timeDeltaMs = Math.Max(0, (long)(when.ToUniversalTime() - DateTime.UtcNow).TotalMilliseconds);

            pluginClass.CallStatic
                ( "SetNotification"
                , id
                , timeDeltaMs
                , (repeatInterval != TimeSpan.Zero) ? (long)repeatInterval.TotalMilliseconds : 0
                , cancelPrevious
                , dataObject
                , (int)scheduleMode);
#endif
        }

        public static bool CancelNotification (string id, bool cancelPending = true, bool cancelShown = true)
        {
#if USE_LOCAL_NOTIFICATIONS
            AndroidJavaClass pluginClass = new AndroidJavaClass(managerClassName);
            if (pluginClass != null)
            {
                var anyScheduledCanceled = pluginClass.CallStatic<bool>
                    ("CancelNotification"
                    , id
                    , cancelPending
                    , cancelShown);

                return anyScheduledCanceled;
            }
            else
            {
                ReportNoJavaClass(managerClassName);
            }
#endif
            return false;
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
        #endregion

        #region Private methods.
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
                dataObject.Set("title",         data.title);
                dataObject.Set("message",       data.message);
                dataObject.Set("ticker",        data.ticker);

                if (data.subText != null)
                {
                    dataObject.Set("subText",   data.subText);
                }
                else if (data.progressMax > 0)
                {
                    dataObject.Set("progressMax", data.progressMax);
                    dataObject.Set("progress",  data.progress);
                    dataObject.Set("progressIndeterminate", data.progressIndeterminate);
                }

                if (data.group != null)
                {
                    dataObject.Set("group",         data.group);
                    dataObject.Set("isGroupSummary", data.isGroupSummary);
                }

                dataObject.Set("category",      data.category);

                dataObject.Set("autoCancel",    data.autoCancel);
                dataObject.Set("localOnly",     data.localOnly);
                dataObject.Set("ongoing",       data.ongoing);
                dataObject.Set("onlyAlertOnce", data.onlyAlertOnce);
                dataObject.Set("priority",      (int)data.priority);

                dataObject.Set("sound",         data.sound);

                dataObject.Set("showWhen",      data.showWhen);
                if (data.customWhen != NotificationData.noCustomWhen)
                {
                    dataObject.Set("useCustomWhen", true);
                    dataObject.Set("customWhen", data.customWhen);
                }

                dataObject.Set("whenIsChronometer", data.whenIsChronometer);
                dataObject.Set("chronometerCountdown", data.chronometerCountdown);

                dataObject.Set("color",         (data.color.a << 24) | (data.color.r << 16) | (data.color.g << 8) | data.color.b);

                if (data.vibrationPattern != null)
                    dataObject.Call("SetVibrationPattern", data.vibrationPattern);

                dataObject.Set("lightsColor",   (data.lightsColor.a << 24) | (data.lightsColor.r << 16) | (data.lightsColor.g << 8) | data.lightsColor.b);
                dataObject.Set("lightsOn",      data.lightsOn);
                dataObject.Set("lightsOff",     data.lightsOff);

                dataObject.Set("smallIconResource", data.smallIconResource);
                dataObject.Set("largeIconResource", data.largeIconResource);

                dataObject.Set("person",        data.person);
            }
            else
            {
                ReportNoJavaClass(notificationDataClassName);
            }
            return dataObject;
        }
#endif
        #endregion
    }
}