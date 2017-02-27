namespace UnifiedNotifications.Extensions.iOS
{
    public enum RepeatInterval
    {
        None = 0,

        // Summary:
        //     ///
        //     Specifies the era unit.
        //     ///
        Era,

        // Summary:
        //     ///
        //     Specifies the year unit.
        //     ///
        Year,

        // Summary:
        //     ///
        //     Specifies the month unit.
        //     ///
        Month,

        // Summary:
        //     ///
        //     Specifies the day unit.
        //     ///
        Day,

        // Summary:
        //     ///
        //     Specifies the hour unit.
        //     ///
        Hour,

        // Summary:
        //     ///
        //     Specifies the minute unit.
        //     ///
        Minute,

        // Summary:
        //     ///
        //     Specifies the second unit.
        //     ///
        Second,

        // Summary:
        //     ///
        //     Specifies the week unit.
        //     ///
        Week,

        // Summary:
        //     ///
        //     Specifies the weekday unit.
        //     ///
        Weekday,

        // Summary:
        //     ///
        //     Specifies the ordinal weekday unit.
        //     ///
        WeekdayOrdinal,

        // Summary:
        //     ///
        //     Specifies the quarter of the calendar.
        //     ///
        Quarter
    }

    public enum CalendarIdentifier
    {
        // Summary:
        //     ///
        //     Identifies the Gregorian calendar.
        //     ///
        GregorianCalendar = 0,

        // Summary:
        //     ///
        //     Identifies the Buddhist calendar.
        //     ///
        BuddhistCalendar = 1,

        // Summary:
        //     ///
        //     Identifies the Chinese calendar.
        //     ///
        ChineseCalendar = 2,

        // Summary:
        //     ///
        //     Identifies the Hebrew calendar.
        //     ///
        HebrewCalendar = 3,

        // Summary:
        //     ///
        //     Identifies the Islamic calendar.
        //     ///
        IslamicCalendar = 4,

        // Summary:
        //     ///
        //     Identifies the Islamic civil calendar.
        //     ///
        IslamicCivilCalendar = 5,

        // Summary:
        //     ///
        //     Identifies the Japanese calendar.
        //     ///
        JapaneseCalendar = 6,

        // Summary:
        //     ///
        //     Identifies the Republic of China (Taiwan) calendar.
        //     ///
        RepublicOfChinaCalendar = 7,

        // Summary:
        //     ///
        //     Identifies the Persian calendar.
        //     ///
        PersianCalendar = 8,

        // Summary:
        //     ///
        //     Identifies the Indian calendar.
        //     ///
        IndianCalendar = 9,

        // Summary:
        //     ///
        //     Identifies the ISO8601.
        //     ///
        ISO8601Calendar = 10
    }

    public interface INotificationExtensionIOS : INotificationExtension
    {
        bool                hasAction                   { get; set; }
        int                 applicationIconBadgeNumber  { get; set; }
        string              soundName                   { get; set; }

        CalendarIdentifier  repeatCalendar              { get; set; }
        RepeatInterval      repeatInterval              { get; set; }
    }
}
