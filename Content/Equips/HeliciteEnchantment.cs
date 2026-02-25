
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
using DestroyerTest.Content.Equips.ScepterAccessories;
using DestroyerTest.Content.Equips.Cards.RiftenDeck;
using Terraria.DataStructures;
using FranciumMultiCrossMod.Common;

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
            HeliciteEnchantmentPlayer HEPlayer = player.GetModPlayer<HeliciteEnchantmentPlayer>();
            HEPlayer.HeliciteEnchantment = true;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            if (player.armor[0].type == ModContent.ItemType<HallowedPall>())
            {
                return false;
            }
            return true;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Item_HeliciteCrystal>(300)
                .AddIngredient<HeliciteGreatbow>()
                .AddIngredient<HeliciteShank>()
                .AddIngredient<HeliciteChausses>()
                .AddIngredient<HeliciteRobe>()
                .AddIngredient<HelicitePendant>()
                .AddIngredient<HeliciteHeadgear>()
                .AddIngredient<HeliciteHeadpiece>()
                .AddIngredient<HeliciteCowl>()
                .AddTile<Tile_RiftConfiguratorTools>()
                .Register();
        }
	}

    public class HeliciteEnchantmentPlayer : ModPlayer
    {
        public bool HeliciteEnchantment = false;

        public int Cooldown = 7200;
        public int currentCooldown = 0;


        //NOTE: All ported back from Hallowed Pall. Its like a snowball! (hey that rhymes!)
        public float BarScale = 0f;
		public float TextScale = 0f;
		public float BarOpacity = 0f;
		public int TimeDisplay = 0;

        public override void ResetEffects()
        {
            base.ResetEffects();
            HeliciteEnchantment = false;
        }

        public override void OnRespawn()
        {
            currentCooldown = 0;
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
			float progress = (float)currentCooldown / (float)Cooldown;
            if (HeliciteEnchantment && currentCooldown > 0)
			{
				if (drawInfo.shadow == 0)
				{
					Bar.DrawHeliciteChargeBar(BarScale, (drawInfo.drawPlayer.Center + new Vector2(0, 40)) - Main.screenPosition, progress, BarOpacity);
					Utils.DrawBorderString(Main.spriteBatch, TimeDisplay.ToString(), (drawInfo.drawPlayer.Center + new Vector2(0, 58)) - Main.screenPosition, ColorLib.Rift * BarOpacity, TextScale, 0.5f, 0.5f);
				}
			}
        }

        public override void PostUpdate()
        {
            if (currentCooldown > 0)
            {
                currentCooldown--;
                if (BarScale < 1f)
                {
                    BarScale += 0.05f;
                }
                if (BarOpacity < 1f)
                {
                    BarOpacity += 0.05f;
                }

                if (TextScale < 0.5f)
                {
                    TextScale += 0.025f;
                }

                if (currentCooldown % 60 == 0)
                {
                    TimeDisplay -= 1;
                }
            }

            if (currentCooldown == 1)
            {
                SoundEngine.PlaySound(new SoundStyle("FranciumMultiCrossMod/Assets/Audio/HeliciteEnchantmentRegen"), Player.position);
                PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRingSharp2>(), Player.Center, Vector2.Zero, ColorLib.Rift, 3f);
            }

            if (currentCooldown <= 0)
            {
                if (BarScale > 0f)
                {
                    BarScale -= 0.05f;
                }
                if (BarOpacity > 0f)
                {
                    BarOpacity -= 0.05f;
                }

                if (TextScale > 0f)
                {
                    TextScale -= 0.025f;
                }
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
                Player.AddBuff(ModContent.BuffType<DaylightOverload>(), 600); // 10 seconds of Bleeding debuff
                currentCooldown = Cooldown;
                TimeDisplay = (Cooldown / 60);
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

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            currentCooldown = 0;
			TimeDisplay = 0;
			BarScale = 0f;
			TextScale = 0f;
			BarOpacity = 0f;
        }
    }
}
