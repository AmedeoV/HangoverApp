// Helpers/Settings.cs

using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace HangoverApp.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {

        private const string NeedSyncFeedbackKey = "need_sync_feedback";
        private static readonly bool NeedSyncFeedbackDefault = false;

        private const string LastSyncKey = "last_sync";
        private static readonly DateTime LastSyncDefault = DateTime.Now.AddDays(-30);
        private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;

    #endregion


    public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
      }
    }

        public static bool NeedsSync
        {
            get { return true; }
        }

        public static DateTime LastSync
        {
            get
            {
                return AppSettings.GetValueOrDefault<DateTime>(LastSyncKey, LastSyncDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<DateTime>(LastSyncKey, value);
            }
        }

        public static bool NeedSyncFeedback
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(NeedSyncFeedbackKey, NeedSyncFeedbackDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(NeedSyncFeedbackKey, value);
            }
        }

    }
}