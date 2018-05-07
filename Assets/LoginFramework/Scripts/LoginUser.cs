/***************************** Module Header *****************************\
Module Name:  LoginUser.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Login User Data 

The MIT License (MIT)

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
using System;

namespace VitonIT.LoginFramework
{
    public class LoginUser
    {
        public string DisplayName;
        public Uri _photoUrl;
        public string UserId;

        public LoginUser(string name, Uri PhotoUrl, string UserId)
        {
            this.DisplayName = name;
            this._photoUrl = PhotoUrl;
            this.UserId = UserId;
        }

        public Uri PhotoUrl
        {
            get
            {
                return _photoUrl;
            }
        }
    }
}