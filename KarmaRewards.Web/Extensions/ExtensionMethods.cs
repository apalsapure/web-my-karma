using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarmaRewards.Web
{
    public static class ExtensionMethods
    {
        public static void UpdateDateString(this User user)
        {
            if (user.DateOfBirth != null && user.DateOfBirth.HasValue)
                user.DateOfBirthStr = Helper.ToDateString(user.DateOfBirth.Value);
            user.JoiningDateStr = Helper.ToDateString(user.JoiningDate);
        }
    }
}