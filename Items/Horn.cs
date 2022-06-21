using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Horn : ModItem
	{
		public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("The green - eyed beauty's favorite flower was the dahlia; \"Your love makes me happy.\"\n" +
                               "The lady's happiness came to an end with the budding of those unsightly horns.\n" +
                               "The dahlia's unfulfilled meaning was borne as a seed in this E.G.O, carrying a lingering emotion.");

        }

		public override void SetDefaults() 
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 22;
			Item.useAnimation = 20;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 3.7f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Horn>();

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
            .AddIngredient(ItemID.JungleRose)
            .AddIngredient(ItemID.Lens, 5)
            .AddIngredient(ItemID.JungleSpores, 9)
            .AddTile(Mod, "BlackBox")
            .Register();
        }
	}
}