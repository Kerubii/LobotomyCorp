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

namespace LobotomyCorp.Projectiles
{
    public class TwilightSpecial : ModProjectile
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

            float rot = Projectile.velocity.ToRotation();
            float distance = 320;
            float progress = 1f - (float)projOwner.itemAnimation / (float)projOwner.itemAnimationMax;
            if (progress < 0.33f)
            {
                float progress2 = progress % .33f / .33f;
                if (progress2 < .3f)
                {
                    rot += MathHelper.ToRadians(Lerp(-140, 30, progress2 % 0.3f / 0.3f)) * Projectile.spriteDirection;
                    projOwner.velocity += Projectile.velocity;
                    projOwner.velocity.Y -= projOwner.gravity;
                    //projOwner.position = PreviousPosition + Projectile.velocity * Lerp(0, distance, progress2 % .3f / .3f);
                }
                else if (progress2 < .5f)
                {
                    rot += MathHelper.ToRadians(30) * Projectile.spriteDirection;
                    projOwner.velocity = Vector2.Zero;
                    projOwner.velocity.Y -= projOwner.gravity;
                    //projOwner.position = PreviousPosition + Projectile.velocity * distance;
                }
                else if (progress2 < .8f)
                {
                    rot += MathHelper.ToRadians(Lerp(30, 180, (progress2 - 0.2f) % 0.3f / 0.3f)) * Projectile.spriteDirection;
                    projOwner.velocity.Y -= projOwner.gravity;
                    //projOwner.position = PreviousPosition + Projectile.velocity * distance;
                }
                else
                {
                    rot += 180 * Projectile.spriteDirection;
                    projOwner.velocity.Y -= projOwner.gravity;
                    //projOwner.position = PreviousPosition + Projectile.velocity * distance;
                }
            }
            else if (progress < 0.66f)
            {
                float progress2 = progress % .33f / .33f;
                if (progress2 < .3f)
                {
                    rot += MathHelper.ToRadians(Lerp(180, -75, progress2 % 0.3f / .3f)) * Projectile.spriteDirection;
                    //projOwner.position = PreviousPosition + Projectile.velocity * distance * (float)Math.Sin(1.57f + 1.57f * (progress2 % .3f / .3f));
                    projOwner.velocity -= Projectile.velocity;
                    projOwner.velocity.Y -= projOwner.gravity;
                }
                else if (progress < .8f)
                {
                    rot += MathHelper.ToRadians(-75) * Projectile.spriteDirection; if (Projectile.ai[1] == 0)
                    {
                        projOwner.velocity *= 0;
                        Projectile.ai[1] = 1;
                    }
                }
                else
                {
                    rot += MathHelper.ToRadians(Lerp(-75, -110, (progress2) % 0.2f / .2f)) * Projectile.spriteDirection;
                }
            }
            else
            {
                float progress2 = progress % .33f / .33f;

                if (progress2 < 0.5f)
                {
                    rot += MathHelper.ToRadians(-80 + 230 * (float)Math.Sin(1.65f * (progress2 % .5f / .5f))) * Projectile.spriteDirection;
                }
                else
                {
                    rot += MathHelper.ToRadians(180 + 320 * (0.5f * (float)Math.Cos(3.14f * (progress2 % .5f / .5f) + 3.14f) + 0.5f)) * Projectile.spriteDirection;
                }  

                //rot += MathHelper.ToRadians(Lerp(-140, 505, progress2)) * Projectile.spriteDirection;

                //rot += MathHelper.ToRadians(360 * (float)Math.Sin(progress2 * 6.48) - 140) * Projectile.spriteDirection;
            }
            //Main.NewText(projOwner.itemAnimation);

            if (((0.33f < progress && progress < 0.4f) ||
                (0.46f < progress && progress < 0.50f) ||
                (0.7f < progress && progress < 0.8f)) && Projectile.localAI[0] == 0)
            {
                SoundEngine.PlaySound(SoundID.Item1, ownerMountedCenter);
                Projectile.localAI[0]++;
            }

            if (progress < .7f)
            {
                projOwner.immune = true;
                projOwner.immuneTime = 15;
                projOwner.immuneNoBlink = true;
                //projOwner.armorEffectDrawShadow = true;
            }


            Vector2 velRot = new Vector2(1, 0).RotatedBy(rot);
            projOwner.itemRotation = (float)Math.Atan2(velRot.Y * Projectile.direction, velRot.X * Projectile.direction);
            projOwner.direction = Projectile.spriteDirection;
            Projectile.rotation = rot;// + MathHelper.ToRadians(Projectile.spriteDirection == 1 ? 45 : 135);

            Projectile.Center = ownerMountedCenter + (80 + Projectile.ai[0]) * velRot;

            if (progress < .1f ||
                (.33f <= progress && progress < .43f) ||
                (.66f <= progress && progress < .77f) ||
                (.87f <= progress && progress < .95f))
            {
                for (int i = 0; i < 24; i++)
                {
                    Vector2 posOffset = new Vector2(Main.rand.NextFloat(-20, Projectile.width / 2f), Main.rand.NextFloat(-Projectile.height / 2, 0)).RotatedBy(Projectile.rotation);
                    Dust d = Dust.NewDustPerfect(Projectile.Center + posOffset, DustID.Wraith);
                    d.noGravity = true;
                    d.color = Color.Black;
                    d.fadeIn = 1.2f;
                    d.scale = 2;
                    d.velocity *= 0;
                }

                for (int i = 0; i < 4; i++)
                {
                    Vector2 posOffset = new Vector2(Main.rand.NextFloat(-Projectile.width / 2f, Projectile.width / 2f), Main.rand.NextFloat(-Projectile.height / 2, 0)).RotatedBy(Projectile.rotation);
                    Dust d = Dust.NewDustPerfect(Projectile.Center + posOffset, 64);
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(1f, 2.5f);
                    d.velocity *= 0;
                }
            }

            if (projOwner.itemAnimation == 1)
            {
                /*
                projOwner.velocity *= 0;
                if (!projOwner.mount.Active)
                    projOwner.velocity.Y -= 6f;*/
                Projectile.Kill();
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += (int)(target.defense / 2f);
            knockback *= 0.1f;
        }

        public override bool? CanHitNPC(NPC target)
        {
            float progress = 1f - (float)Main.player[Projectile.owner].itemAnimation / (float)Main.player[Projectile.owner].itemAnimationMax;
            if (progress < .1f ||
                (.30f <= progress && progress < .5f) ||
                (.60f <= progress && progress < .95f))
            {
                return true;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockBack, bool crit)
        {
            if (LobotomyModPlayer.ModPlayer(Main.player[Projectile.owner]).TwilightSpecial < 9)
            {
                LobotomyModPlayer.ModPlayer(Main.player[Projectile.owner]).TwilightSpecial++;
                if (LobotomyModPlayer.ModPlayer(Main.player[Projectile.owner]).TwilightSpecial == 9)
                {
                    SoundEngine.PlaySound(SoundID.MaxMana, target.position);
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(Main.player[Projectile.owner].position, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height, 45, 0.0f, 0.0f, (int)byte.MaxValue, new Color(), (float)Main.rand.Next(20, 26) * 0.1f);
                        Main.dust[index2].noLight = true;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 0.5f;
                    }
                    LobotomyModPlayer.ModPlayer(Main.player[Projectile.owner]).TwilightSpecial++;
                }
            }
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
}
