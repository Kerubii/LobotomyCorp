using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
    public class TodaysExpression : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Today's Expression");
            Tooltip.SetDefault("Many different expressions are pdamageed on the equipment like patches.\n" +
                               "The inability to show one's face is perhaps a form of shyness.");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 20;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
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
            .AddIngredient(ItemID.Silk, 20)
            .AddIngredient(ItemID.GoldBar, 10)
            .AddIngredient(ItemID.IllegalGunParts)
            .AddTile(Mod, "BlackBox")
            .Register();

            CreateRecipe()
            .AddIngredient(ItemID.Silk, 20)
            .AddIngredient(ItemID.PlatinumBar, 10)
            .AddIngredient(ItemID.IllegalGunParts)
            .AddTile(Mod, "BlackBox")
            .Register();
        }
    }
}
