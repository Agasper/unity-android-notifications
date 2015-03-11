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
import android.graphics.Color;
import android.media.RingtoneManager;

import com.unity3d.player.UnityPlayer;
//import com.unity3d.player.UnityPlayerNativeActivity;

public class UnityNotificationManager extends BroadcastReceiver
{

	public static void SetNotification(int id, String unityClass, long delay, String title, String message, String ticker, int sound, int vibrate, int lights)
	{
		SetNotification(id, unityClass, delay, title, message, ticker, sound, vibrate, lights, "notify_icon_big", "notify_icon_small");
	}

    public static void SetNotification(int id, String unityClass, long delay, String title, String message, String ticker, int sound, int vibrate, 
            int lights, String largeIconResource, String smallIconResource)
    {
        Activity currentActivity = UnityPlayer.currentActivity;
        AlarmManager am = (AlarmManager)currentActivity.getSystemService(Context.ALARM_SERVICE);
        Intent intent = new Intent(currentActivity, UnityNotificationManager.class);
        intent.putExtra("ticker", ticker);
        intent.putExtra("title", title);
        intent.putExtra("message", message);
        intent.putExtra("id", id);
        intent.putExtra("sound", sound == 1);
        intent.putExtra("vibrate", vibrate == 1);
        intent.putExtra("lights", lights == 1);
        intent.putExtra("l_icon", largeIconResource);
        intent.putExtra("s_icon", smallIconResource);
        intent.putExtra("unityClass", unityClass);
        am.set(0, System.currentTimeMillis() + delay, PendingIntent.getBroadcast(currentActivity, id, intent, 0));
    }
    
    public static void SetRepeatingNotification(int id, String unityClass, long delay, String title, String message, String ticker, long rep, int sound, int vibrate, int lights)
    {
    	Activity currentActivity = UnityPlayer.currentActivity;
        AlarmManager am = (AlarmManager)currentActivity.getSystemService(Context.ALARM_SERVICE);
        Intent intent = new Intent(currentActivity, UnityNotificationManager.class);
        intent.putExtra("ticker", ticker);
        intent.putExtra("title", title);
        intent.putExtra("message", message);
        intent.putExtra("id", id);
        intent.putExtra("sound", sound == 1);
        intent.putExtra("vibrate", vibrate == 1);
        intent.putExtra("lights", lights == 1);
        intent.putExtra("l_icon", "notify_icon_big");
        intent.putExtra("s_icon", "notify_icon_small ");
        intent.putExtra("unityClass", unityClass);
        am.setRepeating(0, System.currentTimeMillis() + delay, rep, PendingIntent.getBroadcast(currentActivity, id, intent, 0));
    }

    public void onReceive(Context context, Intent intent)
    {
    	NotificationManager notificationManager = (NotificationManager)context.getSystemService(Context.NOTIFICATION_SERVICE);
    	
    	String unityClass = intent.getStringExtra("unityClass");
        String ticker = intent.getStringExtra("ticker");
        String title = intent.getStringExtra("title");
        String message = intent.getStringExtra("message");
        String s_icon = intent.getStringExtra("s_icon");
        String l_icon = intent.getStringExtra("l_icon");
        Boolean sound = Boolean.valueOf(intent.getBooleanExtra("sound", false));
        Boolean vibrate = Boolean.valueOf(intent.getBooleanExtra("vibrate", false));
        Boolean lights = Boolean.valueOf(intent.getBooleanExtra("lights", false));
        int id = intent.getIntExtra("id", 0);
        
        Resources res = context.getResources();
        Class<?> unityClassActivity = null;
		try {
			unityClassActivity = Class.forName(unityClass);
		} catch (ClassNotFoundException e) {
			e.printStackTrace();
		}
        Intent notificationIntent = new Intent(context, unityClassActivity);
        PendingIntent contentIntent = PendingIntent.getActivity(context, 0, notificationIntent, 0);
        android.app.Notification.Builder builder = new android.app.Notification.Builder(context);
        
        builder.setContentIntent(contentIntent)
        	.setWhen(System.currentTimeMillis())
        	.setAutoCancel(true)
        	.setContentTitle(title)
        	.setContentText(message);
        
        if(ticker != null && ticker.length() > 0)
            builder.setTicker(ticker);
               
		if (s_icon != null && s_icon.length() > 0)
			builder.setSmallIcon(res.getIdentifier(s_icon, "drawable", context.getPackageName()));
		
		if (l_icon != null && l_icon.length() > 0)
			builder.setLargeIcon(BitmapFactory.decodeResource(res, res.getIdentifier(l_icon, "drawable", context.getPackageName())));
		
        if(sound.booleanValue())
            builder.setSound(RingtoneManager.getDefaultUri(2));
        
        if(vibrate.booleanValue())
            builder.setVibrate(new long[] {
                1000L, 1000L
            });
        
        if(lights.booleanValue())
            builder.setLights(Color.GREEN, 3000, 3000);
        
        Notification notification = builder.build();
        notificationManager.notify(id, notification);
    }

    public static void CancelNotification(int id)
    {
        Activity currentActivity = UnityPlayer.currentActivity;
        AlarmManager am = (AlarmManager)currentActivity.getSystemService("alarm");
        Intent intent = new Intent(currentActivity, UnityNotificationManager.class);
        PendingIntent pendingIntent = PendingIntent.getBroadcast(currentActivity, id, intent, 0);
        am.cancel(pendingIntent);
    }
}
