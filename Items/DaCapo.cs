using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class DaCapo : ModItem
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("A scythe that swings silently and with discipline like a conductor's gestures and baton.\n" +
                               "If there were a score for this song, it would be one that sings of the apocalypse.");
		}

		public override void SetDefaults() 
		{
			Item.damage = 64;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.shootSpeed = 1f;
            Item.scale = 0.8f;
		}

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 48;
                Item.useAnimation = 48;
                Item.useStyle = 5;
                Item.shoot = ModContent.ProjectileType<Projectiles.DaCapo>();
                Item.noUseGraphic = true;
                Item.noMelee = true;
            }
            else
            {
                Item.useTime = 26;
                Item.useAnimation = 26;
                Item.useStyle = 1;
                Item.shoot = 0;
                Item.noUseGraphic = false;
                Item.noMelee = false;
            }
            return base.CanUseItem(player);
        }

        public override void AddRecipes() 
		{
            CreateRecipe()
            .AddIngredient(ItemID.DarkShard)
            .AddIngredient(ItemID.LightShard)
            .AddIngredient(ItemID.MusicBox)
            .AddTile(Mod, "BlackBox3")
            .Register();
        }
	}
}