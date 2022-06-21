using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LobotomyCorp;
using Terraria.GameContent;

namespace LobotomyCorp.Projectiles
{
	public class BoundaryOfDeath : ModProjectile
	{
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 220;
            
            Projectile.tileCollide = false;
            Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }    
            
        public override void AI() {
            Projectile.ai[1]++;
			
			Player player = Main.player[Projectile.owner];
			
			player.itemAnimation = 2;
			player.itemTime = 2;
			player.heldProj = Projectile.whoAmI;
			
            if (Projectile.ai[0] > -1)
            {
                NPC n = Main.npc[(int)Projectile.ai[0]];
                if (!n.active || n.life <= 0)
                {
                    Projectile.ai[0] = -1;
					return;
                }
                Projectile.Center = n.Center;
				if (n.life < (int)(Projectile.damage * 44.44f))
				{
					LobotomyGlobalNPC.LNPC(n).BODExecute = true;
					n.velocity *= 0;
					n.noGravity = true;
				}
				if (Projectile.ai[1] == 90)
					n.StrikeNPC((int)(Projectile.damage * 44.44f) + n.defense/2, 0f, 1, true);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
			Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height/6);
            Vector2 origin = new Vector2(texture.Width / 2, frame.Height / 2); 
            Color color = Color.White;
			//Background
			Vector2 Offset = new Vector2(24f, -8f);
			float scaleOffset = 1f;
			AnimationHelper(ref scaleOffset, Projectile.ai[1], 90, 105);
			Offset *= scaleOffset;
			AnimationHelper(ref scaleOffset, Projectile.ai[1], 105, 190);
			Offset *= 1f + 0.3f * scaleOffset;
			float Alpha = 1f;
			AnimationHelper(ref Alpha, Projectile.ai[1], 120, 160, true);
			color *= Alpha;
            Main.EntitySpriteDraw(texture, position + Offset, (Rectangle?)frame, color, 0, origin, Projectile.scale, 0, 0);
			
			Offset *= -1;
			frame.Y += frame.Height;
            Main.EntitySpriteDraw(texture, position + Offset, (Rectangle?)frame, color, 0, origin, Projectile.scale, 0, 0);
			
			if (Projectile.ai[1] < 90)
			{
				//Initial Slash
				frame.Y += frame.Height;
				Main.EntitySpriteDraw(texture, position, (Rectangle?)frame, color, 0, origin, Projectile.scale, 0, 0);
				//RedPart
				frame.Y += frame.Height;
				float RedScale = 0f;
				if (Projectile.ai[1] > 10)
				{
					Vector2 RedOffset = new Vector2(-10, 6);
					AnimationHelper(ref RedScale, Projectile.ai[1], 10, 30, true);
					RedScale = 1f + RedScale * 1f;
					float RedAlpha = 1f;
					AnimationHelper(ref Alpha, Projectile.ai[1], 10, 20);
					color *= RedAlpha;
					Main.EntitySpriteDraw(texture, position + RedOffset, (Rectangle?)frame, color, 0, origin + RedOffset, Projectile.scale * RedScale, 0, 0);
				}
			}
			else
			{
				AnimationHelper(ref Alpha, Projectile.ai[1], 160, 175, true);
				color = Color.White * Alpha;
				
				//Slash Split
				Offset *= -1;
				frame.Y += frame.Height * 3;
				Main.EntitySpriteDraw(texture, position + Offset, (Rectangle?)frame, color, 0, origin, Projectile.scale, 0, 0);
				
				Offset *= -1;
				frame.Y += frame.Height;
				Main.EntitySpriteDraw(texture, position + Offset, (Rectangle?)frame, color, 0, origin, Projectile.scale, 0, 0);
			}
			
			Player player = Main.player[Projectile.owner];
			texture = Mod.Assets.Request<Texture2D>("Projectiles/BoundaryOfDeathSword").Value;
			position = player.RotatedRelativePoint(player.MountedCenter, true) - Main.screenPosition;
			origin = new Vector2(30,32);
			Offset = new Vector2(12 * player.direction, 16);
			AnimationHelper(ref scaleOffset, Projectile.ai[1], 80, 85, true);
			
			Main.EntitySpriteDraw(texture, position + Offset + new Vector2(0, -8 *scaleOffset), (Rectangle?)texture.Frame(), lightColor, MathHelper.ToRadians(135), origin, Projectile.scale, 0, 0);
			texture = Mod.Assets.Request<Texture2D>("Projectiles/BoundaryOfDeathSheath").Value;
			Main.EntitySpriteDraw(texture, position + Offset, (Rectangle?)texture.Frame(), lightColor, MathHelper.ToRadians(135), origin, Projectile.scale, 0, 0);

            return false;
        }

        private void AnimationHelper(ref float scale, float timer,float min, float max, bool reverse = false)
        {
            if (!reverse)
            {
                if (timer < min)
                    scale = 0f;
                else if (timer > max)
                    scale = 1f;
                else
                    scale = (timer - min) / (max - min);
            }
            else
            {
                if (timer < min)
                    scale = 1f;
                else if (timer > max)
                    scale = 0f;
                else
                    scale = 1f - (timer - min) / (max - min);
            }
        }

        /*public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = target.life * 2;
        }*/

        public override bool? CanHitNPC(NPC target)
        {
            return false;//target.whoAmI == (int)Projectile.ai[0] && Projectile.ai[1] == 180;
        }
    }
}
