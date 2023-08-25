using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Chat;
using LobotomyCorp;
using Terraria.GameContent;

namespace LobotomyCorp.NPCs.RedMist
{
    class RedEyesEgg : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 20;
            Projectile.tileCollide = true;

            Projectile.hostile = true;
            Projectile.timeLeft = 60;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(10f);
        }

        public override bool CanHitPlayer(Player target)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 pos = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
            Vector2 origin = tex.Size() / 2;
            Rectangle frame = tex.Frame();
            Main.EntitySpriteDraw(tex, pos, frame, lightColor, Projectile.rotation, origin, 1f, 0, 0);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 target = Projectile.Center + Projectile.velocity;
                float nearest = -1;
                foreach(Player p in Main.player)
                {
                    if (p.active && !p.dead)
                    {
                        if (nearest == -1)
                        {
                            nearest = p.Distance(Projectile.Center);
                            target = p.Center;
                        }
                        else
                        {
                            float distance = p.Distance(Projectile.Center);
                            if (distance < nearest)
                            {
                                nearest = distance;
                                target = p.Center;
                            }
                        }
                    }
                }
                Vector2 vel = Vector2.Normalize(target - Projectile.Center) * Projectile.velocity.Length();
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel * 2, ModContent.ProjectileType<RedEyesSpider>(), Projectile.damage, 0);
            }
        }
    }

    class RedEyesSpider : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;

            Projectile.hostile = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90); 
        }
    }
}
