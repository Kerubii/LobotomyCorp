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
	public class GoldRushRedMist : ModProjectile
	{
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
            if (Projectile.ai[1] > Projectile.ai[0])
                Projectile.timeLeft = 200;

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
