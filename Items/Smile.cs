using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Smile : LobCorpHeavy
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("It has the pale faces of nameless employees and a giant mouth on it.\n" +
                               "Upon striking with the weapon, the monstrous mouth opens wide to devour the target, its hunger insatiable.");

        }

		public override void SetDefaults() 
		{
			Item.damage = 112;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 32;
            Item.useAnimation = 32;
			Item.useStyle = 1;

			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Red;

			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.shootSpeed = 1f;
			Item.noUseGraphic = false;
			Item.noMelee = false;
		}

        public override bool AltFunctionUse(Player player)
        {
			return true;
        }

        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
			{
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.shoot = ModContent.ProjectileType<Projectiles.SmileSpecial>();
				Item.noUseGraphic = true;
				Item.noMelee = true;
			}
			else
			{
				Item.useStyle = -1;
				Item.shoot = 0;
				Item.noUseGraphic = false;
				Item.noMelee = false;
			}

			return base.CanUseItem(player);
        }

        public override void AddRecipes() 
		{
            CreateRecipe()
            .AddIngredient(ItemID.Pwnhammer)
            .AddIngredient(ItemID.SoulofNight, 8)
            .AddIngredient(ItemID.RottenChunk, 5)
            .AddTile(Mod, "BlackBox3")
			.Register();
		}
	}
}