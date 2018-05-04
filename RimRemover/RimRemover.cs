using Harmony;
using BepInEx;
using System;
using System.Reflection;
using UnityEngine;

namespace BepisPlugin
{

    [BepInPlugin(GUID: "aa2g.kant.rim.remover", Name: "Rim Remover", Version: "1.0")]
    public class RimRemover : BaseUnityPlugin
    {
        void Awake()
        {
            var harmony = HarmonyInstance.Create("aa2g.kant.rim.remover");

            MethodInfo original = AccessTools.Method(typeof(ChaControl), "LoadCharaFbxDataAsync");
            var prefix = new HarmonyMethod(typeof(RimRemover).GetMethod("RemoveRim"));
            harmony.Patch(original, prefix, /*postfix*/null);
        }

        public static void RemoveRim(ChaControl __instance, ref Action<GameObject> actObj)
        {
            Action<GameObject> oldAct = actObj;
            actObj = delegate (GameObject o)
            {
                oldAct(o);
                if (o == null)
                    return;
                var renderers = o.GetComponentsInChildren<Renderer>();
                foreach (var r in renderers)
                    r.material.SetFloat("_rimV", 0f);
                //oldAct(o);
            };
        }

   }

}
