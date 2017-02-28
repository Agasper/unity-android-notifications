using System;
using UnityEngine;

using NotificationData = Android.LocalNotifications.NotificationData;
using LocalNotification = Android.LocalNotifications.LocalNotification;

public class NotificationTest : MonoBehaviour
{
    #region Static readonly data.
    static readonly string[] ids = { "Note 1", "Note 2", "Note 3" };
    static readonly float[] delays = { 5.0f, 15.0f, 30.0f, 60.0f, 3600.0f };
    static readonly float[] repeats = { 0.0f, 10.0f, 30.0f, 60.0f, 3600.0f };
    static readonly string[] groups = { null, "Group 1", "Group 2", "Group 3" };
    static readonly Color32[] colors = { NotificationData.noColor, Color.red, Color.green, Color.blue, Color.white };
    static readonly int[][] vibrationPatterns = { null, new int[] {1000, 1000 }, new int[] { 100, 100, 200, 200, 100, 100}, new int[] { 100, 200, 300, 400, 500, 600} };
    static readonly string[] vibrationPatternNames = { "<Default>", "Short", "Long Random", "Long Slowdown" };
    static readonly int[] lightTimes = { 100, 500, 1000, 2000 };
    static readonly long[] whenChronometerValues = { 0, 10000, 30000, 3600000 };
    static readonly DateTime[] whenDateTimeValues = { new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc) };
    static readonly NotificationData.Category[] categories = {
            NotificationData.Category.None,
            NotificationData.Category.Alarm,
            NotificationData.Category.Call,
            NotificationData.Category.Email,
            NotificationData.Category.Error,
            NotificationData.Category.Event,
            NotificationData.Category.Message,
            NotificationData.Category.Progress,
            NotificationData.Category.Promo,
            NotificationData.Category.Recommendation,
            NotificationData.Category.Reminder,
            NotificationData.Category.Service,
            NotificationData.Category.Social,
            NotificationData.Category.Status,
            NotificationData.Category.System,
            NotificationData.Category.Transport };

    static readonly NotificationData.Priority[] priorities = {
            NotificationData.Priority.Min,
            NotificationData.Priority.Low,
            NotificationData.Priority.Default,
            NotificationData.Priority.High,
            NotificationData.Priority.Max };

    static readonly string[] icons = { null, "notify_icon_small", "app_icon" };

    static readonly string[] persons = { null, "mailto:test@example.com", "tel:+0017775552233" };
    #endregion

    #region Inspector fields.
    [SerializeField]
    protected GUISkin skin;
    #endregion

    #region Private data.
    private float delay = 5.0f;
    private float repeat = 0.0f;
    private bool useTicker = false;
    private bool useCustomWhen = false;

    private NotificationData data = new NotificationData(string.Empty, string.Empty);

    private bool cancelPrevious = true;
    private bool scheduleModeExact = false;
    private bool scheduleModeAllowWhileIdle = false;

    private bool cancelPending = true;
    private bool cancelShown = true;

    private Vector2 scrollPosition = Vector2.zero;

    private bool showDelayBlock = true;
    private bool showRepeatBlock = true;
    private bool showGroupBlock = true;
    private bool showCategoryBlock = false;
    private bool showPriorityBlock = false;
    private bool showColorBlock = false;
    private bool showVibrationBlock = false;
    private bool showLightsBlock = false;
    private bool showIconsBlock = false;
    private bool showWhenBlock = false;
    private bool showProgressBlock = false;
    private bool showSoundBlock = false;
    private bool showPersonBlock = false;
    private bool showOtherOptionsBlock = false;
    private bool showScheduleOptionsBlock = true;
    #endregion

    #region Unity API.
    void OnGUI ()
    {
        if (skin != null)
        {
            GUI.skin = skin;
        }

        GUILayout.BeginArea(new Rect(25, 25, Screen.width - 50, Screen.height - 50));

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Delay:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showDelayBlock = !showDelayBlock;

                if (showDelayBlock)
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        for (int i = 0; i < delays.Length; i++)
                        {
                            if (GUILayout.Toggle(delay == delays[i], string.Format("{0} s", (int)delays[i])))
                            {
                                delay = delays[i];
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Repeat:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showRepeatBlock = !showRepeatBlock;

                if (showRepeatBlock)
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        for (int i = 0; i < repeats.Length; i++)
                        {
                            if (GUILayout.Toggle(repeat == repeats[i], string.Format("{0} s", (int)repeats[i])))
                            {
                                repeat = repeats[i];
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Group:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showGroupBlock = !showGroupBlock;

                if (showGroupBlock)
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        for (int i = 0; i < groups.Length; i++)
                        {
                            if (GUILayout.Toggle(data.group == groups[i], groups[i] != null ? groups[i] : "<None>"))
                            {
                                data.SetGroup(groups[i]);
                            }
                        }
                    }

                    GUILayout.EndHorizontal();

                    data.SetIsGroupSummary(GUILayout.Toggle(data.isGroupSummary, "Is a Group summary"));
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Category:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showCategoryBlock = !showCategoryBlock;

                if (showCategoryBlock)
                {
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        for (int i = 0; i < categories.Length; i++)
                        {
                            if (GUILayout.Toggle(data.category == categories[i], categories[i].ToString()))
                            {
                                data.SetCategory(categories[i]);
                            }
                        }
                    }

                    GUILayout.EndVertical();
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Priority:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showPriorityBlock = !showPriorityBlock;

                if (showPriorityBlock)
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        for (int i = 0; i < priorities.Length; i++)
                        {
                            if (GUILayout.Toggle(data.priority == priorities[i], priorities[i].ToString()))
                            {
                                data.SetPriority(priorities[i]);
                            }
                        }
                    }

                    GUILayout.EndHorizontal();
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Person link:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showPersonBlock = !showPersonBlock;

                if (showPersonBlock)
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        for (int i = 0; i < persons.Length; i++)
                        {
                            if (GUILayout.Toggle(data.person == persons[i], persons[i] != null ? persons[i] : "<None>"))
                            {
                                data.SetPerson(persons[i]);
                            }
                        }
                    }

                    GUILayout.EndHorizontal();
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Icon Background Color:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showColorBlock = !showColorBlock;

                if (showColorBlock)
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        for (int i = 0; i < colors.Length; i++)
                        {
                            if (LayoutColoredButton(colors[i], new GUIContent((Color)data.color == (Color)colors[i] ? " +++++ " : " ----- ")))
                            {
                                data.SetColor(colors[i]);
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Sound options:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showSoundBlock = !showSoundBlock;

                if (showSoundBlock)
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        data.SetUseSound(GUILayout.Toggle(data.useSound, "Use sound"));
                    }
                    GUILayout.EndHorizontal();
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Vibration options:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showVibrationBlock = !showVibrationBlock;

                if (showVibrationBlock)
                {
                    data.SetUseVibration ( GUILayout.Toggle(data.useVibration, "Use vibration"));

                    if (data.useVibration)
                    {
                        GUILayout.Label("Vibration pattern:");

                        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        {
                            for (int i = 0; i < vibrationPatterns.Length; i++)
                            {
                                if (GUILayout.Toggle(data.vibrationPattern == vibrationPatterns[i], vibrationPatternNames[i]))
                                {
                                    data.SetVibrationPattern(vibrationPatterns[i]);
                                }
                            }
                        }
                        GUILayout.EndVertical();
                    }
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Lights:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showLightsBlock = !showLightsBlock;

                if (showLightsBlock)
                {
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        data.SetUseLights ( GUILayout.Toggle(data.useVibration, "Use lights"));

                        if (data.useLights)
                        {
                            GUILayout.Label("Custom lights settings");

                            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                            {
                                GUILayout.Label("Color: ");

                                for (int i = 0; i < colors.Length; i++)
                                {
                                    if (LayoutColoredButton(colors[i], new GUIContent((Color)data.lightsColor == (Color)colors[i] ? " +++++ " : " ----- ")))
                                    {
                                        data.SetLightsColor(colors[i]);
                                    }
                                }
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                            {
                                GUILayout.Label("Lights On: ");

                                for (int i = 0; i < lightTimes.Length; i++)
                                {
                                    if (GUILayout.Toggle(data.lightsOn == lightTimes[i], lightTimes[i].ToString() ))
                                    {
                                        data.SetLightsOn(lightTimes[i]);
                                    }
                                }
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                            {
                                GUILayout.Label("Lights Off: ");

                                for (int i = 0; i < lightTimes.Length; i++)
                                {
                                    if (GUILayout.Toggle(data.lightsOff == lightTimes[i], lightTimes[i].ToString() ))
                                    {
                                        data.SetLightsOff(lightTimes[i]);
                                    }
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndVertical();
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Icons:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showIconsBlock = !showIconsBlock;

                if (showIconsBlock)
                {
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        {
                            GUILayout.Label("Small Icon: ");

                            for (int i = 0; i < icons.Length; i++)
                            {
                                if (GUILayout.Toggle(data.smallIconResource == icons[i], icons[i] != null ? icons[i].ToString() : "<None>" ))
                                {
                                    data.SetSmallIconResource(icons[i]);
                                }
                            }
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        {
                            GUILayout.Label("Large Icon: ");

                            for (int i = 0; i < icons.Length; i++)
                            {
                                if (GUILayout.Toggle(data.largeIconResource == icons[i], icons[i] != null ? icons[i].ToString() : "<None>" ))
                                {
                                    data.SetLargeIconResource(icons[i]);
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("'When' icon:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showWhenBlock = !showWhenBlock;

                if (showWhenBlock)
                {
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        data.SetShowWhen(GUILayout.Toggle(data.showWhen, "Show 'When'"));
                        data.SetWhenIsChronometer(GUILayout.Toggle(data.whenIsChronometer, "'When' is chronometer"));
                        data.SetChronometerCountdown(GUILayout.Toggle(data.chronometerCountdown, "Chronometer countdown"));
                        useCustomWhen = GUILayout.Toggle(useCustomWhen, "Use Custom 'When':");
                        if (useCustomWhen)
                        {
                            if (data.whenIsChronometer)
                            {
                                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                                {
                                    GUILayout.Label("Chronometer: ");

                                    for (int i = 0; i < whenChronometerValues.Length; i++)
                                    {
                                        if (GUILayout.Toggle(data.customWhenChronometer == whenChronometerValues[i], whenChronometerValues[i].ToString()))
                                        {
                                            data.SetCustomWhenChronometer(whenChronometerValues[i]);
                                        }
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }
                            else
                            {
                                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                                {
                                    GUILayout.Label("Date: ");

                                    for (int i = 0; i < whenDateTimeValues.Length; i++)
                                    {
                                        if (GUILayout.Toggle(data.customWhenDate == whenDateTimeValues[i], whenDateTimeValues[i].ToString("d")))
                                        {
                                            data.SetCustomWhenDate(whenDateTimeValues[i]);
                                        }
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }
                        }
                    }
                    GUILayout.EndVertical();
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Progress or SubText:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showProgressBlock = !showProgressBlock;

                if (showProgressBlock)
                {
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        if (GUILayout.Toggle(data.subText != null, "Use SubText"))
                        {
                            data.SetSubText("SubText example...");
                        }
                        else
                        {
                            data.SetSubText(null);

                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(string.Format("Progress Max ({0}): \n(0 - no progress)", data.progressMax));
                                data.SetProgressMax((int)GUILayout.HorizontalSlider(data.progressMax, 0.0f, 100.0f, GUILayout.Width(Screen.width * 0.4f)));
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(string.Format("Progress ({0}): ", data.progress));
                                data.SetProgress((int)GUILayout.HorizontalSlider(data.progress, 0.0f, 100.0f, GUILayout.Width(Screen.width * 0.4f)));
                            }
                            GUILayout.EndHorizontal();

                            data.SetProgressIndeterminate(GUILayout.Toggle(data.progressIndeterminate, "Progress indeterminate"));
                        }
                    }
                    GUILayout.EndVertical();
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Other Options:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showOtherOptionsBlock = !showOtherOptionsBlock;

                if (showOtherOptionsBlock)
                {
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        cancelPrevious = GUILayout.Toggle(cancelPrevious, "Cancel Previous");
                        useTicker = GUILayout.Toggle(useTicker, "Use Ticker");
                        data.SetAutoCancel(GUILayout.Toggle(data.autoCancel, "Auto Cancel"));
                        data.SetLocalOnly(GUILayout.Toggle(data.localOnly, "Local only notification"));
                        data.SetOngoing(GUILayout.Toggle(data.ongoing, "'Ongoing' notification"));
                        data.SetOnlyAlertOnce(GUILayout.Toggle(data.onlyAlertOnce, "Only Alert Once"));
                    }
                    GUILayout.EndVertical();
                }

                //----------------------------------------------------------------------------------------
                if (GUILayout.Button("Schedule Options:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    showScheduleOptionsBlock = !showScheduleOptionsBlock;

                if (showScheduleOptionsBlock)
                {
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        scheduleModeExact = GUILayout.Toggle(scheduleModeExact, "Schedule mode: Exact");
                        scheduleModeAllowWhileIdle = GUILayout.Toggle(scheduleModeAllowWhileIdle, "Schedule mode: Allow while Idle");
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (GUILayout.Button(string.Format("Set {0}", ids[i])))
                    {
                        var showDate = DateTime.UtcNow + TimeSpan.FromSeconds(delay);

                        data.SetTitle   (string.Format("Title: {0}", ids[i]));
                        data.SetMessage (string.Format("Message: {0} ({1})", ids[i], showDate));

                        if (useTicker)
                            data.SetTicker(string.Format("Ticker: {0}", ids[i]));
                        else
                            data.SetTicker(null);

                        var scheduleMode = LocalNotification.ScheduleMode.None;
                        if (scheduleModeExact)
                            scheduleMode |= LocalNotification.ScheduleMode.Exact;
                        if (scheduleModeAllowWhileIdle)
                            scheduleMode |= LocalNotification.ScheduleMode.AllowWhileIdle;

                        if (repeat > 0)
                        {
                            LocalNotification.SendNotification
                                ( ids[i]
                                , showDate
                                , TimeSpan.FromSeconds(repeat)
                                , data
                                , cancelPrevious
                                , scheduleMode);
                        }
                        else
                        {
                            LocalNotification.SendNotification
                                ( ids[i]
                                , showDate
                                , data
                                , cancelPrevious
                                , scheduleMode);
                        }
                    }
                }
                
            }
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            
            GUILayout.BeginVertical();
            {
                GUILayout.Box("Cancel", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                cancelPending = GUILayout.Toggle(cancelPending, "Cancel pending");
                cancelShown = GUILayout.Toggle(cancelShown, "Cancel shown");

                GUILayout.BeginHorizontal();
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (GUILayout.Button(string.Format("Cancel {0}", ids[i])))
                        {
                            LocalNotification.CancelNotification(ids[i], cancelPending, cancelShown);
                        }
                    }
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Cancel All Shown"))
                {
                    LocalNotification.CancelAllShownNotifications();
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();

        GUILayout.EndScrollView();

        GUILayout.EndArea();
    }

    void LayoutColoredBox(Color color, GUIContent content = null)
    {
        GUIStyle textureStyle = new GUIStyle(GUI.skin.box);
        textureStyle.normal.background = Texture2D.whiteTexture;

        var backgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = color;
        GUILayout.Box(content ?? GUIContent.none, textureStyle);
        GUI.backgroundColor = backgroundColor;
    }

    bool LayoutColoredButton(Color color, GUIContent content = null)
    {
        GUIStyle textureStyle = new GUIStyle(GUI.skin.button);
        textureStyle.normal.background = Texture2D.whiteTexture;

        var backgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = color;
        var result = GUILayout.Button(content ?? GUIContent.none, textureStyle);
        GUI.backgroundColor = backgroundColor;
        return result;
    }
    #endregion
}
