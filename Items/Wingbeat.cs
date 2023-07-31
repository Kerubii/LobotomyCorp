using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Wingbeat : LobCorpLight
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("Graced by the fairies, the weapon radiates a pale light.\n" +
                               "Despite its cute shape, the E.G.O. itself is rather heavy."); */
		}

		public override void SetDefaults() 
		{
			Item.damage = 18;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 15;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = LobotomyCorp.WeaponSounds.Mace;
			Item.autoReuse = true;
		}

		public override void AddRecipes() 
		{
            CreateRecipe()
            .AddIngredient(ItemID.Firefly, 5)
            .AddIngredient(ItemID.Bottle)
            .AddIngredient(ItemID.Sapphire, 3)
            .AddIngredient(ItemID.Sunflower)
            .AddTile(Mod, "BlackBox")
			.Register();
		}
	}
}