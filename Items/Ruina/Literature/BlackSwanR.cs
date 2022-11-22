using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using System;

namespace LobotomyCorp.Items.Ruina.Literature
{
	public class BlackSwanR : SEgoItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Black Swan"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault(GetTooltip());
		}

		public override void SetDefaults() 
		{
			EgoColor = LobotomyCorp.WawRarity;

			Item.width = 70;
			Item.height = 70;

			Item.damage = 132;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 2.3f;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Shoot;

			Item.value = 10000;
            Item.noMelee = true;
            Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.channel = true;
			Item.rare = ItemRarityID.Yellow;

			Item.shoot = ModContent.ProjectileType<Projectiles.Realized.BlackSwanR>();
			Item.shootSpeed = 4f;
		}

        public override bool SafeCanUseItem(Player player)
        {
			return base.SafeCanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			if (player.altFunctionUse == 2)
			{
				bool shriek = false;
				if (LobotomyModPlayer.ModPlayer(player).BlackSwanNettleClothing >= 4)
				{
					damage = (int)(damage * 0.75f);
					LobotomyModPlayer.ModPlayer(player).BlackSwanNettleRemove(1);
					shriek = true;
				}
				else if (LobotomyModPlayer.ModPlayer(player).BlackSwanBrokenDream)
				{
					damage = (int)(damage * 0.4f);
					shriek = true;
				}

				if (shriek)
				{
					type = ModContent.ProjectileType<Projectiles.Realized.BlackSwanScream>();
				}
				else
					type = 0;
			}
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override bool? UseItem(Player player)
        {
			if (!LobotomyModPlayer.ModPlayer(player).BlackSwanBrokenDream)
				player.AddBuff(ModContent.BuffType<Buffs.NettleClothing>(), 60);
			if (player.altFunctionUse == 2)
            {
				Item.useStyle = ItemUseStyleID.Shoot;
			}
			else
            {
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.shoot = ModContent.ProjectileType<Projectiles.Realized.BlackSwanR>();
			}

            return base.UseItem(player);
        }

        public override float UseSpeedMultiplier(Player player)
        {
			if (LobotomyModPlayer.ModPlayer(player).BlackSwanNettleClothing >= 1)
				return 1.15f;
            return base.UseSpeedMultiplier(player);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes() 
		{
		}
	}
}