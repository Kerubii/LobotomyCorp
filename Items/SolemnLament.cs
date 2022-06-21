using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class SolemnLament : ModItem
	{
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The somber design is a reminder that not a sliver of frivolity is allowed for the minds of those who mourn.\n" +
                               "One handgun symbolizes grief for the dead, while the other symbolizes early lament for the living.\n" +
                               "Switches between range and magic depending on the gun used");
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
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.UseSound = SoundID.Item11;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.DamageType = DamageClass.Magic;
                Item.shoot = 10;
                Item.mana = 8;
                TextureAssets.Item[Item.type] = Mod.Assets.Request<Texture2D>("Items/SolemnLament2");
            }
            else
            {
                Item.DamageType = DamageClass.Ranged;
                Item.shoot = 10;
                Item.mana = 0;
                TextureAssets.Item[Item.type] = Mod.Assets.Request<Texture2D>("Items/SolemnLament1");
            }
            return base.CanUseItem(player);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color ItemColor, Vector2 origin, float scale)
        {
            position = position - (TextureAssets.InventoryBack.Size() * Main.inventoryScale) / 2f + frame.Size() * scale / 2f;
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/SolemnLament").Value;
            frame = tex.Frame();
            scale = 1f;
            if (frame.Width > 32 || frame.Height > 32)
                scale = ((frame.Width <= frame.Height) ? (32f / (float)frame.Height) : (32f / (float)frame.Width));
            scale *= Main.inventoryScale;
            position = position + (TextureAssets.InventoryBack.Size() * Main.inventoryScale) / 2f - frame.Size() * scale / 2f;
            origin = frame.Size() * (1f / 2f - 0.5f);
            spriteBatch.Draw(tex, position, frame, drawColor, 0, origin, scale, 0, 0);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/SolemnLament").Value;
            spriteBatch.Draw(tex, Item.position - Main.screenPosition + new Vector2(Item.width / 2, Item.height - tex.Height / 2), tex.Frame(), lightColor, rotation, tex.Size() / 2, scale, 0, 0);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }

        public override void AddRecipes() {
            CreateRecipe()
            .AddIngredient(ItemID.IllegalGunParts)
            .AddIngredient(ItemID.SilverDye, 3)
            .AddIngredient(ItemID.BlackDye, 3)
            .AddRecipeGroup("LobotomyCorp:Butterflies", 5)
            .AddTile(Mod, "BlackBox2")
            .Register();
        }
	}
}