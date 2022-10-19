using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class DisciplinaryShell : ModItem
	{
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Discplinary Shell");
			Tooltip.SetDefault("Awakens the Red Mist\n" + 
							   "The Boss is highly unfinished I dont recommend fighting it");
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
			Item.createTile = ModContent.TileType<Tiles.DisciplinaryShell>();
		}

        public override void AddRecipes()
        {
			CreateRecipe()
			.AddIngredient(ItemID.HellstoneBar, 20)
			.AddIngredient(ItemID.JungleSpores, 8)
			.AddIngredient(ItemID.Bone, 10)
			.AddTile(TileID.DemonAltar)
			.Register();
		}
    }
}