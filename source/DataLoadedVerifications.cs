using BattleTech;
using BattleTech.Data;
using System;

namespace ModTekTest
{
    internal static class DataLoadedVerifications
    {
        internal static string LogPrefix = "DataLoaded: ";

        internal static void AddedHeatSink(DataManager dm)
        {
            var id = FinishLoadingVerifications.MTTHeatSinkID;
            try
            {
                if (dm.ResourceEntryExists(BattleTechResourceType.HeatSinkDef, id))
                {
                    Control.Logger.Log($"{LogPrefix}Found {id} via dm.ResourceEntryExists.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}Can't find {id} via dm.ResourceEntryExists.");
                }

                { // LanceConfiguratorDataLoaded does not load ALL heatsinkdefs
                    var request = dm.CreateLoadRequest();
                    request.AddAllOfTypeBlindLoadRequest(BattleTechResourceType.HeatSinkDef);
                    //request.AddLoadRequest<HeatSinkDef>(BattleTechResourceType.HeatSinkDef, id, null);
                    request.ProcessRequests();

                    Control.Logger.Log($"{LogPrefix}ProcessRequests AddAllOfTypeBlindLoadRequest HeatSinkDef.");
                }

                if (dm.Exists(BattleTechResourceType.HeatSinkDef, id))
                {
                    Control.Logger.Log($"{LogPrefix}Found {id} via dm.Exists.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}Can't find {id} via dm.Exists.");
                }

                var component = dm.HeatSinkDefs.Get(id);
                if (component.Description.Details == "ModTekTest Component")
                {
                    Control.Logger.Log($"{LogPrefix}{id} was found in datamanager.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}{id} is missing in datamanager.");
                }
            }
            catch (Exception e)
            {
                Control.Logger.LogError($"{LogPrefix}Could not validate {id} presence", e);
            }
        }

        internal static void ModifiedHeatSink(DataManager dm)
        {
            var id = "Gear_HeatSink_Generic_Standard";
            try
            {
                var component = dm.HeatSinkDefs.Get("Gear_HeatSink_Generic_Standard");
                var expected = "mtt test";
                if (component.Description.Details == expected)
                {
                    Control.Logger.Log($"{LogPrefix}{id} has new details.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}{id} has incorrect details.");
                }
            }
            catch (Exception e)
            {
                Control.Logger.LogError($"{LogPrefix}Could not validate {id} modification", e);
            }
        }

        internal static void ModifiedCombatGameConstantsViaAdvangedMerge()
        {
            try
            {
                var constants = CombatGameConstants.GetInstance(UnityGameInstance.BattleTechGame);
                var hit = constants.HitTables.HitMechLocationFromFront[ArmorLocation.CenterTorso];
                var expected = 9;
                if (hit == expected)
                {
                    Control.Logger.Log($"{LogPrefix}CombatGameConstants HitMechLocationFromFront[CenterTorso] was expected {expected}.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}CombatGameConstants HitMechLocationFromFront[CenterTorso] was not expected {expected}, it was {hit}.");
                }
            }
            catch (Exception e)
            {
                Control.Logger.LogError($"{LogPrefix}Could not validate CombatGameConstants modification", e);
            }
        }
        
        internal static void ModifiedCombatGameConstantsViaNormaldMerge()
        {
            try
            {
                var constants = CombatGameConstants.GetInstance(UnityGameInstance.BattleTechGame);
                var hit = constants.Heat.InternalHeatSinkCount;
                var expected = 42;
                if (hit == expected)
                {
                    Control.Logger.Log($"{LogPrefix}CombatGameConstants.Heat.InternalHeatSinkCount was expected {expected}.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}CombatGameConstants.Heat.InternalHeatSinkCount was not expected {expected} it was {hit}.");
                }
            }
            catch (Exception e)
            {
                Control.Logger.LogError($"{LogPrefix}Could not validate CombatGameConstants modification", e);
            }
        }

        internal static void ModifiedDebugSettings()
        {
            try
            {
                var expected = false;
                if (DebugBridge.UseExperimentalPinkMechFix == expected)
                {
                    Control.Logger.Log($"{LogPrefix}DebugBridge.UseExperimentalPinkMechFix was expected {expected}.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}DebugBridge.UseExperimentalPinkMechFix was not expected {expected}.");
                }
            }
            catch (Exception e)
            {
                Control.Logger.LogError($"{LogPrefix}Could not validate CombatGameConstants modification", e);
            }
        }

        internal static void ModifiedGeneralGameTips()
        {
            try
            {
                var list = new GameTipList("general.txt", 0);
                var expected = "ModTekTest Gametip";
                if (list.PickTip() == expected)
                {
                    Control.Logger.Log($"{LogPrefix}GameTipList.General was modified as expected to {expected}.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}GameTipList.General was not modified.");
                }
            }
            catch (Exception e)
            {
                Control.Logger.LogError($"{LogPrefix}Could not validate GameTips", e);
            }
        }
    }
}
