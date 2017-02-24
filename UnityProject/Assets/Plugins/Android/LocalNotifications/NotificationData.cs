using System;
using UnityEngine;

namespace Android.LocalNotifications
{
    public class NotificationData
    {
        #region Static readonly data.
        public static readonly Color32 noColor = new Color32(0, 0, 0, 0); // Means: no explicit color to set.
        public static readonly long noCustomWhen = long.MinValue;
        public static readonly DateTime unixEpochDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        #endregion

        #region Types.
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
        #endregion

        #region Public properties.
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

        public DateTime customWhenDate
        {
            get
            {
                return ((customWhen != noCustomWhen)
                    ? unixEpochDate + TimeSpan.FromMilliseconds(customWhen)
                    : unixEpochDate);
            }
        }

        public long customWhenChronometer
        {
            get
            {
                return customWhen;
            }
        }

        #endregion

        #region Private 
        public long customWhen              { get; private set; }
        #endregion

        #region Constructor.
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
        #endregion

        #region Public API.
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

        public NotificationData SetCustomWhenDate(DateTime customWhen)
        {
            this.customWhen = Convert.ToInt64((customWhen.ToUniversalTime() - unixEpochDate).TotalMilliseconds);
            this.whenIsChronometer = false;
            return this;
        }

        public NotificationData SetCustomWhenChronometer(long chronometerValue)
        {
            this.customWhen = chronometerValue;
            this.whenIsChronometer = true;
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
        #endregion
    }
}
