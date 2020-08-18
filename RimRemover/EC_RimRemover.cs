using BepInEx;
using System;
using HarmonyLib;
using UnityEngine;

namespace EC_RimRemover
{
    [BepInProcess("EmotionCreators")]
    [BepInPlugin("EC.RimRemover", "Rim Remover", "1.0")]
    public class RimRemover : BaseUnityPlugin
    {
        private static readonly int _rimV = Shader.PropertyToID("_rimV");

        private void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(RimRemover));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ChaControl), "LoadCharaFbxDataAsync")]
        public static void RemoveRim(ChaControl __instance, ref Action<GameObject> actObj)
        {
            var oldAct = actObj;
            actObj = delegate (GameObject o)
            {
                oldAct(o);
                if (o == null) return;
                var renderers = o.GetComponentsInChildren<Renderer>();
                foreach (var r in renderers)
                    r.material.SetFloat(_rimV, 0f);
                //oldAct(o);
            };
        }
    }
}
