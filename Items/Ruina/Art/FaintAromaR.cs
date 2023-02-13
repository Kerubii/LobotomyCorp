using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Ruina.Art
{
    [Autoload(LobotomyCorp.TestMode)]
    public class FaintAromaS : SEgoItem
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("\"Bearing the hope to return to dust, it shall go back to the grave with all that desires to live.\"");

            EgoColor = LobotomyCorp.WawRarity;
        }

		public override void SetDefaults() 
		{
            PassiveText = "Autumn's Passing - Hitting enemies or charging up grants a petal that dissapears overtime.\n" +
              "Spring's Genesis - Each petal enhances your normal attacks and damage\n" +
              "Magnificent End - Hitting an enemy with 3 petals removes them deal 300% of your weaponÅ's damage to all enemies around you\n" +
              "Doll Fashioned from the Earth - Negate any negative life regen while a petal is active\n" +
              "|Winter's Inception - 10% increased damage taken from enemies with increased 10% per petals\n" +
              "This Item is incomplete and unobtainable";

            Item.damage = 40;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.knockBack = 0;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.AlriuneDeathAnimation>();
            Item.shootSpeed = 1f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            /*foreach (NPC n in Main.npc)
            {
                if (n.active && n.chaseable && n.CanBeChasedBy(ModContent.ProjectileType<Projectiles.AlriuneDeathAnimation")) && (n.Center - player.Center).Length() < 800)
                    Projectile.NewProjectile(n.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.AlriuneDeathAnimation"), Item.damage, 0, player.whoAmI, n.whoAmI);
            }*/
            if (Main.myPlayer == player.whoAmI)
                Projectile.NewProjectile(source, position, velocity, type, damage, 0, player.whoAmI, (int)(LobotomyModPlayer.ModPlayer(player).FaintAromaPetal/LobotomyModPlayer.ModPlayer(player).FaintAromaPetalMax) - 1);
            return false;
        }

        public override bool SafeCanUseItem(Player player)
        {
            if (player.altFunctionUse != 2)
            {
                Item.useTime = 26;
                Item.useAnimation = 26;
                if (LobotomyModPlayer.ModPlayer(player).FaintAromaPetal > LobotomyModPlayer.ModPlayer(player).FaintAromaPetalMax)
                {
                    Item.shoot = ModContent.ProjectileType<Projectiles.FaintAromaS>();
                    Item.useStyle = 5;
                    Item.noUseGraphic = true;
                    Item.noMelee = true;
                    switch ((int)(LobotomyModPlayer.ModPlayer(player).FaintAromaPetal / LobotomyModPlayer.ModPlayer(player).FaintAromaPetalMax))
                    {
                        case 1:
                            Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/Ali_Sub_Atk");
                            break;
                        case 2:
                            Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/Ali_StrongAtk");
                            break;
                        case 3:
                            Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/Ali_StrongAtk_Finish");
                            break;
                        default:
                            Item.UseSound = SoundID.Item1;
                            break;
                    }
                }
                else
                {
                    Item.shoot = 0;
                    Item.useStyle = 1;
                    Item.noUseGraphic = false;
                    Item.noMelee = false;
                    Item.UseSound = SoundID.Item1;
                }
                Item.shootSpeed = 1f;
            }
            else
            {
                Item.useTime = 2;
                Item.useAnimation = 2;
                Item.shoot = 0;
                Item.shootSpeed = 0;
                Item.useStyle = ItemUseStyleID.HoldUp;
                Item.noUseGraphic = true;
                Item.noMelee = true;
                Item.UseSound = SoundID.Item1;
            }
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.altFunctionUse == 2 && LobotomyModPlayer.ModPlayer(player).FaintAromaPetal < LobotomyModPlayer.ModPlayer(player).FaintAromaPetalMax * 3 + 30)
            {
                LobotomyModPlayer.ModPlayer(player).FaintAromaPetal += 1f + LobotomyModPlayer.ModPlayer(player).FaintAromaDecay * 2;
                if (LobotomyModPlayer.ModPlayer(player).FaintAromaPetal > LobotomyModPlayer.ModPlayer(player).FaintAromaPetalMax * 3 + 30)
                    LobotomyModPlayer.ModPlayer(player).FaintAromaPetal = LobotomyModPlayer.ModPlayer(player).FaintAromaPetalMax * 3 + 30;
            }
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            LobotomyModPlayer.ModPlayer(player).FaintAromaPetal += 30f;
            if (LobotomyModPlayer.ModPlayer(player).FaintAromaPetal > LobotomyModPlayer.ModPlayer(player).FaintAromaPetalMax * 3 + 30)
                LobotomyModPlayer.ModPlayer(player).FaintAromaPetal = LobotomyModPlayer.ModPlayer(player).FaintAromaPetalMax * 3 + 30;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color ItemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Items/Ruina/Art/FaintAromaSDisplay").Value, position, frame, drawColor, 0, origin, scale, 0, 0);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/Ruina/Art/FaintAromaSDisplay").Value;
            spriteBatch.Draw(tex, Item.position - Main.screenPosition + new Vector2(Item.width/2, Item.height - tex.Height/2), tex.Frame(), lightColor, rotation, tex.Size()/2, scale, 0, 0);
            return false;
        }

        public override void AddRecipes() 
		{
		}
	}
}