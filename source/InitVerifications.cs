using HBS.Util;
using System;
using System.IO;

namespace ModTekTest
{
    public static class InitVerifications
    {
        internal static string LogPrefix = "Init: ";
        internal static void ModDirectoryWritableAndLogSystemUsable(string logFilePath)
        {
            if (!File.Exists(logFilePath))
            {
                // test if mod directory can be written too
                Control.Logger.LogError($"{LogPrefix}Can't find logfile {logFilePath}.");
            }
        }

        internal static void CorrectModDirectory(string directory)
        {
            // test if directory is correctly passed on
            {
                var dllPath = Path.Combine(directory, $"{Control.ModName}.dll");
                if (!File.Exists(dllPath))
                {
                    Control.Logger.LogError($"{LogPrefix}Can't find dll {dllPath}, wrong directory?");
                }
            }
        }

        internal static void EmbeddedSettingsJSON(string settingsJSON)
        {
            // test if embedded mod.json settings work
            try
            {
                var settings = new Settings();
                JSONSerializationUtility.FromJSON(settings, settingsJSON);
                if (settings.OldSchoolSettingsKey == "It works")
                {
                    Control.Logger.Log($"{LogPrefix}Parsing embedded json works.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}Embedded settings could not be read, didn't find {nameof(Settings.OldSchoolSettingsKey)}='It works', json data is: {settingsJSON}.");
                }
            }
            catch (Exception e)
            {
                Control.Logger.LogError($"{LogPrefix}Couldn't load embedded json, json data is: {settingsJSON}", e);
            }
        }
    }
}
