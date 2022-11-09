using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace LobotomyCorp.Items
{
	public class Twilight : LobCorpLight
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Just like how the ever-watching eyes...\n" +
                               "The scale that could measure any and all sin...\n" +
                               "The beak that could swallow everything protected the peace of the Black Forest...\n" +
                               "The wielder of this armament may also bring peace as they did");
		}

		public override void SetDefaults() 
		{
			Item.damage = 62;
			Item.scale = 1.3f;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 90;
			Item.useAnimation = 90;
			Item.useStyle = 15;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<Projectiles.TwilightSpecial>();
			Item.shootSpeed = 3f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}

        public override bool AltFunctionUse(Player player)
        {
			return LobotomyModPlayer.ModPlayer(player).TwilightSpecial >= 10;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			damage = (int)(damage * 1.3f);
        }

        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
			{
				LobotomyModPlayer.ModPlayer(player).TwilightSpecial = 0;
				Item.UseSound = LobotomyCorp.WeaponSound("judgement2_1");
				Item.useTime = 90;
				Item.useAnimation = 90;
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.shoot = ModContent.ProjectileType<Projectiles.TwilightSpecial>();
				Item.shootSpeed = 4.2f;
				//Item.noUseGraphic = true;
				Item.noMelee = true;
			}
			else
			{
				Item.UseSound = LobotomyCorp.WeaponSound("judgement1");
				Item.useTime = 26;
				Item.useAnimation = 26;
				Item.useStyle = 15;
				Item.shoot = 0;// ModContent.ProjectileType<Projectiles.TwilightSlash>();
				Item.shootSpeed = 1f;
				//Item.noUseGraphic = false;
				Item.noMelee = false;
			}
			return true;
        }

        public override void UseStyleAlt(Player player, Rectangle heldItemFrame)
        {
			ResetPlayerAttackCooldown(player);
			/*
			if (Main.myPlayer == player.whoAmI && player.itemAnimation > (int)(player.itemAnimationMax * 0.7f))
			{
				int amount = 1 + Main.rand.Next(3);
				for (int i = 0; i < amount; i++)
				{
					float dist = 56f + 200 / (amount + 1) * (i + 1);
					Vector2 vel = new Vector2(dist, Main.rand.NextFloat(-32, 32)).RotatedBy(MathHelper.ToRadians(ItemRotation(player) - 90));
					Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, vel, ModContent.ProjectileType<Projectiles.TwilightEye>(), 0, 0, player.whoAmI, MathHelper.ToRadians(5) * player.direction);
				}
			}*/
            base.UseStyleAlt(player, heldItemFrame);
        }

        public override bool? UseItemAlt(Player player)
        {
			if (player.altFunctionUse == 2)
			{
				Item.noUseGraphic = true;
				Item.useStyle = ItemUseStyleID.Shoot;
			}
			else
			{
				Item.noUseGraphic = false;
				Item.useStyle = 15;
			}
			return true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			if (LobotomyModPlayer.ModPlayer(player).TwilightSpecial < 9)
            {
				LobotomyModPlayer.ModPlayer(player).TwilightSpecial++;
				if (LobotomyModPlayer.ModPlayer(player).TwilightSpecial == 9)
                {
					SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
					for (int index1 = 0; index1 < 5; ++index1)
					{
						int index2 = Dust.NewDust(player.position, player.width, player.height, 45, 0.0f, 0.0f, (int)byte.MaxValue, new Color(), (float)Main.rand.Next(20, 26) * 0.1f);
						Main.dust[index2].noLight = true;
						Main.dust[index2].noGravity = true;
						Main.dust[index2].velocity *= 0.5f;
					}
					LobotomyModPlayer.ModPlayer(player).TwilightSpecial++;
				}
			}
        }

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(Mod, "Justitia")
			.AddIngredient(ItemID.SoulofLight, 5)
			.AddIngredient(ItemID.SoulofNight, 10)
			.AddIngredient(ItemID.DarkShard)
			.AddTile(Mod, "BlackBox3")
			.Register();
		}
	}
}