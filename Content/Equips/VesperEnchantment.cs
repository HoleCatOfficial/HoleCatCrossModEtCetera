
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
using DestroyerTest.Content.Equips.MalakhimSet;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.MeleeWeapons.Flails;

namespace FranciumMultiCrossMod.Content.Equips
{
    public class VesperEnchantment : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<VesperRarity>();
            Item.value = Item.sellPrice(platinum: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.TryGetModPlayer<MalakhimPlayer>(out var MK))
            {
                MK.Active = true;
            }
            if (player.TryGetModPlayer<VesperEnchantmentPlayer>(out var V))
            {
                V.Active = true;
            }
            if (player.TryGetModPlayer<MalakhimHurtSounds>(out var HurtSounds))
            {
                HurtSounds.Active = true;
            }
        }   
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Vesper>(25)
                .AddIngredient<Goliath>()
                .AddIngredient<Quixotism>()
                .AddIngredient<Dreampoon>()
                .AddIngredient<Ambition>()
                .AddIngredient<SpearOfAspiration>()
                .AddIngredient<HypnicJerk>()
                .AddIngredient<MalakhimChaplet>()
                .AddIngredient<MalakhimPlates>()
                .AddIngredient<MalakhimWings>()
                .AddTile(TileID.Anvils)
                .Register();
        }
	}

    public class VesperEnchantmentPlayer : ModPlayer
    {
        public bool Active = false;
        public override void ResetEffects()
        {
            Active = false;
        }

        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (Active)
            {
                if (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed)
                {
                    scale = 1.5f;
                }
            }
        }

        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            if (Active)
            {
                if (item.DamageType == DamageClass.Magic)
                {
                    mult *= 0.8f;
                }
            }
        }

        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (Active)
            {
                crit += 15;
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (Active)
            {
                hurtInfo.Knockback *= 0.5f;
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (Active)
            {
                modifiers.Knockback *= 0.5f;
            }
        }
    }
}
