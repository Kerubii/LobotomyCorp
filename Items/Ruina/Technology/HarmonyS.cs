using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Ruina.Technology
{
	public class HarmonyS : SEgoItem
	{
		public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("\"But nothing could compare to the music it makes when it eats a human.\"\n");
            
        }

		public override void SetDefaults() 
		{
            PassiveText = "Harmony of Despair - Using the weapon increases its hit rate\n" +
                          "Let's Make Music! - Hitting an enemy gives all allies and enemies \"Musical Addiction\" buff.\n" +
                          "Performance - Shoot a soundwave that decreases defense.\n" +
                          "|Musical Addiction - 10% increased damage, decreases life regeneration.\n";

            EgoColor = LobotomyCorp.HeRarity;

            Item.damage = 34;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 22;
			Item.useAnimation = 20;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = 3;
            Item.shootSpeed = 3.7f;
            Item.shoot = ModContent.ProjectileType<Projectiles.HarmonyS>();

            Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
			Item.autoReuse = true;
            Item.channel = true;
		}

        public override bool SafeCanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void AddRecipes() 
		{
		}
	}
}