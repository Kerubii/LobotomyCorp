using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Zayin
{
    public class Soda : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("A pistol painted in a refreshing purple.\n" +
                               "Whenever this E.G.O. is used, a faint scent of grapes wafts through the air."); */
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 20;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = LobotomyCorp.WeaponSounds.Gun;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.shootSpeed = 14f;
            Item.useAmmo = AmmoID.Bullet;
            Item.scale = 0.8f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(RecipeGroupID.IronBar, 10)
            .AddIngredient(ItemID.LesserHealingPotion)
            .AddIngredient(ItemID.LesserManaPotion)
            .AddIngredient(ItemID.BottledHoney)
            .AddTile(Mod, "BlackBox")
            .Register();
        }
    }
}
