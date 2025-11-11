
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.RiftArsenal;
using DestroyerTest.Content.Scepter;
using DestroyerTest.Content.Equips;
using DestroyerTest.Common;
using FranciumMultiCrossMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using DestroyerTest.Rarity;

namespace FranciumMultiCrossMod.Content.Equips
{
    public class RiftEnchantment : ModItem
    {
        // By declaring these here, changing the values will alter the effect, and the tooltip
        public static readonly int MultiplicativeDamageBonus = 8;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MultiplicativeDamageBonus);

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
            RiftEnchantmentPlayer REPlayer = player.GetModPlayer<RiftEnchantmentPlayer>();
            player2.LivingShadowCurrent = player2.LivingShadowMax2;

            float lifePercent = (float)player.statLife / player.statLifeMax2;
            float dynamicBonus = MultiplicativeDamageBonus + ((1f - lifePercent) * MultiplicativeDamageBonus * 2);
            player.GetDamage(DamageClass.Generic) *= 1f + (dynamicBonus / 100f);

            REPlayer.riftEnchantment = true;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Living_Shadow>(300)
                .AddIngredient<ShadowCircuitry>(25)
                .AddIngredient<RiftBroadsword>()
                .AddIngredient<RiftChakram>()
                .AddIngredient<RiftClaymore>()
                .AddIngredient<RiftGreatsword>()
                .AddIngredient<RiftPhasesaber>()
                .AddIngredient<RiftRevolver>()
                .AddIngredient<RiftScabbard>()
                .AddIngredient<RiftScythe>()
                .AddIngredient<RiftScepter>()
                .AddIngredient<RiftSpine>()
                .AddIngredient<RiftStaff>()
                .AddIngredient<RiftTeardrop>()
                .AddIngredient<RiftThrowingKnife>()
                .AddIngredient<RiftTome>()
                .AddIngredient<RiftTome2>()
                .AddIngredient<RiftWhipT1>()
                .AddIngredient<RiftWhipT2>()
                .AddIngredient<RiftYoyoT1>()
                .AddIngredient<RiftYoyoT2>()
                .AddIngredient<RiftYoyoT3>()
                .AddIngredient<RiftZapinator>()
                .AddIngredient<RiftplateAgilityLeggings>()
                .AddIngredient<RiftplateTitanGreaves>()
                .AddIngredient<RiftplateAgilityArmor>()
                .AddIngredient<RiftplateTitanBody>()
                .AddIngredient<RiftPlateHoodedJaw>()
                .AddIngredient<RiftPlateBerserkerHelm>()
                .AddIngredient<RiftPropulsion>()
                .AddTile<Tile_RiftConfiguratorTools>()
                .Register();
        }
	}


    // Some movement effects are not suitable to be modified in ModItem.UpdateAccessory due to how the math is done.
    // ModPlayer.PostUpdateRunSpeeds is suitable for these modifications.
    public class RiftEnchantmentPlayer : ModPlayer
    {
        public bool riftEnchantment = false;

        public override void ResetEffects()
        {
            riftEnchantment = false;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (riftEnchantment)
            {
                for (int C = 0; C < Main.rand.Next(4, 28); C++)
                {

                    Projectile.NewProjectile(
                        Player.GetSource_OnHurt(npc),
                        Player.Center,
                        new Vector2(Main.rand.NextFloat(-3f, 3f), -1f),
                        ModContent.ProjectileType<RiftStar>(),
                        15,
                        1f,
                        Player.whoAmI
                    );
                }

            }
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (riftEnchantment)
                {
                    for (int C = 0; C < Main.rand.Next(4, 28); C++)
                    {

                        Projectile.NewProjectile(
                            Player.GetSource_OnHurt(proj),
                            Player.Center,
                            new Vector2(Main.rand.NextFloat(-3f, 3f), -1f),
                            ModContent.ProjectileType<RiftStar>(),
                            15,
                            1f,
                            Player.whoAmI
                        );
                    }

                }
        }
    }
}
