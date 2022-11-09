using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Lumber : LobCorpHeavy
    {
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("A versatile equipment made to cut down trees and people alike.\n" +
                               "Perhaps sharpening the axe was the one thing it didn't neglect. The blade is always shiny.");
		}

		public override void SetDefaults() 
		{
			Item.damage = 92;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 52;
			Item.useAnimation = 52;
			Item.useStyle = 15;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Yellow;
			SwingSound = new SoundStyle("LobotomyCorp/Sounds/Item/Lumberjack_Atk2") with { Volume = 0.5f, PitchVariance = 0.1f }; ;
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