using System;
using System.Collections.Generic;
using FranciumMultiCrossMod.Content.Equips;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace FranciumMultiCrossMod.Common
{
    // This class is used to store global item data that can be accessed across different mods.
    public class DTCrossModGlobalItem : GlobalItem
    {
         // Define stroke and text colors
        static Color strokeColor1 = new Color(255, 155, 0);
        static Color strokeColor2 = new Color(51, 31, 0);

        static Color textColor1 = new Color(51, 31, 0);
        static Color textColor2 = new Color(0, 0, 0);

        // Set global behavior to affect all items
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return true; // Applies to all items
        }

        // Modify tooltips to add the custom developer line
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {

            // Create a new TooltipLine with a custom color
            TooltipLine line = new TooltipLine(Mod, "CustomTooltip", "M A S T E R  O F  T H E  E C L I P S E")
            {
                OverrideColor = Color.Purple // Optional: Base color
            };

            // Add the custom tooltip to the end of the list
            if (item.type == ModContent.ItemType<RiftEnchantment>())
            {
                tooltips.Add(line);
            }
        }

        // PreDrawTooltipLine - Draw the text and stroke manually
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            // Check if it's our custom tooltip
            if (line.Name == "CustomTooltip" && line.Mod == Mod.Name && item.type == ModContent.ItemType<RiftEnchantment>())
            {
                // Smoothly interpolate between stroke and text colors using sine wave
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);
                Color textColor = Color.Lerp(textColor1, textColor2, lerpAmount);

                // Define the font and position
                DynamicSpriteFont font = FontAssets.MouseText.Value;
                Vector2 position = new Vector2(line.X, line.Y);

                // Draw the stroke by offsetting text in all directions
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        Vector2 offsetPosition = position + new Vector2(i, j);
                        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, line.Text, offsetPosition, strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top with the smooth color transition
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                // Return false to prevent default drawing since we manually drew it
                return false;
            }

            // Allow other tooltips to draw normally
            return true;
        }
    }
}