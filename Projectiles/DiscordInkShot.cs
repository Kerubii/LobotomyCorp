using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static LobotomyCorp.LobotomyCorp;

namespace LobotomyCorp.Projectiles
{
    public class DiscordInkShot : ModProjectile
    {
        public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Spear");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults() {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 6;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            float size = Projectile.timeLeft / 5f;

            for (int i = 0; i < 4; i++)
            {
                Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Wraith)];
                d.noGravity = true;
                d.color = Color.Black;
                d.fadeIn = 1.2f * size;
                d.scale = 2;
                d.velocity *= 0;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 2)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
            }
        }
    }
}
