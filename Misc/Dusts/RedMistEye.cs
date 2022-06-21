//css_ref ../../tModLoader.dll
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Misc.Dusts
{
	public class RedMistEye : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
            dust.noLight = false;
			dust.noGravity = true;
            dust.fadeIn = 1f;
			dust.frame = new Rectangle(0, 0, 6, 6);
            dust.rotation = Main.rand.NextFloat(6.28f);
			//If our texture had 2 different dust on top of each other (a 30x60 pixel image), we might do this:
			//dust.frame = new Rectangle(0, Main.rand.Next(2) * 30, 30, 30);
		}

        public override bool Update(Dust dust)
        {
            if (!dust.noGravity)
            {
                dust.velocity.Y += 0.15f;
            }

            dust.velocity *= 0.95f;

            dust.position += dust.velocity;
            if (dust.fadeIn > 0f)
            {
                dust.fadeIn -= 0.01f;
                dust.scale = 1f;
            }
            else
            {
                dust.scale -= 0.01f;
            }

            if (dust.alpha >= 255 || dust.scale <= 0)
                dust.active = false;

            if (!dust.noLight)
                Lighting.AddLight(dust.position, new Vector3(0.01f, 0.00f, 0.00f));

            if (dust.position.Y > Main.screenPosition.Y + (float)Main.screenHeight)
            {
                dust.active = false;
            }
            dust.rotation = 0;
            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White * (1f - dust.alpha / 255f);
        }
    }
}