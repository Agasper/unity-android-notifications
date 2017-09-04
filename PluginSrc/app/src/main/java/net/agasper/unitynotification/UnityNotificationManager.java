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
import android.graphics.drawable.BitmapDrawable;
import android.media.RingtoneManager;
import android.os.Build;
import android.support.v4.app.NotificationCompat;
import android.support.v4.app.TaskStackBuilder;

import com.unity3d.player.UnityPlayer;

public class UnityNotificationManager extends BroadcastReceiver
{

    public static void SetNotification(int id, long delayMs, String title, String message, String ticker, int sound, int vibrate,
                                       int lights, String largeIconResource, String smallIconResource, int bgColor, String bundle)
    {
        Activity currentActivity = UnityPlayer.currentActivity;
        AlarmManager am = (AlarmManager)currentActivity.getSystemService(Context.ALARM_SERVICE);
        Intent intent = new Intent(currentActivity, UnityNotificationManager.class);
        intent.putExtra("ticker", ticker);
        intent.putExtra("title", title);
        intent.putExtra("message", message);
        intent.putExtra("id", id);
        intent.putExtra("color", bgColor);
        intent.putExtra("sound", sound == 1);
        intent.putExtra("vibrate", vibrate == 1);
        intent.putExtra("lights", lights == 1);
        intent.putExtra("l_icon", largeIconResource);
        intent.putExtra("s_icon", smallIconResource);
        intent.putExtra("bundle", bundle);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
            am.setExact(AlarmManager.RTC_WAKEUP, System.currentTimeMillis() + delayMs, PendingIntent.getBroadcast(currentActivity, id, intent, 0));
        else
            am.set(AlarmManager.RTC_WAKEUP, System.currentTimeMillis() + delayMs, PendingIntent.getBroadcast(currentActivity, id, intent, 0));
    }

    public static void SetRepeatingNotification(int id, long delayMs, String title, String message, String ticker, long rep, int sound, int vibrate, int lights,
                                                String largeIconResource, String smallIconResource, int bgColor, String bundle)
    {
        Activity currentActivity = UnityPlayer.currentActivity;
        AlarmManager am = (AlarmManager)currentActivity.getSystemService(Context.ALARM_SERVICE);
        Intent intent = new Intent(currentActivity, UnityNotificationManager.class);
        intent.putExtra("ticker", ticker);
        intent.putExtra("title", title);
        intent.putExtra("message", message);
        intent.putExtra("id", id);
        intent.putExtra("color", bgColor);
        intent.putExtra("sound", sound == 1);
        intent.putExtra("vibrate", vibrate == 1);
        intent.putExtra("lights", lights == 1);
        intent.putExtra("l_icon", largeIconResource);
        intent.putExtra("s_icon", smallIconResource);
        intent.putExtra("bundle", bundle);
        am.setRepeating(AlarmManager.RTC_WAKEUP, System.currentTimeMillis() + delayMs, rep, PendingIntent.getBroadcast(currentActivity, id, intent, 0));
    }

    public void onReceive(Context context, Intent intent)
    {
        NotificationManager notificationManager = (NotificationManager)context.getSystemService(Context.NOTIFICATION_SERVICE);

        String ticker = intent.getStringExtra("ticker");
        String title = intent.getStringExtra("title");
        String message = intent.getStringExtra("message");
        String s_icon = intent.getStringExtra("s_icon");
        String l_icon = intent.getStringExtra("l_icon");
        int color = intent.getIntExtra("color", 0);
        String bundle = intent.getStringExtra("bundle");
        Boolean sound = intent.getBooleanExtra("sound", false);
        Boolean vibrate = intent.getBooleanExtra("vibrate", false);
        Boolean lights = intent.getBooleanExtra("lights", false);
        int id = intent.getIntExtra("id", 0);

        Resources res = context.getResources();

        Intent notificationIntent = context.getPackageManager().getLaunchIntentForPackage(bundle);

        TaskStackBuilder stackBuilder = TaskStackBuilder.create(context);
        stackBuilder.addNextIntent(notificationIntent);

        PendingIntent pendingIntent = PendingIntent.getActivity(context, 0,
                notificationIntent, PendingIntent.FLAG_UPDATE_CURRENT);

        NotificationCompat.Builder builder = new NotificationCompat.Builder(context);

        builder.setContentIntent(pendingIntent)
                .setAutoCancel(true)
                .setContentTitle(title)
                .setContentText(message);

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP)
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
            builder.setVibrate(new long[] {
                    1000L, 1000L
            });

        if (lights)
            builder.setLights(Color.GREEN, 3000, 3000);

        Notification notification = builder.build();
        notificationManager.notify(id, notification);
    }

    public static void CancelPendingNotification(int id)
    {
        Activity currentActivity = UnityPlayer.currentActivity;
        AlarmManager am = (AlarmManager)currentActivity.getSystemService(Context.ALARM_SERVICE);
        Intent intent = new Intent(currentActivity, UnityNotificationManager.class);
        PendingIntent pendingIntent = PendingIntent.getBroadcast(currentActivity, id, intent, 0);
        am.cancel(pendingIntent);
    }

    public static void ClearShowingNotifications()
    {
        Activity currentActivity = UnityPlayer.currentActivity;
        NotificationManager nm = (NotificationManager)currentActivity.getSystemService(Context.NOTIFICATION_SERVICE);
        nm.cancelAll();
    }
}
