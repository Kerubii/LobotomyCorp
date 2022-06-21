using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using LobotomyCorp.Utils;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace LobotomyCorp.Projectiles.Realized
{
	public class RegretR : ModProjectile
	{
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

		private Bezier chainBezier;

        public override void AI()
        {
			//Get Owner
			Player owner = Main.player[Projectile.owner];
			Vector2 mountedCenter = owner.RotatedRelativePoint(owner.MountedCenter);

			//Regret Alternate Attack - A very different flail, more like a mix between flail and a whip
			//Projectile swings three times, first downwards, then upwards, and finally a slam
			//Projectile loosely follows its supposed position to give it weight, its intended position is always where the mouse is rather than a set position from the player
			//ai0 keeps track of where the projectile will be, this will be affected by meleespeed too
			//This only happens while the use is "channeling", when the player stops channeling the flail is then retracted
			//If the third hit hits an enemy, it applies the debuff and also lingers for half a second before forcing channeling to stop and retracts the flail
			float attackSpeed = owner.GetAttackSpeed(DamageClass.Melee);
			float Accel = 1.2f * attackSpeed;
			float SpeedMax = 12f * attackSpeed;

			Vector2 targetPosition = Projectile.Center;
			//Check if owner channeling
			if (owner.channel)
            {
				//If it hits or hits a tile on third swing, force cancel player channeling
				if (Projectile.ai[0] < 0)
                {
					owner.channel = false;
                }				

				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 deltaMouse = Main.MouseWorld - mountedCenter;
					float distance = deltaMouse.Length();
					if (distance > 260 * attackSpeed)
						distance = 260 * attackSpeed;
					float angle = deltaMouse.ToRotation();

					float prog = Projectile.ai[0] / 20f;
					if (prog > 1)
						prog = 1;

					//targetPosition = mountedCenter + new Vector2(distance * prog, 0).RotatedBy(angle);

					

					//45, 45, 30 swing times, 5 rest frames
					int swingInterval = 45;
					int rest = 5;
					int finalSwing = 30;

					//First Swing
					if (Projectile.ai[0] <= swingInterval)
					{
						float progress = Projectile.ai[0] / swingInterval;
						angle += (MathHelper.ToRadians(-60) + MathHelper.ToRadians(120) * progress) * owner.direction;
						distance *= 0.25f + 0.75f * (float)Math.Sin(progress * 3.14f);
						targetPosition = mountedCenter + new Vector2(distance, 0).RotatedBy(angle);
					}
					//Rest / Ready
					else if (Projectile.ai[0] <= swingInterval + rest)
					{
						angle += (MathHelper.ToRadians(60)) * owner.direction;
						distance *= 0.25f;
						targetPosition = mountedCenter + new Vector2(distance, 0).RotatedBy(angle);
					}
					//SecondSwing
					else if (Projectile.ai[0] > swingInterval + rest && Projectile.ai[0] <= swingInterval * 2 + rest)
                    {
						float progress = (Projectile.ai[0] - (swingInterval + rest)) / swingInterval;
						angle -= (MathHelper.ToRadians(-60) + MathHelper.ToRadians(120) * progress) * owner.direction;
						distance *= 0.25f + 0.75f * (float)Math.Sin(progress * 3.14f);
						targetPosition = mountedCenter + new Vector2(distance, 0).RotatedBy(angle);
					}
					//Rest / Ready
					else if (Projectile.ai[0] <= swingInterval * 2 + rest * 2)
					{
						angle -= (MathHelper.ToRadians(60)) * owner.direction;
						targetPosition = mountedCenter + new Vector2(distance, 0).RotatedBy(angle);
					}
					else if (Projectile.ai[0] > swingInterval * 2 + rest * 2 && Projectile.ai[0] <= swingInterval * 2 + rest * 2 + finalSwing)
					{

                    }
					else
					{
						Projectile.velocity *= 0.95f;
					}

					
				}
            }
			else //Otherwise Retract the flail
            {
				targetPosition = owner.Center;
				float distance = (targetPosition - Projectile.Center).Length();
				//Kill when within 1 tile
				if (distance <= 16f)
					Projectile.Kill();
            }

			Vector2 delta = targetPosition - Projectile.Center;
			float deltaDist = delta.Length();
			delta.Normalize();

			if (deltaDist > 0)
			{
				if (deltaDist < SpeedMax)
				{
					//Projectile.velocity = delta * deltaDist;
				}
				else
				{
					Vector2 vectAccel = delta * Accel;
					delta *= SpeedMax;

					Projectile.velocity.X += vectAccel.X;
					if (delta.X > 0 && Projectile.velocity.X < delta.X)
					{
						if (Projectile.velocity.X < 0)
						{
							Projectile.velocity.X *= 0.5f;
							//Projectile.velocity.X += vectAccel.X * 2;
						}

						if (Projectile.velocity.X > delta.X)
							Projectile.velocity.X = delta.X;
					}
					else if (delta.X < 0 && Projectile.velocity.X > delta.X)
					{
						if (Projectile.velocity.X > 0)
						{
							Projectile.velocity.X *= 0.5f;
							//Projectile.velocity.X += vectAccel.X * 2;
						}

						if (Projectile.velocity.X < delta.X)
							Projectile.velocity.X = delta.X;
					}

					Projectile.velocity.Y += vectAccel.Y;
					if (delta.Y > 0 && Projectile.velocity.Y < delta.Y)
					{
						if (Projectile.velocity.Y < 0)
						{
							Projectile.velocity.Y *= 0.5f;
							//Projectile.velocity.Y += vectAccel.Y;
						}

						if (Projectile.velocity.Y > delta.Y)
							Projectile.velocity.Y = delta.Y;
					}
					else if (delta.Y < 0 && Projectile.velocity.Y > delta.Y)
					{
						if (Projectile.velocity.Y > 0)
						{
							Projectile.velocity.Y *= 0.5f;
							//Projectile.velocity.Y += vectAccel.Y;
						}

						if (Projectile.velocity.Y < delta.Y)
							Projectile.velocity.Y = delta.Y;
					}
				}
			}

			Projectile.ai[0] += 1f * attackSpeed;
			owner.itemTime = 24;
			owner.itemAnimation = 24;

			//Initialize Bezier for Chain
			if (chainBezier == null)
			{
				chainBezier = new Bezier(Projectile.Center, mountedCenter);				
			}

			chainBezier.SetStartEnd(Projectile.Center + Projectile.velocity, mountedCenter);

			Vector2 chainDelta = targetPosition - mountedCenter;
			Vector2 cOffset1 = chainDelta / 3;
			chainBezier.CPoint2Move(mountedCenter + cOffset1, 24f);

			//Vector2 cOffset2 = mountedCenter + (chainDelta / 2) - Projectile.Center;
			//cOffset2.Normalize();
			//cOffset2 = Projectile.Center + cOffset2 * cOffset1.Length();
			Vector2 cOffset2 = targetPosition;
			chainBezier.CPoint1Move(cOffset2, 24f);

			chainBezier.DustTest();
			Projectile.rotation = (chainBezier.Control2 - Projectile.Center).ToRotation() + MathHelper.ToRadians(225);
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
