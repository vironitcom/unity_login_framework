/***************************** Module Header *****************************\
Module Name:  IBackerndPlatform.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Штеукафсу for Backends Platforms

The MIT License (MIT)

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace VitonIT.LoginFramework
{
    public interface IBackerndPlatform
    {
        LoginUser GetUser();
        void Init();
        void RegisterUser(string mail, string pass, string name);
        void SignOut();
    }
}