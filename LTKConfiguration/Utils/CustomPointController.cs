using System;
using System.Collections.Generic;
using UnityEngine;
using LTKConfiguration.Extensions;

namespace LTKConfiguration.Utils
{
    public class CustomPointController
    {
        /* HOW TO MAKE YOUR CUSTOM POINTS
        There are two ways:
        
        1. Use LTKLib
        PROS AND CONS
            It's easier but requires the user to download another mod for yours to work
        SETUP
            follow the developer guide on https://github.com/lotok14/LTKLib

        2. Manual
        PROS AND CONS
            This method requires you to copy the code manually but it's easier for the users as everything is in one mod
        SETUP
            - Copy these files to your mod:
                - Utils/CustomPointController.cs
                - Utils/CustomPoint.cs
                - Extensions/PointBlock.cs
                - Patches/CustomPointPatch.cs
            - replace every mention of LTKConfiguration with your own mod's namespace
            - patch the methods from CustomPointPatch.cs
            - in your main add an int field that will keep your custom point block's id
            - At the start of Awake() do:
                pointBlockId = CustomPointController.CreateCustomPoint(pointName, pointAmount, pointColor, true);
                (example in LTKConfigurationMod.cs)
            - when you want to give the player your custom point, do:
                CustomPointController.GiveCustomPoint(pointBlockId, playerNetworkNumber);
                (example in BeehivePatch.cs)
        */
        public static List<CustomPoint> customPointsData = new();

        public static int CreateCustomPoint(String name, float width, Color color, bool alwaysAward)
        {
            int id = customPointsData.Count;
            customPointsData.Add(new CustomPoint(name, width, color, alwaysAward));
            return id;
        }

        public static void GiveCustomPoint(int id, int characterNetworkNumber)
        {
            PointBlock pb = new PointBlock(PointBlock.pointBlockType.coin, characterNetworkNumber);
            pb.GetAdditionalData().pointBlockCustomId = id;
            ScoreKeeper.Instance.AwardPoint(pb, true);
        }
    }
}