using System.Globalization;
using SDK = Appacitive.Sdk;
using Appacitive.Sdk;
using System;
using System.Collections.Generic;
using ObjectFactory = KarmaRewards.Infrastructure.ObjectFactory;
using Entity = KarmaRewards.Model.Entity;
using KarmaRewards.Model;
using KarmaRewards.Infrastructure.ErrorSpace.Faults;


namespace KarmaRewards.Appacitive
{
    public static class APObjectExtensions
    {
        public static SDK.Credentials ToAPCredential(this Model.Credentials credentials)
        {
            var translator = ObjectFactory.Resolve<ICredentialTranslator>(credentials.Type);
            return translator.Translate(credentials);
        }

        public static User ToUser(this APObject obj)
        {
            if (obj == null) return null;
            EnsureTypeMatches(obj, "user");
            var user = new User
            {
                Id = obj.Id,
                FirstName = obj.Get<string>("firstname"),
                LastName = obj.Get<string>("lastname"),
                Username = obj.Get<string>("username", null),
                Email = obj.Get<string>("email", null),
                Provider = obj.Get<string>("provider", null),
                DateOfBirth = obj.Get<DateTime>("birthdate", DateTime.MinValue),
                ImageUrl = obj.Get<string>("image_url", null),
                Gender = obj.Get<string>("gender", null),
                Designation = obj.Get<string>("designation", null),
                JoiningDate = obj.Get<DateTime>("joining_date")
            };
            obj.CopyEntity(user);
            return user;
        }

        public static void CopyEntity(this APObject obj, Entity entity)
        {
            entity.Id = obj.Id;
            entity.UtcCreatedAt = obj.CreatedAt;
            entity.UtcLastUpdatedAt = obj.LastUpdatedAt;
        }

        public static APUser ToAPUser(this User user)
        {
            APUser obj = null;
            if (string.IsNullOrWhiteSpace(user.Id) == true)
                obj = new APUser();
            else
                obj = new APUser(user.Id);
            obj.Password = user.Password;
            obj.Username = user.Username;
            obj.Set("designation", user.Designation);
            obj.Set("provider", user.Provider);
            obj.Set("gender", user.Gender);
            obj.Set<DateTime>("joining_date", user.JoiningDate);
            obj.Set("image_url", user.ImageUrl);

            obj.FirstName = user.FirstName;
            obj.LastName = user.LastName;
            obj.Email = user.Email;
            obj.DateOfBirth = user.DateOfBirth;
            return obj;
        }

        private static void EnsureTypeMatches(APObject obj, string type)
        {
            if (obj.Type.Equals(type, StringComparison.OrdinalIgnoreCase) == false)
                //throw new Exception("APObject must be of type " + type + ".");
                throw EntityFaults.DoesNotMatch(type);
        }
    }
}
