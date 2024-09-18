using LobotomyCorp.Items;
using LobotomyCorp.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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
    public class SmileRSword : ModProjectile
    {
        public override void SetDefaults() {
            Projectile.width = 92;
            Projectile.height = 92;
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
            Player owner = Main.player[Projectile.owner];
            owner.heldProj = Projectile.whoAmI;
            float rotation = Projectile.velocity.ToRotation();
            float prog = (float)owner.itemAnimation / owner.itemAnimationMax;
            if (Projectile.ai[0] < 2)
            {
                // Swing Motion
                if (prog > 0.5f)
                {
                    prog = 1f - ((prog - 0.5f) / 0.5f);
                    rotation += MathHelper.ToRadians(-135 + 270 * (float)Math.Sin(1.57f * prog));
                    Projectile.Center = LobCorpLight.LobItemLocation(owner, TextureAssets.Item[owner.HeldItem.type].Value.Frame(), rotation, Projectile.spriteDirection);
                    Projectile.scale = 1f + (float)Math.Sin(3.14f * prog);
                }
                // Bite Motion
                else
                {
                    Projectile.ai[0] = 1;
                    prog = (prog / 0.5f);
                    Projectile.localAI[0] = Lerp(MathHelper.ToRadians(80), MathHelper.ToRadians(-10), (1f - prog) * 3);
                    Projectile.scale = 1f + Lerp(0, 0.5f, (1f - prog) * 3);
                    Vector2 offset = new Vector2(10 * prog, 0).RotatedBy(rotation);
                    Projectile.Center = LobCorpLight.LobItemLocation(owner, TextureAssets.Item[owner.HeldItem.type].Value.Frame(), rotation, Projectile.spriteDirection);
                    if (owner.itemAnimation == 1 && owner.channel)
                    {
                        Projectile.ai[0]++;
                        owner.itemAnimation = owner.itemAnimationMax;
                        Projectile.localAI[0] = 0;
                    }
                }
            }
            // Scream Charge
            else if (Projectile.ai[0] == 2)
            {
                owner.itemAnimation = owner.itemAnimationMax;
                // Cancel Scream
                if (!owner.channel)
                {
                    Projectile.ai[0] = 4;
                }
                Projectile.ai[1]++;
                // Scream when fully charged
                if (Projectile.ai[1] > 60)
                {
                    owner.channel = false;
                    Projectile.ai[0] = 3;
                }
            }
            // Screaming
            else if (Projectile.ai[0] == 3)
            {
                int time = owner.itemAnimationMax / 5;
                // Screams Periodically
                if (Main.myPlayer != Projectile.owner && owner.itemAnimation % time == 0)
                {
                    // Spawn Scream Projectiles
                }
                float rand = MathHelper.ToRadians(10);
                rotation += Main.rand.NextFloat(-rand, rand);
                if (owner.itemAnimation == 1)
                {
                    owner.itemAnimation = owner.itemAnimationMax / 2;
                    Projectile.ai[0]++;
                }
            }
            // Cooldown
            else if (Projectile.ai[0] == 4)
            {
                if (owner.itemAnimation > owner.itemAnimationMax / 2)
                    owner.itemAnimation = owner.itemAnimationMax / 2;
            }
            LobCorpLight.LobItemFrame(owner, rotation, Projectile.spriteDirection);
            Projectile.rotation = rotation;
            Projectile.spriteDirection = owner.direction;
            if (owner.ItemAnimationEndingOrEnded)
                Projectile.Kill();
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player projOwner = Main.player[Projectile.owner];
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = tex.Frame(1, 4);
            Vector2 position = Projectile.Center - Main.screenPosition;
            Vector2 origin = new Vector2((Projectile.spriteDirection == 1 ? 0 : frame.Width), frame.Height);
            float rotation = Projectile.rotation + MathHelper.ToRadians(Projectile.spriteDirection == 1 ? 45 : 135);
            SpriteEffects spriteEffect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            if (Projectile.ai[0] > 0)
            {
                frame.Y = frame.Height;
                Rectangle jawFrame = frame;
                float jawRot = Projectile.localAI[0];
                Vector2 jawLower = new Vector2(55, 45);
                Vector2 jawLowerPosition = position + new Vector2(jawLower.X, -jawFrame.Height + jawLower.Y).RotatedBy(rotation) * Projectile.scale;

                Vector2 jawUpper = new Vector2(47, 41);
                Vector2 jawUpperPosition = position + new Vector2(jawUpper.X, -jawFrame.Height + jawUpper.Y).RotatedBy(rotation) * Projectile.scale;

                if (Projectile.spriteDirection < 0)
                {
                    jawRot *= -1;
                    jawLower.X = jawFrame.Width - jawLower.X;
                    jawLowerPosition = position + new Vector2(-jawLower.X, -jawFrame.Height + jawLower.Y).RotatedBy(rotation + MathHelper.ToRadians(45)) * Projectile.scale;
                    jawUpper.X = jawFrame.Width - jawUpper.X;
                    jawUpperPosition = position + new Vector2(-jawUpper.X, -jawFrame.Height + jawUpper.Y).RotatedBy(rotation + MathHelper.ToRadians(45)) * Projectile.scale;
                }

                jawFrame.Y += frame.Height;
                Main.EntitySpriteDraw(tex, jawLowerPosition, jawFrame, lightColor, rotation + jawRot, jawLower, Projectile.scale, spriteEffect, 0);

                jawFrame.Y += frame.Height;
                Main.EntitySpriteDraw(tex, jawUpperPosition, jawFrame, lightColor, rotation - jawRot, jawUpper, Projectile.scale, spriteEffect, 0);
            }
            
            Main.EntitySpriteDraw(tex, position, frame, lightColor, rotation, origin, Projectile.scale, spriteEffect, 0);
            return false;
        }
    }
}
