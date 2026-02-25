using System.Linq;
using DestroyerTest.Content.Particles;
using InnoVault.PRT;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using OpusLib.Content.Helpers;
using Microsoft.Build.Evaluation;
using DestroyerTest.Common.Systems;
using DestroyerTest.Common;
using OpusLib;
using DestroyerTest.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace FranciumMultiCrossMod.Content.Projectiles.player.Enchantment
{
    public class SpiritAmalgam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 34; // The width of projectile hitbox
            Projectile.height = 68; // The height of projectile hitbox
            Projectile.DamageType = DamageClass.Generic; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 0.1f; // How much light emit around the projectile
            Projectile.timeLeft = 60; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        public override void PostDraw(Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Opus.StartSpriteBatchWithBlending(spriteBatch, BlendState.Additive, SpriteSortMode.Immediate);
            Main.EntitySpriteDraw(DTAssetLib.BloomRingSharp.Value, Projectile.Center - Main.screenPosition, null, Color.SkyBlue * 0.5f, 0f, DTAssetLib.BloomRingSharp.Value.Size() / 2, 400f / (DTAssetLib.ShieldRing.Value.Width / 2f), SpriteEffects.None, 0f);
            Opus.ReturnToDefaultDrawing(spriteBatch);
        }

        float Speed = 0f;
        Line line;
        public float EffectRad = 400f * 400f;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.active && !player.dead)
            {
                Projectile.timeLeft = 60;
            }
            else
            {
                Projectile.Kill();
            }

            Vector2 IdealPosition = player.Center;

            line = new Line(Projectile.Center, IdealPosition);

            float D1 = 30f * 30f;
            float D2 = 120f * 120f;
            float D3 = 600f * 600f;
            
            if (Projectile.Center.DistanceSQ(IdealPosition) < D1)
            {
                Speed *= 0.9f;
            }

            if (Projectile.Center.DistanceSQ(IdealPosition) > D1)
            {
                Projectile.velocity = line.GetLineRotation.ToRotationVector2() * Speed;
                if (Projectile.Center.DistanceSQ(IdealPosition) > D2)
                {
                    if (Speed < 5f)
                    {
                        Speed += 0.05f;
                    }
                    else
                    {
                        Speed = 5f;
                    }
                }
                if (Projectile.Center.DistanceSQ(IdealPosition) > D3)
                {
                    Projectile.Center = IdealPosition;
                    SoundEngine.PlaySound(DTAssetLib.Impacts.IceMagicImpact);
                }
            }

            Vector2[] p = Opus.GetEquidistantOrbitVectors(16, Projectile.Center, 0.01f, 400f);
            for (int i = 0; i < p.Length; i++)
            {
                Dust.NewDustPerfect(p[i], ModContent.DustType<ColorableNeonDust>(), Vector2.Zero, 0, Color.SkyBlue, 1f);
            }
            if (player.Center.DistanceSQ(Projectile.Center) < EffectRad)
            {
                player.GetAttackSpeed(DamageClass.Melee) += 0.3f;
            }
        }   
    }
}