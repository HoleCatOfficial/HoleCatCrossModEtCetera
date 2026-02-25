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
        public static ModKeybind EnchantmentKeybind1 { get; private set; }

        public override void Load()
        {
            EnchantmentKeybind1 = KeybindLoader.RegisterKeybind(this, "Enchantment Keybind I", "LeftShift");
        }

        public override void Unload()
        {
            EnchantmentKeybind1 = null;
        }
    }
}
