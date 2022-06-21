﻿using System;
using LobotomyCorp.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class FourthMatchFlameGigaSlash : ModProjectile
	{
		public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Spear");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 126;
            Projectile.height = 126;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 100;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
            Projectile.position += Projectile.velocity * 120;
            if (player.itemAnimation == 1)
                Projectile.Kill();

            if (Projectile.localAI[0] < 12)
            {
                Projectile.localAI[0]++;
                for (int i = 0; i < 8; i++)
                {
                    float distance = Main.rand.NextFloat(1f);
                    float rotation = Main.rand.Next(-240, 240);

                    Vector2 dustPos;
                    dustPos.X = (290 + 40 * distance) * (float)Math.Cos(rotation);
                    dustPos.Y = (45 + 10 * distance) * (float)Math.Sin(rotation);

                    Vector2 dustVel = new Vector2(3f, 0).RotatedBy(dustPos.ToRotation() - 1.57f * Math.Sign(Projectile.velocity.X));
                    dustPos = dustPos.RotatedBy(Projectile.velocity.ToRotation());


                    Dust d = Dust.NewDustPerfect(Projectile.Center + dustPos, DustID.Torch, dustVel);

                    if (Main.rand.Next(3) < 2)
                    {
                        d.fadeIn = Main.rand.NextFloat(0.8f, 1.8f);
                        d.noGravity = true;
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            /*
            if (target.HasBuff(ModContent.BuffType<Buffs.Matchstick>()))
                target.buffTime[target.FindBuffIndex(ModContent.BuffType<Buffs.Matchstick>())] += 300;
            else
                target.AddBuff(ModContent.BuffType<Buffs.Matchstick>(), 300);*/

            target.AddBuff(BuffID.OnFire, 300); 
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            /*
            if (target.HasBuff(ModContent.BuffType<Buffs.Matchstick>()))
                target.buffTime[target.FindBuffIndex(ModContent.BuffType<Buffs.Matchstick>())] += 300;
            else
                target.AddBuff(ModContent.BuffType<Buffs.Matchstick>(), 300);*/

            target.AddBuff(BuffID.OnFire, 300);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = -1; i < 3; i++)
            {
                if (i == 0)
                    continue;

                float rotation = Projectile.velocity.ToRotation();
                Vector2 center = new Vector2(projHitbox.X, projHitbox.Y) + new Vector2(Projectile.width * i,0).RotatedBy(rotation);

                if (new Rectangle((int)center.X, (int)center.Y, Projectile.width, Projectile.height).Intersects(targetHitbox))
                    return true;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            CustomShaderData shader = LobotomyCorp.LobcorpShaders["FourthMatchFlame"].UseOpacity(0.5f * (float)Math.Cos(3.15f * ((float)player.itemAnimation/(float)player.itemAnimationMax)) + 0.5f);

            int dir = Math.Sign(Projectile.velocity.X);
            SlashTrail trail = new SlashTrail(180, 45, 0);
            trail.color = Color.Red;
            trail.DrawEllipse(Projectile.Center, Projectile.velocity.ToRotation(), 1.47f * dir - 0.698132f * (float)Math.Sin(1.57f * (1 - ((float)player.itemAnimation / (float)player.itemAnimationMax))) * dir, dir, 400, 75, 128, shader);

            return false;
        }
    }

}
