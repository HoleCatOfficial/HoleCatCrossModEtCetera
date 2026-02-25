
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
using DestroyerTest.Content.Equips.AuraThiefSet;
using DestroyerTest.Content.MeleeWeapons;
using InnoVault.PRT;
using FranciumMultiCrossMod.Content.Extras;
using OpusLib;
using DestroyerTest.Content.Particles;
using Terraria.DataStructures;
using FranciumMultiCrossMod.Common;
using FranciumMultiCrossMod.Content.Projectiles.player.Enchantment;

namespace FranciumMultiCrossMod.Content.Equips
{
    public class AuraThiefEnchantment : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<LifeEchoRarity>();
            Item.value = Item.sellPrice(platinum: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.TryGetModPlayer<AuraThiefPlayer>(out var p))
            {
                p.Active = true;
            }
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<LifeEcho>(25)
                .AddIngredient<AuraThiefHeadgear>()
                .AddIngredient<AuraThiefHeadress>()
                .AddIngredient<AuraThiefBreastplate>()
                .AddIngredient<AuraThiefCuisses>()
                .AddIngredient<SoulEdge>()
                .AddTile(TileID.Anvils)
                .Register();
        }
	}

    public class AuraThiefPlayer : ModPlayer
    {
        public bool Active = false;
        public override void ResetEffects()
        {
            Active = false;
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (Active)
			{
				if (drawInfo.shadow == 0)
				{
					Bar.DrawAuraChargeBar(1f, (drawInfo.drawPlayer.Center + new Vector2(0, 60)) - Main.screenPosition, Progress, 1f);
				}
			}
        }

        public int DodgePoints = 0;
        public const int MaxDP = 20;
        public int InitWait = 240;
        public int ImmTimer = 0;
        public float Progress = 0f;
        public bool Boss = false;

        public bool Flag1 = false;
        public bool Flag2 = false;
        public override void PostUpdateEquips()
        {
            Progress = (float)DodgePoints / (float)MaxDP;
            if (Active)
            {

                if (ImmTimer > 0)
                {
                    Player.immune = true;
                    ImmTimer--;
                }


                if (Player.ownedProjectileCounts[ModContent.ProjectileType<SpiritAmalgam>()] < 1)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.One, ModContent.ProjectileType<SpiritAmalgam>(), 0, 0, Player.whoAmI);
                }

                foreach (NPC npc in Main.npc)
                {
                    if (npc.boss)
                    {
                        Boss = true;
                    }
                }

                if (Boss)
                {
                    if (InitWait > 0 && ImmTimer <= 0)
                    {
                        InitWait--;
                    }
                    if (InitWait <= 0)
                    {
                        if (Player.miscCounter % 60 == 0)
                        {
                            DodgePoints++;
                        }
                    }

                    if (DodgePoints >= MaxDP)
                    {
                        if (!Flag2)
                        {
                            SoundEngine.PlaySound(SoundID.Item129, Player.Center);
                            Opus.RadialSpreadParticle(PRTLoader.GetParticleID<SimpleParticle>(), 12, Player.Center, 1, Color.SkyBlue, 1f, 3f, RandomOffset: true);
                            Flag2 = true;
                        }
                        if (FranciumMultiCrossMod.EnchantmentKeybind1.JustPressed)
                        {
                            Opus.NewParticleFloatAI(PRTLoader.GetParticleID<AuraThiefSigil>(), Player.Center, Vector2.Zero, Color.SkyBlue, 0.01f, 2f);
                            SoundEngine.PlaySound(SoundID.DD2_WyvernScream, Player.Center);

                            ImmTimer = 300;
                            DodgePoints = 0;
                            InitWait = 240;
                            Flag2 = false;
                        }
                    }
                }
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            DodgePoints = 0;
            InitWait = 240;
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            DodgePoints = 0;
            InitWait = 240;
        }
    }
}
