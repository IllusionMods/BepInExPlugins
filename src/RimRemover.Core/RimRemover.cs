using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using UnityEngine;

namespace RimRemover
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class RimRemover : BaseUnityPlugin
    {
        public const string PluginGUID = "aa2g.kant.rim.remover";
        public const string PluginName = "Rim Remover";
        public const string PluginVersion = "1.2";

        public static ConfigEntry<bool> ConfigDisableRim { get; private set; }

        private void Awake()
        {
            ConfigDisableRim = Config.Bind("General", "Disable Rim Light", false, "Turn off rim light visible around the outline of characters and items." +
                                                      "\nGives the game a more flat-shaded look." +
                                                      "\nReload the character or scene to apply changes.");

            Harmony.CreateAndPatchAll(typeof(Hooks));
        }

        private static class Hooks
        {
            [HarmonyPrefix]
            [HarmonyPatch(typeof(ChaControl), nameof(ChaControl.LoadCharaFbxDataAsync))]
#if KKS
            [HarmonyPatch(typeof(ChaControl), nameof(ChaControl.LoadCharaFbxDataNoAsync))]
#endif
            private static void RemoveRim(ref Action<GameObject> actObj)
            {
                if (!ConfigDisableRim.Value) return;

                var oldAct = actObj;
                actObj = delegate (GameObject o)
                {
                    oldAct?.Invoke(o);

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
