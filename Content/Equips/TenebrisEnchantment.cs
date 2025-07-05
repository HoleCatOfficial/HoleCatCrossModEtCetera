
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.RiftArsenal;
using DestroyerTest.Content.Magic.ScepterSubclass;
using DestroyerTest.Content.Equips;
using DestroyerTest.Common;
using FranciumMultiCrossMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using DestroyerTest.Rarity;
using Terraria.DataStructures;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Projectiles.Tenebrouskatana;
using System.Runtime.CompilerServices;
using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Magic;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.RangedItems;

namespace FranciumMultiCrossMod.Content.Equips
{
    public class TenebrisEnchantment : ModItem
    {
        // By declaring these here, changing the values will alter the effect, and the tooltip
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 33));
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<RiftRarity1>();
            Item.value = Item.sellPrice(platinum: 4);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            LivingShadowPlayer player2 = player.GetModPlayer<LivingShadowPlayer>();
            TenebrisEnchantmentPlayer TEPlayer = player.GetModPlayer<TenebrisEnchantmentPlayer>();
            player2.LivingShadowCurrent = player2.LivingShadowMax2;

            TEPlayer.tenebrisEnchantment = true;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Tenebris>(30)
                .AddIngredient<ShimmeringSludge>(15)
                .AddIngredient<Living_Shadow>(25)
                .AddIngredient<TenebrousTradewinds>()
                .AddIngredient<Tenebrous_Katana>()
                .AddIngredient<TenebrousChakram>()
                .AddIngredient<TenebrousArchmagePants>()
                .AddIngredient<TenebrousArchmageCoat>()
                .AddIngredient<TenebrousArchmageHat>()
                .AddIngredient<TenebrousArchmageCowl>()
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
	}


    // Some movement effects are not suitable to be modified in ModItem.UpdateAccessory due to how the math is done.
    // ModPlayer.PostUpdateRunSpeeds is suitable for these modifications.
    public class TenebrisEnchantmentPlayer : ModPlayer
    {
        public bool tenebrisEnchantment = false;
        public int Cooldown = 1200;

        public override void ResetEffects()
        {
            Cooldown = 1200;
            tenebrisEnchantment = false;
        }

        public override void PostUpdate()
        {
            if (Cooldown > 0)
                Cooldown--;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (tenebrisEnchantment)
            {
                for (int C = 0; C < Main.rand.Next(4, 28); C++)
                {

                    Projectile.NewProjectile(
                        Player.GetSource_OnHurt(npc),
                        Player.Center,
                        new Vector2(Main.rand.NextFloat(-3f, 3f), -1f),
                        ModContent.ProjectileType<TenebrisStar>(),
                        25,
                        1f,
                        Player.whoAmI
                    );
                }

                if (Cooldown <= 0)
                {
                    Projectile.NewProjectile(
                            Player.GetSource_OnHurt(npc),
                            Player.oldPosition,
                            -Player.velocity * 0.5f,
                            ModContent.ProjectileType<TenebrisClone>(),
                            0,
                            0f,
                            Player.whoAmI
                        );
                }

            }
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (tenebrisEnchantment)
            {
                for (int C = 0; C < Main.rand.Next(4, 28); C++)
                {

                    Projectile.NewProjectile(
                        Player.GetSource_OnHurt(proj),
                        Player.Center,
                        new Vector2(Main.rand.NextFloat(-3f, 3f), -1f),
                        ModContent.ProjectileType<TenebrisStar>(),
                        25,
                        1f,
                        Player.whoAmI
                    );
                }

                if (Cooldown <= 0)
                {
                    Projectile.NewProjectile(
                            Player.GetSource_OnHurt(proj),
                            Player.oldPosition,
                            -Player.velocity * 0.5f,
                            ModContent.ProjectileType<TenebrisClone>(),
                            0,
                            0f,
                            Player.whoAmI
                        );
                }

                }
        }
    }
}
