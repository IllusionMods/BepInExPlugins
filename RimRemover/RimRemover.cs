using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace BepisPlugin
{
    [BepInPlugin(GUID, "Rim Remover", Version)]
    public class RimRemover : BaseUnityPlugin
    {
        public const string GUID = "aa2g.kant.rim.remover";
        public const string Version = "1.0.1";

        private void Awake()
        {
            var harmony = new Harmony("aa2g.kant.rim.remover");

            MethodInfo original = AccessTools.Method(typeof(ChaControl), "LoadCharaFbxDataAsync");
            var prefix = new HarmonyMethod(AccessTools.Method(GetType(), nameof(RemoveRim)));
            harmony.Patch(original, prefix, /*postfix*/null);
        }

        private static void RemoveRim(ChaControl __instance, ref Action<GameObject> actObj)
        {
            Action<GameObject> oldAct = actObj;
            actObj = delegate (GameObject o)
            {
                oldAct(o);
                if(o == null)
                    return;
                var renderers = o.GetComponentsInChildren<Renderer>();
                foreach(var r in renderers)
                    r.material.SetFloat("_rimV", 0f);
                //oldAct(o);
            };
        }

    }

}
