using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class FaintAroma : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Even after the E.G.O was extracted, it still carried the fragrance of the archetype.\n" +
							   "Simply carrying it gives the illusion that you're standing in a forest in the middle of nowhere.");
        }

		public override void SetDefaults() {
			Item.damage = 6;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 20;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 10000;
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = LobotomyCorp.WeaponSounds.BowGun;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Arrow;
            Item.scale = 0.8f;
		}

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.WoodenBow)
			.AddIngredient(ItemID.Acorn, 10)
			.AddIngredient(ItemID.FlowerWall, 50)
			.AddTile(Mod, "BlackBox3")
			.Register();
		}
	}
}
