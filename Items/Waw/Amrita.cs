using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Waw
{
    public class Amrita : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("A cane used by monks for when they travel away from their temple.\n" +
                               "It is commonly used to measure the depth of a body of water or to drive animals or insects away."); */

        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;

            Item.useTime = 42;
            Item.useAnimation = 40;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = 20000;
            Item.rare = ItemRarityID.Purple;
            Item.shootSpeed = 3f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Amrita>();

            Item.noUseGraphic = true;
            Item.UseSound = LobotomyCorp.WeaponSound("Templer");
            Item.noMelee = true;
            Item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //On hit, 10% chance to increase sp by 40% for 30 seconds
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.DynastyWood, 30)
            .AddTile(Mod, "BlackBox2")
            .Register();
        }
    }
}