using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;

namespace LobotomyCorp.Items
{
	public class CrimsonScar : ModItem
	{
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("With steel in one hand and gunpowder in the other, there's nothing to fear in this place.\n" +
                               "It's more important to deliver a decisive strike in blind hatred without hesitation than to hold on to insecure courage.");
        }

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.value = 3000;
            Item.rare = ItemRarityID.Purple;
			Item.damage = 32;
            Item.shootSpeed = 12f;
            Item.shoot = 10;
            Item.useAmmo = AmmoID.Bullet;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.statLife <= player.statLifeMax / 2)
            {
                damage += 0.5f;    
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.DamageType = DamageClass.Ranged;
                Item.shoot = 10;
                Item.useTime = 36;
                Item.useAnimation = 36;
                Item.useStyle = ItemUseStyleID.Shoot;
                TextureAssets.Item[Item.type] = Mod.Assets.Request<Texture2D>("Items/CrimsonScarGun");
                Item.noMelee = true;
                Item.UseSound = SoundID.Item11;
            }
            else
            {   
                Item.DamageType = DamageClass.Melee;
                Item.shoot = 0;
                Item.useTime = 18;
                Item.useAnimation = 18;
                Item.useStyle = ItemUseStyleID.Swing;
                TextureAssets.Item[Item.type] = Mod.Assets.Request<Texture2D>("Items/CrimsonScarScythe");
                Item.noMelee = false;
                Item.UseSound = SoundID.Item1;
            }
            return base.CanUseItem(player);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color ItemColor, Vector2 origin, float scale)
        {
            position = position - (TextureAssets.InventoryBack.Value.Size() * Main.inventoryScale) / 2f + frame.Size() * scale / 2f;
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/CrimsonScar").Value;
            frame = tex.Frame();
            scale = 1f;
            if (frame.Width > 32 || frame.Height > 32)
                scale = ((frame.Width <= frame.Height) ? (32f / (float)frame.Height) : (32f / (float)frame.Width));
            scale *= Main.inventoryScale;
            position = position + (TextureAssets.InventoryBack.Value.Size() * Main.inventoryScale) / 2f - frame.Size() * scale / 2f;
            origin = frame.Size() * (1f / 2f - 0.5f);
            spriteBatch.Draw(tex, position, frame, drawColor, 0, origin, scale, 0, 0);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/CrimsonScar").Value;
            spriteBatch.Draw(tex, Item.position - Main.screenPosition + new Vector2(Item.width / 2, Item.height - tex.Height / 2), tex.Frame(), lightColor, rotation, tex.Size() / 2, scale, 0, 0);
            return false;
        }

        public override bool CanShoot(Player player)
        {
            return player.altFunctionUse == 2;
        }

        /*public override bool ConsumeAmmo(Player player)
        {
            return player.altFunctionUse != 2;
        }*/

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FlintlockPistol)
            .AddIngredient(ItemID.Sickle)
            .AddIngredient(ItemID.RedHusk, 2)
            .AddTile(Mod, "BlackBox3")
            .Register();
        }
    }
}