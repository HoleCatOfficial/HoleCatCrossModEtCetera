
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
using InnoVault.PRT;
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
            if (player.TryGetModPlayer<AetherianShieldPlayer>(out var shield))
            {
                shield.Active = true;
            }
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

    public class AetherianShieldPlayer : ModPlayer
    {
        public int MaxDurability = 30;

        public int Durability = 30;

        public int Radius = 60;

        public bool Active = false;

        public bool Absorb = false;

        public bool Recharge = false;

        public Color themeColor = DTColorUtils.Pastel(Main.DiscoColor, 0.5f);

        public SoundStyle Regen = SoundID.Item130;
    
        public SoundStyle Hit = SoundID.Item150;
        /// <summary>
        public SoundStyle Break = SoundID.DD2_KoboldExplosion;

        public NetworkText[] DeathMSGs = new NetworkText[4];
        public int RechargeHealthTax = 3;

        public virtual int Priority {get; set;} = 0;


        public override void ResetEffects()
        {
            MathHelper.Clamp(Priority, 0, 10);
            Active = false;
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            DTUtils Utility = new DTUtils();
            Vector2 drawPos = Player.Center - Main.screenPosition;
            SpriteBatch spriteBatch = Main.spriteBatch;
            drawPos.Y -= 200;

            string text = $"{Durability.ToString()} / {MaxDurability.ToString()}";

            if (Active)
            {
                Utils.DrawBorderString(spriteBatch, text, drawPos, themeColor, 2f, 0.5f, 0.5f);
            }
        }

        public override void PostUpdateEquips()
        {
            themeColor = DTColorUtils.Pastel(Main.DiscoColor, 0.5f);
            var shieldManager = ShieldManager.Instance;
            if (shieldManager == null)
                return;

            if (Active)
            {
                if (Durability >= MaxDurability && !Recharge)
                {
                    Absorb = true;
                }
            }

            if (Active && Absorb && !Recharge)
            {
                foreach (Projectile p in Main.projectile)
                {
                    if (!p.active || !p.hostile || p.friendly || p.damage <= 0)
                        continue;

                    if (p.Distance(Player.Center) > Radius)
                        continue;

                    if (!p.TryGetGlobalProjectile(out ShieldGlobal hostile))
                        continue;

                    if (hostile.Blocked)
                        continue;

                    hostile.Blocked = true;

                    Durability -= p.damage;

                    SoundEngine.PlaySound(Hit, Player.Center);

                    for (int y = 0; y < 9; y++)
                    {
                        PRTLoader.NewParticle(
                            PRTLoader.GetParticleID<SparkParticle>(),
                            p.Center,
                            new Vector2(Main.rand.NextFloat(-2f, 2.1f), Main.rand.NextFloat(-4f, -6.1f)),
                            themeColor,
                            0.4f,
                            2
                        );
                    }

                    Projectile.NewProjectile(Player.GetSource_FromAI(), p.Center, Main.rand.NextVector2Circular(10, 10), ProjectileID.FairyQueenMagicItemShot, 20, 2, Player.whoAmI);
                    p.Kill();

                    if (Durability <= 0)
                    {
                        SoundEngine.PlaySound(Break, Player.Center);
                        Opus.RadialSpreadProjectile(ProjectileID.FairyQueenMagicItemShot, 5, Player.Center, 20, 2, 2, RandomOffset: false);
                        Absorb = false;
                        Recharge = true;
                        break;
                    }
                }

                if (Main.rand.NextBool(2400))
                {
                    SoundEngine.PlaySound(SoundID.Pixie with { Pitch = -2 }, Player.Center);
                }

                List<Dust> WallDusts = new List<Dust>();
                List<BasePRT> WallPRTs = new List<BasePRT>();

                var WallDustPositions = Opus.GetEquidistantOrbitVectors(5, Player.Center, (0.05f * Player.direction) + ((0.0005f * Player.velocity.Length()) * Player.direction), Radius);
                foreach(Vector2 p in WallDustPositions)
                {
                    Dust WallDust = Dust.NewDustPerfect(p, ModContent.DustType<ColorableNeonDust>(), Vector2.Zero, 0, themeColor, 1.35f);
                    WallDusts.Add(WallDust);
                    //WallDust.velocity = Player.velocity;

                    BasePRT WallPRT = PRTLoader.NewParticle(PRTLoader.GetParticleID<SimpleParticle>(), p, Vector2.Zero, themeColor * 0.75f, 0.3f);
                    WallPRTs.Add(WallPRT);
                    //WallPRT.Velocity = Player.velocity;
                }

                if (Durability <= 0)
                {

                    Absorb = false;   // shield can’t block anymore
                    Recharge = true;  // enter recharge mode
                }
            }

            // --- Recharge phase ---
            if (Recharge)
            {
                if (Main.GameUpdateCount % 20 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Unlock with { Pitch = -2, MaxInstances = 0 }, Player.Center);

                    PlayerDeathReason deathReason = PlayerDeathReason.ByCustomReason(DeathMSGs[Main.rand.Next(DeathMSGs.Length)]);
                    int Decrement = (int)(RechargeHealthTax - (0.5f * Player.statDefense));
                    if (RechargeHealthTax < 0.5f * Player.statDefense)
                    {
                        Decrement = (int)(0.2f * Player.statDefense);
                    }
                    Player.statLife -= Decrement;
                    if (Player.statLife <= 0)
                    {
                        Player.KillMe(deathReason, (double)RechargeHealthTax, 0, false);
                        Durability = MaxDurability;
                    }
                    Durability += RechargeHealthTax;
                }


                if (Durability >= MaxDurability)
                {
                    SoundEngine.PlaySound(Regen, Player.Center);
                    Recharge = false;
                    Absorb = true;
                }
            }
        }

        public override void PostUpdate()
        {
            var shieldManager = ShieldManager.Instance;
            if (shieldManager == null)
                return;

            if (Active && Absorb)
            {
                
            }
        }

        public override void OnRespawn()
        {
            Durability = MaxDurability;
        }


    }
    
    

    public class AetherianShieldGlobal : GlobalProjectile
    {
        public bool Blocked = false;
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Blocked = false;
        }
    }

    public class AetherianShieldDrawLayer : PlayerDrawLayer
    {
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.TryGetModPlayer<AetherianShieldPlayer>(out var Shield))
            {
                return Shield.Active && Shield.Absorb;
            }
            return false;
        }

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.CaptureTheGem);
        
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            var Shield = ModContent.GetInstance<AetherianShieldPlayer>();
            
            Color color = DTColorUtils.Pastel(Main.DiscoColor, 0.5f);
            var position = drawInfo.Center - Main.screenPosition;
			position = new Vector2((int)position.X, (int)position.Y);

            drawInfo.DrawDataCache.Add(new DrawData(
                DTAssetLib.ShieldRing.Value,
                position,
                null,
                color with {A = 0},
                0f,
                DTAssetLib.ShieldRing.Size() / 2,
                Shield.Radius / (DTAssetLib.ShieldRing.Value.Width / 2f),
                SpriteEffects.None,
                0
            ));
        }
    }
}
