using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.Audio;
using System.Collections.Generic;

namespace LobotomyCorp.Items.Waw
{
    public class CrimsonScar : LobCorpLight
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("With steel in one hand and gunpowder in the other, there's nothing to fear in this place.\n" +
                               "It's more important to deliver a decisive strike in blind hatred without hesitation than to hold on to insecure courage.\n" +
                               "50% increased damage under 50% health"); */
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        /*
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var x = tooltips.Find(x => x.Name == "Damage");
            x.Text = 

            base.ModifyTooltips(tooltips);
        }*/

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 3000;
            Item.rare = ItemRarityID.Purple;
            Item.damage = 58;
            Item.knockBack = 3f;
            Item.shootSpeed = 12f;
            Item.shoot = 10;
            Item.useAmmo = AmmoID.Bullet;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage.CombineWith(player.GetDamage(DamageClass.Ranged));
            if (player.statLife <= player.statLifeMax / 2)
            {
                damage += 0.5f;
            }
        }
        
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                return 1.2f;
            }
            return base.UseSpeedMultiplier(player);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                //Item.DamageType = DamageClass.Ranged;
                Item.shoot = 10;
                Item.useTime = 36;
                Item.useAnimation = 36;
                Item.useStyle = ItemUseStyleID.Shoot;
                TextureAssets.Item[Item.type] = Mod.Assets.Request<Texture2D>("Items/Waw/CrimsonScarGun");
                Item.noMelee = true;
                Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/RedHood_Gun") with { Volume = 0.5f, PitchVariance = 0.1f };
            }
            else
            {
                //Item.DamageType = DamageClass.Melee;
                Item.shoot = 0;
                Item.useTime = 18;
                Item.useAnimation = 18;
                Item.useStyle = 15;
                TextureAssets.Item[Item.type] = Mod.Assets.Request<Texture2D>("Items/Waw/CrimsonScarScythe");
                Item.noMelee = false;
                Item.UseSound = SoundID.Item1;
                Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/RedHood_Atk1") with { Volume = 0.5f, PitchVariance = 0.1f };
            }
            return base.CanUseItem(player);
        }

        public override void UseStyleAlt(Player player, Rectangle heldItemFrame)
        {
            if (player.altFunctionUse != 2)
                Item.useStyle = 15;
            base.UseStyleAlt(player, heldItemFrame);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color ItemColor, Vector2 origin, float scale)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/Waw/CrimsonScar").Value;
            Rectangle texFrame = tex.Frame();
            scale = 1f;
            float num = 1f;
            if ((float)texFrame.Width > 32f || (float)texFrame.Height > 32f)
            {
                num = ((texFrame.Width <= texFrame.Height) ? (32f / (float)texFrame.Height) : (32f / (float)texFrame.Width));
            }

            spriteBatch.Draw(tex, position, texFrame, drawColor, 0f, texFrame.Size() / 2, scale * num, 0, 0);
            return false;
            /*
            position = position - TextureAssets.InventoryBack.Value.Size() * Main.inventoryScale / 2f + frame.Size() * scale / 2f;
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/Waw/CrimsonScar").Value;
            frame = tex.Frame();
            scale = 1f;
            if (frame.Width > 32 || frame.Height > 32)
                scale = frame.Width <= frame.Height ? 32f / frame.Height : 32f / frame.Width;
            scale *= Main.inventoryScale;
            position = position + TextureAssets.InventoryBack.Value.Size() * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            origin = frame.Size() * (1f / 2f - 0.5f);
            spriteBatch.Draw(tex, position, frame, drawColor, 0, origin, scale, 0, 0);
            return false;*/
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/Waw/CrimsonScar").Value;
            spriteBatch.Draw(tex, Item.position - Main.screenPosition + new Vector2(Item.width / 2, Item.height - tex.Height / 2), tex.Frame(), lightColor, rotation, tex.Size() / 2, scale, 0, 0);
            return false;
        }

        public override bool CanShoot(Player player)
        {
            return player.altFunctionUse == 2;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            damage = (int)(damage * 0.7f);
            knockback *= 0.7f;
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
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