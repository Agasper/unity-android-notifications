using UnifiedNotifications.Extensions;

namespace UnifiedNotifications.Private.Extensions
{
    public class NotificationExtensionIOS : INotificationExtensionIOS
    {
        public bool                     hasAction                       { get; set; }
        public int                      applicationIconBadgeNumber      { get; set; }
        public string                   soundName                       { get; set; }

        public CalendarIdentifier       repeatCalendar                  { get; set; }
        public RepeatInterval           repeatInterval                  { get; set; }
    }
}
