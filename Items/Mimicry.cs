using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace LobotomyCorp.Items
{
	public class Mimicry : ModItem
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("The yearning to imitate the human form is sloppily reflected on the E.G.O.\n" +
							   "As if it were a reminder that it should remain a mere desire.\n" +
							   "It can deliver a powerful downswing that should be impossible for a human.\n" +
							   "Recovers 25% damage dealt on hit");
		}

		public override void SetDefaults() 
		{
			Item.damage = 32;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.channel = true;
			Item.rare = ItemRarityID.Red;
			//Item.UseSound = SoundID.Item1;
		}

        public override bool CanUseItem(Player player)
        {
			Item.scale = 1f;
			LobotomyModPlayer.ModPlayer(player).ChargeWeaponHelper = 0;
			return true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.channel)
            {
				LobotomyModPlayer modPlayer = LobotomyModPlayer.ModPlayer(player);
				player.itemAnimation = (int)(Item.useAnimation * ItemLoader.UseSpeedMultiplier(Item, player));
				player.itemRotation += Main.rand.NextFloat(-0.06f, 0.06f);
				if (modPlayer.ChargeWeaponHelper < 1f)
					modPlayer.ChargeWeaponHelper += 0.0166f;
				else
				{
					modPlayer.ChargeWeaponHelper = 1f;
					player.channel = false;
				}
				Item.scale = 1f + 0.5f * modPlayer.ChargeWeaponHelper;
			}
			if (player.itemAnimation == (int)(Item.useAnimation * ItemLoader.UseSpeedMultiplier(Item, player)) - 1)
				SoundEngine.PlaySound(SoundID.Item1, player.Center);
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage += 2f * LobotomyModPlayer.ModPlayer(player).ChargeWeaponHelper;
        }

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            noHitbox = player.channel;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			int heal = (int)(damage * 0.25f);
			player.HealEffect(heal);
			player.statLife += heal;
        }

        public override void AddRecipes() 
		{
            CreateRecipe()
            .AddIngredient(ItemID.BreakerBlade)
            .AddIngredient(ItemID.SoulofNight, 8)
            .AddIngredient(ItemID.Vertebrae, 5)
            .AddTile(Mod, "BlackBox3")
			.Register();
		}

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (!player.channel && Main.rand.Next(3) == 0)
            {
				Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Blood);
            }
        }
    }
}