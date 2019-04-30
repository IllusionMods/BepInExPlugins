using Harmony;
using BepInEx;
using System;
using UnityEngine;

namespace EC_RimRemover
{
    //Original Koikatsu plugin by https://github.com/geneishouko
    [BepInProcess("EmotionCreators")]
    [BepInPlugin(GUID: "EC.RimRemover", Name: "Rim Remover", Version: "1.0")]
    public class RimRemover : BaseUnityPlugin
    {
        void Awake()
        {

            var harmony = HarmonyInstance.Create("Rim Remover");            
            BepInEx.Harmony.HarmonyWrapper.PatchAll(typeof(RimRemover));
          
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ChaControl),"LoadCharaFbxDataAsync")]
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
