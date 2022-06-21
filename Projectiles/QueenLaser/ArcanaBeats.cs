using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles.QueenLaser
{
	public class ArcanaBeats : ModProjectile
	{
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 300;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }    
            
        public override void AI() {
            Player player = Main.player[Projectile.owner];
            Vector2 mountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            if (player.channel)
            {
                if (Projectile.ai[0] < 30)
                    Projectile.ai[0]++;
                if (player.whoAmI == Main.myPlayer)
                {
                    Projectile.velocity = Main.MouseWorld - mountedCenter;
                    Projectile.velocity.Normalize();
                }
                player.itemTime = 16;
                player.itemAnimation = 16;

                if (Projectile.ai[1] >= 24)
                {
                    if (player.CheckMana(player.HeldItem, -1, true))
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 12f, ProjectileID.Starfury, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    else
                        player.channel = false;
                    Projectile.ai[1] = 0;
                }
                Projectile.ai[1]++;
                Projectile.timeLeft = 300;
            }
            else
            {
                if (Projectile.ai[0] <= 10)
                {
                    Projectile.Kill();
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 12f, ProjectileID.Starfury, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    return;
                }
                else
                {
                    if (Projectile.ai[0] == 11)
                    {
                        Projectile.Kill();
                    }

                    else if (Projectile.ai[0] < 30)
                    {
                        Projectile.ai[0]--;
                    }
                    
                    else
                    {
                        Projectile.ai[0]++;

                        if (Projectile.ai[0] == 35)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 71, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f);
                            }
                            for (int i = 0; i < 60; i++)
                            {
                                Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 86, Projectile.velocity.X * Main.rand.NextFloat(8f), Projectile.velocity.Y * Main.rand.NextFloat(8f))];
                                dust.noGravity = true;
                                dust.fadeIn = 1.3f + Main.rand.NextFloat(1f);
                            }
                        }
                        if (Projectile.ai[0] > 47)
                        {
                            Projectile.ai[0] = 29;
                        }
                    }
                }

                player.itemTime = 2;
                player.itemAnimation = 2;
            }
            Projectile.Center = mountedCenter + 105f * Projectile.velocity;
            Projectile.direction = Math.Sign(Projectile.velocity.X);
            player.direction = Projectile.direction;
            Projectile.rotation += MathHelper.ToRadians(3);
            player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * player.direction, Projectile.velocity.X * player.direction);
            player.heldProj = Projectile.whoAmI ;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = LobotomyCorp.ArcanaSlaveBackground.Value;
            float rot = Projectile.velocity.ToRotation();
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
            Vector2 origin = new Vector2(61, 61);
            Rectangle frame = new Rectangle(0, 0, 122, 122);
            Vector2 scale = new Vector2(0.5f, 1f);
            Color color = Color.White * (1 - ((float)Projectile.alpha / 255));

            float mult = 1f;
            MultRange(ref mult, 10, 30);
            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color * 0.5f, rot, origin, (scale + new Vector2(0.03f + 0.02f * (float)Math.Sin(Projectile.rotation))) * mult * 0.5f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            
            var rotateShader = GameShaders.Misc["LobotomyCorp:Rotate"];
            rotateShader.UseShaderSpecificData(LobotomyCorp.ShaderRotation(Projectile.rotation / (2 * (float)Math.PI)));
            rotateShader.Apply(null);

            texture = Mod.Assets.Request<Texture2D>("Projectiles/QueenLaser/Circle1Color").Value;
            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, rot, origin, (scale + new Vector2(0.03f + 0.02f * (float)Math.Sin(Projectile.rotation))) * mult, SpriteEffects.None, 0);
       
            texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, rot, origin, mult * scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            if (Projectile.ai[0] > 35 && Projectile.ai[0] < 48)
            {
                if (Projectile.localAI[0] == 0)
                {
                    Projectile.localAI[0] = Main.rand.Next(1, 360);
                }
                texture = Mod.Assets.Request<Texture2D>("Projectiles/QueenLaser/ArcanaBeatsBlast").Value;
                origin = new Vector2(8, 25);
                //int frameY = (int)((Projectile.ai[0] - 35) /2);
                frame = new Rectangle(0, 0, 70, 50);
                float BlastScale = 1.1f * (float)Math.Sin(MathHelper.ToRadians(10 * (Projectile.ai[0] -  35)));
                if (Projectile.ai[0] > 40)
                    color *= 1 - ((Projectile.ai[0] - 40) / 7);
                for (int i = 0; i < 9 ; i++)
                {
                    if (i == 4) continue;
                    float BeatRotation = MathHelper.ToRadians(-95 + 23.75f * i + 4 * (float)Math.Sin(MathHelper.ToRadians(Projectile.localAI[0] * i))) + rot;
                    Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, BeatRotation, origin, mult * BlastScale, SpriteEffects.None, 0);
                }
            }

            return false;
        }

        private void MultRange(ref float mult , float min, float max)
        {
            if (Projectile.ai[0] < min)
                mult = 0;
            else if (Projectile.ai[0] > max)
                mult = 1;
            else
                mult = (Projectile.ai[0] - min) / (max - min);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.ai[0] >= 35;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            //80
            Vector2 center = Projectile.Center + Projectile.velocity * 25f;
            hitbox = new Rectangle((int)Projectile.Center.X - 50, (int)Projectile.Center.Y - 50, 100, 100);
        }
    }
}
