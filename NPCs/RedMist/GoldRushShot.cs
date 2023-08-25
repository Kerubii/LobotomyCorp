using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace LobotomyCorp.NPCs.RedMist
{
    class GoldRushShot : ModProjectile
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
