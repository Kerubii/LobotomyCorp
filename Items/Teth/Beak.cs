using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Teth
{
    public class Beak : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.knockBack = 4;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 16;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.noMelee = true;
            Item.value = 7500;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = LobotomyCorp.WeaponSounds.Gun;
            Item.autoReuse = true;

            Item.shoot = 10;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FlintlockPistol)
            .AddIngredient(ItemID.Feather, 2)
            .AddTile(Mod, "BlackBox")
            .Register();
        }
    }
}
