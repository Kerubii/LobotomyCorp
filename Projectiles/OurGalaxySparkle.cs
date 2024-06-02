using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class OurGalaxySparkle : ModProjectile
	{
        public override string Texture => "Terraria/Images/Extra_57";

        public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Spear");
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = -1;
            //Projectile.aiStyle = 6;
            //AIType = 10;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 30;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.95f;
            if (Projectile.ai[1] == 0)
            {
                Projectile.localAI[0] = Main.rand.NextFloat(0.7f, 0.2f);
                Projectile.scale = Projectile.localAI[0] * (1f + Projectile.ai[0]);
                Projectile.ai[1]++;

                int num954 = 10 + 10;
                int num966 = 5;
                for (int num977 = 0; num977 < num966; num977++)
                {
                    Dust dust209 = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, num954, Projectile.velocity.X, Projectile.velocity.Y, 50)]; 
                }
            }

            if (Projectile.timeLeft < 15)
            {
                Projectile.scale *= 0.9f;
            }
            else
                Projectile.scale += 0.05f;
        }
    }
}
