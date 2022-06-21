using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Lamp : LobCorpHeavy
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Big Bird's eyes gained another in number for every creature it saved.\n" +
                               "On this weapon, the radiant pride is apparent.");
		}

		public override void SetDefaults() 
		{
			Item.damage = 72;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 32;
            Item.useAnimation = 32;
			Item.useStyle = -1;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes() 
		{
			CreateRecipe()
			.AddRecipeGroup("LobotomyCorp:DungeonLantern")
			.AddIngredient(ItemID.Feather, 15)
			.AddIngredient(ItemID.Bone, 10)
			.AddTile(Mod, "BlackBox3")
			.Register();
		}
	}
}