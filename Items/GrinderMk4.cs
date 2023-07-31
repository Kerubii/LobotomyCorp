using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class GrinderMk4 : ModItem
	{
		public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("The sharp sawtooth of the grinder makes a clean cut through its enemy.\n" +
                               "Its operation is simple and straightforward, but that doesn't necessarily make it easy to wield."); */

        }

		public override void SetDefaults() 
		{
			Item.damage = 14;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 42;
			Item.useAnimation = 40;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 1.8f;
            Item.shoot = ModContent.ProjectileType<Projectiles.GrinderMk4>();

            Item.noUseGraphic = true;
			Item.UseSound = LobotomyCorp.WeaponSound("helper");
            Item.noMelee = true;
			Item.autoReuse = true;
		}

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //On hit, 10% chance to increase sp by 40% for 30 seconds
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.WoodYoyo)
            .AddIngredient(ItemID.Lens, 5)
            .AddIngredient(ItemID.IronBar, 10)
            .AddTile(Mod, "BlackBox2")
            .Register();
        }
    }
}