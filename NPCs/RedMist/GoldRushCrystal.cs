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
    class GoldRushCrystal : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.tileCollide = true;

            Projectile.hostile = true;
            Projectile.timeLeft = 30;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.9f;
            Projectile.rotation = Projectile.ai[0] + 1.57f;
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
            Main.EntitySpriteDraw(tex, pos, frame, lightColor, Projectile.rotation, origin, Projectile.scale, 0, 0);

            if (Projectile.timeLeft < 15)
            {
                float scale = Projectile.scale + 1f - 0.8f * (float)Math.Sin(1.57f + 1.57f *  Projectile.timeLeft / 15f);

                Color bright = Color.White;
                bright.A = 150;
                bright *= 0.9f * (1f - Projectile.timeLeft / 15f);

                Main.EntitySpriteDraw(tex, pos, frame, bright, Projectile.rotation, origin, scale, 0, 0);
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 vel = new Vector2(4f, 0).RotatedBy(Projectile.ai[0]);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel * 3, ModContent.ProjectileType<GoldRushShot>(), Projectile.damage, 0);
            }
        }
    }

    class   GoldRushShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 24;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;

            Projectile.extraUpdates = 4;
            Projectile.hostile = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90); 
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = tex.Size() / 2;
            Vector2 pos = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
            Rectangle frame = tex.Frame();
            Color Bright = Color.White;
            Bright.A = 200;

            for (int i = 0; i < 24; i++)
            {
                pos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
                float scale = 1.2f - 0.6f * i / 24f;
                Color Bright2 = Bright * 0.4f * (1f - i / 24f);
                Main.EntitySpriteDraw(tex, pos, frame, Bright2, Projectile.rotation, origin, Projectile.scale * scale, 0, 0);
            }
            pos = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
            Main.EntitySpriteDraw(tex, pos, frame, lightColor, Projectile.rotation, origin, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(tex, pos, frame, Bright * 0.8f, Projectile.rotation, origin, Projectile.scale + .2f, 0, 0);
            return false;
        }
    }
}
