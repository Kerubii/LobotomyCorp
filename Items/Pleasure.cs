using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Pleasure : ModItem
	{
		public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("If you grasp for pleasure you cannot endure, you will end up losing yourself.\n" +
							   "When the powder that falls from the thorns becomes known to the world,\n" +
							   "People may forever sink into the swamp of intoxication.") ; */
        }

		public override void SetDefaults() 
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 22;
			Item.useAnimation = 26;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 10.3f;
            Item.shoot = ModContent.ProjectileType<Projectiles.PleasureSpear>();

            Item.noUseGraphic = true;
			Item.UseSound = LobotomyCorp.WeaponSound("Porccu");
            Item.noMelee = true;
			Item.autoReuse = true;
		}

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void AddRecipes() 
		{
			CreateRecipe()
			.AddIngredient(ItemID.Stinger, 8)
			.AddIngredient(ItemID.JungleSpores, 4)
			.AddIngredient(ItemID.Cactus, 14)
			.AddTile(Mod, "BlackBox2")
			.Register();
		}
	}
}