using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using UnityEngine;

namespace RimRemover
{
    [BepInPlugin(GUID, "Rim Remover", Version)]
    public class RimRemover : BaseUnityPlugin
    {
        public const string GUID = "aa2g.kant.rim.remover";
        public const string Version = "1.1";

        public static ConfigEntry<bool> ConfigDisableRim { get; private set; }

        internal void Awake()
        {
            ConfigDisableRim = Config.Bind("General", "Disable Rim Light", false, "Turn off rim light visible around the outline of characters and items." +
                                                      "\nGives the game a more flat-shaded look. It can make some mods from KK look better in EC." +
                                                      "\nReload the character or scene to apply changes.");

            Harmony.CreateAndPatchAll(typeof(Hooks));
        }

        internal static class Hooks
        {
            [HarmonyPrefix, HarmonyPatch(typeof(ChaControl), "LoadCharaFbxDataAsync")]
            internal static void RemoveRim(ref Action<GameObject> actObj)
            {
                if (!ConfigDisableRim.Value) return;

                var oldAct = actObj;
                actObj = delegate (GameObject o)
                {
                    oldAct(o);

                    if (o != null)
                    {
                        foreach (var r in o.GetComponentsInChildren<Renderer>())
                            if (r != null && r.materials != null)
                                foreach (var mat in r.materials)
                                    if (mat != null)
                                        mat.SetFloat("_rimV", 0f);
                    }
                };
            }
        }
    }
}
