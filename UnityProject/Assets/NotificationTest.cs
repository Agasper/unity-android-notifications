using System;
using UnityEngine;

public class NotificationTest : MonoBehaviour
{
    static readonly string[] ids = { "Note 1", "Note 2", "Note 3" };
    static readonly float[] delays = { 5.0f, 15.0f, 30.0f, 60.0f, 3600.0f };
    static readonly float[] repeats = { 0.0f, 10.0f, 30.0f, 60.0f, 3600.0f };
    static readonly string[] groups = { null, "Group 1", "Group 2", "Group 3" };
    static readonly Color32[] colors = { Net.Agasper.UnityNotifications.NotificationData.noColor, Color.red, Color.green, Color.blue, Color.white };
    static readonly int[][] vibrationPatterns = { null, new int[] {1000, 1000 }, new int[] { 100, 100, 200, 200, 100, 100}, new int[] { 100, 200, 300, 400, 500, 600} };
    static readonly string[] vibrationPatternNames = { "<None>", "Short", "Long Random", "Long Slowdown" };
    static readonly int[] lightTimes = { 100, 500, 1000, 2000 };

    static readonly string[] icons = { null, "notify_icon_small", "app_icon" };

    public GUISkin skin;

    private float delay = 5.0f;
    private float repeat = 0.0f;
    private bool useTicker = false;
    private Color32 bgColor = Net.Agasper.UnityNotifications.NotificationData.noColor;
    private string group = null;
    private bool showTime = true;
    private bool sound = true;
    private int[] vibrationPattern = null;
    private Color32 lightsColor = Net.Agasper.UnityNotifications.NotificationData.noColor;
    private int lightTimeOn = 1000;
    private int lightTimeOff = 1000;
    private string smallIcon = "notify_icon_small";
    private string largeIcon = null;
    private bool cancelPrevious = true;
    private bool scheduleModeExact = false;
    private bool scheduleModeAllowWhileIdle = false;

    private bool cancelPending = true;
    private bool cancelShown = true;

