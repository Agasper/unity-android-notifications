package net.agasper.unitynotification;

import android.os.Parcel;
import android.os.Parcelable;

/**
 * Created by gileadis on 11/18/17.
 */

public class NotificationAction implements Parcelable {
    public static final Creator CREATOR = new Creator();

    private String identifier;
    private String title;
    private String icon;
    private boolean foreground;
    private String gameObject;
    private String handlerMethod;

    public String getIdentifier() {
        return identifier;
    }
    public void setIdentifier(String identifier) {
        this.identifier = identifier;
    }

    public String getTitle() {
        return title;
    }
    public void setTitle(String title) {
        this.title = title;
    }

    public String getIcon() {
        return icon;
    }
    public void setIcon(String icon) {
        this.icon = icon;
    }

    public boolean isForeground() { return foreground; }
    public void setForeground(boolean foreground) { this.foreground = foreground; }

    public String getGameObject() {
        return gameObject;
    }
    public void setGameObject(String gameObject) {
        this.gameObject = gameObject;
    }

    public String getHandlerMethod() {
        return handlerMethod;
    }
    public void setHandlerMethod(String handlerMethod) {
        this.handlerMethod = handlerMethod;
    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeString(getIdentifier());
        dest.writeString(getTitle());
        dest.writeString(getIcon());
        dest.writeInt(isForeground() ? 1 : 0);
        dest.writeString(getGameObject());
        dest.writeString(getHandlerMethod());
    }

    private static class Creator implements Parcelable.Creator<NotificationAction> {

        @Override
        public NotificationAction createFromParcel(Parcel source) {
            NotificationAction action = new NotificationAction();
            action.setIdentifier(source.readString());
            action.setTitle(source.readString());
            action.setIcon(source.readString());
            action.setForeground(source.readInt() == 1);
            action.setGameObject(source.readString());
            action.setHandlerMethod(source.readString());
            return action;
        }

        @Override
        public NotificationAction[] newArray(int size) {
            return new NotificationAction[size];
        }
    }
}
