//css_ref ../../tModLoader.dll
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Misc.Dusts
{
	public class ElectricTrail : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noLight = false;
			dust.frame = new Rectangle(0, 20, 10, 10);
		}

		public override bool MidUpdate(Dust dust)
        {
			float num10 = dust.scale;
			if (num10 > 1f)
			{
				num10 = 1f;
			}
			if (!dust.noLight)
			{
				Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num10 * 1f, num10 * 1f, 0);
			}
			if (dust.noGravity)
			{
				dust.velocity *= 0.93f;
				if (dust.fadeIn == 0f)
				{
					dust.scale += 0.0025f;
				}
			}
			dust.velocity *= new Vector2(0.97f, 0.99f);

			//Janker Dust Trail
			if (dust.customData != null && dust.customData is Dust)
			{
				Dust parentDust = (Dust)dust.customData;
				if (dust.active)
				{
					dust.position = parentDust.position - parentDust.velocity * 2;
					dust.velocity = parentDust.velocity;
				}
			}
			dust.scale -= 0.01f;

			return true;
		}
    }
}