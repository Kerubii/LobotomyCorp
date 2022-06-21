//css_ref ../../tModLoader.dll
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Misc.Dusts
{
	public class BlossomDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
            dust.noLight = true;
			dust.noGravity = true;
            dust.fadeIn = 0.2f;
			dust.frame = new Rectangle(0, 0, 10, 16);
		}
    }
}