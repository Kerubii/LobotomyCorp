//css_ref ../../tModLoader.dll
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Misc.Dusts
{
	public class HornetDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
            dust.noLight = true;
			dust.noGravity = true;
            dust.fadeIn = 1f;
			dust.frame = new Rectangle(0, Main.rand.Next(3) * 10, 8, 8);
			//If our texture had 2 different dust on top of each other (a 30x60 pixel image), we might do this:
			//dust.frame = new Rectangle(0, Main.rand.Next(2) * 30, 30, 30);
		}
		
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
            Color color = Color.Yellow;
            color.A = (byte)(color.A * 0.3f);
            return !dust.noLight ? color : lightColor;
		}

        public override bool Update(Dust dust)
        {
            if (!dust.noLight)
                dust.frame.Y = 0;

            if (!dust.noGravity)
            {
                dust.velocity.Y += 0.05f;
            }
            dust.velocity = dust.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-10, 11)));

            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.05f;
            if (dust.fadeIn > 0f && dust.fadeIn < 100f)
            {
                dust.scale += 0.03f;
                if (dust.scale > dust.fadeIn)
                {
                    dust.fadeIn = 0;
                }
            }
            else
                dust.scale -= 0.03f;
            if (dust.scale <= 0)
                dust.active = false;
            if (dust.position.Y > Main.screenPosition.Y + (float)Main.screenHeight)
            {
                dust.active = false;
            }
            return false;
        }
    }
}