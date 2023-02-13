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
	public class RegretR4 : ModProjectile
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
			float Accel = 6f * attackSpeed;
			float SpeedMax = 24f * attackSpeed;

			Vector2 targetPosition = Projectile.Center;
			Vector2 projectilePosition = Projectile.Center;
			//Check if owner channeling
			if (owner.channel)
            {
				//If it hits or hits a tile on third swing, force cancel player channeling
				if (Projectile.ai[0] < 0)
                {
					//owner.channel = false;
                }

				if (Main.myPlayer == Projectile.owner)
				{

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

			Dust.NewDustPerfect(targetPosition, 66).noGravity = true;

			Vector2 delta = targetPosition - Projectile.Center;
			Projectile.Center = targetPosition;
			targetPosition = Projectile.Center + delta * 4;
			
			Projectile.ai[0] += 1f * attackSpeed;
			owner.itemTime = 24;
			owner.itemAnimation = 24;

			//Initialize Bezier for Chain
			if (chainBezier == null)
			{
				chainBezier = new Bezier(Projectile.Center, mountedCenter);				
			}

			chainBezier.SetStartEnd(Projectile.Center, mountedCenter);

			Vector2 chainDelta = targetPosition - mountedCenter;
			Vector2 cOffset1 = chainDelta / 3;
			chainBezier.CPoint2Move(mountedCenter + cOffset1, 24f);

			//Vector2 cOffset2 = mountedCenter + (chainDelta / 2) - Projectile.Center;
			//cOffset2.Normalize();
			//cOffset2 = Projectile.Center + cOffset2 * cOffset1.Length();
			Vector2 cOffset2 = targetPosition;
			chainBezier.CPoint1Move(cOffset2, 24f);

			//chainBezier.DustTest();
			Projectile.rotation = (chainBezier.BezierPoint(0f) - chainBezier.BezierPoint(0.05f)).ToRotation() + MathHelper.ToRadians(225);
			//Dust.NewDustPerfect(chainBezier.BezierPoint(1f), 66).noGravity = true;
			//Dust.NewDustPerfect(chainBezier.BezierPoint(0.9f), 66).noGravity = true;
		}

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        private Vector2 Where(float ai0, float attackSpeed)
        {
			Player owner = Main.player[Projectile.owner];
			Vector2 mountedCenter = owner.RotatedRelativePoint(owner.MountedCenter);

			Vector2 targetPosition = Projectile.Center;

			float distance = Projectile.velocity.Length();
			if (distance > 400 * attackSpeed)
				distance = 400 * attackSpeed;
			float angle = Projectile.velocity.ToRotation();

			//targetPosition = mountedCenter + new Vector2(distance * prog, 0).RotatedBy(angle);



			//45, 45, 30 swing times, 5 rest frames
			int swingInterval = 60;
			int rest = 20;
			int finalSwing = 30;

			//First Swing
			if (ai0 <= swingInterval)
			{
				float progress = ai0 / swingInterval;
				angle -= MathHelper.ToRadians(120) + (float)Math.Sin(progress * 3.14f) * MathHelper.ToRadians(240);
				float x = (float)Math.Cos(angle) * distance;
				float y = (float)Math.Sin(angle) * 16f;
				targetPosition = mountedCenter + new Vector2(x, y);
			}
			//Rest / Ready
			else if (ai0 <= swingInterval + rest)
			{
				float progress = (ai0 - swingInterval) / rest;
				angle += MathHelper.ToRadians(120) + MathHelper.ToRadians(10) * progress;

				float x = (float)Math.Cos(angle) * distance;
				float y = (float)Math.Sin(angle) * 16f;
				targetPosition = mountedCenter + new Vector2(x, y);
			}
			//SecondSwing
			else if (ai0 > swingInterval + rest && ai0 <= swingInterval * 2 + rest)
			{
				float progress = (ai0 - (swingInterval + rest)) / swingInterval;
				angle += MathHelper.ToRadians(130) - (float)Math.Sin(progress * 3.14f) * MathHelper.ToRadians(250);
				float x = (float)Math.Cos(angle) * distance;
				float y = (float)Math.Sin(angle) * 16f;
				targetPosition = mountedCenter + new Vector2(x, y);
			}
			//Rest / Ready
			else if (ai0 <= swingInterval * 2 + rest * 2)
			{
				float progress = (ai0 - (swingInterval * 2 + rest)) / swingInterval;
				angle -= MathHelper.ToRadians(120) - MathHelper.ToRadians(10) * progress; targetPosition = mountedCenter + new Vector2(distance, 0).RotatedBy(angle);
				float x = (float)Math.Cos(angle) * distance;
				float y = (float)Math.Sin(angle) * 16f;
				targetPosition = mountedCenter + new Vector2(x, y);
			}
			else if (ai0 > swingInterval * 2 + rest * 2 && ai0 <= swingInterval * 2 + rest * 2 + finalSwing)
			{

			}
			else
			{

			}

			return targetPosition;
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
