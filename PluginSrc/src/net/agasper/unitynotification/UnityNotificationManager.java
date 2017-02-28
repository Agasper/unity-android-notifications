package net.agasper.unitynotification;

import android.app.Activity;
import android.app.AlarmManager;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.res.Resources;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Build;
import android.util.Log;

import android.support.v4.app.*;

import com.unity3d.player.UnityPlayer;

public class UnityNotificationManager extends BroadcastReceiver
{
	private static final int ScheduleMode_None = 0;
	private static final int ScheduleMode_Exact = 1;
	private static final int ScheduleMode_AllowWhileIdle = 2;
	
    public static void SetNotification
    	( String id
    	, long delayMs
    	, long repeatMs
    	, boolean cancelPrevious
    	, NotificationData data
    	, int scheduleMode)
    {
    	if (id == null || id.length() == 0)
    	{
    		Log.e(UnityNotificationManager.class.getSimpleName(), "'id' parameter is null or empty string.");
    		return;
    	}

    	if (delayMs <= 0)
    	{
    		Log.w(UnityNotificationManager.class.getSimpleName(), "'delayMs' parameter must be greater than 0.");
    		return;
    	}
    	
    	if (data == null)
    	{
    		Log.e(UnityNotificationManager.class.getSimpleName(), "'data' parameter is null.");
    		return;
    	}
    	
        Activity currentActivity = UnityPlayer.currentActivity;
        AlarmManager am = (AlarmManager)currentActivity.getSystemService(Context.ALARM_SERVICE);
        
        Intent intent = BuildAlarmIntent(currentActivity, id);
        intent.putExtra("id", id);
        intent.putExtra("data", data);
        
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.M)
        {
        	scheduleMode &= (~ScheduleMode_AllowWhileIdle); // Set to no "AllowWhileIdle" mode.
        }
        
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.KITKAT)
        {
        	scheduleMode &= (~ScheduleMode_Exact);
        }
        
        int getIntentFlags = (cancelPrevious) ? PendingIntent.FLAG_CANCEL_CURRENT : 0;
        long alarmTime = System.currentTimeMillis() + delayMs;
        
        if (repeatMs > 0)
        {
        	if ((scheduleMode & ScheduleMode_Exact) != 0)
        	{
            	am.setRepeating(0, alarmTime, repeatMs, PendingIntent.getBroadcast(currentActivity, 0, intent, getIntentFlags));
        	}
        	else
        	{
            	am.setInexactRepeating(0, alarmTime, repeatMs, PendingIntent.getBroadcast(currentActivity, 0, intent, getIntentFlags));
        	}
        }
        else
        {
	        switch (scheduleMode)
	        {
	        	case ScheduleMode_None:
	        		am.set(0, alarmTime, PendingIntent.getBroadcast(currentActivity, 0, intent, getIntentFlags));
	        		break;
	        		
	        	case ScheduleMode_Exact:
	        		am.setExact(0, alarmTime, PendingIntent.getBroadcast(currentActivity, 0, intent, getIntentFlags));
	        		break;
	        		
	        	case ScheduleMode_AllowWhileIdle:
	        		am.setAndAllowWhileIdle(0, alarmTime, PendingIntent.getBroadcast(currentActivity, 0, intent, getIntentFlags));
	        		break;
	
	        	case (ScheduleMode_Exact | ScheduleMode_AllowWhileIdle):
	        		am.setExactAndAllowWhileIdle(0, alarmTime, PendingIntent.getBroadcast(currentActivity, 0, intent, getIntentFlags));
	        		break;
			}
        }
    }

    public static boolean CancelNotification(String id, boolean cancelPending, boolean cancelShown)
    {
    	boolean anyCanceled = false;
    	
        Activity currentActivity = UnityPlayer.currentActivity;
        
        if (cancelPending)
        {
	        AlarmManager am = (AlarmManager)currentActivity.getSystemService(Context.ALARM_SERVICE);
	        
	        Intent intent = BuildAlarmIntent(currentActivity, id);
	        PendingIntent pendingIntent = PendingIntent.getBroadcast(currentActivity, 0, intent, PendingIntent.FLAG_NO_CREATE);
	
	        if (pendingIntent != null)
	        {
	            am.cancel(pendingIntent);
	            pendingIntent.cancel();
	            anyCanceled = true;
	        }
        }

        if (cancelShown)
        {
        	NotificationManager notificationManager = (NotificationManager)currentActivity.getApplicationContext().getSystemService(Context.NOTIFICATION_SERVICE);
        	notificationManager.cancel(id, 0);
        }
        
        return anyCanceled;
    }

    public static void CancelAllShownNotifications()
    {
        NotificationManager notificationManager = (NotificationManager)UnityPlayer.currentActivity.getApplicationContext().getSystemService(Context.NOTIFICATION_SERVICE);
        notificationManager.cancelAll();
    }

    public void onReceive(Context context, Intent intent)
    {
    	NotificationManager notificationManager = (NotificationManager)context.getSystemService(Context.NOTIFICATION_SERVICE);

    	String id = intent.getStringExtra("id");
    	NotificationData data = (NotificationData)intent.getSerializableExtra("data");
    	
        Resources res = context.getResources();

        Class<?> unityClassActivity;
		try
		{
			unityClassActivity = GetMainActivityClassName(context);
		}
		catch (ClassNotFoundException e)
		{
			return;
		}

        Intent notificationIntent = new Intent(context, unityClassActivity);
        PendingIntent contentIntent = PendingIntent.getActivity(context, 0, notificationIntent, 0);
        
        NotificationCompat.Builder builder = new NotificationCompat.Builder(context);
        
        int defaultsFlags = 0;
        
        builder.setContentIntent(contentIntent)
        	.setShowWhen(data.showWhen)
        	.setAutoCancel(data.autoCancel)
        	.setContentTitle(data.title)
        	.setContentText(data.message)
        	.setLocalOnly(data.localOnly)
        	.setOngoing(data.ongoing)
        	.setOnlyAlertOnce(data.onlyAlertOnce)
        	.setPriority(data.priority);
        
        if (data.subText != null && data.subText.length() > 0)
        {
        	builder.setSubText(data.subText);
        }
        else if (data.progressMax > 0)
        {
        	builder.setProgress(data.progressMax, data.progress, data.progressIndeterminate);
        }
        
        if (data.useCustomWhen)
        	builder.setWhen(data.customWhen);
        else
        	builder.setWhen(System.currentTimeMillis());
        	
        if (data.group != null && data.group.length() > 0)
        {
        	builder.setGroupSummary(data.isGroupSummary);
        	builder.setGroup(data.group);
        }
        
        builder.setUsesChronometer(data.whenIsChronometer);
        
        // A bit of hackery: EXTRA_CHRONOMETER_COUNT_DOWN is a relatively new feature (API 24+) and it doesn't have explicit function in NotificationCompat.Builder yet.
        if (data.chronometerCountdown)
        	builder.getExtras().putBoolean("android.chronometerCountDown", data.chronometerCountdown);
        
        if (data.color != 0)
        	builder.setColor(data.color);
        
        if (data.ticker != null && data.ticker.length() > 0)
            builder.setTicker(data.ticker);
               
		if (data.smallIconResource != null && data.smallIconResource.length() > 0)
			builder.setSmallIcon(res.getIdentifier(data.smallIconResource, "drawable", context.getPackageName()));
		
		if (data.largeIconResource != null && data.largeIconResource.length() > 0)
			builder.setLargeIcon(BitmapFactory.decodeResource(res, res.getIdentifier(data.largeIconResource, "drawable", context.getPackageName())));

        if (data.useSound)
        {
        	if (data.customSoundName != null && data.customSoundName.length() > 0)
        	{
        		builder.setSound(Uri.parse("android.resource://" + context.getPackageName() + "/raw/" + data.customSoundName));
        	}
        	else
        	{
                // builder.setSound(RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION));
        		defaultsFlags |= Notification.DEFAULT_SOUND;
        	}
        }

        if (data.useVibration)
        {
	        if (data.vibrationPattern != null)
	        {
	        	long[] tmpArray = new long[data.vibrationPattern.length];
	        	for (int i = 0; i < data.vibrationPattern.length; i++)
	        		tmpArray[i] = data.vibrationPattern[i];
	        	
	            builder.setVibrate(tmpArray);
	        }
	        else
	        {
	        	defaultsFlags |= Notification.DEFAULT_VIBRATE;
	        }
        }

        if (data.useLights)
        {
        	if (data.lightsColor != 0 && data.lightsOn > 0 && data.lightsOff > 0)
        	{
        		builder.setLights(data.lightsColor, data.lightsOn, data.lightsOff);
        	}
        	else
        	{
        		defaultsFlags |= Notification.DEFAULT_LIGHTS;
        	}
        }
        
        if (data.person != null && data.person.length() > 0)
        	builder.addPerson(data.person);
        
        if (data.category != NotificationData.CATEGORY_NONE)
        {
        	switch (data.category)
        	{
        		case NotificationData.CATEGORY_ALARM: 			builder.setCategory(Notification.CATEGORY_ALARM); break;
        		case NotificationData.CATEGORY_CALL: 			builder.setCategory(Notification.CATEGORY_CALL); break;
        		case NotificationData.CATEGORY_EMAIL: 			builder.setCategory(Notification.CATEGORY_EMAIL); break;
        		case NotificationData.CATEGORY_ERROR: 			builder.setCategory(Notification.CATEGORY_ERROR); break;
        		case NotificationData.CATEGORY_EVENT: 			builder.setCategory(Notification.CATEGORY_EVENT); break;
        		case NotificationData.CATEGORY_MESSAGE: 		builder.setCategory(Notification.CATEGORY_MESSAGE); break;
        		case NotificationData.CATEGORY_PROGRESS: 		builder.setCategory(Notification.CATEGORY_PROGRESS); break;
        		case NotificationData.CATEGORY_PROMO: 			builder.setCategory(Notification.CATEGORY_PROMO); break;
        		case NotificationData.CATEGORY_RECOMMENDATION: 	builder.setCategory(Notification.CATEGORY_RECOMMENDATION); break;
        		case NotificationData.CATEGORY_REMINDER: 		builder.setCategory(Notification.CATEGORY_REMINDER); break;
        		case NotificationData.CATEGORY_SERVICE: 		builder.setCategory(Notification.CATEGORY_SERVICE); break;
        		case NotificationData.CATEGORY_SOCIAL: 			builder.setCategory(Notification.CATEGORY_SOCIAL); break;
        		case NotificationData.CATEGORY_STATUS: 			builder.setCategory(Notification.CATEGORY_STATUS); break;
        		case NotificationData.CATEGORY_SYSTEM: 			builder.setCategory(Notification.CATEGORY_SYSTEM); break;
        		case NotificationData.CATEGORY_TRANSPORT: 		builder.setCategory(Notification.CATEGORY_TRANSPORT); break; 
        	}
        }
        
        builder.setDefaults(defaultsFlags);
        
		Notification notification = builder.build();
        notificationManager.notify(id, 0, notification);
    }
    
    private static Intent BuildAlarmIntent(Activity currentActivity, String id)
    {
        Intent intent = new Intent(currentActivity, UnityNotificationManager.class);
        intent.setData(Uri.parse("notification://" + id));
        return intent;
    }
    
    private static Class<?> GetMainActivityClassName(Context context) throws ClassNotFoundException
    {
    	String packageName = context.getPackageName();
    	Intent launchIntent = context.getPackageManager().getLaunchIntentForPackage(packageName);
    	String className = launchIntent.getComponent().getClassName();
    	
		try 
		{
			return Class.forName(className);
		}
		catch (ClassNotFoundException e) 
		{
			Log.e(UnityNotificationManager.class.getSimpleName(), Log.getStackTraceString(e));
			throw e;
		}
    }
}
