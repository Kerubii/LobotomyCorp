using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles.KingPortal
{
	public class KingPortal : ModProjectile
	{
        public override string Texture => "LobotomyCorp/Projectiles/KingPortal/KingPortal1";

        public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Road to Happiness");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 300;

            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }    
            
        public override void AI() {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.scale = 0.1f;
                Projectile.localAI[0]++;
            }
            if (Projectile.scale < 1f)
                Projectile.scale += 0.1f;
            Projectile.rotation += MathHelper.ToRadians(1);
            if (Projectile.rotation > (float)Math.PI * 2)
                Projectile.rotation -= (float)Math.PI * 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[0] == 0)
                return false;

            Texture2D texture = LobotomyCorp.KingPortal1.Value;
            float rot = Projectile.velocity.ToRotation();
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY) - new Vector2(32f, 0).RotatedBy(rot);
            Vector2 origin = new Vector2(61, 61);
            Rectangle frame = new Rectangle(0, 0, 122, 122);
            Vector2 scale = new Vector2(0.75f, 1f) * Projectile.scale;
            Color color = Color.White * (1 - ((float)Projectile.alpha / 255));

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            float rotateProg = Projectile.rotation / (2f * (float)Math.PI);

            var resizeShader = GameShaders.Misc["LobotomyCorp:Rotate"];
            resizeShader.UseOpacity(1f - ((float)Projectile.alpha / 255));
            resizeShader.UseShaderSpecificData(LobotomyCorp.ShaderRotation(rotateProg));
            resizeShader.Apply(null);

            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, rot, origin, 1f, SpriteEffects.None, 0);
            resizeShader.UseOpacity(1f - ((float)Projectile.alpha / 255));
            resizeShader.UseShaderSpecificData(LobotomyCorp.ShaderRotation(1f - rotateProg));
            resizeShader.Apply(null);

            texture = LobotomyCorp.KingPortal2.Value;
            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, rot, origin, 1f, SpriteEffects.None, 0);
            resizeShader.UseOpacity(1f - ((float)Projectile.alpha / 255));
            resizeShader.UseShaderSpecificData(LobotomyCorp.ShaderRotation(rotateProg));
            resizeShader.Apply(null);

            texture = LobotomyCorp.KingPortal3.Value;
            frame = new Rectangle(0, 0, texture.Width, texture.Height);
            origin = frame.Size() / 2;
            Main.EntitySpriteDraw(texture, position, (Rectangle?)(frame), color, rot, origin, scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            
            return false;
        }
    }
}
