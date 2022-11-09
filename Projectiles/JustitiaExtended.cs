using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static LobotomyCorp.LobotomyCorp;

namespace LobotomyCorp.Projectiles
{
    public class JustitiaExtended : ModProjectile
    {
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                BrightTexture1 = ModContent.Request<Texture2D>("LobotomyCorp/Projectiles/JustitiaExtendedEdge", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                BrightTexture2 = ModContent.Request<Texture2D>("LobotomyCorp/Projectiles/JustitiaExtendedEdgeFlip", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                Main.QueueMainThreadAction(() =>
                {
                    LobotomyCorp.PremultiplyTexture(BrightTexture1);
                    LobotomyCorp.PremultiplyTexture(BrightTexture2);
                });
            }
            base.Load();
        }

        public override void Unload()
        {
            BrightTexture1 = null;
            BrightTexture2 = null;
        }

        private static Texture2D BrightTexture1;
        private static Texture2D BrightTexture2;

        public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Spear");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }

        public override void SetDefaults() {
            Projectile.width = 40;
            Projectile.height = 120;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.timeLeft = 15;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override void AI() {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);

            Projectile.ai[1]++;
            Projectile.Center = ownerMountedCenter + Projectile.ai[1] * Projectile.velocity + new Vector2( 0, (projOwner.height / 2 - 60) * projOwner.direction).RotatedBy(Projectile.velocity.ToRotation());

            Projectile.alpha = (int)(255 * (float)Math.Sin(3.14f * Projectile.ai[1] / 16));

            for (int i = 0; i < 6; i++)
            {
                int d = Dust.NewDust(Projectile.position - Vector2.UnitY * 30, Projectile.width, Projectile.height + 30, DustID.MagicMirror, -Projectile.velocity.X, 0);
                Main.dust[d].noGravity = true;
            }

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y * projOwner.direction, Projectile.velocity.X * projOwner.direction);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            if (Math.Abs(Projectile.velocity.Y) > Math.Abs(Projectile.velocity.X))
            {
                hitbox.X -= 40;
                hitbox.Width += 40;
                hitbox.Y += 40;
                hitbox.Height -= 40;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[0] == 0)
                for (int i = 0; i < 16; i++)    
                {
                    int d = Dust.NewDust(Projectile.position - Vector2.UnitY * 30, Projectile.width, Projectile.height + 30, DustID.MagicMirror);
                    Main.dust[d].noGravity = true;
                }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Dust.NewDustPerfect(ownerMountedCenter, 14, Vector2.Zero);
            Rectangle frame = TextureAssets.Projectile[Projectile.type].Value.Frame();
            float originOffset = -60 * Projectile.spriteDirection;
            Vector2 origin = frame.Size() / 2 + originOffset * Vector2.UnitX;
            SpriteEffects spriteEffect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            for (int i = 0; i < 3; i++)
            {
                Vector2 oldPos = Projectile.Center - Projectile.velocity * (i + 1) - Main.screenPosition + Projectile.gfxOffY * Vector2.UnitY;
                float opacity = (Projectile.alpha / 255f) * (1f - i / 3f);
                Vector2 scale = Projectile.scale * new Vector2(0.75f, 0.85f) * (1f - i / 3f);
                if (Projectile.ai[0] == 0 || Projectile.ai[0] == 1)
                    Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, oldPos, frame, Color.LightGray * opacity, Projectile.rotation, origin, scale, spriteEffect, 0);
                if (Projectile.ai[0] == 0 || Projectile.ai[0] == 2)
                    Main.EntitySpriteDraw(ModContent.Request<Texture2D>("LobotomyCorp/Projectiles/JustitiaExtendedFlip").Value, oldPos, frame, Color.LightGray * opacity, Projectile.rotation, origin, scale, spriteEffect, 0);
            }

            Vector2 position = Projectile.Center - Main.screenPosition + Projectile.gfxOffY * Vector2.UnitY;
            if (Projectile.ai[0] == 0 || Projectile.ai[0] == 1)
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, frame, Color.White * (Projectile.alpha / 255f), Projectile.rotation, origin, Projectile.scale, spriteEffect, 0);
            if (Projectile.ai[0] == 0 || Projectile.ai[0] == 2)
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>("LobotomyCorp/Projectiles/JustitiaExtendedFlip").Value, position, frame, Color.White * (Projectile.alpha / 255f), Projectile.rotation, origin, Projectile.scale, spriteEffect, 0);
            if (Projectile.alpha > 85)
            {
                float opacity = (Projectile.alpha - 85) / 170f;
                if (Projectile.ai[0] == 0 || Projectile.ai[0] == 1)
                    Main.EntitySpriteDraw(BrightTexture1, position, frame, Color.White * opacity, Projectile.rotation, origin, Projectile.scale, spriteEffect, 0);
                if (Projectile.ai[0] == 0 || Projectile.ai[0] == 2)
                    Main.EntitySpriteDraw(BrightTexture2, position, frame, Color.White * opacity, Projectile.rotation, origin, Projectile.scale, spriteEffect, 0);
            }

            return false;
        }
    }
}
