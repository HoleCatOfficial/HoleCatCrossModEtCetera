
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

namespace FranciumMultiCrossMod.Content.Equips
{
    public class RiftEnchantment : ModItem
    {
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

            player.GetDamage(DamageClass.Ranged) *= 1.05f;
            player.whipRangeMultiplier *= 1.08f;
            player.maxMinions += 1;
            player.GetAttackSpeed(DamageClass.Ranged) *= 0.95f;
            player.GetArmorPenetration(DamageClass.Generic) += 6;

            if (player.TryGetModPlayer<HollowShield>(out var shield))
            {
                shield.Active = true;
            }
            if (player.TryGetModPlayer<RiftCanisterPlayer>(out var Canister))
            {
                Canister.Active = true;
            }
            player.statDefense += 10;

            foreach (Item item in Main.item)
            {
                int tm = player.GetItemGrabRange(item);
                tm = (int)(tm * 1.10f);
            }

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
                .AddIngredient<ShineShadeDeck>()
                .AddIngredient<RiftCanister>()
                .AddTile<Tile_RiftConfiguratorTools>()
                .Register();
        }
	}

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

                    Projectile.NewProjectile(Player.GetSource_OnHurt(npc), Player.Center, new Vector2(Main.rand.NextFloat(-3f, 3f), -1f), ModContent.ProjectileType<RiftStarFriendly>(), 15, 1f, Player.whoAmI);
                }

            }
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (riftEnchantment)
            {
                for (int C = 0; C < Main.rand.Next(4, 28); C++)
                {

                    Projectile.NewProjectile(Player.GetSource_OnHurt(proj), Player.Center, new Vector2(Main.rand.NextFloat(-3f, 3f), -1f), ModContent.ProjectileType<RiftStarFriendly>(), 15, 1f, Player.whoAmI);
                }
            }
        }
    }

    public class RiftEnchantmentDash : ModPlayer
	{
		bool HasRiftPropulsionEquipped()
		{
			for (int i = 3; i < 8; i++) // Accessory slots (vanilla)
			{
				if (Player.armor[i].type == ModContent.ItemType<RiftEnchantment>())
					return true;
			}
			return false;
		}
		// These indicate what direction is what in the timer arrays used
		public const int DashDown = 0;
		public const int DashUp = 1;
		public const int DashRight = 2;
		public const int DashLeft = 3;

		public const int DashCooldown = 100; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
		public const int DashDuration = 40; // Duration of the dash afterimage effect in frames

		// The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
		public const float DashVelocity = 30f;

		// The direction the player has double tapped.  Defaults to -1 for no dash double tap
		public int DashDir = -1;

		// The fields related to the dash accessory
		public bool DashAccessoryEquipped;
		public int DashDelay = 0; // frames remaining till we can dash again
		public int DashTimer = 6; // frames remaining in the dash

		public override void ResetEffects() 
        {
			// Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
			DashAccessoryEquipped = true;

			// ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
			// When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
			// If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
			if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15 && HasRiftPropulsionEquipped()) {
				DashDir = DashDown;
			}
			else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15 && HasRiftPropulsionEquipped()) {
				DashDir = DashUp;
			}
			else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15 && Player.doubleTapCardinalTimer[DashLeft] == 0 && HasRiftPropulsionEquipped()) {
				DashDir = DashRight;
			}
			else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15 && Player.doubleTapCardinalTimer[DashRight] == 0 && HasRiftPropulsionEquipped()) {
				DashDir = DashLeft;
			}
			else {
				DashDir = -1;
			}
		}

		// This is the perfect place to apply dash movement, it's after the vanilla movement code, and before the player's position is modified based on velocity.
		// If they double tapped this frame, they'll move fast this frame
		public override void PreUpdateMovement() 
        {
			// if the player can use our dash, has double tapped in a direction, and our dash isn't currently on cooldown
			if (CanUseDash() && DashDir != -1 && DashDelay == 0) {
				Vector2 newVelocity = Player.velocity;

				switch (DashDir) {
					// Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
					case DashUp when Player.velocity.Y > -DashVelocity:
					case DashDown when Player.velocity.Y < DashVelocity: {
							// Y-velocity is set here
							// If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
							// This adjustment is roughly 1.3x the intended dash velocity
							float dashDirection = DashDir == DashDown ? 1 : -1.3f;
							newVelocity.Y = dashDirection * DashVelocity;
							break;
						}
					case DashLeft when Player.velocity.X > -DashVelocity:
					case DashRight when Player.velocity.X < DashVelocity: {
							// X-velocity is set here
							float dashDirection = DashDir == DashRight ? 1 : -1;
							newVelocity.X = dashDirection * DashVelocity;
							break;
						}
					default:
						return; // not moving fast enough, so don't start our dash
				}

				// start our dash
				DashDelay = DashCooldown;
				DashTimer = DashDuration;
				Player.velocity = newVelocity;
				
				// Here you'd be able to set an effect that happens when the dash first activates
				// Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi
				Dust.NewDust(newVelocity, 15, 15, ModContent.DustType<ColorableNeonDust>(), 6f, 0f, 0, ColorLib.Rift, 10f);
                SoundEngine.PlaySound(new SoundStyle($"DestroyerTest/Assets/Audio/RiftDash"));

			}

			if (DashDelay > 0)
				DashDelay--;

			if (DashTimer > 0) { // dash is active
				// This is where we set the afterimage effect.  You can replace these two lines with whatever you want to happen during the dash
				// Some examples include:  spawning dust where the player is, adding buffs, making the player immune, etc.
				// Here we take advantage of "player.eocDash" and "player.armorEffectDrawShadowEOCShield" to get the Shield of Cthulhu's afterimage effect
				Player.eocDash = DashTimer;
				Player.armorEffectDrawShadowEOCShield = true;

				// count down frames remaining
				DashTimer--;
			}
		}

		private bool CanUseDash() 
        {
			return DashAccessoryEquipped
				&& Player.dashType == DashID.None // player doesn't have Tabi or EoCShield equipped (give priority to those dashes)
				&& HasRiftPropulsionEquipped()
				&& !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
		}
	}
}
