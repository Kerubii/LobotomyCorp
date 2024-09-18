using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.ItemTiles
{
    public class BlackBox2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Black Box - Container");
            /* Tooltip.SetDefault("Contains an abnormal energy\n" +
							   "Allows the creation of HE E.G.O"); */
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
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.createTile = ModContent.TileType<Tiles.BlackBox2>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(Mod, "BlackBox")
            .AddIngredient(ItemID.ShadowScale, 10)
            .AddTile(TileID.DemonAltar)
            .Register();

            CreateRecipe()
            .AddIngredient(Mod, "BlackBox")
            .AddIngredient(ItemID.TissueSample, 10)
            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}