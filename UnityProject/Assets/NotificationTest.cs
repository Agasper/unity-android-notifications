using UnityEngine;
using System.Collections;

public class NotificationTest : MonoBehaviour {

    float sleepUntil = 0;
	
	void OnGUI () {
        //Color is supported only in Android >= 5.0
        GUI.enabled = sleepUntil < Time.time;

        if (GUILayout.Button("5 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotification.SendNotification(1, 5, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
            sleepUntil = Time.time + 5;
        }

        if (GUILayout.Button("5 SECONDS BIG ICON", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotification.SendNotification(1, 5, "Title", "Long message text with big icon", new Color32(0xff, 0x44, 0x44, 255), true, true, true, "app_icon");
            sleepUntil = Time.time + 5;
        }

        if (GUILayout.Button("EVERY 5 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotification.SendRepeatingNotification(1, 5, 5, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
            sleepUntil = Time.time + 99999;
        }

        if (GUILayout.Button("10 SECONDS EXACT", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotification.SendNotification(1, 10, "Title", "Long exact message text", new Color32(0xff, 0x44, 0x44, 255), executeMode: LocalNotification.NotificationExecuteMode.ExactAndAllowWhileIdle);
            sleepUntil = Time.time + 10;
        }

        GUI.enabled = true;

        if (GUILayout.Button("STOP", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotification.CancelNotification(1);
            sleepUntil = 0;
        }
	}
}
