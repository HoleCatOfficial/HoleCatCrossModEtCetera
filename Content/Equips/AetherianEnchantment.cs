
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
using DestroyerTest.Content.Equips.AuraThiefSet;
using DestroyerTest.Content.MeleeWeapons;
using FranciumMultiCrossMod.Content.Extras;
using OpusLib;
using DestroyerTest.Content.Particles;
using Terraria.DataStructures;
using FranciumMultiCrossMod.Common;
using FranciumMultiCrossMod.Content.Projectiles.player.Enchantment;
using DestroyerTest.Content.Equips.AetherianSet;
using Microsoft.Xna.Framework.Graphics;
using OpusLib.Content.Helpers;
using System.Collections.Generic;

namespace FranciumMultiCrossMod.Content.Equips
{
    public class AetherianEnchantment : ModItem
    {
        public static List<NetworkText> DeathMSG = new List<NetworkText>
        {
            NetworkText.FromLiteral($"{Main.LocalPlayer.name} began to shimmer out of existence."),
            NetworkText.FromLiteral($"{Main.LocalPlayer.name} learned what it meant to become a true aetherian."),
            NetworkText.FromLiteral($"{Main.LocalPlayer.name} didn't have what it takes to sustain a shield."),
            NetworkText.FromLiteral($"{Main.LocalPlayer.name} came to rest in a poof of sparkles."),
        };
        Shield AetherShield = new Shield("AetherShield", 160, 100, OpusColorUtils.Pastel(Main.DiscoColor, 0.7f), SoundID.Item130, SoundID.Item150, SoundID.DD2_KoboldExplosion, DeathMSG, 2, 1);
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.sellPrice(gold: 12);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.TryGetModPlayer<AetherianPlayer>(out var p))
            {
                p.Active = true;
            }
            if (player.TryGetModPlayer<AetherianScepterPlayer>(out AetherianScepterPlayer Scptr))
            {
                Scptr.Active = true;
            }
            ShieldManager.ActiveShields[player.whoAmI].Add(AetherShield);
            player.shimmerImmune = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShimmerBlock, 25)
                .AddIngredient<AetherianHood>()
                .AddIngredient<AetherianRegalia>()
                .AddIngredient(ItemID.AegisCrystal)
                .AddIngredient(ItemID.AegisFruit)
                .AddIngredient(ItemID.GalaxyPearl)
                .AddIngredient(ItemID.GummyWorm)
                .AddIngredient(ItemID.Ambrosia)
                .AddIngredient(ItemID.PeddlersSatchel)
                .AddIngredient(ItemID.ShimmerflyinaBottle)
                .AddIngredient<SynergyWrap>()
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class AetherianPlayer : ModPlayer
    {
        public bool Active = false;
        public override void ResetEffects()
        {
            Active = false;
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (Active)
            {
                drawInfo.colorHair = Main.DiscoColor;
                drawInfo.colorEyes = Main.DiscoColor;
            }
        }

        public override void PostUpdateEquips()
        {
            if (Active)
            {

            }
        }

    }
}
