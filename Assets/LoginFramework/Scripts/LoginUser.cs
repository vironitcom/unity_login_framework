using System;

public class LoginUser
{
    public string DisplayName;
    public Uri _photoUrl;
    public string UserId;

    public LoginUser (string name, Uri PhotoUrl, string UserId)
    {
        this.DisplayName = name;
        this._photoUrl = PhotoUrl;
        this.UserId = UserId;
    }

    public Uri PhotoUrl {
        get
        {
            return _photoUrl;
        }
    }
}