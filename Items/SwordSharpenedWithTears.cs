using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class SwordSharpenedWithTears : ModItem
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("A sword suitable for swift thrusts.\n" +
                               "Even someone unskilled in dueling can rapidly puncture an enemy using this E.G.O with remarkable agility.");
		}

		public override void SetDefaults() 
		{
			Item.damage = 36;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 22;
			Item.useAnimation = 16;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Purple;
            Item.shootSpeed = 2.4f;
            Item.shoot = ModContent.ProjectileType<Projectiles.SwordSharpenedWithTearsProj>();

            Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
			Item.autoReuse = true;
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom((float)MathHelper.ToRadians(player.altFunctionUse == 2 ? 30 : 15));
            if (player.altFunctionUse == 2)
                damage = (int)(damage * 0.75f);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.knockBack = 6;
                Item.useTime = 7;
                Item.reuseDelay = 25;
            }
            else
            {
                Item.knockBack = 2.4f;
                Item.useTime = 16;
                Item.reuseDelay = 0;
            }

            return base.CanUseItem(player);
        }

        public override void AddRecipes() 
		{
            CreateRecipe()
            .AddIngredient(ItemID.Swordfish)
            .AddIngredient(ItemID.Sapphire)
            .AddIngredient(ItemID.FallenStar, 8)
            .AddTile(Mod, "BlackBox3")
            .Register();
        }
	}
}