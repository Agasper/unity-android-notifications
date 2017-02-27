using UnifiedNotifications.Extensions;

namespace UnifiedNotifications
{
    public interface ILocalNotification
    {
        ExtensionT GetExtension<ExtensionT> (bool createIfNotExists = true) where ExtensionT : INotificationExtension;

        string      title               { get; set; }
        string      message             { get; set; }

        bool        useSound            { get; set; }
    }
}
