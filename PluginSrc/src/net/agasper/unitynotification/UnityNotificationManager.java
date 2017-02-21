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
import android.media.RingtoneManager;
import android.net.Uri;
import android.os.Build;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

public class UnityNotificationManager extends BroadcastReceiver
{
	private static final int ScheduleMode_None = 0;
	private static final int ScheduleMode_Exact = 1;
	private static final int ScheduleMode_AllowWhileIdle = 2;
	
    public static void SetNotification
    	( String id
    	, String group
    	, long delayMs
    	, int cancelPrevious
    	, String title
    	, String message
    	, String ticker
    	, long repeatMs
    	, int showTime
    	, int sound
    	, int vibrate
    	, int lightsColor
    	, int lightsOn
    	, int lightsOff
    	, String largeIconResource
    	, String smallIconResource
    	, int bgColor
    	, int scheduleMode)
    {
        Activity currentActivity = UnityPlayer.currentActivity;
        AlarmManager am = (AlarmManager)currentActivity.getSystemService(Context.ALARM_SERVICE);
        
        Intent intent = BuildAlarmIntent(currentActivity, id);
        intent.putExtra("id", id);
        intent.putExtra("group", group);
        intent.putExtra("ticker", ticker);
        intent.putExtra("title", title);
        intent.putExtra("message", message);
        intent.putExtra("color", bgColor);
        intent.putExtra("showTime", showTime == 1);
        intent.putExtra("sound", sound == 1);
        intent.putExtra("vibrate", vibrate == 1);
        intent.putExtra("lightsColor", lightsColor);
        intent.putExtra("lightsOn", lightsOn);
        intent.putExtra("lightsOff", lightsOff);
        intent.putExtra("l_icon", largeIconResource);
        intent.putExtra("s_icon", smallIconResource);
        
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.M)
        {
        	scheduleMode &= (~ScheduleMode_AllowWhileIdle); // Set to no "AllowWhileIdle" mode.
        }
        
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.KITKAT)
        {
        	scheduleMode &= (~ScheduleMode_Exact);
        }
        
        int getIntentFlags = (cancelPrevious == 1) ? PendingIntent.FLAG_CANCEL_CURRENT : 0;
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

    public static void CancelNotification(String id, int cancelPending, int cancelShown)
    {
        Activity currentActivity = UnityPlayer.currentActivity;
        
        if (cancelPending == 1)
        {
	        AlarmManager am = (AlarmManager)currentActivity.getSystemService(Context.ALARM_SERVICE);
	        
	        Intent intent = BuildAlarmIntent(currentActivity, id);
	        PendingIntent pendingIntent = PendingIntent.getBroadcast(currentActivity, 0, intent, PendingIntent.FLAG_NO_CREATE);
	
	        if (pendingIntent != null)
	        {
	            am.cancel(pendingIntent);
	            pendingIntent.cancel();
	        }
        }

        if (cancelShown == 1)
        {
        	NotificationManager notificationManager = (NotificationManager)currentActivity.getApplicationContext().getSystemService(Context.NOTIFICATION_SERVICE);
        	notificationManager.cancel(id, 0);
        }
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
    	String group = intent.getStringExtra("group");
    	
        String ticker = intent.getStringExtra("ticker");
        String title = intent.getStringExtra("title");
        String message = intent.getStringExtra("message");
        String s_icon = intent.getStringExtra("s_icon");
        String l_icon = intent.getStringExtra("l_icon");
        int color = intent.getIntExtra("color", 0);
        Boolean showTime = intent.getBooleanExtra("showTime", false);
        Boolean sound = intent.getBooleanExtra("sound", false);
        Boolean vibrate = intent.getBooleanExtra("vibrate", false);
        
        int lightsColor = intent.getIntExtra("lightsColor", 0);
        int lightsOn = intent.getIntExtra("lightsOn", 0);
        int lightsOff = intent.getIntExtra("lightsOff", 0);

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
        
        Notification.Builder builder = new Notification.Builder(context);
        
        builder.setContentIntent(contentIntent)
        	.setWhen(System.currentTimeMillis())
        	.setShowWhen(showTime)
        	.setAutoCancel(true)
        	.setContentTitle(title)
        	.setContentText(message);
        
        if (group != null && group.length() > 0)
        	builder.setGroup(group);
        
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP && color != 0)
        	builder.setColor(color);
        
        if (ticker != null && ticker.length() > 0)
            builder.setTicker(ticker);
               
		if (s_icon != null && s_icon.length() > 0)
			builder.setSmallIcon(res.getIdentifier(s_icon, "drawable", context.getPackageName()));
		
		if (l_icon != null && l_icon.length() > 0)
			builder.setLargeIcon(BitmapFactory.decodeResource(res, res.getIdentifier(l_icon, "drawable", context.getPackageName())));

        if (sound)
            builder.setSound(RingtoneManager.getDefaultUri(2));

        if (vibrate)
            builder.setVibrate(new long[] { 1000L, 1000L });

        if (lightsColor != 0 && lightsOn > 0 && lightsOff > 0)
            builder.setLights(lightsColor, lightsOn, lightsOff);
        
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
