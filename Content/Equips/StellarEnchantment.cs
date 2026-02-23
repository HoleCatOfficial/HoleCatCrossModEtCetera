
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.RiftArsenal;
using DestroyerTest.Content.Equips;
using DestroyerTest.Common;
using FranciumMultiCrossMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Scepter;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Equips.Cards.RiftenDeck;
using Terraria.Audio;
using DestroyerTest.Content.Dusts;
using DestroyerTest.Content.MeleeWeapons.SwordLineage;
using DestroyerTest.Content.RangedItems;
using DestroyerTest.Content.Magic;
using DestroyerTest.Content.RogueItems;
using DestroyerTest.Content.Equips.ScepterAccessories;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Content.SummonItems;
using DestroyerTest.Content.Buffs;

namespace FranciumMultiCrossMod.Content.Equips
{
    public class StellarEnchantment : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<StellarRarity>();
            Item.value = Item.sellPrice(platinum: 1);
            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.TryGetModPlayer<GalantineIncensePlayer>(out GalantineIncensePlayer Incense))
            {
                Incense.Active = true;
            }
            if (player.TryGetModPlayer<ScrollScepterUsePlayer>(out ScrollScepterUsePlayer Scptr))
            {
                Scptr.StarScroll = true;
                Scptr.GalantineScroll = true;
            }

            player.AddBuff(ModContent.BuffType<WeaponImbueGB>(), 60);
            player.AddBuff(ModContent.BuffType<ScepterImbueGB>(), 60);
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<StellarMatter>(25)
                .AddIngredient<Constitution>()
                .AddIngredient<StellarBow>()
                .AddIngredient<StellarFoxScepter>()
                .AddIngredient<StellarFlames>()
                .AddIngredient<GalantineKnife>()
                .AddIngredient<GalantineLance>()
                .AddIngredient<StarConstruct>()
                .AddIngredient<GalantineIncense>()
                .AddIngredient<StellarManuscript>()
                .AddIngredient<StellarFlamesFlask>(15)
                .AddTile(TileID.Anvils)
                .Register();
        }
	}
}
