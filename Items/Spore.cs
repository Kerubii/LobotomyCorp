using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Spore : ModItem
	{
		public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("A spear covered in spores and affection.\n" +
                               "It lights the employee's heart, shines like a star, and steadily tames them.");

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
            Item.shootSpeed = 4.7f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Spore>();

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
			.AddIngredient(ItemID.GlowingMushroom, 75)
			.AddIngredient(ItemID.JungleSpores, 4)
			.AddTile(Mod, "BlackBox2")
			.Register();
		}
	}
}