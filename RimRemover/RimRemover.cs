using BepInEx;
using BepInEx.Harmony;
using HarmonyLib;
using System;
using UnityEngine;

namespace RimRemover
{
    [BepInPlugin(GUID, "Rim Remover", Version)]
    public class RimRemover : BaseUnityPlugin
    {
        public const string GUID = "aa2g.kant.rim.remover";
        public const string Version = "1.0.1";

        internal void Awake() => HarmonyWrapper.PatchAll(typeof(Hooks));

        internal static class Hooks
        {
            [HarmonyPrefix, HarmonyPatch(typeof(ChaControl), "LoadCharaFbxDataAsync")]
            internal static void RemoveRim(ref Action<GameObject> actObj)
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
                };
            }
        }
    }
}
