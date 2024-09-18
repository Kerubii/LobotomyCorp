using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class TwilightTeleport : ModProjectile
	{
		public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 4;
        }

		public override void SetDefaults() {
			Projectile.width = 150;
			Projectile.height = 150;
			Projectile.aiStyle = -1;
			Projectile.scale = 1f;
            Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
		}

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.frameCounter = Main.rand.Next(10 * 3);
                Projectile.localAI[1] = Main.rand.NextFloat(3.14f);
                Projectile.localAI[0]++;
            }

            if (Projectile.ai[0] == 0)
            {
                if (Projectile.ai[1] == 0)
                {
                    Projectile.scale = 0f;
                }

                if (Projectile.ai[1] > 30)
                {
                    Projectile.alpha += 25;
                    Projectile.scale -= 0.01f;
                    if (Projectile.alpha > 255)
                        Projectile.Kill();
                }
                else
                {
                    if (Projectile.scale < 1.5f)
                        Projectile.scale += 0.075f;
                }

                Projectile.ai[1]++;
            }
            else
            {
                if (Projectile.ai[1] == 0)
                {
                    Projectile.scale = 1.5f;
                    Projectile.alpha = 255;
                }

                if (Projectile.ai[1] > 30)
                {
                    if (Projectile.scale > 0)
                        Projectile.scale -= 0.15f;
                    if (Projectile.scale <= 0)
                        Projectile.Kill();
                }
                else
                {
                    if (Projectile.alpha > 0)
                        Projectile.alpha -= 25;
                    if (Projectile.alpha < 0)
                        Projectile.alpha = 0;
                }

                Projectile.ai[1]++;
            }

            Projectile.frameCounter++;
            int frames = 6;
            if (Projectile.frameCounter > frames * 3)
            {
                Projectile.frameCounter = 0;
            }
            Projectile.frame = (int)(Projectile.frameCounter / (float)frames);
            Projectile.rotation = Projectile.localAI[1] + 1.57f / 2 * Projectile.frame;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
