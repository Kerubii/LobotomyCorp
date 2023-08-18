﻿using System;
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
            Texture2D tex = ModContent.Request<Texture2D>("LobotomyCorp/Projectiles/JustitiaExtended").Value;
            Texture2D texFlipped = ModContent.Request<Texture2D>("LobotomyCorp/Projectiles/JustitiaExtendedFlip").Value;

            Texture2D brightTex = Projectiles.JustitiaExtended.JustitiaTexture(false);
            Texture2D brighTexFlipped = Projectiles.JustitiaExtended.JustitiaTexture(true);

            Rectangle frame = tex.Frame();
            float originOffset = -60 * Projectile.spriteDirection;
            Vector2 origin = frame.Size() / 2 + originOffset * Vector2.UnitX;
            SpriteEffects spriteEffect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            float YMult = 1f;
            for (int i = 0; i < 3; i++)
            {
                Vector2 oldPos = Projectile.Center - Projectile.velocity * (i + 1) - Main.screenPosition + Projectile.gfxOffY * Vector2.UnitY;
                float opacity = (Projectile.alpha / 255f) * (1f - i / 3f);
                Vector2 scale = Projectile.scale * new Vector2(0.75f, 0.85f) * (1f - i / 3f);
                scale.Y *= YMult;
                if (Projectile.ai[0] == 0 || Projectile.ai[0] == 1)
                    Main.EntitySpriteDraw(tex, oldPos, frame, Color.LightGray * opacity, Projectile.rotation, origin, scale, spriteEffect, 0);
                if (Projectile.ai[0] == 0 || Projectile.ai[0] == 2)
                    Main.EntitySpriteDraw(texFlipped, oldPos, frame, Color.LightGray * opacity, Projectile.rotation, origin, scale, spriteEffect, 0);
            }

            Vector2 position = Projectile.Center - Main.screenPosition + Projectile.gfxOffY * Vector2.UnitY;
            if (Projectile.ai[0] == 0 || Projectile.ai[0] == 1)
                Main.EntitySpriteDraw(tex, position, frame, Color.White * (Projectile.alpha / 255f), Projectile.rotation, origin, Projectile.scale, spriteEffect, 0);
            if (Projectile.ai[0] == 0 || Projectile.ai[0] == 2)
                Main.EntitySpriteDraw(texFlipped, position, frame, Color.White * (Projectile.alpha / 255f), Projectile.rotation, origin, Projectile.scale, spriteEffect, 0);
            if (Projectile.alpha > 85)
            {
                float opacity = (Projectile.alpha - 85) / 170f;
                if (Projectile.ai[0] == 0 || Projectile.ai[0] == 1)
                    Main.EntitySpriteDraw(brightTex, position, frame, Color.White * opacity, Projectile.rotation, origin, Projectile.scale, spriteEffect, 0);
                if (Projectile.ai[0] == 0 || Projectile.ai[0] == 2)
                    Main.EntitySpriteDraw(brighTexFlipped, position, frame, Color.White * opacity, Projectile.rotation, origin, Projectile.scale, spriteEffect, 0);
            }

            return false;
        }
	}
}
