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
    public class CensoredGrab : ModProjectile
    {
        public override void SetStaticDefaults() {
        }

        public override void SetDefaults() {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1.3f;
            Projectile.alpha = 0;
            Projectile.timeLeft = 120;

            //Projectile.hide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;

            DrawHeldProjInFrontOfHeldItemAndArms = true;
        }

        public override void AI() {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;

            float rot = Projectile.velocity.ToRotation();

            float progress = 1f - (float)projOwner.itemAnimation / (float)projOwner.itemAnimationMax;
            if (progress < 0.3f)
            {
                Projectile.ai[0] = Lerp(0f, 90, progress % 0.3f / 0.3f);
            }
            else if (progress >= 0.5f)
            {
                Projectile.ai[0] = 45 + 45f * (float)Math.Sin(1.57f + 3.14f * (progress % 0.5f / 0.5f));
            }
            else
                Projectile.ai[0] = 90;

            if (progress > 0.25f)
            {
                if (Projectile.localAI[0] < 0.959931f)
                    Projectile.localAI[0] += 0.0898132f;
            }
            
            Vector2 velRot = new Vector2(1, 0).RotatedBy(rot);
            projOwner.itemRotation = (float)Math.Atan2(velRot.Y * Projectile.direction, velRot.X * Projectile.direction);
            projOwner.direction = Projectile.spriteDirection;
            Projectile.rotation = rot;// + MathHelper.ToRadians(Projectile.spriteDirection == 1 ? 45 : 135);

            Projectile.Center = ownerMountedCenter + (40 + Projectile.ai[0]) * velRot;

            if (projOwner.itemAnimation == 1)
                Projectile.Kill();
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
            Vector2 position = ownerMountedCenter - Main.screenPosition + RandomizeTexture();
            Vector2 origin = new Vector2(Projectile.spriteDirection == 1 ? 8 : 68, 14);
            float rotation = Projectile.rotation + (Projectile.spriteDirection == 1 ? 0 : 3.14f);
            float scale = Projectile.scale * 0.33f;

            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, TextureAssets.Projectile[Projectile.type].Frame(), lightColor, rotation, origin, scale, 0f, 0);

            position += (Projectile.Center - ownerMountedCenter) / 5 + RandomizeTexture();
            scale = Projectile.scale * 0.50f;
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, TextureAssets.Projectile[Projectile.type].Frame(), lightColor, rotation, origin, scale, 0f, 0);
            
            for (int i = -1; i < 2; i += 2)
            {
                position = Projectile.Center + new Vector2(-14, -5 * i).RotatedBy(Projectile.rotation) - Main.screenPosition;
                scale = Projectile.scale * 0.33f;
                float rotation2 = Projectile.rotation - 1.047f * i + Projectile.localAI[0] * i;
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position + RandomizeTexture(), TextureAssets.Projectile[Projectile.type].Frame(), lightColor, rotation2 + (Projectile.spriteDirection == 1 ? 0 : 3.14f), origin, scale, 0f, 0);

                position += new Vector2(25, 0).RotatedBy(rotation2) + RandomizeTexture();
                rotation2 += 0.78f * i + (Projectile.spriteDirection == 1 ? 0 : 3.14f);
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, TextureAssets.Projectile[Projectile.type].Frame(), lightColor, rotation2, origin, scale, 0f, 0);
            }

            position = Projectile.Center + new Vector2(-60, 0).RotatedBy(Projectile.rotation) - Main.screenPosition + RandomizeTexture();
            scale = Projectile.scale * 0.66f;
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, TextureAssets.Projectile[Projectile.type].Frame(), lightColor, rotation, origin, scale, 0f, 0);

            return false;
        }

        private Vector2 RandomizeTexture()
        {
            return new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
        }
    }

    public class CensoredSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1.3f;
            Projectile.alpha = 0;
            Projectile.timeLeft = 120;

            //Projectile.hide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;

            DrawHeldProjInFrontOfHeldItemAndArms = true;
        }

        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;

            float rot = Projectile.velocity.ToRotation();

            float progress = 1f - (float)projOwner.itemAnimation / (float)projOwner.itemAnimationMax;
            if (progress < 0.15f)
            {
                Projectile.ai[0] = Lerp(0f, 360, progress % 0.15f / 0.15f);
            }
            else if (progress >= 0.5f)
            {
                Projectile.ai[0] = 180 + 180 * (float)Math.Sin(1.57f + 3.14f * (progress % 0.5f / 0.5f));
            }
            else
                Projectile.ai[0] = 360;

            if (progress > 0.0f)
            {
                if (Projectile.localAI[0] < 1.0472)
                    Projectile.localAI[0] += 0.1298132f;
            }

            Vector2 velRot = new Vector2(1, 0).RotatedBy(rot);
            projOwner.itemRotation = (float)Math.Atan2(velRot.Y * Projectile.direction, velRot.X * Projectile.direction);
            projOwner.direction = Projectile.spriteDirection;
            Projectile.rotation = rot;// + MathHelper.ToRadians(Projectile.spriteDirection == 1 ? 45 : 135);

            Projectile.Center = ownerMountedCenter + (40 + Projectile.ai[0]) * velRot;

            if (projOwner.itemAnimation == 1)
                Projectile.Kill();
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.player[Projectile.owner].itemAnimation < Main.player[Projectile.owner].itemAnimationMax / 2)
                target.immune[Projectile.owner] = Main.player[Projectile.owner].itemAnimation;
            else
                target.immune[Projectile.owner] = Main.player[Projectile.owner].itemAnimationMax / 2;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            if (Collision.CheckAABBvLineCollision2(targetHitbox.TopLeft(), targetHitbox.Size(), ownerMountedCenter, Projectile.Center))
                return true;
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Vector2 position = ownerMountedCenter - Main.screenPosition;
            Vector2 origin = new Vector2(Projectile.spriteDirection == 1 ? 8 : 68, 14);
            float rotation = Projectile.rotation + (Projectile.spriteDirection == 1 ? 0 : 3.14f);
            float scale = Projectile.scale * 0.33f;

            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position + RandomizeTexture(), TextureAssets.Projectile[Projectile.type].Frame(), lightColor, rotation, origin, scale, 0f, 0);

            position += (Projectile.Center - ownerMountedCenter);
            position += new Vector2(-12, 0).RotatedBy(Projectile.rotation);
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position + RandomizeTexture(), TextureAssets.Projectile[Projectile.type].Frame(), lightColor, rotation, origin, scale, 0f, 0);

            for (int i = 0; i < 7; i++)
            {
                position -= (Projectile.Center - ownerMountedCenter) / 14;
                if (i == 3)
                    position -= (Projectile.Center - ownerMountedCenter) / 26;
                if (i > 2)
                    scale = Projectile.scale * 0.5f;
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position + RandomizeTexture(), TextureAssets.Projectile[Projectile.type].Frame(), lightColor, rotation, origin, scale, 0f, 0);
            }

            position = ownerMountedCenter - Main.screenPosition + (Projectile.Center - ownerMountedCenter) / 12;
            scale = Projectile.scale * 0.50f;
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position + RandomizeTexture(), TextureAssets.Projectile[Projectile.type].Frame(), lightColor, rotation, origin, scale, 0f, 0);

            position += (Projectile.Center - ownerMountedCenter) / 8;

            for (int i = -1; i < 2; i += 2)
            {
                float length = 40;
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 2)
                    length = Lerp(-20, 40, (float)projOwner.itemAnimation / ((float)projOwner.itemAnimationMax / 2f));

                Vector2 position2 = position + new Vector2(length, -5 * i).RotatedBy(Projectile.rotation);
                scale = Projectile.scale * 0.33f;
                float rotation2 = Projectile.rotation - 1.047f * i + Projectile.localAI[0] * i;
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position2 + RandomizeTexture(), TextureAssets.Projectile[Projectile.type].Frame(), lightColor, rotation2 + (Projectile.spriteDirection == 1 ? 0 : 3.14f), origin, scale, 0f, 0);

                position2 += new Vector2(25, 0).RotatedBy(rotation2) + RandomizeTexture();
                rotation2 += 0.78f * i + (Projectile.spriteDirection == 1 ? 0 : 3.14f) - (Projectile.localAI[0] - 0.261799f) * i;
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position2 + RandomizeTexture(), TextureAssets.Projectile[Projectile.type].Frame(), lightColor, rotation2, origin, scale, 0f, 0);
            }

            scale = Projectile.scale * 0.66f;
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position + RandomizeTexture(), TextureAssets.Projectile[Projectile.type].Frame(), lightColor, rotation, origin, scale, 0f, 0);

            return false;
        }

        private Vector2 RandomizeTexture()
        {
            return new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
        }
    }
}
