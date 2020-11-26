﻿using CustomRegions.Mod;
using RWCustom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DevInterface;

namespace CustomRegions.DevInterface
{
    static class MapRenderOutputHook
    {
        public static void ApplyHooks()
        {
            On.DevInterface.MapRenderOutput.Signal += MapRenderOutput_Signal;
        }

        private static void MapRenderOutput_Signal(On.DevInterface.MapRenderOutput.orig_Signal orig, global::DevInterface.MapRenderOutput self, global::DevInterface.DevUISignalType type, global::DevInterface.DevUINode sender, string message)
        {
			string customFilePath = string.Empty;
			string pathToRegion = Custom.RootFolderDirectory() +
					"World" + Path.DirectorySeparatorChar + "Regions" + Path.DirectorySeparatorChar + self.owner.game.world.name;

            if (!File.Exists(pathToRegion))
            {
                // From a Custom Region
                foreach (KeyValuePair<string, string> keyValues in CustomWorldMod.loadedRegions)
                {
                    customFilePath = CustomWorldMod.resourcePath + keyValues.Value + Path.DirectorySeparatorChar +
                        "World" + Path.DirectorySeparatorChar + "Regions" + Path.DirectorySeparatorChar + self.owner.game.world.name;


                    if (File.Exists(customFilePath))
                    {
                        CustomWorldMod.Log($"Saving custom Map Config to Properties.txt from [{keyValues.Value}]");
                        string pathToMapFile = customFilePath + Path.DirectorySeparatorChar + "map_" + self.owner.game.world.name + ".png";

                        PNGSaver.SaveTextureToFile(self.texture, pathToMapFile);
                        self.ClearSprites();
                        (self.parentNode as MapPage).renderOutput = null;
                        (self.parentNode as MapPage).modeSpecificNodes.Remove(self);
                        self.parentNode.subNodes.Remove(self);
                        return;
                    }
                }
            }

            CustomWorldMod.Log($"No Custom Properties.txt file found for [{self.owner.game.world.name}], using vanilla...");

			orig(self, type, sender, message);
        }


    }
}
