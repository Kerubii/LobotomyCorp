//css_ref ../../tModLoader.dll
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Misc.Dusts
{
	public class BlackFeather : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
            dust.noLight = true;
			dust.noGravity = true;
            //dust.fadeIn = 1.2f;
			dust.frame = new Rectangle(0, 0, 12, 12);
            dust.rotation = Main.rand.NextFloat(6.28f);
            dust.scale *= Main.rand.NextFloat(0.75f, 1.2f);
		}

        public override bool Update(Dust dust)
        {
            if (!dust.noGravity)
            {
                dust.velocity.Y += 0.15f;
            }

            dust.velocity *= 0.95f;

            float rot = dust.velocity.Length() / 12f;
            rot = MathHelper.Clamp(rot, 0.1f, 1);
            dust.rotation += rot * 0.2f;
            dust.position += dust.velocity;
            if (dust.fadeIn > 0f && dust.fadeIn < 100f)
            {
                if (dust.scale < dust.fadeIn)
                    dust.scale += 0.1f;
                else
                    dust.fadeIn = 0;
            }
            else if (dust.velocity.LengthSquared() < 1f)
            {
                dust.alpha += 10;
                dust.scale -= 0.01f;
            }
            if (dust.alpha >= 255 || dust.scale <= 0)
                dust.active = false;

            return false;
        }
    }
}