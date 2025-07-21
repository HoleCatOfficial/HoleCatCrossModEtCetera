using DestroyerTest.Common;
using FranciumMultiCrossMod.Content.Particles;
using InnoVault.PRT;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace FranciumMultiCrossMod.Content.Projectiles
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class NetrahalisProjectile : ModProjectile
    {
       

        public override void SetStaticDefaults()
        {
           
        }

        public override void SetDefaults()
        {
            Projectile.width = 180; // The width of projectile hitbox
            Projectile.height = 180; // The height of projectile hitbox

            Projectile.DamageType = DamageClass.Melee; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 1f; // How much light emit around the projectile
            Projectile.timeLeft = 60; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Tex = TextureAssets.Projectile[Projectile.type].Value;
            SpriteBatch sb = Main.spriteBatch;

            sb.End(); // End vanilla drawing
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(
                Tex,
                Projectile.Center - Main.screenPosition,
                null,
                Color.HotPink,
                Projectile.rotation,
                Tex.Size() / 2,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            sb.End(); // End additive
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        // Custom AI
        public override void AI()
        {
            Projectile.rotation += 0.3f * Projectile.direction;
            Projectile.velocity *= 0.9f;

            Vector2 spawnOffset = Main.rand.NextVector2Circular(90f, 90f);
            Vector2 spawnPosition = Projectile.Center + spawnOffset;

            PRTLoader.NewParticle(PRTLoader.GetParticleID<StarParticle>(), spawnPosition, Projectile.Center, Color.FloralWhite, 1f);
        }

	}
}