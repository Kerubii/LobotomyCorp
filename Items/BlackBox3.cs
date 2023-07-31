using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class BlackBox3 : ModItem
	{
        public override void SetStaticDefaults()
        {
			// DisplayName.SetDefault("Black Box - Extractor");
			/* Tooltip.SetDefault("A Tool to extract from the subconsciousness of mankind\n" +
							   "Allows the creation of WAW and ALEPH E.G.O"); */
		}

        public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(0, 20, 0, 0);
			Item.createTile = ModContent.TileType<Tiles.BlackBox3>();
		}

        public override void AddRecipes()
        {
			CreateRecipe()
			.AddIngredient(Mod, "BlackBox2")
			.AddIngredient(ItemID.HellstoneBar, 4)
			.AddIngredient(ItemID.JungleSpores, 8)
			.AddIngredient(ItemID.Bone, 10)
			.AddTile(TileID.DemonAltar)
			.Register();

			CreateRecipe()
			.AddIngredient(Mod, "BlackBox2")
			.AddIngredient(ItemID.HellstoneBar, 4)
			.AddIngredient(ItemID.JungleSpores, 8)
			.AddIngredient(ItemID.BeeWax, 4)
			.AddTile(TileID.DemonAltar)
			.Register();
		}
    }
}