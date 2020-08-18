using BepInEx;
using System;
using HarmonyLib;
using UnityEngine;

namespace RimRemover
{
    [BepInPlugin(GUID, "Rim Remover", Version)]
    public class RimRemover : BaseUnityPlugin
    {
        public const string GUID = "aa2g.kant.rim.remover";
        public const string Version = "1.1";

        private static int _rimVid;

        private void Awake()
        {
            var s = Config.Bind("General", "Disable Rim Light", false, "Turn off rim light visible around the outline of characters and items." +
                                                                       "\nGives the game a more flat-shaded look. It can make some mods from KK look better in EC." +
                                                                       "\nRestart the game to apply changes.");
            if (s.Value)
            {
                _rimVid = Shader.PropertyToID("_rimV");
                Harmony.CreateAndPatchAll(typeof(RimRemover));
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ChaControl), "LoadCharaFbxDataAsync")]
        private static void RemoveRim(ChaControl __instance, ref Action<GameObject> actObj)
        {
            var oldAct = actObj;
            actObj = delegate (GameObject o)
            {
                oldAct(o);
                if (o == null) return;
                var renderers = o.GetComponentsInChildren<Renderer>();
                foreach (var r in renderers)
                    r.material.SetFloat(_rimVid, 0f);
            };
        }
    }
}
