using HarmonyLib;
using System;
using System.Collections.Generic;

namespace Distance.RandomizeCar.Harmony
{
    /// <summary>
    /// Randomizes the car in the player's profile just before loading a level
    /// </summary>
    [HarmonyPatch(typeof(ApplicationEx), "LoadLevel")]
    internal static class ApplicationEx__LoadLevel
    {
        [HarmonyPrefix]
        internal static void RandomizeCarPrefix()
        {
            //Assign some variables
            Random random = new Random();
            ProfileManager profileManager = G.Sys.ProfileManager_;
            Profile currentProfile_ = profileManager.CurrentProfile_;

            if (Mod.Instance.Config.RandomizeCar)
            {
                //Create List for cars to randomize from. 
                List<string> keys = new List<string>();
                if (!Mod.Instance.Config.IncludeCustomCars)
                {
                    foreach (var unlockableCar in ProfileManager.unlockableCars_)
                    {
                        keys.Add(profileManager.CarInfos_[unlockableCar.index_].name_);
                    }
                }
                else
                {
                    keys = new List<string>(profileManager.knownCars_.Keys);
                }

                //Choose a random car from the list
                int randomIndex = random.Next(0, keys.Count);
                if (keys[randomIndex] == "Catalyst" && !G.Sys.SteamworksManager_.OwnsCatalystDLC())
                {
                    currentProfile_.CarName_ = "Spectrum";
                }
                else
                {
                    currentProfile_.CarName_ = keys[randomIndex];
                }
                Mod.Instance.Logger.Debug("Car Name: " + currentProfile_.CarName_);

                
            }

            //Randomize colors as well if that's enabled
            if (Mod.Instance.Config.RandomizeColors)
            {
                currentProfile_.CarColors_ = new CarColors(UnityEngine.Random.ColorHSV(), UnityEngine.Random.ColorHSV(), UnityEngine.Random.ColorHSV(), UnityEngine.Random.ColorHSV());
            }
        }
    }
}
