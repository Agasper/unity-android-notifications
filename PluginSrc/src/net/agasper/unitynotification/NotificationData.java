package net.agasper.unitynotification;

import java.io.Serializable;

public final class NotificationData implements Serializable
{
	private static final long serialVersionUID = -7733864630402717326L;

	public static final int CATEGORY_NONE 				= 0;
	public static final int CATEGORY_ALARM 				= 1; // Notification category: alarm or timer.
	public static final int CATEGORY_CALL 				= 2; //	Notification category: incoming call (voice or video) or similar synchronous communication request.
	public static final int CATEGORY_EMAIL				= 3; // Notification category: asynchronous bulk message (email).
	public static final int CATEGORY_ERROR				= 4; //	Notification category: error in background operation or authentication status.
	public static final int CATEGORY_EVENT				= 5; // Notification category: calendar event.
	public static final int CATEGORY_MESSAGE			= 6; // Notification category: incoming direct message (SMS, instant message, etc.).
	public static final int CATEGORY_PROGRESS			= 7; // Notification category: progress of a long-running background operation.
	public static final int CATEGORY_PROMO				= 8; // Notification category: promotion or advertisement.
	public static final int CATEGORY_RECOMMENDATION 	= 9; // Notification category: a specific, timely recommendation for a single thing.
	public static final int CATEGORY_REMINDER			= 10; // Notification category: user-scheduled reminder.
	public static final int CATEGORY_SERVICE			= 11; // Notification category: indication of running background service.
	public static final int CATEGORY_SOCIAL				= 12; // Notification category: social network or sharing update.
	public static final int CATEGORY_STATUS				= 13; // Notification category: ongoing information about device or contextual status.
	public static final int CATEGORY_SYSTEM				= 14; // Notification category: system or device status update.
	public static final int CATEGORY_TRANSPORT			= 15; // Notification category: media transport control for playback. 
	
	public String title;
	public String message;
	public String ticker;

	public String subText;
	public int progressMax;
	public int progress;
	public boolean progressIndeterminate;
	
	public String group;
	public boolean isGroupSummary;
	public int category;

	public boolean autoCancel;
	public boolean localOnly;
	public boolean ongoing;
	public boolean onlyAlertOnce;
	public int priority;
	
	public boolean sound;

	public boolean showWhen;
	public boolean useCustomWhen;
	public long customWhen;
	public boolean whenIsChronometer;
	public boolean chronometerCountdown;
	
	public int color;
	
	public int[] vibrationPattern;

	public int lightsColor;
	public int lightsOn;
	public int lightsOff;
	
	public String smallIconResource;
	public String largeIconResource;
	
	public String person; 
	
	public NotificationData()
	{
		title = null;
		message = null;
		ticker = null;
		
		subText = null;
		progressMax = 0;
		progress = 0;
		progressIndeterminate = false;

		group = null;
		isGroupSummary = false;
		category = CATEGORY_NONE;

		autoCancel = true;
		localOnly = false;
		ongoing = false;
		onlyAlertOnce = false;
		priority = 0; // Notification.PRIORITY_DEFAULT;
		
		sound = true;

		showWhen = true;
		useCustomWhen = false;
		customWhen = 0;
		whenIsChronometer = false;
		chronometerCountdown = false;
		
		color = 0;
		
		vibrationPattern = null;

		lightsColor = 0;
		lightsOn = 1000;
		lightsOff = 3000;
		
		largeIconResource = null;
		smallIconResource = null;
		
		person = null;
	}
	
	public void SetVibrationPattern (int[] pattern)
	{
		vibrationPattern = pattern;
	}
}
