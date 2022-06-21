using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class HarmonyShot : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 1;
			Projectile.scale = 1f;

            Projectile.alpha = 255;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
		}

		public override void AI() {
			for (int i = 0; i < 3; i++)
            {
				int n = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Misc.Dusts.NoteDust>());
				Main.dust[n].scale = 0.5f;
				Main.dust[n].noGravity = true;
			}
			if (Main.rand.Next(3) == 0)
			{
				int i = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Misc.Dusts.NoteDust>());
				Main.dust[i].scale = 1f;
				Main.dust[i].noGravity = true;
			}
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(target.position, target.width, target.height, ModContent.DustType<Misc.Dusts.NoteDust>())].velocity.Y -= 1f;
			}
		}
    }
}
