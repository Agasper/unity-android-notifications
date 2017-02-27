using System;

namespace UnifiedNotifications.Private
{
    public abstract class NotificationsServiceManagerBase : INotificationsServiceManager
    {
        #region Public static API.
        public static INotificationsServiceManager Instantiate()
        {
#if UNITY_IOS
            return new Private.NotificationsServiceManagerIOS();
#elif UNITY_ANDROID
            return new Private.NotificationsServiceManagerAndroid();
#else
            return null;
#endif
        }
        #endregion

        #region INotificationsServiceManager API.
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
        #endregion
    }
}
