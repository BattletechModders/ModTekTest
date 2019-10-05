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
                Logger.LogError("Init: Can't create log file", e);
            }

            InitVerifications.CorrectModDirectory(directory);
            InitVerifications.EmbeddedSettingsJSON(settingsJSON);

            try
            {
                var harmony = HarmonyInstance.Create(ModName);
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                Logger.Log("Init: Patched via harmony.");
            }
            catch (Exception e)
            {
                Logger.LogError("Init: Couldn't patch via harmony", e);
            }

            Logger.Log("Init: Done.");
        }

        // FinishedLoading indicates that the base game manifest and the custom manifest are ready for consumption
        public static void FinishLoadingCustomEntry(Dictionary<string, Dictionary<string, VersionManifestEntry>> customResources)
        {
            FinishLoadingVerifications.AddedCustomEntry(customResources);
            FinishLoadingVerifications.AddedManifestEntry();
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
            var dm = __instance.dataManager;
            DataLoadedVerifications.AddedHeatSink(dm);
            DataLoadedVerifications.ModifiedHeatSink(dm);
            DataLoadedVerifications.ModifiedCombatGameConstantsViaAdvangedMerge();
            DataLoadedVerifications.ModifiedCombatGameConstantsViaNormaldMerge();
            DataLoadedVerifications.ModifiedDebugSettings();
        }
     }
}
