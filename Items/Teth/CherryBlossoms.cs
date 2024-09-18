using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Teth
{
    public class CherryBlossoms : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Magic; ;
            Item.mana = 4;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 2.4f;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = LobotomyCorp.WeaponSound("sakura");
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.CherryBlossomsPetal>();
            Item.shootSpeed = 14f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                damage = (int)(damage * 0.6f);
                for (int i = 0; i < 3; i++)
                {
                    Vector2 speed = velocity.RotatedByRandom(MathHelper.ToRadians(15));
                    Projectile.NewProjectile(source, position, speed, type, damage, knockback, player.whoAmI);
                }
            }
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust dust;
            dust = Main.dust[Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Misc.Dusts.BlossomDust>(), 0f, 0f, 0, new Color(255, 255, 255), 1f)];
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Acorn, 3)
            .AddIngredient(ItemID.DynastyWood, 20)
            .AddTile(Mod, "BlackBox")
            .Register();
        }
    }
}