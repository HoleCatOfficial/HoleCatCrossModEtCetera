
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
<<<<<<< HEAD
            }
        }

        public override void PostUpdateEquips()
        {
            if (Active)
            {

=======
			}
        }

        public override void PostUpdateEquips()
        {
            if (Active)
            {

            }
        }

    }

    public class AetherianShieldPlayer : ShieldPlayer
    {
        public override int MaxDurability => 30;

        public override int Durability => 30;

        public override int Radius => 60;



        public override Color themeColor => DTColorUtils.Pastel(Main.DiscoColor, 0.5f);

        public override SoundStyle Regen => SoundID.Item130;
    
        public override SoundStyle Hit => SoundID.Item150;
        /// <summary>
        public override SoundStyle Break => SoundID.DD2_KoboldExplosion;

        public override NetworkText[] DeathMSGs => new NetworkText[4];
        public override int RechargeHealthTax => 3;


        public override void ResetEffects()
        {
            Active = false;
        }

        public override void PostUpdateEquips()
        {
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
                            PRTLoader.GetParticleID<SparkParticlemmm>(),
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
>>>>>>> 0e282864b50c13aa88a4092b9c15859b9b820c22
            }
        }

    }
}
