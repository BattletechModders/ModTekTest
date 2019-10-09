using BattleTech;
using HBS;
using HBS.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace ModTekTest
{
    internal class FinishLoadingVerifications
    {
        internal static string LogPrefix = "FinishedLoading: ";

        internal const string MTTHeatSinkID = "mtt_heatsink";
        internal static void AddedCustomEntry(Dictionary<string, Dictionary<string, VersionManifestEntry>> customResources)
        {
            try
            {
                var manifest = customResources["MTTResource"];
                var entry = manifest["MTTType_ModTekTest"];
                var json = File.ReadAllText(entry.FilePath);
                var customs = new Customs();
                JSONSerializationUtility.FromJSON(customs, json);
                if (customs.Entries[0].id == "first entry")
                {
                    Control.Logger.Log($"{LogPrefix}customResources works.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}Could not find 'first entry' in custom resource.");
                }
            }
            catch (Exception e)
            {
                Control.Logger.LogError($"{LogPrefix}Couldn't read custom resource", e);
            }
        }

        internal static void AddedManifestEntry()
        {
            try
            {
                if (!LazySingletonBehavior<UnityGameInstance>.HasInstance)
                {
                    Control.Logger.LogError($"{LogPrefix}Could not test for {nameof(AddedManifestEntry)} as game not yet init (UnitGameInstance==null).");
                    return;
                }
                var dm = UnityGameInstance.BattleTechGame.DataManager;
                var id = MTTHeatSinkID;

                if (dm.ResourceEntryExists(BattleTechResourceType.HeatSinkDef, id))
                {
                    Control.Logger.Log($"{LogPrefix}Found {id} in manifest.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}Can't find {id} in manifest, make sure the data manager is up-to-date when calling a mods finishedloading.");
                }
            }
            catch (Exception e)
            {
                Control.Logger.LogError($"{LogPrefix}Could not test manifest data", e);
            }
        }
    }
}
