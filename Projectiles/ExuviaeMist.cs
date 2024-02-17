using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class ExuviaeMist: ModProjectile
	{
		public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Spear");
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 60;
            Projectile.alpha = 110;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
        }    
            
        public override void AI() {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0]++;
                Projectile.frame += Main.rand.Next(2);
                Projectile.rotation = Main.rand.NextFloat(3.14f);
            }
            if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 4;
            }
            Projectile.rotation += 0.008f * Math.Sign(Projectile.velocity.X);

            if (Main.rand.NextBool(15))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenMoss);
                Main.dust[d].noGravity = true;
                Main.dust[d].fadeIn = 1.2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 300);
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}
