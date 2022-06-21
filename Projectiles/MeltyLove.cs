using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class MeltyLove : ModProjectile
	{
		public override void SetStaticDefaults() {
            DisplayName.SetDefault("Gunk");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 300;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            if (Projectile.localAI[1]++ == 2)
            {
                Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 251, -Projectile.velocity.X/2, -Projectile.velocity.Y/2    )].noGravity = true;
                Projectile.localAI[1] = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 251);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Slow>(), 300);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                tex.Frame(),
                lightColor,
                Projectile.rotation,
                tex.Size() / 2 + new Vector2(4, 0),
                Projectile.scale,
                0,
                0);
            return false;
        }
    }
}
