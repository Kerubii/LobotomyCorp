using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles.KingPortal
{
    public class GoldRushRedMistImpact : ModProjectile
    {
        public override string Texture => "LobotomyCorp/Projectiles/GoldRushPunches";

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 5;

            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.localAI[0] == 0)
            {
                GoldRushHold.DiamondDust(Projectile.Center, DustID.GoldCoin, 6, 4, 10, 1.3f, Projectile.velocity.ToRotation());
                Projectile.localAI[0]++;
            }
            for (int i = 0; i < 16; i++)
            {
                Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin)];
                d.noGravity = true;
                d.fadeIn = 1.5f;
                d.velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f));
            }
        }
    }

	public class GoldRushRedMist : ModProjectile
	{
        public override string Texture => "LobotomyCorp/Projectiles/GoldRushPunches";

        public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Gold Rush");
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 120000;

            Projectile.tileCollide = false;
            Projectile.hostile = true;
        }    
            
        public override void AI() {
            Projectile.scale = 2f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.ai[1] > Projectile.ai[0] && Projectile.timeLeft > 200)
                Projectile.timeLeft = 200;
            if (Projectile.timeLeft % 5 == 0)
                GoldRushHold.DiamondDust(Projectile.position + new Vector2(-16 + Main.rand.Next(32), -16 + Main.rand.Next(32)), DustID.Enchanted_Gold, 4, 5, 5);

            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Enchanted_Gold);
            Main.dust[d].noGravity = true;
            Main.dust[d].velocity *= 0.2f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = tex.Frame();
            Vector2 origin = frame.Size();
            origin.X -= Projectile.width / 2;
            origin.Y = Projectile.height / 2;

            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation + MathHelper.ToRadians(45), origin, Projectile.scale, 0f, 0);
            return false;
        }
    }
}
