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

            Scale = MathHelper.Lerp(0f, 3f, Progress);

            Position += Velocity;

            if (Lifetime > MaxLifetime)
            {
                ShouldBeRemovedFromRenderer = true;
            }
        }

        public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
        {
            Texture2D texture = ModContent.Request<Texture2D>(GetNamespacePath<AuraThiefSigil>()).Value;
            spritebatch.UseBlendState(BlendState.Additive);
            spritebatch.Draw(texture, Position - Main.screenPosition, null, ColorLib.LifeEcho, Rotation, texture.Size() / 2, Scale, SpriteEffects.None, 0f);
            spritebatch.ResetToDefault();
        }

        //TODO: Put this in the regular Utility class.
        public static string GetNamespacePath<T>()
        {
            return (typeof(T).Namespace ?? string.Empty)
                .Replace('.', Path.DirectorySeparatorChar);
        }

    }
}