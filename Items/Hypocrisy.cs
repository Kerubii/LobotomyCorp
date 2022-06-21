using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Hypocrisy : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("The tree turned out to be riddled with hypocrisy and deception;\n" +
                               "Those who wear its blessing act in the name of bravery and faith.\n" +
							   "However, be warned that nature does not know the difference between a blessing and a curse.");
        }

		public override void SetDefaults() {
			Item.damage = 24;
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
			Item.UseSound = SoundID.Item5;
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
		}
	}
}
