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
	public class TwilightSlash : ModProjectile
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

			float prog = Projectile.ai[0] / 30f;

			Projectile.velocity = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(-135 + 225 * (float)Math.Sin(1.57f * prog)));
			Projectile.ai[0]++;
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
			Player projOwner = Main.player[Projectile.owner];
			Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);

			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 pos = ownerMountedCenter - Main.screenPosition + Projectile.gfxOffY * Vector2.UnitY;
			Rectangle frame = tex.Frame();
			Vector2 origin = frame.Size() / 2;
			float alpha = (Projectile.alpha / 255f);

			Color color = Color.Black * alpha;
			color.A = (byte)(color.A * 0.7f);
			color *= 0.9f;
			Main.EntitySpriteDraw(tex, pos, frame, color, Projectile.velocity.ToRotation() + Projectile.rotation, origin, 2f, 0, 0);

			return false;
        }
    }
}