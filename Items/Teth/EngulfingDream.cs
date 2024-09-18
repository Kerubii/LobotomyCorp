using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Teth
{
    public class EngulfingDream : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("We must be awake at all times.\n" +
                               "Not even sweet dreams in a sound sleep are allowed here; this weapon shall wake those who swim in such illusions.\n" +
                               "And when the crying stops, dawn will break."); */
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Magic; ;
            Item.mana = 3;
            Item.width = 26;
            Item.height = 20;
            Item.useTime = 9;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = LobotomyCorp.WeaponSound("Dreamy", false, 2);
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.EngulfingDreamCall>();
            Item.shootSpeed = 0.1f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(8, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Cloud, 30)
            .AddIngredient(ItemID.Feather)
            .AddIngredient(ItemID.FallenStar, 2)
            .AddTile(Mod, "BlackBox")
            .Register();
        }
    }
}
