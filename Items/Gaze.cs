using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Gaze : ModItem
	{
		public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("The gaze from the keyhole is fixed on its target without ever stopping.\n" +
                               "No one knows what it wanted to peep at so dearly.\n" + 
                               "Can cause damageitional damage over time");

        }

		public override void SetDefaults() 
		{
			Item.damage = 14;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 42;
			Item.useAnimation = 40;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 2.2f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Gaze>();

            Item.noUseGraphic = true;
			Item.UseSound = LobotomyCorp.WeaponSound("DontWatch");
            Item.noMelee = true;
			Item.autoReuse = true;
		}

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            //On hit, 10% chance to increase sp by 40% for 30 seconds
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.WoodYoyo)
            .AddIngredient(ItemID.Lens, 5)
            .AddIngredient(ItemID.LeadBar, 10)
            .AddTile(Mod, "BlackBox2")
            .Register();
        }
    }
}