using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Censored : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("[CENSORED]"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("[CENSORED] has the ability to [CENSORED], but this is a horrendous sight for those watching.\n" +
							   "Looking at the E.G.O for more than 3 seconds will make you sick.\n" +
							   "Heal 40% damage taken");
		}

		public override void SetDefaults() 
		{
			Item.damage = 42;
			Item.knockBack = 6;
			Item.scale = 1.3f;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = 5;

			Item.value = 10000;
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.NPCHit18;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			Item.shoot = ModContent.ProjectileType<Projectiles.CensoredGrab>();
			Item.shootSpeed = 1f;
		}

		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one spear can be thrown out, use this when using autoReuse
			if (player.altFunctionUse == 2)
            {
				Item.shoot = ModContent.ProjectileType<Projectiles.CensoredSpike>();
				Item.useTime = 56;
				Item.useAnimation = 56;
			}
			else
            {
				Item.shoot = ModContent.ProjectileType<Projectiles.CensoredGrab>();
				Item.useTime = 22;
				Item.useAnimation = 22;
			}

			return player.ownedProjectileCounts[Item.shoot] < 1;
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