using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;

namespace LobotomyCorp.Items.Ruina.Literature
{
	public class SanguineDesireR : SEgoItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Sanguine Desire"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault(GetTooltip());
		}

		public override void SetDefaults() 
		{
			EgoColor = LobotomyCorp.HeRarity;

			Item.width = 70;
			Item.height = 70;

			Item.damage = 62;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 2.3f;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;

			Item.value = 10000;
            Item.noMelee = true;
            Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.Yellow;

			Item.shoot = ModContent.ProjectileType<Projectiles.Realized.SanguineDesireR>();
			Item.shootSpeed = 16f;
		}

        public override bool SafeCanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
            {
				Item.shoot = ModContent.ProjectileType<Projectiles.Realized.SanguineDesireRHeavy>();
			}
			else
            {
				Item.shoot = ModContent.ProjectileType<Projectiles.Realized.SanguineDesireR>();
			}

			return base.SafeCanUseItem(player);
        }

        public override float UseSpeedMultiplier(Player player)
        {
			if (player.altFunctionUse == 2)
			{
				return 0.8f;
			}
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