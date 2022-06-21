//css_ref ../../tModLoader.dll
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Misc.Dusts
{
	public class NoteDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
            dust.noLight = true;
			dust.noGravity = false;
            //dust.fadeIn = 1f;
            dust.scale = 0.5f;
			dust.frame = new Rectangle(0, Main.rand.Next(4) * 22, 22, 22);
            dust.rotation = Main.rand.NextFloat(-0.87f, 0.13f);
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
            if (dust.fadeIn > 0f && dust.fadeIn < 100f)
            {
                if (dust.scale < dust.fadeIn)
                    dust.scale += 0.1f;
                else
                    dust.fadeIn = 0;
            }
            else
                dust.alpha += 10;
            if (dust.alpha >= 255 || dust.scale <= 0)
                dust.active = false;

            if (dust.position.Y > Main.screenPosition.Y + (float)Main.screenHeight)
            {
                dust.active = false;
            }
            return false;
        }
    }
}