using LobotomyCorp.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static LobotomyCorp.LobotomyCorp;
using static Terraria.Player;

namespace LobotomyCorp.Projectiles.Realized
{
    public class SanguineDesireR : ModProjectile
    {
        public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Spear");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults() {
            Projectile.width = 82;
            Projectile.height = 82;
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

        //private Vector2 PreviousPosition;

        public override void AI() {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;

            /*if (Projectile.ai[1] == 0)
            {
                PreviousPosition = projOwner.position;
                Projectile.ai[1] = 1;
            }*/

            float rot = Projectile.velocity.ToRotation() - MathHelper.ToRadians(145) * projOwner.direction;

            float progress = 1f - (float)projOwner.itemAnimation / (float)projOwner.itemAnimationMax;
            
            /*if (progress < 0.3f)
            {
                float swing = progress / 0.3f;
                rot += MathHelper.ToRadians(290) * (float)Math.Sin(1.57f * swing) * projOwner.direction;
                Projectile.scale = 1f + 0.2f * (float)Math.Sin(3.14f * swing);
            }
            else if (progress < 0.5f)
            {
                rot += MathHelper.ToRadians(290) * projOwner.direction;
            }
            else if (progress < 0.8f)
            {
                float swing = 1f - ((progress - 0.5f) / 0.3f);
                rot += MathHelper.ToRadians(290) * (float)Math.Sin(1.57f * swing) * projOwner.direction;
                Projectile.scale = 1f + 0.2f * (float)Math.Sin(3.14f * swing);
            }*/

            if (progress < 0.3f)
            {
                progress = (progress) / 0.3f;
                rot += MathHelper.ToRadians(190) * (float)Math.Sin(1.57f * progress) * projOwner.direction;
                Projectile.scale = 1f + 0.2f * (float)Math.Sin(3.14f * progress);
            }
            else if (progress < 0.4f)
            {
                progress = (progress - 0.3f) / 0.1f;
                rot += MathHelper.ToRadians(190 + 10 * progress) * projOwner.direction;
            }
            else if (progress < 0.5f)
            {
                progress = (progress - 0.4f) / 0.1f;
                rot += MathHelper.ToRadians(200 - 200 * progress) * projOwner.direction;
            }
            else if (progress < 0.8f)
            {
                progress = (progress - 0.5f) / 0.3f;
                rot += MathHelper.ToRadians(190) * (float)Math.Sin(1.57f * progress) * projOwner.direction;
                Projectile.scale = 1f + 0.2f * (float)Math.Sin(3.14f * progress);
            }
            else
            {
                progress = (progress - 0.8f) / 0.2f;
                rot += MathHelper.ToRadians(190 + 10 * progress) * projOwner.direction;
            }

            Vector2 velRot = new Vector2(1, 0).RotatedBy(rot);
            projOwner.itemRotation = (float)Math.Atan2(velRot.Y * Projectile.direction, velRot.X * Projectile.direction);
            projOwner.direction = Projectile.spriteDirection;
            Projectile.rotation = rot;// + MathHelper.ToRadians(Projectile.spriteDirection == 1 ? 45 : 135);
            //projOwner.SetCompositeArmFront(enabled: true, CompositeArmStretchAmount.Full, Projectile.rotation - 1.57f); //- 0.785f);// * owner.direction);

            Items.LobCorpLight.LobItemFrame(projOwner, rot);

            Projectile.Center = ownerMountedCenter + (60 + Projectile.ai[0]) * velRot;

            if (projOwner.itemAnimation == 2)
            {
                if (Projectile.ai[0] < 0)
                    Projectile.Kill();
                else
                {
                    Projectile.ai[1]++;
                    if (Projectile.ai[1] == 10)
                    {
                        Projectile.ai[0] = -1;
                    }
                    projOwner.itemAnimation = 3;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.ai[0] = -1;
            int maximumDamage = target.damage * 20;
            if (maximumDamage < Projectile.damage * 20)
                maximumDamage = Projectile.damage * 20;
            LobotomyGlobalNPC.SanguineDesireApplyBleed(target, damage / 2f, maximumDamage, 60, 600);
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
            Vector2 originOffset = new Vector2(Projectile.ai[0] + 12, 0).RotatedBy(MathHelper.ToRadians(Projectile.direction == 1 ? 135 : 45));
            Vector2 origin = new Vector2((Projectile.spriteDirection == 1 ? 9 : 74), 74) + originOffset;
            float rotation = Projectile.rotation + MathHelper.ToRadians(Projectile.spriteDirection == 1 ? 45 : 135);
            SpriteEffects spriteEffect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, new Microsoft.Xna.Framework.Rectangle?
                                    (
                                        new Rectangle
                                        (
                                            0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()
                                        )
                                    ),
                lightColor * ((float)(255 - Projectile.alpha) / 255f), rotation, origin, Projectile.scale, spriteEffect, 0);
            /*
            SlashTrail trail = new SlashTrail(80, 1.57f);
            trail.DrawTrail(Projectile, LobcorpShaders["TwilightSlash"]);*/

            return false;
        }
    }

