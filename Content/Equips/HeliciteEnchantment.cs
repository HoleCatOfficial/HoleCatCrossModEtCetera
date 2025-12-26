
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.RiftArsenal;
using DestroyerTest.Content.Equips;
using DestroyerTest.Common;
using FranciumMultiCrossMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.RangedItems;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.Buffs;
using Terraria.Audio;
using InnoVault.PRT;
using FranciumMultiCrossMod.Content.Particles;

namespace FranciumMultiCrossMod.Content.Equips
{
    public class HeliciteEnchantment : ModItem
    {
        // By declaring these here, changing the values will alter the effect, and the tooltip
        public static readonly int MultiplicativeDamageBonus = 12;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MultiplicativeDamageBonus);

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<RiftRarity1>();
            Item.value = Item.sellPrice(platinum: 6);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            LivingShadowPlayer player2 = player.GetModPlayer<LivingShadowPlayer>();
            HeliciteEnchantmentPlayer HEPlayer = player.GetModPlayer<HeliciteEnchantmentPlayer>();
            player2.LivingShadowCurrent = player2.LivingShadowMax2;

            float lifePercent = (float)player.statLife / player.statLifeMax2;
            float dynamicBonus = MultiplicativeDamageBonus + ((1f - lifePercent) * MultiplicativeDamageBonus * 2);
            player.GetDamage(DamageClass.Generic) *= 1f + (dynamicBonus / 100f);

            HEPlayer.HeliciteEnchantment = true;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Item_HeliciteCrystal>(300)
                .AddIngredient<HeliciteGreatbow>()
                .AddIngredient<HeliciteShank>()
                .AddIngredient<HeliciteChausses>()
                .AddIngredient<HeliciteRobe>()
                .AddIngredient<HeliciteHeadgear>()
                .AddIngredient<HeliciteCowl>()
                .AddTile<Tile_RiftConfiguratorTools>()
                .Register();
        }
	}


    // Some movement effects are not suitable to be modified in ModItem.UpdateAccessory due to how the math is done.
    // ModPlayer.PostUpdateRunSpeeds is suitable for these modifications.
    public class HeliciteEnchantmentPlayer : ModPlayer
    {
        public bool HeliciteEnchantment = false;

        public int Cooldown = 7200;
        public int currentCooldown = 0;

        public override void ResetEffects()
            {
                base.ResetEffects();
                HeliciteEnchantment = false;
            }

        public override void OnRespawn()
        {
            currentCooldown = 0;
        }

        public override void PostUpdate()
        {
            if (currentCooldown > 0)
                currentCooldown--;

            if (currentCooldown == 1)
            {
                SoundEngine.PlaySound(new SoundStyle("FranciumMultiCrossMod/Assets/Audio/HeliciteEnchantmentRegen"), Player.position);
                PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRingSharp2>(), Player.Center, Vector2.Zero, ColorLib.Rift, 3f);
            }
        }

        private void TrySurviveFatalHit(Player.HurtInfo hurtInfo)
            {
                if (!HeliciteEnchantment || currentCooldown > 0)
                return;

                if (hurtInfo.Damage > Player.statLife)
                {
                Player.GetModPlayer<ScreenshakePlayer>().screenshakeTimer = 5;
			    Player.GetModPlayer<ScreenshakePlayer>().screenshakeMagnitude = 16;
                PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRingSharp1>(), Player.Center, Vector2.Zero, ColorLib.Rift, 1f);
                SoundEngine.PlaySound(SoundID.DeerclopsIceAttack with { Volume = 1.50f }, Player.position);
                Player.statLife = Player.statLifeMax2 / 2;
                CombatText.NewText(Player.getRect(), ColorLib.Rift, "Death Evaded!", true);
                Player.AddBuff(ModContent.BuffType<DaylightOverload>(), 1200); // 10 seconds of Bleeding debuff
                currentCooldown = Cooldown;
                hurtInfo.Damage = 0;
                //Player.NinjaDodge(); // Optional: visual effect
                }
            }
            
        

         public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {

            TrySurviveFatalHit(hurtInfo);
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
            {
                TrySurviveFatalHit(hurtInfo);
            }
    }
}
