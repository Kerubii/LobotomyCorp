using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Lumber : ModItem
    {
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("A versatile equipment made to cut down trees and people alike.\n" +
                               "Perhaps sharpening the axe was the one thing it didn't neglect. The blade is always shiny.");
		}

		public override void SetDefaults() 
		{
			Item.damage = 26;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes() 
		{
			CreateRecipe()
			.AddIngredient(ItemID.IronAxe)
			.AddIngredient(ItemID.IronBar, 20)
			.AddIngredient(ItemID.Wood, 50)
			.AddTile(Mod, "BlackBox2")
			.Register();

			CreateRecipe()
			.AddIngredient(ItemID.LeadAxe)
			.AddIngredient(ItemID.LeadBar, 20)
			.AddIngredient(ItemID.Wood, 50)
			.AddTile(Mod, "BlackBox2")
			.Register();
		}
	}
}