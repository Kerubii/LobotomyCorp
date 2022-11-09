using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class TwilightEye : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.aiStyle = -1;

			Projectile.tileCollide = false;
			Projectile.timeLeft = 30;
			Projectile.friendly = true;
		}

        public override void AI()
        {
			if (Projectile.localAI[0] == 0)
            {
				Projectile.localAI[0]++;
				Projectile.scale = Main.rand.NextFloat(0.5f, 1.2f);
            }

			Player projOwner = Main.player[Projectile.owner];
			Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
			Projectile.direction = projOwner.direction;
			Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
			Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);

			Projectile.alpha = (int)(255 * (float)Math.Sin(3.14f * (1f - (Projectile.timeLeft) / 30f)));
				
			Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]);
			Projectile.ai[0] *= 0.95f;
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			overPlayers.Add(index);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 pos = Projectile.Center + Projectile.velocity - Main.screenPosition + Projectile.gfxOffY * Vector2.UnitY;
			Rectangle frame = tex.Frame();
			Vector2 origin = frame.Size() / 2;
			Color color = Color.White * (Projectile.alpha / 255f);
			color.A = (byte)(color.A * 0.7f);
			color *= 0.9f;
			Main.EntitySpriteDraw(tex, pos, frame, color, Projectile.velocity.ToRotation() + Projectile.rotation + 1.57f, origin, 1f, 0, 0);

			if (Projectile.alpha > 100)
			{
				Vector2 scale = new Vector2(1f * (frame.Width * 2 / 124f), 1f * (frame.Height * 2 / 124f));
				tex = LobotomyCorp.CircleGlow.Value;
				color = Color.Yellow * ((Projectile.alpha - 100) / 155f);
				color.A = (byte)(color.A * 0.7f);
				color *= 0.9f;
				frame = tex.Frame();
				origin = frame.Size() / 2;
				Main.EntitySpriteDraw(tex, pos, frame, color, Projectile.velocity.ToRotation() + Projectile.rotation + 1.57f, origin, scale, 0, 0);
			}

			return false;
        }
    }
}