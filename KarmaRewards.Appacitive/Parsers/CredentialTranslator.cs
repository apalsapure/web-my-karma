using KarmaRewards.Infrastructure.ErrorSpace;
using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDK = Appacitive.Sdk;

namespace KarmaRewards.Appacitive
{
    public interface ICredentialTranslator
    {
        SDK.Credentials Translate(Credentials credentials);
    }

    public class UsernamePasswordCredentialTranslator : ICredentialTranslator
    {
        public SDK.Credentials Translate(Credentials modelCreds)
        {
            var username = string.Empty;
            var password = string.Empty;
            if (modelCreds.Tokens == null || modelCreds.Tokens.Count == 0) throw new BadRequestException("Credential tokens cannot be null or empty.");
            if (modelCreds.Tokens.ContainsKey("username") == false || string.IsNullOrWhiteSpace(modelCreds.Tokens["username"])) throw new BadRequestException("Username cannot be null or empty.");
            username = modelCreds.Tokens["username"];
            if (modelCreds.Tokens.ContainsKey("password") == false || string.IsNullOrWhiteSpace(modelCreds.Tokens["password"])) throw new BadRequestException("Password cannot be null or empty.");
            password = modelCreds.Tokens["password"];
            var creds = new SDK.UsernamePasswordCredentials(username, password)
            {
                TimeoutInSeconds = modelCreds.Expiry <= 0 ? 60 * 60 : modelCreds.Expiry,
                MaxAttempts = modelCreds.Attempts <= 0 ? int.MaxValue : modelCreds.Attempts
            };

            return creds;
        }
    }

    public class TokenCredentialTranslator : ICredentialTranslator
    {
        public SDK.Credentials Translate(Credentials modelCreds)
        {
            var token = string.Empty;
            if (modelCreds.Tokens == null || modelCreds.Tokens.Count == 0) throw new BadRequestException("Credential tokens cannot be null or empty.");
            if (modelCreds.Tokens.ContainsKey("token") == false || string.IsNullOrWhiteSpace(modelCreds.Tokens["token"])) throw new BadRequestException("token cannot be null or empty.");
            token = modelCreds.Tokens["token"];

            var creds = new SDK.UserTokenCredentials(token)
            {
                TimeoutInSeconds = modelCreds.Expiry <= 0 ? 15 * 60 : modelCreds.Expiry,
                MaxAttempts = modelCreds.Attempts <= 0 ? int.MaxValue : modelCreds.Attempts
            };

            return creds;
        }
    }

    public class FacebookCredentialTranslator : ICredentialTranslator
    {
        public SDK.Credentials Translate(Credentials modelCreds)
        {
            var token = string.Empty;
            if (modelCreds.Tokens == null || modelCreds.Tokens.Count == 0) throw new BadRequestException("Credential tokens cannot be null or empty.");
            if (modelCreds.Tokens.ContainsKey("accesstoken") == false || string.IsNullOrWhiteSpace(modelCreds.Tokens["accesstoken"])) throw new BadRequestException("accesstoken cannot be null or empty.");
            token = modelCreds.Tokens["accesstoken"];

            var creds = new SDK.OAuth2Credentials(token, "facebook")
            {
                TimeoutInSeconds = modelCreds.Expiry <= 0 ? 60 * 60 : modelCreds.Expiry,
                MaxAttempts = modelCreds.Attempts <= 0 ? int.MaxValue : modelCreds.Attempts,
                CreateUserIfNotExists = modelCreds.CreateNew
            };

            return creds;
        }
    }

    public class TwitterCredentialTranslator : ICredentialTranslator
    {
        public SDK.Credentials Translate(Credentials modelCreds)
        {
            var consumerkey = string.Empty;
            var consumersecret = string.Empty;
            var token = string.Empty;
            var tokensecret = string.Empty;
            if (modelCreds.Tokens == null || modelCreds.Tokens.Count == 0) throw new BadRequestException("Credential tokens cannot be null or empty.");
            if (modelCreds.Tokens.ContainsKey("consumerkey") && string.IsNullOrWhiteSpace(modelCreds.Tokens["consumerkey"]) == false)
                consumerkey = modelCreds.Tokens["consumerkey"];
            if (modelCreds.Tokens.ContainsKey("consumersecret") && string.IsNullOrWhiteSpace(modelCreds.Tokens["consumersecret"]) == false)
                consumersecret = modelCreds.Tokens["consumersecret"];
            if (modelCreds.Tokens.ContainsKey("accesstoken") == false || string.IsNullOrWhiteSpace(modelCreds.Tokens["accesstoken"])) throw new BadRequestException("accesstoken cannot be null or empty.");
            if (modelCreds.Tokens.ContainsKey("accesstokensecret") == false || string.IsNullOrWhiteSpace(modelCreds.Tokens["accesstokensecret"])) throw new BadRequestException("accesstokensecret cannot be null or empty.");
            token = modelCreds.Tokens["accesstoken"];
            tokensecret = modelCreds.Tokens["accesstokensecret"];

            var creds = new SDK.OAuth1Credentials(consumerkey, consumersecret, token, tokensecret, "twitter")
            {
                TimeoutInSeconds = modelCreds.Expiry <= 0 ? 60 * 60 : modelCreds.Expiry,
                MaxAttempts = modelCreds.Attempts <= 0 ? int.MaxValue : modelCreds.Attempts,
                CreateUserIfNotExists = modelCreds.CreateNew
            };

            return creds;
        }
    }

    public class GooglePlusCredentialTranslator : ICredentialTranslator
    {
        public SDK.Credentials Translate(Credentials modelCreds)
        {
            var token = string.Empty;
            if (modelCreds.Tokens == null || modelCreds.Tokens.Count == 0) throw new BadRequestException("Credential tokens cannot be null or empty.");
            if (modelCreds.Tokens.ContainsKey("accesstoken") == false || string.IsNullOrWhiteSpace(modelCreds.Tokens["accesstoken"])) throw new BadRequestException("accesstoken cannot be null or empty.");
            token = modelCreds.Tokens["accesstoken"];

            var creds = new SDK.OAuth2Credentials(token, "google+")
            {
                TimeoutInSeconds = modelCreds.Expiry <= 0 ? 60 * 60 : modelCreds.Expiry,
                MaxAttempts = modelCreds.Attempts <= 0 ? int.MaxValue : modelCreds.Attempts,
                CreateUserIfNotExists = modelCreds.CreateNew
            };

            return creds;
        }
    }
}