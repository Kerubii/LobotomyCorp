using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static LobotomyCorp.LobotomyCorp;

namespace LobotomyCorp.Projectiles
{
    public class DiscordSlash : ModProjectile
    {
        public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Spear");
        }

        public override void SetDefaults() {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1.3f;
            Projectile.alpha = 0;
            Projectile.timeLeft = 120;

            //Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

        public override void AI() {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;

            float rot = Projectile.velocity.ToRotation();

            float progress = 1f - (float)projOwner.itemAnimation / (float)projOwner.itemAnimationMax;
            rot += MathHelper.ToRadians(205f * (float) Math.Sin(1.6f * progress) - 160f) * Projectile.spriteDirection;

            Vector2 velRot = new Vector2(1, 0).RotatedBy(rot);
            projOwner.itemRotation = (float)Math.Atan2(velRot.Y * Projectile.direction, velRot.X * Projectile.direction);
            projOwner.direction = Projectile.spriteDirection;
            Projectile.rotation = rot + MathHelper.ToRadians(Projectile.spriteDirection == 1 ? 45 : 135);

            Projectile.Center = ownerMountedCenter + (120 + Projectile.ai[0]) * velRot;

            if (projOwner.itemAnimation == 1)
                Projectile.Kill();

            if (0.14f < progress && progress < 0.63f)
                for (int i = 0; i < 16; i++)
                {
                    Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Wraith)];
                    d.noGravity = true;
                    d.color = Color.Black;
                    d.fadeIn = 1.2f;
                    d.scale = 2;
                    d.velocity *= 0;
                }

            /*if (Projectile.ai[1] == 0 && projOwner.itemAnimation < projOwner.itemAnimationMax / 2)
            {
                Projectile.NewProjectile(Projectile.Center, Projectile.velocity, ModContent.ProjectileType<DiscordLingeringSlash>(), Projectile.damage, 0f, Projectile.owner);
                Projectile.ai[1]++;
            }*/
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            //Dust.NewDustPerfect(ownerMountedCenter, 14, Vector2.Zero);
            Vector2 position = ownerMountedCenter - Main.screenPosition;
            Vector2 originOffset = new Vector2(Projectile.ai[0] - 20, 0).RotatedBy(MathHelper.ToRadians(Projectile.direction == 1 ? 135 : 45));
            Vector2 origin = new Vector2((Projectile.spriteDirection == 1 ? 14 : 95), 95) + originOffset;
            SpriteEffects spriteEffect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, new Microsoft.Xna.Framework.Rectangle?
                                    (
                                        new Rectangle
                                        (
                                            0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()
                                        )
                                    ),
                lightColor * ((float)(255 - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, spriteEffect, 0);
            return false;
        }
    }
}
