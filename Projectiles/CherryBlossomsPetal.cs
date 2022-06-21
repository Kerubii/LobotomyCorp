using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class CherryBlossomsPetal : ModProjectile
	{
		public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Spear");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 120;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
        }    
            
        public override void AI() {
            if (Projectile.ai[0] == 0)
                Projectile.rotation = Main.rand.NextFloat(MathHelper.ToRadians(360));
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 10)
                Projectile.velocity *= 0.98f;
            Projectile.rotation += MathHelper.ToRadians(4) * ((float)Projectile.velocity.Length() / 14f);
            if (Projectile.timeLeft < 50)
                Projectile.alpha += 5;

            if (Main.rand.Next(5) == 0)
            {
                Dust dust;
                dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 205, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust;
                dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 205, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }
        }
    }
}
