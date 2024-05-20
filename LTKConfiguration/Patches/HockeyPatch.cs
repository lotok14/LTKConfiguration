using System.Runtime.CompilerServices;
using BepInEx;
using HarmonyLib;
using LTKConfiguration.Extensions;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace LTKConfiguration.Patches
{
    public class HockeyPatch
    {
        public static GameObject puckBarPrefab;
        public static float fillAmount = -1;
        public static GameObject puckBarInstance;
        public static bool filling = false;

        // loads the puckBar prefab
        public static void LoadPuckBarPrefab()
        {
            AssetBundle puckBarAssetBundle = AssetBundle.LoadFromFile(Paths.PluginPath + "/LTKConfiguration/Assets/PuckBar/puckbar.assetBundle");
            UnityEngine.Object[] puckBarAssets = puckBarAssetBundle.LoadAllAssets();
            foreach (UnityEngine.Object asset in puckBarAssets)
            {
                if (asset.name == "puckBarCanvas")
                {
                    HockeyPatch.puckBarPrefab = asset as GameObject;
                }
            }
        }

        // fills the bar when filling set to true
        public static void Update()
        {
            //LTKConfigurationMod.Log.LogWarning($"FILL AMOUNT JEST {fillAmount}");
            if (Object.FindObjectsOfType(typeof(HockeyShooter)).Length == 0 & puckBarPrefab is not null)
            {
                fillAmount = -1;
                Object.Destroy(puckBarInstance);
                filling = false;
            }

            if (filling)
            {
                fillAmount += Time.deltaTime;
            }

            if (fillAmount >= 1 / Modifiers.GetInstance().RateOfFire)
            {
                filling = false;
                fillAmount = 0;
            }
            else if (fillAmount >= 0)
            {
                if (!puckBarInstance)
                {
                    puckBarInstance = Object.Instantiate(puckBarPrefab);
                }
                puckBarInstance.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = fillAmount;
            }
        }

        // starts the filling process
        public static void HockeyShooterWarningSoundPostfix()
        {
            HockeyPatch.filling = true;
            HockeyPatch.fillAmount = 0;
        }
    }
}