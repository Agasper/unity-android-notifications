#if UNITY_IOS

using System;
using UnityEngine.iOS;
using UnifiedNotifications.Extensions;

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
                    NotificationServices.CancelLocalNotification(scheduledNotification);
                }
            }

            var notificationObject = PrepareNotificationObject(id, when, notification);

            NotificationServices.ScheduleLocalNotification(notificationObject);
        }

        public override void PresentLocalNotificationNow
            ( string id
            , ILocalNotification notification)
        {
            var notificationObject = PrepareNotificationObject(id, DateTime.Now, notification);

            NotificationServices.PresentLocalNotificationNow(notificationObject);
        }

        public override void CancelLocalNotification (string id, bool cancelPending, bool cancelShown)
        {
            // NOTE: cancelShown is not supported on iOS, only all shown notifications can be cleared using ClearLocalNotifications(?, true);

            if (cancelPending)
            {
                var scheduledNotification = FindScheduledNotificationById(id);
                if (scheduledNotification != null)
                {
                    NotificationServices.CancelLocalNotification(scheduledNotification);
                }
            }
        }

        public override void ClearLocalNotifications (bool cancelPending, bool cancelShown)
        {
            if (cancelPending)
            {
                NotificationServices.CancelAllLocalNotifications();
            }

            if (cancelShown)
            {
                NotificationServices.ClearLocalNotifications();
            }
        }
        #endregion

        #region Private methods.
        private UnityEngine.iOS.LocalNotification FindScheduledNotificationById(string id)
        {
            var notifications = NotificationServices.scheduledLocalNotifications;
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
                        case RepeatInterval.Era:                notificationObject.repeatInterval = CalendarUnit.Era;               break;
                        case RepeatInterval.Year:               notificationObject.repeatInterval = CalendarUnit.Year;              break;
                        case RepeatInterval.Month:              notificationObject.repeatInterval = CalendarUnit.Month;             break;
                        case RepeatInterval.Day:                notificationObject.repeatInterval = CalendarUnit.Day;               break;
                        case RepeatInterval.Hour:               notificationObject.repeatInterval = CalendarUnit.Hour;              break;
                        case RepeatInterval.Minute:             notificationObject.repeatInterval = CalendarUnit.Minute;            break;
                        case RepeatInterval.Second:             notificationObject.repeatInterval = CalendarUnit.Second;            break;
                        case RepeatInterval.Week:               notificationObject.repeatInterval = CalendarUnit.Week;              break;
                        case RepeatInterval.Weekday:            notificationObject.repeatInterval = CalendarUnit.Weekday;           break;
                        case RepeatInterval.WeekdayOrdinal:     notificationObject.repeatInterval = CalendarUnit.WeekdayOrdinal;    break;
                        case RepeatInterval.Quarter:            notificationObject.repeatInterval = CalendarUnit.Quarter;           break;
                    }

                    switch (iosExtension.repeatCalendar)
                    {
                        case UnifiedNotifications.Extensions.CalendarIdentifier.GregorianCalendar:          notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.GregorianCalendar;       break;
                        case UnifiedNotifications.Extensions.CalendarIdentifier.BuddhistCalendar:           notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.BuddhistCalendar;        break;
                        case UnifiedNotifications.Extensions.CalendarIdentifier.ChineseCalendar:            notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ChineseCalendar;         break;
                        case UnifiedNotifications.Extensions.CalendarIdentifier.HebrewCalendar:             notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.HebrewCalendar;          break;
                        case UnifiedNotifications.Extensions.CalendarIdentifier.IslamicCalendar:            notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.IslamicCalendar;         break;
                        case UnifiedNotifications.Extensions.CalendarIdentifier.IslamicCivilCalendar:       notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.IslamicCivilCalendar;    break;
                        case UnifiedNotifications.Extensions.CalendarIdentifier.JapaneseCalendar:           notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.JapaneseCalendar;        break;
                        case UnifiedNotifications.Extensions.CalendarIdentifier.RepublicOfChinaCalendar:    notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.RepublicOfChinaCalendar; break;
                        case UnifiedNotifications.Extensions.CalendarIdentifier.PersianCalendar:            notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.PersianCalendar;         break;
                        case UnifiedNotifications.Extensions.CalendarIdentifier.IndianCalendar:             notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.IndianCalendar;          break;
                        case UnifiedNotifications.Extensions.CalendarIdentifier.ISO8601Calendar:            notificationObject.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ISO8601Calendar;         break;
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
