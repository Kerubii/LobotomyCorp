using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using LobotomyCorp.Projectiles.Realized;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static LobotomyCorp.LobotomyCorp;

namespace LobotomyCorp.Projectiles
{
    public class OurGalaxyHeal : ModProjectile
    {
        public override string Texture => "LobotomyCorp/Items/Aleph/Justitia";

        public override void SetDefaults() {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 240;
        }

        public override void AI()
        {
            int targetPlayer = (int)Projectile.ai[0];
            Player target = Main.player[targetPlayer];
            float num1 = 8;
            Vector2 vector = Projectile.Center;
            float targetX = target.Center.X - vector.X;
            float targetY = target.Center.Y - vector.Y;
            float distance = (float)Math.Sqrt(targetX * targetX + targetY * targetY);
            float distance2 = distance;
            if (distance2 < 50f && 
                Projectile.position.X < target.position.X + target.width && Projectile.position.X + Projectile.width > target.position.X &&
                Projectile.position.Y < target.position.Y + target.height && Projectile.position.Y + Projectile.height > target.position.Y)
            {
                if (Projectile.owner == Main.myPlayer && !Main.player[Main.myPlayer].moonLeech)
                {
                    int heal = (int)Projectile.ai[1];
                    target.HealEffect(heal, broadcast: false);
                    target.statLife += heal;
                    if (target.statLife > target.statLifeMax2)
                    {
                        target.statLife = target.statLifeMax2;
                    }
                    NetMessage.SendData(66, -1, -1, null, targetPlayer, heal);
                }
                OurGalaxyComet.ApplyStoneBuff(target, Projectile.owner);
                Projectile.Kill();
            }
            distance = num1 / distance;
            targetX *= distance;
            targetY *= distance;
            Projectile.velocity.X = (Projectile.velocity.X * 15f + targetX) / 16f;
            Projectile.velocity.Y = (Projectile.velocity.Y * 15f + targetY) / 16f;
            for (int num440 = 0; num440 < 2; num440++)
            {
                float num441 = Projectile.velocity.X * 0.2f * (float)num440;
                float num442 = (0f - Projectile.velocity.Y * 0.2f) * (float)num440;
                int num443 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.SilverCoin, 0f, 0f, 100, default(Color), 1.3f);
                Main.dust[num443].noGravity = true;
                Dust dust116 = Main.dust[num443];
                Dust dust212 = dust116;
                dust212.velocity *= 0f;
                Main.dust[num443].position.X -= num441;
                Main.dust[num443].position.Y -= num442;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
