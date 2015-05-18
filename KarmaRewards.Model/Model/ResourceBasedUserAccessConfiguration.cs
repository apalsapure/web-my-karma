using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class ResourceBasedUserAccessConfiguration : IUserAccessConfiguration
    {
        public static ResourceBasedUserAccessConfiguration Instance = new ResourceBasedUserAccessConfiguration();

        private ResourceBasedUserAccessConfiguration()
        {
            LoadAccessRules();
        }

        private List<Role> _roles;
        private List<Feature> _features;

        public IEnumerable<Feature> Features
        {
            get { return _features; }
        }

        public IEnumerable<Role> Roles
        {
            get { return _roles; }
        }

        public int PermissionLength { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private void LoadAccessRules()
        {
            JObject json = null;
            using (var buffer = new MemoryStream(UserAccessResources.Permissions))
            {
                using (var reader = new StreamReader(buffer, Encoding.UTF8))
                {
                    using (var jsonReader = new JsonTextReader(reader))
                    {
                        json = JObject.Load(jsonReader);
                    }
                }
            }

            // Load the features.
            List<Feature> features = new List<Feature>();
            foreach (var featureJson in json["features"] as JArray)
            {
                features.Add(new Feature(int.Parse(featureJson["index"].Value<string>()), featureJson["name"].Value<string>()));
            }

            // Load the roles.
            var userTypes = (json["userTypes"] as JObject).Properties().Select(x => x.Name).ToList();
            List<Role> roles = new List<Role>();
            foreach (var userType in userTypes)
            {
                var roleJsons = (json["userTypes"][userType]["roles"] as JArray).Children<JObject>();
                foreach (var roleJson in roleJsons)
                {
                    roles.Add(
                        new Role
                        {
                            Name = roleJson["role"].Value<string>(),
                            Permission = new AccessPermissions()
                            {
                                Allowed = roleJson["permission"].Value<string>().Split(',').ToList()
                            }
                        });
                }
            }

            // Validate the configuration.
            EnsureFeatureNamesAreUnique(features);

            // Set the values.
            _roles = roles;
            _features = features;
        }

        private void EnsureFeatureNamesAreUnique(List<Feature> features)
        {
            var duplicateNames = features.GroupBy(f => f.Name).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
            if (duplicateNames.Length > 0)
                throw new Exception("Incorrect user access configuration. The following feature names are duplicate - " + string.Join(", ", duplicateNames));
        }

        public AccessPermissions AnonymousPermissions
        {
            get;
            private set;
        }
    }
}
