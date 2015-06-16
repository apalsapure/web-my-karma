using KarmaRewards.Infrastructure;
using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace KarmaRewards.Web
{
    public static class Helper
    {
        private const string DATE_FORMAT = "dd/MM/yyyy";
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
            return DateTime.TryParseExact(date, DATE_FORMAT, new CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out dateTime);
        }

        public static string ToDateString(DateTime dateTime)
        {
            return dateTime.ToString(DATE_FORMAT);
        }

        public static string GetProfileImageForEmail(string email)
        {
            return ConfigurationManager.AppSettings["gravatar-url"] + CalculateMD5Hash(email);
        }

        public static string GetProfileImage(string userImage, string size)
        {
            return string.IsNullOrEmpty(userImage) ? "/Resources/img/avatar.png" : userImage + "?s=" + size + "&d=" + KarmaRewards.Web.Helper.GetDefaultProfileImage();
        }

        public static string GetDefaultProfileImage()
        {
            return ConfigurationManager.AppSettings["default-image"];
        }

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.Default.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }
    }
}