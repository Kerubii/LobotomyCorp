using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using LobotomyCorp.Utils;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.DataStructures;

namespace LobotomyCorp.Projectiles.Realized
{
	public class RegretR3 : ModProjectile
	{
		public override string Texture => "LobotomyCorp/Projectiles/Realized/RegretR";

		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.scale = 1.5f;

			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
		}
		private Vector2 OldMouse = Vector2.Zero;
		private Spring chainSpring;
		private Bezier chainBezier;

		public override void AI()
        {
			//Get Owner
			Player owner = Main.player[Projectile.owner];
			Vector2 mountedCenter = owner.RotatedRelativePoint(owner.MountedCenter);
			float springLength = 50;
			if (chainBezier == null)
			{
				chainBezier = new Bezier(Projectile.Center, mountedCenter);
				chainSpring = new Spring(Projectile.Center, 4, springLength);
			}

			Vector2 targetPos = Main.MouseWorld - mountedCenter;
			targetPos.Normalize();
			targetPos *= 32f;

			float resistance = 0.95f;
			float k = 0.025f;
			chainSpring.ApplyVelocity(Vector2.UnitY * 0.2f);
			chainSpring.UpdateVelocity(targetPos + mountedCenter, k, resistance);
			chainSpring.DustTest();

			Vector2 springEnd = chainSpring.GetPosition(3);
			//Projectile.velocity.Y += 1.2f;
			//Projectile.velocity += Spring.SpringForce(springEnd, Projectile.Center, k, springLength);
			//Projectile.velocity *= resistance;

			if (owner.channel)
			{
				owner.itemTime = 5;
				owner.itemAnimation = 5;
            }
            else //Otherwise Retract the flail
			{
				Projectile.Kill();
			}

			chainBezier.SetStartEnd(Projectile.Center, mountedCenter);

			Vector2 chainDelta = Projectile.Center - mountedCenter;
			Vector2 cOffset1 = chainDelta / 3;
			chainBezier.CPoint2Move(mountedCenter + cOffset1, 24f);

			//Vector2 cOffset2 = mountedCenter + (chainDelta / 2) - Projectile.Center;
			//cOffset2.Normalize();
			//cOffset2 = Projectile.Center + cOffset2 * cOffset1.Length();
			Vector2 cOffset2 = Projectile.Center;
			chainBezier.CPoint1Move(cOffset2, 24f);

			//chainBezier.DustTest();
			Projectile.rotation = (chainBezier.BezierPoint(0f) - chainBezier.BezierPoint(0.05f)).ToRotation() + MathHelper.ToRadians(45);

			OldMouse = Main.MouseWorld;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (chainBezier != null)
			{
				float accuracy = 150;
				float steps = 0.001f;

				float BezierLength = chainBezier.SegmentBezierLength(accuracy);

				Texture2D tex = Mod.Assets.Request<Texture2D>("Projectiles/Realized/RegretRChain").Value;
				int chainWidth = 8;
				Rectangle frame = Terraria.Utils.Frame(tex, 1, 2);
				Vector2 origin = new Vector2(5, 7);

				int segments = (int)(BezierLength / chainWidth);
				bool draw = true;
				if (segments <= 0)
					draw = false;

				if (draw)
				{
					for (int j = 1; j >= 0; j--)
					{
						float t = 0;
						for (int i = 0; i < segments + 1; i++)
						{
							Vector2 Pos = chainBezier.BezierPoint(t);
							float t1 = t;
							chainBezier.NextApproximatePoint(chainWidth, ref t, steps);
							float t2 = t;

							Color color = Lighting.GetColor((int)(Pos.X / 16f), (int)(Pos.Y / 16f));

							if (i % 2 == j)
							{
								frame.Y = ((i + 1) % 2) * frame.Height;

								Main.EntitySpriteDraw(chainBezier.DrawCurveRotatedtoNext(tex, frame, color, 1f, 0f, origin, 0, t1, t2));
							}
						}
					}
				}
			}
			Vector2 position = Projectile.Center - Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;

			Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, null, lightColor, Projectile.rotation, new Vector2(5, 27), Projectile.scale, 0, 0);

			return false;
		}
    }
}
