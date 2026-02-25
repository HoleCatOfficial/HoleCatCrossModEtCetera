using System;
using System.Collections.Generic;
using FranciumMultiCrossMod.Content.Equips;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace FranciumMultiCrossMod.Common
{
    public static class Bar
    {
        public const string HelicitePath = "FranciumMultiCrossMod/Content/Extras/Helicite";
        public static Asset<Texture2D> HeliciteBarBack = ModContent.Request<Texture2D>($"{HelicitePath}BarBack", AssetRequestMode.AsyncLoad);
        public static Asset<Texture2D> HeliciteBarFront = ModContent.Request<Texture2D>($"{HelicitePath}BarFront", AssetRequestMode.AsyncLoad);
        public static Asset<Texture2D> HeliciteBarFrame = ModContent.Request<Texture2D>($"{HelicitePath}BarFrame", AssetRequestMode.AsyncLoad);
        public static void DrawHeliciteChargeBar(float barScale, Vector2 position, float progress, float Opacity)
        {
            var barBG = HeliciteBarBack.Value;
            var barFG = HeliciteBarFront.Value;
            var barFrame = HeliciteBarFrame.Value;

            Vector2 barOrigin = barBG.Size() * 0.5f;
            Vector2 drawPos = position;
            Rectangle frameCrop = new Rectangle(0, 0, (int)(progress * barFG.Width), barFG.Height);

            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.Draw(barBG, drawPos, null, Color.White * Opacity, 0f, barOrigin, barScale, 0f, 0f);
            spriteBatch.Draw(barFG, drawPos, frameCrop, Color.White * Opacity, 0f, barOrigin, barScale, 0f, 0f);
            spriteBatch.Draw(barFrame, drawPos, null, Color.White * Opacity, 0f, barOrigin, barScale, 0f, 0f);
        }

        public const string AuraPath = "FranciumMultiCrossMod/Content/Extras/Aura";
        public static Asset<Texture2D> AuraBarBack = ModContent.Request<Texture2D>($"{AuraPath}BarBack", AssetRequestMode.AsyncLoad);
        public static Asset<Texture2D> AuraBarFront = ModContent.Request<Texture2D>($"{AuraPath}BarFront", AssetRequestMode.AsyncLoad);
        public static Asset<Texture2D> AuraBarFrame = ModContent.Request<Texture2D>($"{AuraPath}BarFrame", AssetRequestMode.AsyncLoad);
        public static void DrawAuraChargeBar(float barScale, Vector2 position, float progress, float Opacity)
        {
            var barBG = AuraBarBack.Value;
            var barFG = AuraBarFront.Value;
            var barFrame = AuraBarFrame.Value;

            Vector2 barOrigin = barBG.Size() * 0.5f;
            Vector2 drawPos = position;
            Rectangle frameCrop = new Rectangle(0, 0, (int)(progress * barFG.Width), barFG.Height);

            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.Draw(barBG, drawPos, null, Color.White * Opacity, 0f, barOrigin, barScale, 0f, 0f);
            spriteBatch.Draw(barFG, drawPos, frameCrop, Color.White * Opacity, 0f, barOrigin, barScale, 0f, 0f);
            spriteBatch.Draw(barFrame, drawPos, null, Color.White * Opacity, 0f, barOrigin, barScale, 0f, 0f);
        }
    }
}