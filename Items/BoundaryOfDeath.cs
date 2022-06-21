using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class BoundaryOfDeath : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Shi Association South Section 2 Sword (Yujin)");
			Tooltip.SetDefault("Sets your health to 1/4 when above that threshold.\n" +
							   "4% chance to increase weapon damage by 4444% and do a special attack");
		}

		public override void SetDefaults() 
		{
			Item.damage = 44;
			Item.DamageType = DamageClass.Melee;
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = 1;
			Item.knockBack = 4.4f;
			Item.value = 44444;
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.BoundaryOfDeathInitial>();
			Item.shootSpeed = 8f;
			Item.noUseGraphic = true;
		}

		public override void AddRecipes() 
		{
		}
	}
}