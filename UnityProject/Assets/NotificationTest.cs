using UnityEngine;
using System.Collections;

public class NotificationTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () {
		if (GUILayout.Button("5 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
			LocalNotification.SendNotification(1, 5, "Title", "Long message text");
			
		if (GUILayout.Button("EVERY 5 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
			LocalNotification.SendRepeatingNotification(1, 5, 5, "Title", "Long message text");
			
		if (GUILayout.Button("STOP",GUILayout.Height(Screen.height * 0.2f)))
			LocalNotification.CancelNotification(1);
	}
}
