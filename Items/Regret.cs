using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Regret : LobCorpHeavy
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Enemies crushed by this regret can never return to their normal life.\n" +
                               "Before swinging this weapon, expressing one's condolences for the demise of the inmate who couldn't even have a funeral would be nice.");

        }

		public override void SetDefaults() 
		{
			Item.damage = 42;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 56;
            Item.useAnimation = 56;
			Item.useStyle = 15;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			SwingSound = LobotomyCorp.WeaponSounds.Hammer;
			Item.autoReuse = true;
		}

		public override void AddRecipes() 
		{
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 10)
            .AddIngredient(ItemID.Chain, 12)
            .AddIngredient(ItemID.Wood, 5)
            .AddTile(Mod, "BlackBox")
			.Register();
		}
	}
}