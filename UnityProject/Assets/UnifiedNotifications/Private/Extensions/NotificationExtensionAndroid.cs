using System;
using UnifiedNotifications.Extensions;
using UnityEngine;

namespace UnifiedNotifications.Private.Extensions
{
    public class NotificationExtensionAndroid : INotificationExtensionAndroid
    {
        public ScheduleMode scheduleMode           { get; set; }
        public TimeSpan    repeatInterval          { get; set; }
        
        public string      ticker                  { get; set; }

        public string      subText                 { get; set; }
        public int         progressMax             { get; set; }
        public int         progress                { get; set; }
        public bool        progressIndeterminate   { get; set; }

        public string      group                   { get; set; }
        public bool        isGroupSummary          { get; set; }
        public Category    category                { get; set; }

        public bool        autoCancel              { get; set; }
        public bool        localOnly               { get; set; }
        public bool        ongoing                 { get; set; }
        public bool        onlyAlertOnce           { get; set; }
        public Priority    priority                { get; set; }

        public bool        sound                   { get; set; }

        public bool        showWhen                { get; set; }
        public bool        whenIsChronometer       { get; set; }
        public bool        chronometerCountdown    { get; set; }
        public DateTime    customWhenDate          { get; set; }
        public long        customWhenChronometer   { get; set; }

        public Color32     color                   { get; set; }

        public int[]       vibrationPattern        { get; set; }

        public Color32     lightsColor             { get; set; }
        public int         lightsOn                { get; set; } // In milliseconds.
        public int         lightsOff               { get; set; } // In milliseconds.

        public string      smallIconResource       { get; set; }
        public string      largeIconResource       { get; set; }

        public string      person                  { get; set; } // Person URI reference.
    }
}
