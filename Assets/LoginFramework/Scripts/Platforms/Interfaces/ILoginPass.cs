/***************************** Module Header *****************************\
Module Name:  ILoginPass.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Interface for Backends Platforms

The MIT License (MIT)

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace VitonIT.LoginFramework
{
    public interface ILoginPassPlatform
    {
        void SetLoginPass(string login, string pass);
    }
}