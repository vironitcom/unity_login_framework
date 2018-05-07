/***************************** Module Header *****************************\
Module Name:  LoginErrorCodes.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Error codes called Login Framework

The MIT License (MIT)

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace VitonIT.LoginFramework
{

    public struct LoginErrorCodes
    {
        public static string EMAIL_PASS_IS_NULL = "pass_or_mail_are_null";
        public static string SIGNINASYNC_WAS_CANCELED = "signinasync_was_canceled";
        public static string SIGNINASYNC_ENCOUNTERED_AN_ERROR = "signinasync_encountered_an_error";
    }
}