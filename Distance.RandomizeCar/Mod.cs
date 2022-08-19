using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System;
using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Data;
using Centrifuge.Distance.GUI.Controls;
using UnityEngine;

namespace Distance.RandomizeCar
{
	/// <summary>
	/// The mod's main class containing its entry point
	/// </summary>
	[ModEntryPoint("RandomizeCar")]
	public sealed class Mod : MonoBehaviour
	{
		public static Mod Instance { get; private set; }

		public IManager Manager { get; private set; }

		public Log Logger { get; private set; }

        public ConfigLogic Config { get; private set; }

        /// <summary>
        /// Method called as soon as the mod is loaded.
        /// WARNING:	Do not load asset bundles/textures in this function
        ///				The unity assets systems are not yet loaded when this
        ///				function is called. Loading assets here can lead to
        ///				unpredictable behaviour and crashes!
        /// </summary>
        public void Initialize(IManager manager)
		{
			// Do not destroy the current game object when loading a new scene
			DontDestroyOnLoad(this);

			Instance = this;

			Manager = manager;

			// Create a log file
			Logger = LogManager.GetForCurrentAssembly();
            Config = gameObject.AddComponent<ConfigLogic>();


            Logger.Info("Thanks for using randomize car!");

            try
            {
                CreateSettingsMenu();
            }
            catch (Exception e)
            {
                Logger.Info(e);
                Logger.Info("This likely happened because you have the wrong version of Centrifuge.Distance. \nTo fix this, be sure to use the Centrifuge.Distance.dll file that came included with the mod's zip file. \nDespite this error, the mod will still function, however, you will not have access to the mod's menu.");
            }

            RuntimePatcher.AutoPatch();
		}

        private void CreateSettingsMenu()
        {
            MenuTree settingsMenu = new MenuTree("menu.mod.randomizecar", "Randomize Car Settings")
            {
                new CheckBox(MenuDisplayMode.Both, "settings:randomize_car", "RANDOMIZE CAR")
                .WithGetter(() => Config.RandomizeCar)
                .WithSetter((x) => Config.RandomizeCar = x)
                .WithDescription("Toggle whether or not cars randomize"),

                new CheckBox(MenuDisplayMode.Both, "settings:randomize_colors", "RANDOMIZE COLORS")
                .WithGetter(() => Config.RandomizeColors)
                .WithSetter((x) => Config.RandomizeColors = x)
                .WithDescription("Toggle whether or not colors randomize"),
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "RANDOMIZE CAR", "Settings what for the randomize car mod.");
        }
	}
}



