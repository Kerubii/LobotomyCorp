using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class SyrinxSound: ModProjectile
	{
		public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Spear");
        }

        public override void SetDefaults()
        {
            Projectile.width = 96;
            Projectile.height = 96;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 8;
            Projectile.scale = 20f / 180f;
            Projectile.timeLeft = 160;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.friendly = true;

            Projectile.alpha = 30;
        }    
            
        public override void AI() {
            // Vary the sound waves size
            Projectile.scale += 1f / 180f * Projectile.ai[0];

            if (Projectile.timeLeft < 10f)
                Projectile.alpha += 25;

            Projectile.rotation = Projectile.velocity.ToRotation();

            int size = (int)(96 * Projectile.scale);
            int size2 = size / 2;
            Projectile.width = size;
            Projectile.height = size;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int size = (int)(96 * Projectile.scale);
            int size2 = size / 2;
            hitbox.X = (int)Projectile.Center.X - size2;
            hitbox.Y = (int)Projectile.Center.Y - size2;
            hitbox.Width = size;
            hitbox.Height = size;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Projectile.position, 10, 10, 36);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, tex.Frame(), lightColor * (1f - Projectile.alpha / 255f), Projectile.rotation, tex.Size() / 2, Projectile.scale, 0, 0);

            return false;
        }
    }
}
