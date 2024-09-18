using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class KaleidoscopeEffect : ModProjectile
	{
		public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Kaleidoscope of Butterflies");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 30;

            Projectile.tileCollide = false;
            Projectile.friendly = false;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.scale = Main.rand.NextFloat(0.7f, 1.1f);
            }

            if (Projectile.ai[0] <= 0)
            {
                Projectile.ai[0] = 1 + Main.rand.Next(2);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

            Projectile.alpha += 10;

            if (Projectile.alpha > 255)
                Projectile.Kill();

            if (Projectile.frameCounter <= 24)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 18)
                    Projectile.frame = 1;
                else
                    Projectile.frame = (int)Math.Floor(Projectile.frameCounter / 6f);
            }
            else
                Projectile.frameCounter = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 position = Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Rectangle frame = tex.Frame(2, Main.projFrames[Projectile.type], Projectile.ai[0] == 1 ? 1 : 0, Projectile.frame);

            Vector2 origin = frame.Size() / 2;

            Main.EntitySpriteDraw(tex, position, frame, lightColor * (1f - Projectile.alpha / 255f), Projectile.rotation, origin, Projectile.scale, Projectile.spriteDirection > 0 ? 0 : SpriteEffects.FlipHorizontally, 0);

            return false;
        }
    }
}
