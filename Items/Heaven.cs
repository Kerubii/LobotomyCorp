using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Heaven : ModItem
	{
		public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("Just contain it in your sight.\n" +
                               "As it spreads its wings for an old god, a heaven just for you burrows its way.") ;

        }

		public override void SetDefaults() 
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 22;
			Item.useAnimation = 20;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Purple;
            Item.shootSpeed = 3.7f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Heaven>();

            Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
			Item.autoReuse = true;
		}

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void AddRecipes() 
		{
			CreateRecipe()
			.AddIngredient(ItemID.Javelin)
			.AddIngredient(ItemID.TheRottedFork)
			.AddIngredient(ItemID.CrimtaneBar, 5)
			.AddTile(Mod, "BlackBox2")
			.Register();

			CreateRecipe()
			.AddIngredient(ItemID.Javelin)
			.AddIngredient(ItemID.BallOHurt)
			.AddIngredient(ItemID.DemoniteBar, 5)
			.AddTile(Mod, "BlackBox2")
			.Register();
		}
	}
}