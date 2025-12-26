using DestroyerTest.Common;
using DestroyerTest.Content.Projectiles.ParentClasses;
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
    public class NetrahalisProjectile : SpinningSlash
    {

        public override void SetDefaults()
        {
            themeColor = Color.HotPink;
            Blending = true;
            DustVelocityMultiplier = 2.67f;
        }

        // Custom AI
        public override void AI()
        {
            Projectile.velocity *= 0.95f;

            Vector2 spawnOffset = Main.rand.NextVector2Circular(90f, 90f);
            Vector2 spawnPosition = Projectile.Center + spawnOffset;

            PRTLoader.NewParticle(PRTLoader.GetParticleID<StarParticle>(), spawnPosition, Projectile.Center, Color.FloralWhite, 1f);
        }

	}
}