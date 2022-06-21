using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class BlackSwan : ModItem
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Believing that it would turn white,\n" +
							   "The black swan wanted to lift the curse by weaving together nettles.\n" +
							   "All that was left is a worn parasol it once treasured.\n" +
							   "Small chance to reflect damage taken");
		}

		public override void SetDefaults() 
		{
			Item.damage = 32;
			Item.knockBack = 6;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 1;

			Item.value = 10000;
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

        public override void HoldItem(Player player)
        {
			LobotomyModPlayer.ModPlayer(player).BlackSwanParryChance += 10;
        }

        public override void AddRecipes() 
		{
			CreateRecipe()
			.AddIngredient(ItemID.Umbrella)
			.AddIngredient(ItemID.BlackDye, 2)
			.AddIngredient(ItemID.Feather, 8)
			.AddTile(Mod, "BlackBox3")
			.Register();
		}
	}
}