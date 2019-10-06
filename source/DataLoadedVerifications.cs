using BattleTech;
using BattleTech.Data;
using System;

namespace ModTekTest
{
    internal static class DataLoadedVerifications
    {
        private static string LogPrefix = "SkirmishMechBay.DataLoaded: ";

        internal static void AddedHeatSink(DataManager dm)
        {
            var id = FinishLoadingVerifications.MTTHeatSinkID;
            try
            {
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
                if (component.Description.Details == "mtt test")
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
                if (hit == 9)
                {
                    Control.Logger.Log($"{LogPrefix}CombatGameConstants HitMechLocationFromFront[CenterTorso] was expected 9.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}CombatGameConstants HitMechLocationFromFront[CenterTorso] was not expected 9, it was {hit}.");
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
                if (hit == 24)
                {
                    Control.Logger.Log($"{LogPrefix}CombatGameConstants.Heat.InternalHeatSinkCount was expected 42.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}CombatGameConstants.Heat.InternalHeatSinkCount was not expected 42 it was {hit}.");
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
                if (DebugBridge.UseExperimentalPinkMechFix == false)
                {
                    Control.Logger.Log($"{LogPrefix}DebugBridge.UseExperimentalPinkMechFix was expected false.");
                }
                else
                {
                    Control.Logger.LogError($"{LogPrefix}DebugBridge.UseExperimentalPinkMechFix was not expected false.");
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
                if (list.PickTip() == "ModTekTest Gametip")
                {
                    Control.Logger.Log($"{LogPrefix}GameTipList.General was modified as expected.");
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
