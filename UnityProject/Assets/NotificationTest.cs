using UnityEngine;
using System.Collections;

public class NotificationTest : MonoBehaviour {

	
	void OnGUI () {
        //Color is supported only in Android >= 5.0

		if (GUILayout.Button("5 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
			LocalNotification.SendNotification(1, 5, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));

        if (GUILayout.Button("5 SECONDS BIG ICON", GUILayout.Height(Screen.height * 0.2f)))
            LocalNotification.SendNotification(2, 5, "Title", "Long message text with big icon", new Color32(0xff, 0x44, 0x44, 255), true, true, true, "app_icon");

        if (GUILayout.Button("EVERY 5 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
			LocalNotification.SendRepeatingNotification(3, 5, 5, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
			
		if (GUILayout.Button("STOP",GUILayout.Height(Screen.height * 0.2f)))
			LocalNotification.CancelNotification(1);
	}
}
