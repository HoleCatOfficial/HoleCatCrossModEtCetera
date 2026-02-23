
using FranciumMultiCrossMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FranciumMultiCrossMod.Content.Melee
{

	public class Netrahalis : ModItem
	{
        
		public override void SetDefaults()
		{
			// Common Properties
			Item.width = 56;
			Item.height = 56;
			Item.value = Item.sellPrice(gold: 36, silver: 2);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = new SoundStyle("FranciumMultiCrossMod/Assets/Audio/FlurrySlash4");


			// Use Properties
			// Note that useTime and useAnimation for this item don't actually affect the behavior because the held projectile handles that. 
			// Each attack takes a different amount of time to execute
			// Conforming to the item useTime and useAnimation makes it much harder to design
			// It does, however, affect the item tooltip, so don't leave it out.
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.channel = true;

			// Weapon Properties
			Item.autoReuse = true;
			Item.knockBack = 7;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
			Item.damage = 50; // The damage of your sword, this is dynamically adjusted in the projectile code.
			Item.DamageType = DamageClass.MeleeNoSpeed; // Deals melee damage
			Item.noMelee = true;  // This makes sure the item does not deal damage from the swinging animation
			Item.noUseGraphic = true; // This makes sure the item does not get shown when the player swings his hand

			// Projectile Properties
			Item.shoot = ModContent.ProjectileType<NetrahalisSlice>(); // The sword as a projectile
		}

		public override bool MeleePrefix() {
			return true; // return true to allow weapon to have melee prefixes (e.g. Legendary)
		}

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D Glow = ModContent.Request<Texture2D>("FranciumMultiCrossMod/Content/Melee/NetrahalisGlow").Value;
			Vector2 BladeCenter = new Vector2(0, 0); // measured from top-left of texture

			float offsetDistance = 4f;
			float glowOpacity = 0.4f; // adjust opacity to your liking
			float rotationAngle = MathHelper.ToRadians(Main.GameUpdateCount % 360); // or any rotation speed

			// Offsets for cardinal directions (up, right, down, left)
			Vector2[] offsets = new Vector2[]
			{
				new Vector2(0, -offsetDistance),
				new Vector2(offsetDistance, 0),
				new Vector2(0, offsetDistance),
				new Vector2(-offsetDistance, 0)
			};

			//spriteBatch.End();
			//spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			for (int i = 0; i < offsets.Length; i++)
			{
				// Rotate offset vector around blade center by rotationAngle
				Vector2 rotatedOffset = offsets[i].RotatedBy(rotationAngle);

				// Calculate draw position: base position + rotated offset - blade center scaled
				Vector2 drawPos = position + rotatedOffset;

				spriteBatch.Draw(
					Glow,
					drawPos,
					null,
					drawColor * glowOpacity,
					0f,
					origin,
					scale,
					SpriteEffects.None,
					0f
				);
			}

			//spriteBatch.End();
			//spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			return true;
		}

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
			Texture2D Glow = ModContent.Request<Texture2D>("FranciumMultiCrossMod/Content/Melee/NetrahalisGlow").Value;
			Vector2 BladeCenter = new Vector2(0, 0); // measured from top-left of texture

			float offsetDistance = 4f;
			float glowOpacity = 0.4f; // adjust opacity to your liking
			float rotationAngle = MathHelper.ToRadians(Main.GameUpdateCount % 360); // or any rotation speed

			// Offsets for cardinal directions (up, right, down, left)
			Vector2[] offsets = new Vector2[]
			{
				new Vector2(0, -offsetDistance),
				new Vector2(offsetDistance, 0),
				new Vector2(0, offsetDistance),
				new Vector2(-offsetDistance, 0)
			};

			//spriteBatch.End();
			//spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			for (int i = 0; i < offsets.Length; i++)
			{
				// Rotate offset vector around blade center by rotationAngle
				Vector2 rotatedOffset = offsets[i].RotatedBy(rotationAngle);

				// Calculate draw position: base position + rotated offset - blade center scaled
				Vector2 drawPos = Item.Center + rotatedOffset;

				spriteBatch.Draw(
					Glow,
					drawPos  - Main.screenPosition,
					null,
					lightColor * glowOpacity,
					0f,
					Item.Center - Main.screenPosition,
					scale,
					SpriteEffects.None,
					0f
				);
			}

			//spriteBatch.End();
			//spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		
            return true;
        }


		public override void AddRecipes()
		{
			if (ModLoader.TryGetMod("SOTS", out Mod SOTS))
			{
				if (SOTS.TryFind("TwilightAlloy", out ModItem TA))
				{
					CreateRecipe()
						.AddIngredient(ItemID.Arkhalis)
						.AddIngredient(ItemID.Terragrim)
						.AddIngredient(TA.Type)
						.AddTile(TileID.Anvils)
						.Register();
				}
			}
		}
	}
}