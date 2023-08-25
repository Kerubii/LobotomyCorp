using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using LobotomyCorp.Projectiles;

namespace LobotomyCorp.Items.Aleph
{
    public class Adoration : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("A big mug filled with mysterious slime that never runs out.\n" +
                               "ItÅ's the byproduct of some horrid experiment in a certain laboratory that eventually failed.\n" +
                               "Inflicts Slow"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 48;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.width = 40;
            Item.height = 16;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4;
            Item.value = 8000;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = LobotomyCorp.WeaponSound("Slime");
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MeltyLove>();
            Item.shootSpeed = 7.6f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Gel, 300)
            .AddIngredient(ItemID.PinkGel, 20)
            .AddIngredient(ItemID.AdamantiteBar, 10)
            .AddIngredient(ItemID.Bottle)
            .AddTile<Tiles.BlackBox3>()
            .Register();

            CreateRecipe()
            .AddIngredient(ItemID.Gel, 300)
            .AddIngredient(ItemID.PinkGel, 20)
            .AddIngredient(ItemID.TitaniumBar, 10)
            .AddIngredient(ItemID.Bottle)
            .AddTile<Tiles.BlackBox3>()
            .Register();
        }
    }
}
