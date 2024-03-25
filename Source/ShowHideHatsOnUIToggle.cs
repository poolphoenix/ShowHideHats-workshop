using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ShowHideHatsOnUIToggle
{
    // This mod adds a new toggleable icon to the game's user interface that allows players to toggle the HatsOnlyOnMap setting on and off
    [StaticConstructorOnStartup]
    public static class ShowHideHatsOnUIToggle
    {
        public static readonly Texture2D ShowHatsIcon = ContentFinder<Texture2D>.Get("Things/Pawn/Humanlike/Apparel/CowboyHat/CowboyHat");

        static ShowHideHatsOnUIToggle()
        {
            Log.Message("[ShowHideHats] Initialized.");

            var harmony = new Harmony("com.phoenix.showhidehats");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(PlaySettings), "DoPlaySettingsGlobalControls")]
        public static class PlaySettings_DoPlaySettingsGlobalControls_Patch
        {
            public static void Postfix(WidgetRow row, bool worldView)
            {
                if (worldView || row == null) return;

                var hatsOnlyOnMap = !Prefs.HatsOnlyOnMap;
                row.ToggleableIcon(ref hatsOnlyOnMap, ShowHatsIcon, "ToggleVisibilityOfHats".Translate(), SoundDefOf.Mouseover_ButtonToggle);

                if (hatsOnlyOnMap == Prefs.HatsOnlyOnMap)
                {
                    Prefs.HatsOnlyOnMap = !hatsOnlyOnMap;
                    PortraitsCache.Clear();
                    Prefs.Save();
                }
            }
        }
    }
}
