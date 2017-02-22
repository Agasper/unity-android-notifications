package net.agasper.unitynotification;

import java.io.Serializable;

public final class NotificationData implements Serializable
{
	private static final long serialVersionUID = -7733864630402717326L;
	
	public String title;
	public String message;
	public String ticker;

	public String group;

	public boolean showTime;
	public boolean sound;

	public int color;
	
	public int[] vibrationPattern;

	public int lightsColor;
	public int lightsOn;
	public int lightsOff;
	
	public String smallIconResource;
	public String largeIconResource;
	
	public NotificationData()
	{
		group = null;
		title = null;
		message = null;
		ticker = null;
		showTime = false;
		sound = false;
		vibrationPattern = null;
		lightsColor = 0;
		lightsOn = 0;
		lightsOff = 0;
		largeIconResource = null;
		smallIconResource = null;
		color = 0;
	}
	
	public void SetVibrationPattern (int[] pattern)
	{
		vibrationPattern = pattern;
	}
}
