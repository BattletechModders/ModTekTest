using BattleTech;
using BattleTech.UI;
using Harmony;
using HBS.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ModTekTest
{
    //missing verifications:
    // - resources from a mods unity assets file
    // - gametips
    // - video
    // - soundbank
    // - removal of vanilla data

    public static class Control
    {
        internal static ILog Logger;
        internal const string ModName = nameof(ModTekTest);

        public static void Init(string directory, string settingsJSON)
        {
            Logger = HBS.Logging.Logger.GetLogger(ModName, LogLevel.Debug);

            // test if hbs logging subsystem is accessible
            try
            {
                var logFilePath = Path.Combine(directory, "log.txt");
                var logAppender = new FileLogAppender(logFilePath, FileLogAppender.WriteMode.INSTANT);
                HBS.Logging.Logger.AddAppender(ModName, logAppender);
                InitVerifications.ModDirectoryWritableAndLogSystemUsable(logFilePath);
            }
            catch (Exception e)
            {
                Logger.LogError($"{InitVerifications.LogPrefix}Can't create log file", e);
            }

            Logger.Log("Start the game and enter the skirmish mechaby. You should see Init, FinishLoading and DataLoaded steps in the log.");

            InitVerifications.CorrectModDirectory(directory);
            InitVerifications.EmbeddedSettingsJSON(settingsJSON);

            try
            {
                var harmony = HarmonyInstance.Create(ModName);
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                Logger.Log($"{InitVerifications.LogPrefix}Patched via harmony.");
            }
            catch (Exception e)
            {
                Logger.LogError($"{InitVerifications.LogPrefix}Couldn't patch via harmony", e);
            }
            
            Logger.Log($"{InitVerifications.LogPrefix}Done.");
        }

        // FinishedLoading indicates that the base game manifest and the custom manifest are ready for consumption
        public static void FinishedLoading(Dictionary<string, Dictionary<string, VersionManifestEntry>> customResources)
        {
            try
            {
                FinishLoadingVerifications.AddedCustomEntry(customResources);
                FinishLoadingVerifications.AddedManifestEntry();
                Logger.Log($"{FinishLoadingVerifications.LogPrefix}Done.");
            }
            catch (Exception e)
            {
                Logger.LogError(FinishLoadingVerifications.LogPrefix, e);
            }
        }
    }

    public class Settings
    {
        public string OldSchoolSettingsKey = "It doesn't work";
    }

    public class Customs
    {
        public List<CustomEntry> Entries;

        public class CustomEntry
        {
            public string id;
        }
    }

    [HarmonyPatch(typeof(SkirmishMechBayPanel), "LanceConfiguratorDataLoaded")]
    internal static class SkirmishMechBayPanel_LanceConfiguratorDataLoaded_Patch
    {
        internal static void Postfix(SkirmishMechBayPanel __instance)
        {
            try
            {
                var dm = __instance.dataManager;
                DataLoadedVerifications.AddedHeatSink(dm);
                DataLoadedVerifications.ModifiedHeatSink(dm);
                DataLoadedVerifications.ModifiedCombatGameConstantsViaAdvangedMerge();
                DataLoadedVerifications.ModifiedCombatGameConstantsViaNormaldMerge();
                DataLoadedVerifications.ModifiedDebugSettings();
                DataLoadedVerifications.ModifiedGeneralGameTips();
                Control.Logger.Log($"{DataLoadedVerifications.LogPrefix}Done.");
            }
            catch (Exception e)
            {
                Control.Logger.LogError(DataLoadedVerifications.LogPrefix, e);
            }
        }
    }
}
