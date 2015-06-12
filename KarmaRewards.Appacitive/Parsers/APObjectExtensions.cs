﻿using System.Globalization;
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
                ImageUrl = obj.Get<string>("image_url", string.Empty),
                IsEnabled = obj.Get<bool>("isenabled", false),
                Gender = obj.Get<string>("gender", null),
                Designation = obj.Get<string>("designation", null),
                JoiningDate = obj.Get<DateTime>("joining_date"),
                BirthInDays = obj.Get<int>("birth_days", 0)
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
            obj.Set("isenabled", user.IsEnabled);
            obj.Set("birth_days", user.BirthInDays);

            obj.FirstName = user.FirstName;
            obj.LastName = user.LastName;
            obj.Email = user.Email;
            if (user.DateOfBirth != DateTime.MinValue)
                obj.DateOfBirth = user.DateOfBirth;
            return obj;
        }

        public static APObject ToAPObject(this Profile profile)
        {
            APObject obj = null;
            if (string.IsNullOrWhiteSpace(profile.Id) == true)
                obj = new APObject("profile");
            else
                obj = new APObject("profile", profile.Id);

            profile.CurrentAddress.CopyToAPObject(obj);

            return obj;
        }

        public static void CopyProfile(this APObject obj, Profile profile)
        {
            if (obj == null) return;
            EnsureTypeMatches(obj, "profile");
            profile.CurrentAddress = obj.ToCurrentAddress();
            obj.CopyEntity(profile);
        }

        public static Address ToCurrentAddress(this APObject obj)
        {
            if (obj == null) return null;
            EnsureTypeMatches(obj, "profile");
            var entity = new Address
            {
                AddressLine = obj.Get<string>("cur_address"),
                City = obj.Get<string>("cur_city"),
                State = obj.Get<string>("cur_state"),
                Country = obj.Get<string>("cur_country"),
                Zipcode = obj.Get<string>("cur_zipcode")
            };
            obj.CopyEntity(entity);
            return entity;
        }

        public static void CopyToAPObject(this Address address, APObject obj)
        {
            obj.Set<string>("cur_address", address.AddressLine);
            obj.Set<string>("cur_city", address.City);
            obj.Set<string>("cur_state", address.State);
            obj.Set<string>("cur_country", address.Country);
            obj.Set<string>("cur_zipcode", address.Zipcode);
        }

        public static APObject ToAPPoint(this KarmaPoints points)
        {
            APObject obj = null;
            if (string.IsNullOrWhiteSpace(points.Id) == true)
                obj = new APObject("points");
            else
                obj = new APObject("points", points.Id);

            obj.Set("to", points.To);
            obj.Set("from", points.From);
            obj.Set("reason", points.Reason);
            obj.Set("points", points.Points);
            obj.Set("moderated_by", points.ModeratedBy);
            obj.Set("moderate_reason", points.ModerateReason);

            return obj;
        }

        public static KarmaPoints ToPoints(this APObject obj)
        {
            var point = new KarmaPoints();

            point.To = obj.Get<string>("to", string.Empty);
            point.From = obj.Get<string>("from", string.Empty);
            point.Reason = obj.Get<string>("reason", string.Empty);
            point.ModeratedBy = obj.Get<string>("moderated_by", string.Empty);
            point.ModerateReason = obj.Get<string>("moderate_reason", string.Empty);
            point.Points = obj.Get<int>("points", 0);

            obj.CopyEntity(point);
            return point;
        }

        private static void EnsureTypeMatches(APObject obj, string type)
        {
            if (obj.Type.Equals(type, StringComparison.OrdinalIgnoreCase) == false)
                //throw new Exception("APObject must be of type " + type + ".");
                throw EntityFaults.DoesNotMatch(type);
        }
    }
}
