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
            Random random = new Random();
            List<string> keys = new List<string>(G.Sys.ProfileManager_.knownCars_.Keys);
            Profile currentProfile_ = G.Sys.ProfileManager_.CurrentProfile_;
            int randomIndex = random.Next(0, keys.Count);


            if (Mod.Instance.Config.RandomizeCar)
            {
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
            if (Mod.Instance.Config.RandomizeColors)
            {
                currentProfile_.CarColors_ = new CarColors(UnityEngine.Random.ColorHSV(), UnityEngine.Random.ColorHSV(), UnityEngine.Random.ColorHSV(), UnityEngine.Random.ColorHSV());
            }
        }
    }
}
