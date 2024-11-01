using System.Runtime.CompilerServices;
using BepInEx;
using GameEvent;
using HarmonyLib;
using LTKConfiguration.Extensions;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using System;
using LTKConfiguration.Utils;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;
using static UnityEngine.ParticleSystem;
using System.Drawing;
using System.Collections;

namespace LTKConfiguration.Utils
{
    public class PuckBarController
    {
        private static GameObject puckBarPrefab;
        private static float fillAmount = -1;
        private static GameObject puckBarInstance;
        private static bool filling = false;
        private static int puckBarType;
        private static LayerMask puckInteractsLayers;
        private static AnimalCannon lastCannon;
        private static Teleporter lastTeleporter;

        // loads the puckBar prefab
        public static void LoadPuckBarPrefab()
        {
            AssetBundle puckBarAssetBundle = AssetBundle.LoadFromFile(Paths.PluginPath + "/LTKConfiguration/Assets/PuckBar/puckbar.assetBundle");
            UnityEngine.Object[] puckBarAssets = puckBarAssetBundle.LoadAllAssets();
            foreach (UnityEngine.Object asset in puckBarAssets)
            {
                if (asset.name == "puckBarCanvas")
                {
                    PuckBarController.puckBarPrefab = asset as GameObject;
                }
            }
        }

        // sets the layers for the puckbar line drawing
        public static void SetPuckInteractLayers(LayerMask layers)
        {
            puckInteractsLayers = layers;
        }

        // sets the puckbar type
        public static void SetPuckBarType(int type)
        {
            puckBarType = type;
        }

        // fills the bar when filling set to true
        public static void Update()
        {
            if (puckBarType == 0)
            {
                return;
            }
            if (UnityEngine.Object.FindObjectsOfType(typeof(HockeyShooter)).Length == 0 && puckBarInstance is not null)
            {
                Remove();
            }

            if (filling && !GameState.GetInstance().Paused)
            {
                Fill();
            }

            if (fillAmount >= 1)
            {
                Reset();
            }
            else if (fillAmount > 0)
            {
                if (puckBarType == 1)
                {
                    if (!puckBarInstance)
                    {
                        InstantiatePuckBar();
                    }
                    SetPuckBarToFillAmount();
                }
                else
                {
                    StartDrawingPuckPaths();
                }
            }
        }

        // creates the puckbar icon
        private static void InstantiatePuckBar()
        {
            if (puckBarType == 1)
            {
                puckBarInstance = UnityEngine.Object.Instantiate(puckBarPrefab);
                // Set the puckbar size and position
                Vector3 newPuckBarPosition = new Vector3(LTKConfigurationMod.PuckBarPosition.Value.x, LTKConfigurationMod.PuckBarPosition.Value.y, 0);
                puckBarInstance.transform.GetChild(0).position = newPuckBarPosition;
                puckBarInstance.transform.GetChild(0).GetChild(0).localScale *= LTKConfigurationMod.PuckBarSize.Value; //puckBarFill
                puckBarInstance.transform.GetChild(0).GetChild(1).localScale *= LTKConfigurationMod.PuckBarSize.Value; //puckBarBox
            }
        }

        // sets puckbar icon to fillAmount
        private static void SetPuckBarToFillAmount()
        {
            puckBarInstance.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = fillAmount;
        }

        // calls DrawPuckPath for every hockey shooter
        private static void StartDrawingPuckPaths()
        {
            bool queriesHitTriggers = Physics2D.queriesHitTriggers;
            Physics2D.queriesHitTriggers = true; // makes the raycast also hit triggers
            foreach (HockeyShooter hockeyShooter in (HockeyShooter[])UnityEngine.Object.FindObjectsOfType(typeof(HockeyShooter)))
            {
                lastCannon = null;
                lastTeleporter = null;
                Vector3 rayStart = hockeyShooter.RayStart.position;
                Vector3 rayDir = hockeyShooter.RayEnd.position - hockeyShooter.RayStart.position;
                DrawPuckPath(rayStart, rayDir);
            }
            Physics2D.queriesHitTriggers = queriesHitTriggers;
        }

