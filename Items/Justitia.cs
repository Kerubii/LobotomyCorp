using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Justitia : ModItem
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("It remembers the balance of the Long Bird that never forgot others' sins.\n" +
                               "This weapon may be able to not only cut flesh but trace of sins as well.\n" +
							   "Special attack ignores immunity frames");
		}

		public override void SetDefaults() 
		{
			Item.damage = 36;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 80;
			Item.useAnimation = 80;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shootSpeed = 1f;
			Item.scale = 1.3f;	
		}

        public override bool CanUseItem(Player player)
        {

			if (Main.rand.Next(3) == 0)
            {
				Item.useTime = 80;
				Item.useAnimation = 80;
				Item.useStyle = 5;
				Item.noMelee = true;
				Item.shoot = ModContent.ProjectileType<Projectiles.JustitiaAlt>();
			}
			else
            {
				Item.useTime = 22;
				Item.useAnimation = 22;
				Item.useStyle = 1;
				Item.noMelee = false;
				Item.shoot = 0;
			}
            return base.CanUseItem(player);
        }

        public override bool? UseItem(Player player)
        {
			Item.noUseGraphic = Item.noMelee;
			return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			damage /= 2;
		}

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
			if (player.blind)
				damage += 0.2f;
        }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
			target.immune[player.whoAmI] = 3;
		}

        public override void AddRecipes() 
		{
			CreateRecipe()
			.AddIngredient(ItemID.LightsBane)
			.AddIngredient(ItemID.Feather, 15)
			.AddIngredient(ItemID.Bone, 10)
			.AddIngredient(ItemID.Silk, 20)
			.AddTile(Mod, "BlackBox3")
			.Register();
		}
	}
}