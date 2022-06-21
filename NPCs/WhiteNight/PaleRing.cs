using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace LobotomyCorp.NPCs.WhiteNight
{
    class PaleRing : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.damage = 100;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.scale = 0;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.scale += 0.03f;
            if (Projectile.timeLeft < 60)
                Projectile.alpha += 15;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 Pos = new Vector2(targetHitbox.X + targetHitbox.Width / 2, targetHitbox.Y + targetHitbox.Height / 2);
            float Distance = Vector2.Distance(Pos, Projectile.Center);
            float length = 110 * Projectile.scale;
            return Distance > length - 8 && Distance < length + 8;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = new Rectangle(0, Projectile.frame, texture.Width, texture.Height);
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height - texture.Width/2);
            Vector2 pos = Projectile.position - Main.screenPosition + Projectile.Size / 2 + new Vector2(0, Projectile.gfxOffY);
            Main.EntitySpriteDraw(
                texture,
                pos,
                new Rectangle?(frame),
                Color.White * ((255 - (float)Projectile.alpha)/255f),
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0);
            return false;
        }
    }
}
