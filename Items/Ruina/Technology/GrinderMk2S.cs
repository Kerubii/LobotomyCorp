using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace LobotomyCorp.Items.Ruina.Technology
{
	public class GrinderMk2S : SEgoItem
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("A complete E.G.O.\n" +
                               "\"Blood covers the whole floor, screams echo, people are running away...\"");
            
            EgoColor = LobotomyCorp.HeRarity;
		}

		public override void SetDefaults() 
		{
            PassiveText = "Disable Limiter - 15% increased movement speed while active\n" +
                          "Clean - Invincibility while spinning your blades\n" +
                          "|Charge - Using the weapon decreases battery, Become immobilized after draining your battery";

            Item.damage = 63;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 26;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
			Item.autoReuse = true;
            Item.channel = true;
		}

        public override bool SafeCanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.GrinderMk2Cleaner>()] == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.GrinderMk2Cleaner>(), Item.damage, Item.knockBack, player.whoAmI, i);
                }
                SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Helper_On") with {Volume = 0.25f}, player.Center);
            }
            if (player.altFunctionUse == 2)
            {
                Item.useAnimation = 20;
                Item.useTime = 20;
                LobotomyModPlayer.ModPlayer(player).GrinderMk2Order = Main.rand.Next(5);
            }
            else
            {
                Item.useTime = 26;
                Item.useAnimation = 20;
            }

            return !LobotomyModPlayer.ModPlayer(player).GrinderMk2Recharging;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes() 
		{
		}
	}
}