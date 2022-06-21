using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles.QueenLaser
{
	public class Circle1 : ModProjectile
	{
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Arcana Slave");
        }

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
            Projectile.ai[1]++;

            Projectile.rotation += MathHelper.ToRadians(3);
            if (Projectile.rotation > (float)Math.PI * 2)
                Projectile.rotation -= (float)Math.PI * 2;

            //86
            //71
            if (Projectile.timeLeft < 30)
                Projectile.alpha += 15;

            if (Projectile.ai[1] > 60)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 71, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f);
                }

                for (Projectile.ai[0] = 0; Projectile.ai[0] <= 2200f; Projectile.ai[0] += 5f)
                {
                    var start = Projectile.Center + Projectile.velocity * Projectile.ai[0];
                    if (!Collision.CanHit(Projectile.Center, 1, 1, start, 1, 1))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Dust.NewDust(start - new Vector2 (15, 15), 30, 30, 86, -Projectile.velocity.X * 2f, -Projectile.velocity.Y * 2f);
                        }
                        Projectile.ai[0] -= 5f;
                        break;
                    }
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.ai[1] < 60)
                return false;
            
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + unit * Projectile.ai[0], 22, ref point);
        }


        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = LobotomyCorp.ArcanaSlaveBackground.Value;
            float rot = Projectile.velocity.ToRotation();
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY) - new Vector2(32f, 0).RotatedBy(rot);
            Vector2 origin = new Vector2(61, 61);
            Rectangle frame = new Rectangle(0, 0, 122, 122);
            Vector2 scale = new Vector2(0.5f, 1f);
            Color color = Color.White * (1 - ((float)Projectile.alpha / 255));

            float mult = 1f;
            MultRange(ref mult, 0, 13);
            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, rot, origin, (scale + new Vector2(0.03f + 0.02f * (float)Math.Sin(Projectile.rotation))) * mult, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

            texture = Mod.Assets.Request<Texture2D>("Projectiles/QueenLaser/Circle1Color").Value;
            DrawData circle = new DrawData(texture, position, frame, color, rot, origin, (scale + new Vector2(0.03f + 0.02f * (float)Math.Sin(Projectile.rotation))) * mult, SpriteEffects.None, 0);

            var rotateShader = GameShaders.Misc["LobotomyCorp:Rotate"];
            float rotateprog = Projectile.rotation / (2 * (float)Math.PI);
            rotateShader.UseShaderSpecificData(LobotomyCorp.ShaderRotation(rotateprog));
            Main.NewText(Projectile.rotation / (2 * (float)Math.PI));
            rotateShader.Apply();

            Main.EntitySpriteDraw(circle);

            texture = TextureAssets.Projectile[Projectile.type].Value;
            circle = new DrawData(texture, position, (Rectangle?)(frame), color, rot, origin, mult * scale, SpriteEffects.None, 0);
            rotateShader.Apply(null);
            Main.EntitySpriteDraw(circle);

            texture = Mod.Assets.Request<Texture2D>("Projectiles/QueenLaser/Circle1Outer").Value;
            MultRange(ref mult, 6, 24);
            circle = new DrawData(texture, position, (Rectangle?)(frame), color, rot, origin, mult * scale, SpriteEffects.None, 0);

            rotateShader.UseShaderSpecificData(LobotomyCorp.ShaderRotation(1f - rotateprog));
            rotateShader.Apply(null);
                        
            Main.EntitySpriteDraw(circle);


            origin = new Vector2(63, 63);
            frame = new Rectangle(0, 0, texture.Width, texture.Height);

            position += new Vector2(32f, 0).RotatedBy(rot);
            texture = Mod.Assets.Request<Texture2D>("Projectiles/QueenLaser/Circle2").Value;
            MultRange(ref mult, 20, 42);
            circle = new DrawData(texture, position, (Rectangle?)(frame), color, rot, origin, mult * scale, SpriteEffects.None, 0);

            rotateShader.UseShaderSpecificData(LobotomyCorp.ShaderRotation(rotateprog));
            rotateShader.Apply(null);

            Main.EntitySpriteDraw(circle);

            position += new Vector2(32f, 0).RotatedBy(rot);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

            origin = new Vector2(61, 61);
            frame = new Rectangle(0, 0, texture.Width, texture.Height);

            SpriteEffects spriteeffect = rot > 1.57f || rot < -1.57f ? SpriteEffects.FlipVertically : SpriteEffects.None;

            string Side = "R";

            texture = Mod.Assets.Request<Texture2D>("Projectiles/QueenLaser/HeartWing" + Side).Value;
            MultRange(ref mult, 38, 50);
            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, rot, origin, (scale + new Vector2(0.05f + 0.025f * (float)Math.Cos(Projectile.rotation)) * 1.2f) * mult, spriteeffect, 0);
            texture = Mod.Assets.Request<Texture2D>("Projectiles/QueenLaser/HeartColor" + Side).Value;
            MultRange(ref mult, 40, 60);
            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, rot, origin, mult * scale * 1.2f, spriteeffect, 0);
            texture = Mod.Assets.Request<Texture2D>("Projectiles/QueenLaser/HeartOutline" + Side).Value;
            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, rot, origin, mult * scale * 1.2f, spriteeffect, 0);

            if (Projectile.ai[1] >= 60)
            {
                float alpha = 1f;
                float laserScale = 1f;
                if (Projectile.ai[1] < 90)
                {
                    alpha = (Projectile.ai[1] - 60f) / 30f;
                    laserScale = 0.2f + 0.8f * ((Projectile.ai[1] - 60) / 30);
                }
                else
                    laserScale += 0.05f * (float)Math.Cos(Projectile.rotation);
                float step = 8f * laserScale;
                texture = LobotomyCorp.ArcanaSlaveLaser.Value;
                for (float i = 8; i <= Projectile.ai[0]; i += step)
                {
                    Color c = Color.White;
                    origin = Projectile.Center + i * Projectile.velocity;
                    Main.EntitySpriteDraw(texture, origin - Main.screenPosition,
                        new Rectangle(0, 0, 60, 8), Color.White * alpha * (1 - ((float)Projectile.alpha / 255)), rot + 1.57f,
                        new Vector2(30, 4), laserScale, 0, 0);
                }
            }

            Side = "L";
            origin = new Vector2(61, 61);

            texture = Mod.Assets.Request<Texture2D>("Projectiles/QueenLaser/HeartWing" + Side).Value;
            MultRange(ref mult, 38, 50);
            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, rot, origin, (scale + new Vector2(0.05f + 0.025f * (float)Math.Cos(Projectile.rotation)) * 1.2f) * mult, spriteeffect, 0);
            texture = Mod.Assets.Request<Texture2D>("Projectiles/QueenLaser/HeartColor" + Side).Value;
            MultRange(ref mult, 40, 60);
            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, rot, origin, mult * scale * 1.2f, spriteeffect, 0);
            texture = Mod.Assets.Request<Texture2D>("Projectiles/QueenLaser/HeartOutline" + Side).Value;
            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, rot, origin, mult * scale * 1.2f, spriteeffect, 0);

            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
        }

        private void MultRange(ref float mult , float min, float max)
        {
            if (Projectile.ai[1] < min)
                mult = 0;
            else if (Projectile.ai[1] > max)
                mult = 1;
            else
                mult = (Projectile.ai[1] - min) / (max - min);
        }
    }
}
