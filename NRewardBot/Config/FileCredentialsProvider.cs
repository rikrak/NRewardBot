#nullable enable
using System;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace NRewardBot.Config
{
    public class FileCredentialsProvider
    {
        #region Logging

        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        #endregion

        private const string FileName = "credentials.nrb";

        public ICredentials SetCredentials(string username, string password, string key)
        {
            var credentials = new CredentialsJson()
            {
                Username = username,
                Password = password
            };

            var json = JsonConvert.SerializeObject(credentials);
            var jsonEncrypted = Encrypt(json, key);
            File.WriteAllText(FileName, jsonEncrypted);

            return GetCredentials(key);
        }

        public ICredentials GetCredentials(string key)
        {
            if (!File.Exists(FileName))
            {
                return new CredentialsJson();
            }
            
            var encryptedCredentials = File.ReadAllText(FileName);

            var credentialsJson = Decrypt(encryptedCredentials, key);

            CredentialsJson? credentials;
            try
            {
                credentials = JsonConvert.DeserializeObject<CredentialsJson>(credentialsJson);
            }
            catch (Exception e)
            {
                Log.Error("The credentials could not be de-coded");
                Log.Debug(e);
                credentials = new CredentialsJson();
            }

            return credentials ?? new CredentialsJson();
        }

        private static string Encrypt(string plaintext, string key)
        {
            return plaintext.Encrypt(key: key);
        }

        public static string Decrypt(string cipherText, string key)
        {
            return cipherText.Decrypt(key: key);
        }
    }
}
