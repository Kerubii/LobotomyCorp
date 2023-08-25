using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection.Metadata;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
    /// <summary>
    /// Negative ai0 for arc type, Positive ai0 for Shotgun type. Number is how many bits it releases. 0 is none
    /// ai1 controls speed bobs release
    /// ai2 target, if target exists Gravity is enabled
    /// </summary>
    internal class SmileBobs : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Smile");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 30;
            Projectile.scale = 0.5f;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.frame = Main.rand.Next(3);
            }
            Projectile.rotation += 0.01f;

            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Wraith);
                Main.dust[d].noGravity = true;
            }

            if (Projectile.ai[2] >= 0)
            {
                Projectile.velocity.Y += 0.02f;
            }

            if (Projectile.timeLeft < 10)
                Projectile.scale -= 0.025f;
        }

        public override bool CanHitPlayer(Player target)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(1f, 2f), 0).RotateRandom(6.28f);
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Wraith, vel.X, vel.Y);
                Main.dust[d].noGravity = true;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Vector2 delta = Projectile.velocity;
            if ((int)Projectile.ai[2] >= 0)
            {
                Player player = Main.player[(int)Projectile.ai[2]];
                if (player.active && !player.dead)
                {
                    delta = player.Center - Projectile.Center;
                }
            }
            delta.Normalize();
            // Arc type
            if (Projectile.ai[0] < 0)
            {
                int amount = (int)(Projectile.ai[0] * -1f);

                for (int i = 0; i < amount; i++)
                {
                    float angle = MathHelper.ToRadians(-45 + 90 * (i / (amount - 1)));
                    Vector2 vel = (delta * Projectile.ai[0]).RotatedBy(angle);

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, ModContent.ProjectileType<SmileBits>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
            // Random hotgun type
            else if (Projectile.ai[0] > 0)
            {
                int amount = (int)(Projectile.ai[0]);

                for (int i = 0; i < amount; i++)
                {
                    float speed = Projectile.ai[1] * Main.rand.NextFloat(-0.8f, 1.2f);
                    Vector2 vel = (delta * Projectile.ai[0]).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45)));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, ModContent.ProjectileType<SmileBits>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
        }
    }
}
