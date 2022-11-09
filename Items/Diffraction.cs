using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Diffraction : LobCorpLight
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("To see this E.G.O, one must thoroughly concentrate.\n" +
                               "You can ignore the ridiculous advice that you can see it with your mind.");
        }

		public override void SetDefaults() 
		{
			Item.damage = 46;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 26;
			Item.useAnimation = 22;
			Item.useStyle = 15;
			Item.knockBack = 6;
			Item.value = 5000;
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = LobotomyCorp.WeaponSounds.Mace;
			Item.autoReuse = true;
		}

		public override void AddRecipes() 
		{
			CreateRecipe()
			.AddIngredient(ItemID.TheBreaker)
			.AddIngredient(ItemID.InvisibilityPotion, 4)
			.AddTile(Mod, "BlackBox3")
			.Register();

			CreateRecipe()
			.AddIngredient(ItemID.FleshGrinder)
			.AddIngredient(ItemID.InvisibilityPotion, 4)
			.AddTile(Mod, "BlackBox3")
			.Register();
		}
	}
}