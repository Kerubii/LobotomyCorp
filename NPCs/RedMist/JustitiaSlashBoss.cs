using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Chat;
using Terraria.GameContent;
using LobotomyCorp;


namespace LobotomyCorp.NPCs.RedMist
{
    //[AutoloadBossHead]
	class JustitiaSlashBoss : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Justitia");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1200;

            Projectile.extraUpdates = 2;
            Projectile.hostile = true;
            Projectile.friendly = false;
        }

        public override void AI()
        {
            if (Projectile.velocity.Length() < 24)
            {
                Projectile.velocity *= 1.01f;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 230)];
                d.noGravity = true;

            }

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= 4)
                    Projectile.frame = 0;
            }    
        }
        
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            float pale = (50f + Main.rand.Next(21)) / 100f;
            modifiers.SourceDamage.Base = (int)((float)target.statLifeMax2 * pale);// + target.statDefense / 2);
            modifiers.SourceDamage /= 2;
            if (Main.expertMode)
                modifiers.SourceDamage /= 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float rotation = Projectile.rotation;
            if (Projectile.spriteDirection == -1)
            {
                rotation += 3.14f;
            }
            Rectangle frame = TextureAssets.Projectile[Projectile.type].Frame();
            //frame.Height /= 4;
            //frame.Y = frame.Height * Projectile.frame;
            Vector2 origin = frame.Size()/2;
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value,
                             Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
                             frame,
                             lightColor,
                             rotation,
                             origin,
                             Projectile.scale,
                             Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                             0);
            return false;
		}
	}
}
