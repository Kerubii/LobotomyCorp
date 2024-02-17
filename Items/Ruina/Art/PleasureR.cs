using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using System;

namespace LobotomyCorp.Items.Ruina.Art
{
    public class PleasureR : SEgoItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<Configs.LobotomyServerConfig>().TestItemEnable;
		}

		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Our Galaxy");
			// Tooltip.SetDefault(GetTooltip());
		}

		public override void SetDefaults() 
		{
			EgoColor = LobotomyCorp.HeRarity;

			Item.width = 64;
			Item.height = 64;

			Item.damage = 90;
			Item.DamageType = DamageClass.Summon;
			Item.knockBack = 1f;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;

			Item.value = 100000;
            Item.noMelee = true;
            Item.noUseGraphic = true;
			//Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.Yellow;

			Item.shoot = ModContent.ProjectileType<Projectiles.Realized.PleasureSpike>();
			Item.shootSpeed = 12;
		}

        public override bool? UseItem(Player player)
        {
			player.AddBuff(ModContent.BuffType<Buffs.PleasureTail>(), 60);
			LobotomyModPlayer.ModPlayer(player).PleasureTail = true;
            return base.UseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, -1);
			return false;
        }

        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }
}