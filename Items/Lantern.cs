using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Lantern : LobCorpHeavy
	{
		public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("The luminous organ shines brilliantly, making it useful for lighting up the dark.\n" +
                               "Though teeth sticking out of some spots of the equipment is a rather frightening sight.");
        }

		public override void SetDefaults() 
		{
			Item.damage = 62;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 26;
            Item.useAnimation = 26;
			Item.useStyle = -1;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes() 
		{
			CreateRecipe()
			.AddIngredient(ItemID.Coral, 8)
			.AddIngredient(ItemID.Topaz, 4)
			.AddIngredient(ItemID.AntlionMandible, 4)
			.AddTile<Tiles.BlackBox>()
			.Register();
		}
	}
}