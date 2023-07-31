using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class ScreamingWedge : ModItem
	{
		public override void SetStaticDefaults() {
			/* Tooltip.SetDefault("Hair has grown on the crossbow as if to express that the woman's dejection will never be forgotten.\n" +
                               "The sound of the projectile splitting the air is reminiscent of her piercing scream."); */
        }

		public override void SetDefaults() {
			Item.damage = 14;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 20;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 10000;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = LobotomyCorp.WeaponSounds.BowGun;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Arrow;
            Item.scale = 0.8f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                //Shoots Screaming Arrow
                return true;
            }
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Ebonwood, 30)
            .AddIngredient(ItemID.BlackString)
            .AddIngredient(ItemID.EbonstoneBlock, 10)
            .AddTile(Mod, "BlackBox2")
            .Register();

            CreateRecipe()
            .AddIngredient(ItemID.Shadewood, 30)
            .AddIngredient(ItemID.BlackString)
            .AddIngredient(ItemID.CrimstoneBlock, 10)
            .AddTile(Mod, "BlackBox2")
            .Register();
        }
    }
}