    public class SanguineDesireRHeavy : ModProjectile
    {
        public override string Texture => "LobotomyCorp/Projectiles/Realized/SanguineDesireR";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spear");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1.3f;
            Projectile.alpha = 0;
            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 3;

            //Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

        //private Vector2 PreviousPosition;

        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;

            /*if (Projectile.ai[1] == 0)
            {
                PreviousPosition = projOwner.position;
                Projectile.ai[1] = 1;
            }*/

            float rot = Projectile.velocity.ToRotation() - MathHelper.ToRadians(135) * projOwner.direction;

            float progress = Projectile.ai[1] / ((float)projOwner.itemAnimationMax * Projectile.extraUpdates);
            if (progress < 0.5f)
            {
                rot += MathHelper.ToRadians(2) * Main.rand.NextFloat(-1, 1);
            }
            else if (progress < 0.8f)
            {
                float time = (float)Math.Sin(1.57f * ((progress - 0.5f) / 0.3f));
                rot += MathHelper.ToRadians(290) * time * projOwner.direction;
            }
            else
            {
                rot += MathHelper.ToRadians(290) * projOwner.direction;
            }
            if (Projectile.ai[0] < 4)
                Projectile.ai[1]++;

            if (Projectile.ai[0] > 0)
                Projectile.ai[0]++;

            Vector2 velRot = new Vector2(1, 0).RotatedBy(rot);
            projOwner.itemRotation = (float)Math.Atan2(velRot.Y * Projectile.direction, velRot.X * Projectile.direction);
            projOwner.direction = Projectile.spriteDirection;
            Projectile.rotation = rot;// + MathHelper.ToRadians(Projectile.spriteDirection == 1 ? 45 : 135);
            projOwner.SetCompositeArmFront(enabled: true, CompositeArmStretchAmount.Full, Projectile.rotation - 1.57f); //- 0.785f);// * owner.direction);

            Projectile.Center = ownerMountedCenter + 86 * velRot;

            if (projOwner.itemAnimation == 1)
            {
                /*
                projOwner.velocity *= 0;
                if (!projOwner.mount.Active)
                    projOwner.velocity.Y -= 6f;*/
                Projectile.Kill();
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            float progress = Projectile.ai[1] / ((float)Main.player[Projectile.owner].itemAnimationMax * Projectile.extraUpdates);
            if (progress < 0.5f || progress > 0.8f)
                return false;
            return base.CanHitNPC(target);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.ai[0]++;
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
            Vector2 originOffset = new Vector2(36, 0).RotatedBy(MathHelper.ToRadians(Projectile.direction == 1 ? 135 : 45));
            Vector2 origin = new Vector2((Projectile.spriteDirection == 1 ? 25 : 51), 51) + originOffset;
            float rotation = Projectile.rotation + MathHelper.ToRadians(Projectile.spriteDirection == 1 ? 45 : 135);
            SpriteEffects spriteEffect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, new Microsoft.Xna.Framework.Rectangle?
                                    (
                                        new Rectangle
                                        (
                                            0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()
                                        )
                                    ),
                lightColor * ((float)(255 - Projectile.alpha) / 255f), rotation, origin, Projectile.scale, spriteEffect, 0);
            /*
            SlashTrail trail = new SlashTrail(80, 1.57f);
            trail.DrawTrail(Projectile, LobcorpShaders["TwilightSlash"]);*/

            return false;
        }
    }
}
