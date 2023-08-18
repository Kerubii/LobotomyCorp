using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class BlackBox : ModItem
	{
        public override void SetStaticDefaults()
        {
			/* Tooltip.SetDefault("Absorbs an unknown energy around itself\n" +
							   "Allows the creation of ZAYIN and TETH E.G.O"); */
		}

        public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 14;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(0, 0, 0, 35);
			Item.createTile = ModContent.TileType<Tiles.BlackBox>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.WorkBench)
			.AddTile(TileID.DemonAltar)
			.Register();
		}
	}
}