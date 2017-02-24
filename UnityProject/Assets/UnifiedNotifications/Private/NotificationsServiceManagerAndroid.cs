using System;

namespace UnifiedNotifications.Private
{
    public class NotificationsServiceManagerAndroid : NotificationsServiceManagerBase
    {
        public override void ScheduleLocalNotification
            ( string id
            , DateTime when
            , ILocalNotification notification
            , bool cancelPrevious)
        {
        }

        public override void PresentLocalNotificationNow
            ( string id
            , ILocalNotification notification)
        {
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
    }
}
