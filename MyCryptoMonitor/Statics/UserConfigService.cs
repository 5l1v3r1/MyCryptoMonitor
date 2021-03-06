﻿using MyCryptoMonitor.Configs;
using MyCryptoMonitor.Objects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyCryptoMonitor.Statics
{
    public static class UserConfigService
    {
        #region Private Fields

        private const string FILENAME = "User.config";

        #endregion Private Fields

        #region Public Properties

        public static string Currency { get { return UserConfig.Currency; } set { UserConfig.Currency = value; Save(); MainService.SetCurrencySymbol(); } }
        public static List<string> CustomCoins { get => UserConfig.CustomCoins.ToList(); }
        public static bool DeleteAlerts { get { return UserConfig.DeleteAlerts; } set { UserConfig.DeleteAlerts = value; Save(); } }
        public static bool Encrypted { get { return UserConfig.Encrypted; } set { UserConfig.Encrypted = value; Save(); } }
        public static string EncryptionCheck { get { return UserConfig.EncryptionCheck; } set { UserConfig.EncryptionCheck = value; Save(); } }
        public static int RefreshTime { get { return UserConfig.RefreshTime; } set { UserConfig.RefreshTime = value; Save(); } }
        public static string SaltKey { get => UserConfig.SaltKey; }
        public static string StartupPortfolio { get { return UserConfig.StartupPortfolio; } set { UserConfig.StartupPortfolio = value; Save(); } }
        public static Theme Theme { get { return UserConfig.Theme; } set { UserConfig.Theme = value; Save(); } }
        public static bool TutorialCompleted { get { return UserConfig.TutorialCompleted; } set { UserConfig.TutorialCompleted = value; Save(); } }
        public static bool MinerEnabled { get { return UserConfig.MinerEnabled; } set { UserConfig.MinerEnabled = value; Save(); } }
        public static int MinerPercentage { get { return UserConfig.MinerPercentage; } set { UserConfig.MinerPercentage = value; Save(); } }

        #endregion Public Properties

        #region Private Properties

        private static UserConfig UserConfig { get; set; }

        #endregion Private Properties

        #region Public Methods

        public static void Create()
        {
            File.WriteAllText(FILENAME, JsonConvert.SerializeObject(new UserConfig()));
            Load();
        }

        public static void Delete()
        {
            if (File.Exists(FILENAME))
                File.Delete(FILENAME);

            Create();
        }

        public static void Load()
        {
            if (File.Exists(FILENAME))
                UserConfig = JsonConvert.DeserializeObject<UserConfig>(File.ReadAllText(FILENAME));
            else
                Create();
        }

        #endregion Public Methods

        #region Private Methods

        private static void Save()
        {
            File.WriteAllText(FILENAME, JsonConvert.SerializeObject(UserConfig));
        }

        #endregion Private Methods
    }
}