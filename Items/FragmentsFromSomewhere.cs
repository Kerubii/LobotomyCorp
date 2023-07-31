using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class FragmentsFromSomewhere : ModItem
	{
		public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("Do not attempt to understand it, just use it.\n" +
                               "The spear often tries to lead the wielder into a long and endless realm of mind...\n" +
                               "But they must try to not be swayed by it."); */

        }

		public override void SetDefaults() 
		{
			Item.damage = 14;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 22;
			Item.useAnimation = 20;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 3.7f;
            Item.shoot = ModContent.ProjectileType<Projectiles.FragmentsFromSomewhere>();

            Item.noUseGraphic = true;
			Item.UseSound = LobotomyCorp.WeaponSounds.Spear;
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
            .AddIngredient(ItemID.FallenStar, 10)
            .AddIngredient(ItemID.Amethyst)
            .AddIngredient(ItemID.Topaz)
            .AddIngredient(ItemID.Emerald)
            .AddIngredient(ItemID.Ruby)
            .AddIngredient(ItemID.Sapphire)
            .AddIngredient(ItemID.Diamond)
            .AddTile(Mod, "BlackBox")
            .Register();
        }
	}
}