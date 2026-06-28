using BreadLibrary.Core.Graphics.Particles;
using BreadLibrary.Core.Utilities;
using DestroyerTest.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

namespace FranciumMultiCrossMod.Content.Extras
{
    public class AuraThiefSigil : BaseParticle<AuraThiefSigil>
    {
        public int MaxLifetime;
        public int Lifetime;
        public float Progress;

        public Vector2 Position;
        public Vector2 Velocity;

        public float Rotation;

        float Scale;
        float Opacity;

        public static AuraThiefSigil Create(int maxLifetime, Vector2 position, Vector2 velocity, float rotation = 0f)
        {
            AuraThiefSigil A = new();
            A.MaxLifetime = maxLifetime;
            A.Position = position;
            A.Velocity = velocity;
            A.Rotation = rotation;

            return A;
        }

        public override void Update(ref ParticleRendererSettings settings)
        {
            Lifetime++;
            Progress = (float)Lifetime / (float)MaxLifetime;

            Opacity = MathHelper.Lerp(1f, 0f, Progress);
            Scale = MathHelper.Lerp(0f, 4f, Progress);

            Position += Velocity;

            if (Lifetime > MaxLifetime)
            {
                ShouldBeRemovedFromRenderer = true;
            }
        }

        public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
        {
            Texture2D texture = ModContent.Request<Texture2D>("FranciumMultiCrossMod/Content/Extras/AuraThiefSigil").Value;
            spritebatch.UseBlendState(BlendState.Additive);
            spritebatch.Draw(texture, Position - Main.screenPosition, null, ColorLib.LifeEcho * Opacity, Rotation, texture.Size() / 2, Scale, SpriteEffects.None, 0f);
            spritebatch.ResetToDefault();
        }

    }
}