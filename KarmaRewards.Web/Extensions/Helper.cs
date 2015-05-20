﻿using KarmaRewards.Infrastructure;
using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace KarmaRewards.Web
{
    public static class Helper
    {
        /// <summary>
        /// Creates Current Identity Instance with specified parameters
        /// </summary>
        /// <param name="pUserName">UserName provided by Provider</param>
        /// <param name="emailAddress">EmailAddress provided by Provider</param>
        /// <param name="authenticationProvider">Authentication Provider</param>
        /// <param name="firstName">First Name of User</param>
        /// <param name="lastName">Last Name of User</param>
        public static Identity BuildIdentity(string pUserName, string emailAddress, string authenticationProvider, string firstName, string lastName)
        {
            AuthenticationProvider authProvider = Helper.EnumTryParse<AuthenticationProvider>(authenticationProvider);
            return new Identity(pUserName, emailAddress, authProvider, firstName, lastName);
        }


        /// <summary>
        /// Tries to Parse Given Enum
        /// </summary>
        /// <typeparam name="T">EnumType</typeparam>
        /// <param name="enumValue">EnumValue</param>
        /// <returns>Enum</returns>
        public static T EnumTryParse<T>(string enumValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), enumValue, true);
            }
            catch (Exception)
            {
                throw;
                //ExceptionManager.Log(ex); throw;
            }
        }

        public static string EncryptPassword(string token)
        {
            var encryptedPwd = EncryptionProvider.Encrypt(token);
            if (encryptedPwd.Length > 100) encryptedPwd = encryptedPwd.Substring(0, 99);
            return encryptedPwd;
        }

        public static bool TryParseDate(string date, out DateTime dateTime)
        {
            return DateTime.TryParseExact(date, "dd/MM/yyyy", new CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out dateTime);
        }
    }
}