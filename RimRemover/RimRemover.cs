using BepInEx;
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

        internal void Awake()
        {
            var s = Config.Bind("General", "Disable Rim Light", false, "Turn off rim light visible around the outline of characters and items." +
                                                                       "\nGives the game a more flat-shaded look. It can make some mods from KK look better in EC." +
                                                                       "\nRestart the game to apply changes.");

            if (s.Value)
                Harmony.CreateAndPatchAll(typeof(Hooks));
        }

        internal static class Hooks
        {
            [HarmonyPrefix, HarmonyPatch(typeof(ChaControl), "LoadCharaFbxDataAsync")]
            internal static void RemoveRim(ref Action<GameObject> actObj)
            {
                var oldAct = actObj;
                actObj = delegate (GameObject o)
                {
                    oldAct(o);
                    if (o == null) return;
                    foreach (var r in o.GetComponentsInChildren<Renderer>())
                        r.material.SetFloat("_rimV", 0f);
                };
            }
        }
    }
}
