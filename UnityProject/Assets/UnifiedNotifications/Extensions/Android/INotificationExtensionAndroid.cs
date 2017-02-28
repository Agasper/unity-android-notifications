using System;
using UnityEngine;

namespace UnifiedNotifications.Extensions.Android
{
    /// <summary>
    /// Mode of scheduling notification to Android Alarm Manager.
    /// 
    /// Exact uses `setExact` method (API 19+, Android 4.4 or higher) or 'setExactAndAllowWhileIdle' method (API 23+, Android 6.0 or higher). Inexact alternatives will be used on older Android versions.
    /// AllowWhileIdle uses `setAndAllowWhileIdle` method or `setExactAndAllowWhileIdle` method (API 23+, Android 6.0 or higher). Normal versions will be used on older Android versions.
    /// 
    /// Documentation: https://developer.android.com/intl/ru/reference/android/app/AlarmManager.html
    /// </summary>
    [Flags]
    public enum ScheduleMode
    {
        None = 0,
        Exact = 1,
        AllowWhileIdle = 2,
    }

    public enum Category
    {
        None            = 0,    // No explicit category. Default.
        Alarm           = 1,    // Notification category: alarm or timer.
        Call            = 2,    // Notification category: incoming call (voice or video) or similar synchronous communication request.
        Email           = 3,    // Notification category: asynchronous bulk message (email).
        Error           = 4,    // Notification category: error in background operation or authentication status.
        Event           = 5,    // Notification category: calendar event.
        Message         = 6,    // Notification category: incoming direct message (SMS, instant message, etc.).
        Progress        = 7,    // Notification category: progress of a long-running background operation.
        Promo           = 8,    // Notification category: promotion or advertisement.
        Recommendation  = 9,    // Notification category: a specific, timely recommendation for a single thing.
        Reminder        = 10,   // Notification category: user-scheduled reminder.
        Service         = 11,   // Notification category: indication of running background service.
        Social          = 12,   // Notification category: social network or sharing update.
        Status          = 13,   // Notification category: ongoing information about device or contextual status.
        System          = 14,   // Notification category: system or device status update.
        Transport       = 15    // Notification category: media transport control for playback. 
    }

    public enum Priority
    {
        Min = -2,
        Low = -1,
        Default = 0,
        High = 1,
        Max = 2
    }

    interface INotificationExtensionAndroid : INotificationExtension
    {
        ScheduleMode scheduleMode           { get; set; }
        TimeSpan    repeatInterval          { get; set; }

        string      ticker                  { get; set; }

        string      subText                 { get; set; }
        int         progressMax             { get; set; }
        int         progress                { get; set; }
        bool        progressIndeterminate   { get; set; }

        string      group                   { get; set; }
        bool        isGroupSummary          { get; set; }
        Category    category                { get; set; }

        bool        autoCancel              { get; set; }
        bool        localOnly               { get; set; }
        bool        ongoing                 { get; set; }
        bool        onlyAlertOnce           { get; set; }
        Priority    priority                { get; set; }

        bool        showWhen                { get; set; }
        bool        whenIsChronometer       { get; set; }
        bool        chronometerCountdown    { get; set; }
        DateTime    customWhenDate          { get; set; }
        long        customWhenChronometer   { get; set; }

        Color32     color                   { get; set; }

        bool        useVibration            { get; set; }
        int[]       vibrationPattern        { get; set; }

        bool        useLights               { get; set; }
        Color32     lightsColor             { get; set; }
        int         lightsOn                { get; set; } // In milliseconds.
        int         lightsOff               { get; set; } // In milliseconds.

        string      smallIconResource       { get; set; }
        string      largeIconResource       { get; set; }

        string      person                  { get; set; } // Person URI reference.
    }
}
