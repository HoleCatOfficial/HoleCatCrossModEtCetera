using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using System.Collections.Generic;
using FranciumMultiCrossMod.Content.Equips;

namespace FranciumMultiCrossMod.Common
{
	public class HeliciteEnchantmentTimerOverlay : ModResourceOverlay
	{
		private Asset<Texture2D> heartTexture;
		private Dictionary<string, Asset<Texture2D>> vanillaAssetCache = new();

		public override void PostDrawResource(ResourceOverlayDrawContext context)
		{
			Asset<Texture2D> asset = context.texture;

			if (asset == TextureAssets.Heart || asset == TextureAssets.Heart2)
			{
				DrawCooldownOverlay(context);
			}
		}

		private bool CompareAssets(Asset<Texture2D> existingAsset, string compareAssetPath)
		{
			if (!vanillaAssetCache.TryGetValue(compareAssetPath, out var asset))
				asset = vanillaAssetCache[compareAssetPath] = Main.Assets.Request<Texture2D>(compareAssetPath);

			return existingAsset == asset;
		}

		private void DrawCooldownOverlay(ResourceOverlayDrawContext context)
		{
			Player player = Main.LocalPlayer;
			var modPlayer = player.GetModPlayer<HeliciteEnchantmentPlayer>();

			if (modPlayer.currentCooldown <= 0)
				return;

			float progress = modPlayer.currentCooldown / (float)modPlayer.Cooldown;

			// Tint white but faded
			context.color = Color.White * progress;

			// Your custom fading heart overlay texture
			context.texture = heartTexture ??= ModContent.Request<Texture2D>("FranciumMultiCrossMod/Content/Extras/HeliciteEnchantmentTimerHeart");

			context.Draw();
		}
	}
}