    void OnGUI ()
    {
        if (skin != null)
        {
            GUI.skin = skin;
        }

        GUILayout.BeginArea(new Rect(30, 30, Screen.width - 60, Screen.height - 60));

        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                //----------------------------------------------------------------------------------------
                GUILayout.Box("Delay:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

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

                //----------------------------------------------------------------------------------------
                GUILayout.Box("Repeat:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

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

                //----------------------------------------------------------------------------------------
                GUILayout.Box("Group:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    for (int i = 0; i < groups.Length; i++)
                    {
                        if (GUILayout.Toggle(group == groups[i], groups[i] != null ? groups[i] : "<None>"))
                        {
                            group = groups[i];
                        }
                    }
                }
                GUILayout.EndHorizontal();

                //----------------------------------------------------------------------------------------
                GUILayout.Box("BG Color:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    for (int i = 0; i < colors.Length; i++)
                    {
                        if (LayoutColoredButton(colors[i], new GUIContent((Color)bgColor == (Color)colors[i] ? " +++++ " : " ----- ")))
                        {
                            bgColor = colors[i];
                        }
                    }
                }
                GUILayout.EndHorizontal();

                //----------------------------------------------------------------------------------------
                GUILayout.Box("Vibration pattern:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    for (int i = 0; i < vibrationPatterns.Length; i++)
                    {
                        if (GUILayout.Toggle(vibrationPattern == vibrationPatterns[i], vibrationPatternNames[i]))
                        {
                            vibrationPattern = vibrationPatterns[i];
                        }
                    }
                }
                GUILayout.EndHorizontal();

                //----------------------------------------------------------------------------------------
                GUILayout.Box("Lights:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        GUILayout.Label("Color: ");

                        for (int i = 0; i < colors.Length; i++)
                        {
                            if (LayoutColoredButton(colors[i], new GUIContent((Color)lightsColor == (Color)colors[i] ? " +++++ " : " ----- ")))
                            {
                                lightsColor = colors[i];
                            }
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        GUILayout.Label("Lights On: ");

                        for (int i = 0; i < lightTimes.Length; i++)
                        {
                            if (GUILayout.Toggle(lightTimeOn == lightTimes[i], lightTimes[i].ToString() ))
                            {
                                lightTimeOn = lightTimes[i];
                            }
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        GUILayout.Label("Lights Off: ");

                        for (int i = 0; i < lightTimes.Length; i++)
                        {
                            if (GUILayout.Toggle(lightTimeOff == lightTimes[i], lightTimes[i].ToString() ))
                            {
                                lightTimeOff = lightTimes[i];
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                //----------------------------------------------------------------------------------------
                GUILayout.Box("Icons:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        GUILayout.Label("Small Icon: ");

                        for (int i = 0; i < icons.Length; i++)
                        {
                            if (GUILayout.Toggle(smallIcon == icons[i], icons[i] != null ? icons[i].ToString() : "<None>" ))
                            {
                                smallIcon = icons[i];
                            }
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        GUILayout.Label("Large Icon: ");

                        for (int i = 0; i < icons.Length; i++)
                        {
                            if (GUILayout.Toggle(largeIcon == icons[i], icons[i] != null ? icons[i].ToString() : "<None>" ))
                            {
                                largeIcon = icons[i];
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                //----------------------------------------------------------------------------------------
                GUILayout.Box("Other Options:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                cancelPrevious = GUILayout.Toggle(cancelPrevious, "Cancel Previous");

                GUILayout.Space(10.0f);

                useTicker = GUILayout.Toggle(useTicker, "Use Ticker");
                showTime = GUILayout.Toggle(showTime, "Show Time");
                sound = GUILayout.Toggle(sound, "Sound");

                GUILayout.Space(10.0f);

                scheduleModeExact = GUILayout.Toggle(scheduleModeExact, "Schedule mode: Exact");
                scheduleModeAllowWhileIdle = GUILayout.Toggle(scheduleModeAllowWhileIdle, "Schedule mode: Allow while Idle");

                GUILayout.Space(10.0f);
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (GUILayout.Button(string.Format("Set {0}", ids[i])))
                    {
                        var showDate = DateTime.UtcNow + TimeSpan.FromSeconds(delay);

                        var data = new Net.Agasper.UnityNotifications.NotificationData
                            ( string.Format("Title: {0}", ids[i])
                            , string.Format("Message: {0} ({1})", ids[i], showDate));

                        if (useTicker)
                            data.SetTicker(string.Format("Ticker: {0}", ids[i]));

                        data.SetShowTime(showTime);
                        data.SetSound(sound);
                        data.SetColor(bgColor);
                        data.SetGroup(group);
                        data.SetVibrationPattern(vibrationPattern);
                        data.SetLightsColor(lightsColor);
                        data.SetLightsOn(lightTimeOn);
                        data.SetLightsOff(lightTimeOff);
                        data.SetSmallIconResource(smallIcon);
                        data.SetLargeIconResource(largeIcon);

                        var scheduleMode = Net.Agasper.UnityNotifications.LocalNotification.ScheduleMode.None;
                        if (scheduleModeExact)
                            scheduleMode |= Net.Agasper.UnityNotifications.LocalNotification.ScheduleMode.Exact;
                        if (scheduleModeAllowWhileIdle)
                            scheduleMode |= Net.Agasper.UnityNotifications.LocalNotification.ScheduleMode.AllowWhileIdle;

                        if (repeat > 0)
                        {
                            Net.Agasper.UnityNotifications.LocalNotification.SendNotification
                                ( ids[i]
                                , showDate
                                , TimeSpan.FromSeconds(repeat)
                                , data
                                , cancelPrevious
                                , scheduleMode);
                        }
                        else
                        {
                            Net.Agasper.UnityNotifications.LocalNotification.SendNotification
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
                            Net.Agasper.UnityNotifications.LocalNotification.CancelNotification(ids[i], cancelPending, cancelShown);
                        }
                    }
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Cancel All Shown"))
                {
                    Net.Agasper.UnityNotifications.LocalNotification.CancelAllShownNotifications();
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();

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
}
