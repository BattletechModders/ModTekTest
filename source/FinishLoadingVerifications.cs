using BattleTech;
using HBS.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace ModTekTest
{
    internal class FinishLoadingVerifications
    {
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
                    Control.Logger.Log("FinishedLoading: customResources works.");
                }
                else
                {
                    Control.Logger.LogError("FinishedLoading: Could not find 'first entry' in custom resource.");
                }
            }
            catch (Exception e)
            {
                Control.Logger.LogError("FinishedLoading: Couldn't read custom resource", e);
            }
        }

        internal static void AddedManifestEntry()
        {
            try
            {
                var dm = UnityGameInstance.BattleTechGame.DataManager;

                if (dm.ResourceEntryExists(BattleTechResourceType.HeatSinkDef, MTTHeatSinkID))
                {
                    Control.Logger.Log($"FinishedLoading: Found {MTTHeatSinkID} in manifest.");
                }
                else
                {
                    Control.Logger.LogError($"FinishedLoading: Can't find {MTTHeatSinkID} in manifest.");
                }
            }
            catch (Exception e)
            {
                Control.Logger.LogError("FinishedLoading: Could not test manifest data", e);
            }

            Control.Logger.Log("FinishedLoading: Done.");
        }
    }
}
