#if UNITY_IOS

using System;
using UnifiedNotifications.Extensions.iOS;

namespace UnifiedNotifications.Private
{
    public class NotificationsServiceManagerIOS : NotificationsServiceManagerBase
    {
        #region INotificationsServiceManager API.
        public override void ScheduleLocalNotification
            ( string id
            , DateTime when
            , ILocalNotification notification
            , bool cancelPrevious)
        {
            if (cancelPrevious)
            {
                var scheduledNotification = FindScheduledNotificationById(id);
                if (scheduledNotification != null)
                {
                    UnityEngine.iOS.NotificationServices.CancelLocalNotification(scheduledNotification);
                }
            }

            var notificationObject = PrepareNotificationObject(id, when, notification);

            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notificationObject);
        }

        public override void PresentLocalNotificationNow
            ( string id
            , ILocalNotification notification)
        {
            var notificationObject = PrepareNotificationObject(id, DateTime.Now, notification);

            UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow(notificationObject);
        }

        public override void CancelLocalNotification (string id, bool cancelPending, bool cancelShown)
        {
            // NOTE: cancelShown is not supported on iOS, only all shown notifications can be cleared using ClearLocalNotifications(?, true);

            if (cancelPending)
            {
                var scheduledNotification = FindScheduledNotificationById(id);
                if (scheduledNotification != null)
                {
                    UnityEngine.iOS.NotificationServices.CancelLocalNotification(scheduledNotification);
                }
            }
        }

        public override void ClearLocalNotifications (bool cancelPending, bool cancelShown)
        {
            if (cancelPending)
            {
                UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
            }

            if (cancelShown)
            {
                UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
            }
        }
        #endregion

        #region Private methods.
        private UnityEngine.iOS.LocalNotification FindScheduledNotificationById(string id)
        {
            var notifications = UnityEngine.iOS.NotificationServices.scheduledLocalNotifications;
            for (int i = 0; i < notifications.Length; i++)
            {
                var notification = notifications[i];
                if (notification != null && notification.userInfo != null && notification.userInfo.Contains("id"))
                {
                    var idValue = notification.userInfo["id"];
                    if (idValue != null && idValue is string && (string)idValue == id)
                    {
                        return notification;
                    }
                }
            }

            return null;
        }

        private UnityEngine.iOS.LocalNotification PrepareNotificationObject (string id, DateTime when, ILocalNotification notification)
        {
            UnityEngine.iOS.LocalNotification notificationObject = new UnityEngine.iOS.LocalNotification();

            notificationObject.userInfo.Add("id", id);

            notificationObject.fireDate = when;
            notificationObject.alertAction = notification.message;
            notificationObject.alertBody = notification.message;

            var iosExtension = notification.GetExtension<INotificationExtensionIOS>(false);
            if (iosExtension != null)
            {
                notificationObject.hasAction = iosExtension.hasAction;
                notificationObject.applicationIconBadgeNumber = iosExtension.applicationIconBadgeNumber;
                
                if (notification.useSound)
                {
                    if (iosExtension.soundName != null)
                    {
                        notificationObject.soundName = iosExtension.soundName;
                    }
                    else
                    {
                        notificationObject.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
                    }
                }

                if (iosExtension.repeatInterval != RepeatInterval.None)
                {
                    switch (iosExtension.repeatInterval)
                    {
                        case RepeatInterval.Era:                notificationObject.repeatInterval = UnityEngine.iOS.CalendarUnit.Era;               break;
                        case RepeatInterval.Year:               notificationObject.repeatInterval = UnityEngine.iOS.CalendarUnit.Year;              break;
                        case RepeatInterval.Month:              notificationObject.repeatInterval = UnityEngine.iOS.CalendarUnit.Month;             break;
                        case RepeatInterval.Day:                notificationObject.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;               break;
                        case RepeatInterval.Hour:               notificationObject.repeatInterval = UnityEngine.iOS.CalendarUnit.Hour;              break;
                        case RepeatInterval.Minute:             notificationObject.repeatInterval = UnityEngine.iOS.CalendarUnit.Minute;            break;
                        case RepeatInterval.Second:             notificationObject.repeatInterval = UnityEngine.iOS.CalendarUnit.Second;            break;
                        case RepeatInterval.Week:               notificationObject.repeatInterval = UnityEngine.iOS.CalendarUnit.Week;              break;
                        case RepeatInterval.Weekday:            notificationObject.repeatInterval = UnityEngine.iOS.CalendarUnit.Weekday;           break;
                        case RepeatInterval.WeekdayOrdinal:     notificationObject.repeatInterval = UnityEngine.iOS.CalendarUnit.WeekdayOrdinal;    break;
                        case RepeatInterval.Quarter:            notificationObject.repeatInterval = UnityEngine.iOS.CalendarUnit.Quarter;           break;
                    }

                    switch (iosExtension.repeatCalendar)
                    {
                        case CalendarIdentifier.GregorianCalendar:          notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.GregorianCalendar;       break;
                        case CalendarIdentifier.BuddhistCalendar:           notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.BuddhistCalendar;        break;
                        case CalendarIdentifier.ChineseCalendar:            notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ChineseCalendar;         break;
                        case CalendarIdentifier.HebrewCalendar:             notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.HebrewCalendar;          break;
                        case CalendarIdentifier.IslamicCalendar:            notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.IslamicCalendar;         break;
                        case CalendarIdentifier.IslamicCivilCalendar:       notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.IslamicCivilCalendar;    break;
                        case CalendarIdentifier.JapaneseCalendar:           notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.JapaneseCalendar;        break;
                        case CalendarIdentifier.RepublicOfChinaCalendar:    notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.RepublicOfChinaCalendar; break;
                        case CalendarIdentifier.PersianCalendar:            notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.PersianCalendar;         break;
                        case CalendarIdentifier.IndianCalendar:             notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.IndianCalendar;          break;
                        case CalendarIdentifier.ISO8601Calendar:            notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ISO8601Calendar;         break;
                    }
                }
            }
            else
            {
                if (notification.useSound)
                {
                    notificationObject.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
                }
            }

            return notificationObject;
        }
        #endregion
    }
}
#endif
