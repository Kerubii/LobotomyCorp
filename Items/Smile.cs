using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
			Item.damage = 88;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 64;
            Item.useAnimation = 64;
			Item.useStyle = 15;

			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Red;

			SwingSound = LobotomyCorp.WeaponSounds.Hammer;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.SmileScream>();
			Item.shootSpeed = 1f;
			Item.noUseGraphic = false;
			Item.noMelee = false;
		}

        public override bool AltFunctionUse(Player player)
        {
			return true;
        }

        public override void UseStyleAlt(Player player, Rectangle heldItemFrame)
        {
			if (player.altFunctionUse != 2)
				ResetPlayerAttackCooldown(player);
			float rotation = ItemRotation(player);
            if ((player.direction == 1 && rotation < 0) ||
                    (player.direction == -1 && rotation > 0))
            {
				player.itemLocation.X += 12 * player.direction;
            }
            else if ((player.direction == 1 && rotation < 90) ||
                     (player.direction == -1 && rotation > -90))
            {
				player.itemLocation.X -= 12 * player.direction;
			}
            else if ((player.direction == 1 && rotation < 180) ||
                    (player.direction == -1 && rotation > -180))
            {
				player.itemLocation.X -= 12 * player.direction;
			}
        }

        public override bool? UseItem(Player player)
        {
			if (player.altFunctionUse == 2)
			{
				Item.noMelee = true;
			}
			else
			{
				Item.noMelee = false;
				return true;
			}
            return base.UseItem(player);
        }

        public override void UseAnimation(Player player)
        {
			if (player.altFunctionUse == 2)
			{
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noUseGraphic = true;
			}
			else
			{
				SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/LWeapons/Danggo_Lv2") with { Volume = 0.25f }, player.Center);
				Item.useStyle = 15;
				Item.noUseGraphic = false;
			}
		}

        public override bool CanShoot(Player player)
        {
            return base.CanShoot(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			if (player.altFunctionUse != 2)
            {
				float scale = Item.scale;
				ItemLoader.ModifyItemScale(Item, player, ref scale);
				Vector2 offset = new Vector2(100 * scale, 0).RotatedBy(player.itemRotation - MathHelper.ToRadians(45) + (player.direction < 0 ? -1.57f : 0));
				position += offset;
				velocity = Vector2.Zero;
				type = ModContent.ProjectileType<Projectiles.SmileScream>();
				damage = (int)(damage * 0.2f);
				knockback *= 0.1f;
			}
			else
            {
				type = ModContent.ProjectileType<Projectiles.SmileSpecial>();
			}

            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
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