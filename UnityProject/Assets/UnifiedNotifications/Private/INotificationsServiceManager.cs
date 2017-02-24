using System;

namespace UnifiedNotifications.Private
{
    public interface INotificationsServiceManager
    {
        ILocalNotification CreateLocalNotification();

        void ScheduleLocalNotification
            ( string id
            , DateTime when
            , ILocalNotification notification
            , bool cancelPrevious = false);

        void PresentLocalNotificationNow
            ( string id
            , ILocalNotification notification);

        void CancelLocalNotification (string id, bool cancelPending = true, bool cancelShown = true);
        void ClearLocalNotifications (bool cancelPending = true, bool cancelShown = true);
    }
}
