﻿using CustomRegions.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomRegions.CustomPearls
{
    static class DataPearlHook
    {
        public class EnumExt_DataPearlType
        {
            public static DataPearl.AbstractDataPearl.DataPearlType CustomDataPearl;
        }

        public static void ApplyHooks()
        {
            On.DataPearl.ApplyPalette += DataPearl_ApplyPalette;

        }

        private static void DataPearl_ApplyPalette(On.DataPearl.orig_ApplyPalette orig, DataPearl self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            orig(self, sLeaser, rCam, palette);

            bool foundPearl = false;
            foreach (KeyValuePair<string, CustomWorldMod.CustomPearl> pearls in CustomWorldMod.customPearls)
            {
                if (foundPearl) { break; }

                DataPearl.AbstractDataPearl.DataPearlType dataPearlType = (DataPearl.AbstractDataPearl.DataPearlType)
                            Enum.Parse(typeof(DataPearl.AbstractDataPearl.DataPearlType), pearls.Key);

                if ((self.abstractPhysicalObject as DataPearl.AbstractDataPearl).dataPearlType == dataPearlType)
                {
                    foundPearl = true;
                    self.color = pearls.Value.color;
                    self.highlightColor = pearls.Value.secondaryColor;
                }

            }

        }
    }
}