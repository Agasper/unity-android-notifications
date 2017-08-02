using UnityEngine;
using System.Collections;

public class NotificationTest : MonoBehaviour
{
    void Awake()
    {
        LocalNotification.ClearNotifications();
    }

    public void OneTime()
    {
        LocalNotification.SendNotification(1, 5000, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
    }

    public void OneTimeBigIcon()
    {
        LocalNotification.SendNotification(1, 5000, "Title", "Long message text with big icon", new Color32(0xff, 0x44, 0x44, 255), true, true, true, "app_icon");
    }

    public void Repeating()
    {
        LocalNotification.SendRepeatingNotification(1, 5000, 5000, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
    }
        
    public void Stop()
    {
        LocalNotification.CancelNotification(1);
    }
}
