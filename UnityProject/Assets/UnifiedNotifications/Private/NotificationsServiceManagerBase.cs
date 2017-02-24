using System;

namespace UnifiedNotifications.Private
{
    public abstract class NotificationsServiceManagerBase : INotificationsServiceManager
    {
        public ILocalNotification CreateLocalNotification()
        {
            return new LocalNotification();
        }

        public abstract void ScheduleLocalNotification
            ( string id
            , DateTime when
            , ILocalNotification notification
            , bool cancelPrevious);

        public abstract void PresentLocalNotificationNow
            ( string id
            , ILocalNotification notification);

        public abstract void CancelLocalNotification (string id, bool cancelPending, bool cancelShown);
        public abstract void ClearLocalNotifications (bool cancelPending, bool cancelShown);
    }
}
