#if UNITY_ANDROID

using System;
using UnifiedNotifications.Extensions;
using UnityEngine;

namespace UnifiedNotifications.Private
{
    public class NotificationsServiceManagerAndroid : NotificationsServiceManagerBase
    {
        #region INotificationsServiceManager API.
        public override void ScheduleLocalNotification
            ( string id
            , DateTime when
            , ILocalNotification notification
            , bool cancelPrevious)
        {
            ScheduleMode scheduleMode = ScheduleMode.None;
            TimeSpan repeatInterval = TimeSpan.Zero;

            Android.LocalNotifications.NotificationData notificationObject = new Android.LocalNotifications.NotificationData(notification.title, notification.message);

            notificationObject.SetSound ( notification.useSound );

            var androidExtension = notification.GetExtension<INotificationExtensionAndroid>(false);
            if (androidExtension != null)
            {
                scheduleMode = androidExtension.scheduleMode;
                repeatInterval = androidExtension.repeatInterval;

                notificationObject.SetTicker(androidExtension.ticker);

                notificationObject.SetSubText(androidExtension.subText);
                notificationObject.SetProgressMax(androidExtension.progressMax);

                notificationObject.SetProgressMax(androidExtension.progressMax);
                notificationObject.SetProgress(androidExtension.progress);
                notificationObject.SetProgressIndeterminate(androidExtension.progressIndeterminate);

                notificationObject.SetGroup(androidExtension.group);
                notificationObject.SetIsGroupSummary(androidExtension.isGroupSummary);
                notificationObject.SetCategory((Android.LocalNotifications.NotificationData.Category)androidExtension.category);

                notificationObject.SetAutoCancel(androidExtension.autoCancel);
                notificationObject.SetLocalOnly(androidExtension.localOnly);
                notificationObject.SetOngoing(androidExtension.ongoing);
                notificationObject.SetOnlyAlertOnce(androidExtension.onlyAlertOnce);
                notificationObject.SetPriority((Android.LocalNotifications.NotificationData.Priority)androidExtension.priority);

                notificationObject.SetSound(androidExtension.sound);

                notificationObject.SetShowWhen(androidExtension.showWhen);
                notificationObject.SetWhenIsChronometer(androidExtension.whenIsChronometer);
                notificationObject.SetChronometerCountdown(androidExtension.chronometerCountdown);
                notificationObject.SetCustomWhenDate(androidExtension.customWhenDate);
                notificationObject.SetCustomWhenChronometer(androidExtension.customWhenChronometer);

                notificationObject.SetColor(androidExtension.color);

                notificationObject.SetVibrationPattern(androidExtension.vibrationPattern);

                notificationObject.SetLightsColor(androidExtension.lightsColor);
                notificationObject.SetLightsOn(androidExtension.lightsOn);
                notificationObject.SetLightsOff(androidExtension.lightsOff);

                notificationObject.SetSmallIconResource(androidExtension.smallIconResource);
                notificationObject.SetLargeIconResource(androidExtension.largeIconResource);

                notificationObject.SetPerson(androidExtension.person);
            }

            if (repeatInterval > TimeSpan.Zero)
            {
                Android.LocalNotifications.LocalNotification.SendNotification
                    ( id, when, notificationObject, cancelPrevious, (Android.LocalNotifications.LocalNotification.ScheduleMode)scheduleMode);
            }
            else
            {
                Android.LocalNotifications.LocalNotification.SendNotification
                    ( id, when, repeatInterval, notificationObject, cancelPrevious, (Android.LocalNotifications.LocalNotification.ScheduleMode)scheduleMode);
            }
        }

        public override void PresentLocalNotificationNow
            ( string id
            , ILocalNotification notification)
        {
            ScheduleLocalNotification(id, DateTime.UtcNow, notification, true);
        }

        public override void CancelLocalNotification (string id, bool cancelPending, bool cancelShown)
        {
            Android.LocalNotifications.LocalNotification.CancelNotification(id, cancelPending, cancelShown);
        }

        public override void ClearLocalNotifications (bool cancelPending, bool cancelShown)
        {
            // NOTE: cancelPending is not supported by Android, pending notifications must be explicitly canceled by 'id'.

            if (cancelShown)
            {
                Android.LocalNotifications.LocalNotification.CancelAllShownNotifications();
            }
        }
        #endregion
    }
}

#endif
