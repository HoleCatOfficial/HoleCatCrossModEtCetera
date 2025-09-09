using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FranciumMultiCrossMod.Common;
using Terraria.ModLoader;
using FargowiltasSouls.Core;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Content.UI.Elements;
using FargowiltasSouls.Core.Systems;

namespace FranciumMultiCrossMod
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class FranciumMultiCrossMod : Mod
	{
	public static bool EternityModeEnabled = WorldSavingSystem.EternityMode;

    public override object Call(params object[] args)
    {
        if (args.Length < 1)
            return null;

        string message = args[0] as string;

        switch (message)
        {
            case "CheckEternity":
                return EternityModeEnabled;

            default:
                Logger.Warn("Unknown call: " + message);
                return null;
        }
    }

	}
}