        // draws the path for one hockey shooter
        private static void DrawPuckPath(Vector3 rayStart, Vector3 rayDir, int recursions = 0)
        {
            if (recursions > 10)
            {
                return;
            }

            List<RaycastHit2D> hitList = Physics2D.RaycastAll(rayStart, rayDir, Mathf.Infinity, puckInteractsLayers).ToList();
            hitList.Sort((hit1, hit2) => hit1.distance.CompareTo(hit2.distance));

            HitFirstObject(rayStart, rayDir, recursions, hitList);
        }

        // draws a line to the first valid object and redirects if it's a cannon or teleporter
        private static void HitFirstObject(Vector3 rayStart, Vector3 rayDir, int recursions, List<RaycastHit2D> hitList)
        {
            foreach (RaycastHit2D hit in hitList)
            {
                // if animal cannon
                AnimalCannon animalCannon = hit.collider.GetComponentInParent<AnimalCannon>();
                if (animalCannon && animalCannon != lastCannon)
                {
                    LineDrawer.DrawRedLine(rayStart, hit.point, fillAmount * fillAmount * fillAmount);
                    lastCannon = animalCannon;
                    rayStart = animalCannon.LaunchTarget.position;
                    float num = 0f;
                    Vector3 localScale = animalCannon.transform.localScale;
                    if (localScale.y < 0f)
                    {
                        if (localScale.x < 0f)
                        {
                            num = 180f;
                        }
                        else
                        {
                            num = 270f;
                        }
                    }
                    else if (localScale.x < 0f)
                    {
                        num = 90f;
                    }
                    rayDir = Quaternion.AngleAxis(animalCannon.transform.eulerAngles.z + num + animalCannon.boostAngle, Vector3.forward) * Vector3.right;
                    DrawPuckPath(rayStart, rayDir, recursions + 1);
                    return;
                }

                //if teleporter
                Teleporter teleporter = hit.collider.GetComponentInParent<Teleporter>();
                if (teleporter && teleporter.CanTeleport && teleporter != lastTeleporter)
                {
                    LineDrawer.DrawRedLine(rayStart, hit.point, fillAmount * fillAmount * fillAmount);
                    lastTeleporter = teleporter.Destination;
                    // set the correct position and rotation
                    rayStart = teleporter.transform.InverseTransformPoint(hit.point);
                    rayStart = teleporter.Destination.transform.TransformPoint(rayStart);
                    rayDir = Quaternion.Inverse(teleporter.transform.rotation) * rayDir;
                    rayDir = teleporter.Destination.transform.rotation * rayDir;
                    DrawPuckPath(rayStart, rayDir, recursions + 1);
                    return;
                }

                // if anything other than start, end, teleport or cannon
                if (hit.collider.tag != "Goal" && hit.collider.tag != "Start" && !teleporter && !animalCannon)
                {
                    LineDrawer.DrawRedLine(rayStart, hit.point, fillAmount * fillAmount * fillAmount);
                    return;
                }
            }
            // if not hit anything draw until "infinity"
            LineDrawer.DrawRedLine(rayStart, rayStart + rayDir.normalized * 100, fillAmount * fillAmount * fillAmount);
        }

        // starts filling the puckBar
        public static void StartFilling()
        {
            PuckBarController.filling = true;
            PuckBarController.fillAmount = 0;
        }

        // adds the deltaTime to fillAmount
        private static void Fill()
        {
            fillAmount += Time.deltaTime * Modifiers.GetInstance().RateOfFire * 0.9f; // for some reason it takes a bit more than a second by default
        }

        // destroys the puckBar
        private static void Remove()
        {
            fillAmount = -1;
            filling = false;
            if (puckBarPrefab is not null)
            {
                UnityEngine.Object.Destroy(puckBarInstance);
            }
        }

        // resets the fillAmount
        private static void Reset()
        {
            filling = false;
            fillAmount = 0;
            if (puckBarType == 1)
            {
                SetPuckBarToFillAmount();
            }
        }
    }
}