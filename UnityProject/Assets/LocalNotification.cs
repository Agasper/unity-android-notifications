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
        public static readonly DateTime noCustomWhen = DateTime.MinValue;

        public enum Category
        {
            None            = 0,    // No explicit category. Default.
            Alarm           = 1,    // Notification category: alarm or timer.
            Call            = 2,    // Notification category: incoming call (voice or video) or similar synchronous communication request.
            Email           = 3,    // Notification category: asynchronous bulk message (email).
            Error           = 4,    // Notification category: error in background operation or authentication status.
            Event           = 5,    // Notification category: calendar event.
            Message         = 6,    // Notification category: incoming direct message (SMS, instant message, etc.).
            Progress        = 7,    // Notification category: progress of a long-running background operation.
            Promo           = 8,    // Notification category: promotion or advertisement.
            Recommendation  = 9,    // Notification category: a specific, timely recommendation for a single thing.
            Reminder        = 10,   // Notification category: user-scheduled reminder.
            Service         = 11,   // Notification category: indication of running background service.
            Social          = 12,   // Notification category: social network or sharing update.
            Status          = 13,   // Notification category: ongoing information about device or contextual status.
            System          = 14,   // Notification category: system or device status update.
            Transport       = 15    // Notification category: media transport control for playback. 
        }

        public enum Priority
        {
            Min = -2,
            Low = -1,
            Default = 0,
            High = 1,
            Max = 2
        }

        public string   title               { get; private set; }
        public string   message             { get; private set; }
        public string   ticker              { get; private set; }

        public string   subText             { get; private set; }
        public int      progressMax         { get; private set; }
        public int      progress            { get; private set; }
        public bool     progressIndeterminate { get; private set; }

        public string   group               { get; private set; }
        public bool     isGroupSummary      { get; private set; }
        public Category category            { get; private set; }

        public bool     autoCancel          { get; private set; }
        public bool     localOnly           { get; private set; }
        public bool     ongoing             { get; private set; }
        public bool     onlyAlertOnce       { get; private set; }
        public Priority priority            { get; private set; }

        public bool     sound               { get; private set; }

        public bool     showWhen            { get; private set; }
        public DateTime customWhen          { get; private set; }
        public bool     whenIsChronometer   { get; private set; }
        public bool     chronometerCountdown { get; private set; }

        public Color32  color               { get; private set; }

        public int[]    vibrationPattern    { get; private set; }

        public Color32  lightsColor         { get; private set; }
        public int      lightsOn            { get; private set; } // In milliseconds.
        public int      lightsOff           { get; private set; } // In milliseconds.

        public string   smallIconResource   { get; private set; }
        public string   largeIconResource   { get; private set; }

        public string   person              { get; private set; } // Person URI reference.

        public NotificationData (string title, string message)
        {
            this.title = title;
            this.message = message;
            this.ticker = null;

            this.subText = null;
            this.progressMax = 0;
            this.progress = 0;
            this.progressIndeterminate = false;

            this.group = null;
            this.isGroupSummary = false;
            this.category = Category.None;

            this.autoCancel = true;
            this.localOnly = false;
            this.ongoing = false;
            this.onlyAlertOnce = false;
            this.priority = Priority.Default;

            this.sound = true;

            this.showWhen = true;
            this.customWhen = noCustomWhen;
            this.whenIsChronometer = false;
            this.chronometerCountdown = false;

            this.color = noColor;

            this.vibrationPattern = null;

            this.lightsColor = noColor;
            this.lightsOn = 1000;
            this.lightsOff = 3000;

            this.smallIconResource = "notify_icon_small";
            this.largeIconResource = null;

            this.person = null;
        }

        public NotificationData SetTitle(string title)
        {
            this.title = title;
            return this;
        }

        public NotificationData SetMessage(string message)
        {
            this.message = message;
            return this;
        }

        public NotificationData SetTicker(string ticker)
        {
            this.ticker = ticker;
            return this;
        }

        public NotificationData SetSubText(string subText)
        {
            this.subText = subText;
            return this;
        }

        public NotificationData SetProgressMax(int progressMax)
        {
            this.progressMax = progressMax;
            return this;
        }

        public NotificationData SetProgress(int progress)
        {
            this.progress = progress;
            return this;
        }

        public NotificationData SetProgressIndeterminate(bool progressIndeterminate)
        {
            this.progressIndeterminate = progressIndeterminate;
            return this;
        }

        public NotificationData SetGroup(string group)
        {
            this.group = group;
            return this;
        }

        public NotificationData SetIsGroupSummary(bool isGroupSummary)
        {
            this.isGroupSummary = isGroupSummary;
            return this;
        }

        public NotificationData SetCategory(Category category)
        {
            this.category = category;
            return this;
        }

        public NotificationData SetAutoCancel(bool autoCancel)
        {
            this.autoCancel = autoCancel;
            return this;
        }

        public NotificationData SetLocalOnly(bool localOnly)
        {
            this.localOnly = localOnly;
            return this;
        }

        public NotificationData SetOngoing(bool ongoing)
        {
            this.ongoing = ongoing;
            return this;
        }

        public NotificationData SetOnlyAlertOnce(bool onlyAlertOnce)
        {
            this.onlyAlertOnce = onlyAlertOnce;
            return this;
        }

        public NotificationData SetPriority(Priority priority)
        {
            this.priority = priority;
            return this;
        }

        public NotificationData SetSound(bool sound)
        {
            this.sound = sound;
            return this;
        }

        public NotificationData SetShowWhen(bool showTime)
        {
            this.showWhen = showTime;
            return this;
        }

        public NotificationData SetCustomWhen(DateTime customWhen)
        {
            this.customWhen = customWhen;
            return this;
        }

        public NotificationData SetWhenIsChronometer(bool whenIsChronometer)
        {
            this.whenIsChronometer = whenIsChronometer;
            return this;
        }

        public NotificationData SetChronometerCountdown(bool chronometerCountdown)
        {
            this.chronometerCountdown = chronometerCountdown;
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

        public NotificationData SetPerson(string person)
        {
            this.person = person;
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
                    var unixEpochDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    var customWhen = Convert.ToInt64((data.customWhen.ToUniversalTime() - unixEpochDate).TotalMilliseconds);

                    dataObject.Set("useCustomWhen", true);
                    dataObject.Set("customWhen", customWhen);
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
    }
}