using LobotomyCorp.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles.Realized
{
	public class BlackSwanRExtended : ModProjectile
	{
		public override string Texture => "LobotomyCorp/Projectiles/Realized/BlackSwanR";

		public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 2;
			//ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			//ProjectileID.Sets.TrailingMode[Projectile.type] = 4;
		}

        public override void SetDefaults() {
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.scale = 1.3f;
			Projectile.alpha = 0;
			Projectile.timeLeft = 14;

			//Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
		}

		// In here the AI uses this example, to make the code more organized and readable
		// Also showcased in ExampleJavelinProjectile.cs
		public float movementFactor // Change this value to alter how fast the spear moves
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		// It appears that for this AI, only the ai0 field is used!
		public override void AI() {
			// Since we access the owner player instance so much, it's useful to create a helper local variable for this
			// Sadly, Projectile/ModProjectile does not have its own
			Player projOwner = Main.player[Projectile.owner];
			// Here we set some of the Projectile's owner properties, such as held item and itemtime, along with Projectile direction and position based on the player
			projOwner.heldProj = Projectile.whoAmI;
			Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
			Projectile.direction = projOwner.direction;
			Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
			Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
			if (!projOwner.frozen) {
				if (movementFactor == 0f) // When initially thrown out, the ai0 will be 0f
				{
					SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
					movementFactor = 2f; // Make sure the spear moves forward when initially thrown out
					Projectile.netUpdate = true; // Make sure to netUpdate this spear
				}
				movementFactor += 6f;
			}
			// Change the spear position based off of the velocity and the movementFactor
			Projectile.position += Projectile.velocity * movementFactor;
			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Projectile.timeLeft % 3 == 0)
            {
				int type = Main.rand.Next(2, 4);
				for (int i = 0; i < 8; i++)
				{
					Vector2 vel = new Vector2(3 * (float)Math.Cos(MathHelper.ToRadians(45 * i)), 6 * (float)Math.Sin(MathHelper.ToRadians(45 * i)));
					vel = vel.RotatedBy(Projectile.velocity.ToRotation()) - Projectile.velocity * 0.2f;

					Dust d = Dust.NewDustPerfect(Projectile.Center, type, vel);
					d.noGravity = true;

				}
			}

			for (int i = 0; i < 3; i++)
            {
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.Next(2, 4));
				Main.dust[d].velocity = Projectile.velocity * -0.2f;
				Main.dust[d].noGravity = true;
			
			}

			if (projOwner.dead)
				Projectile.Kill();
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if (LobotomyModPlayer.ModPlayer(Main.player[Projectile.owner]).BlackSwanNettleClothing >= 5)
			{
				target.AddBuff(BuffID.Ichor, 300);
			}
			base.OnHitNPC(target, damage, knockback, crit);
        }

		public override bool PreDraw(ref Color lightColor)
        {
			SlashTrail trail = new SlashTrail(24, MathHelper.ToRadians(0));//, 0.785f - MathHelper.ToRadians(Projectile.direction == 1 ? 0 : 90));
			trail.color = new Color(0, 100, 0);

			Player projOwner = Main.player[Projectile.owner];
			float prog = (float)Math.Sin(1.57f * (Projectile.timeLeft / 14f));
			if (prog < 0)
				prog = 0;
			CustomShaderData windTrail = LobotomyCorp.LobcorpShaders["WindTrail"].UseOpacity(prog);

			int max = Projectile.timeLeft > 4 ? 10 - (Projectile.timeLeft - 4) : 10;
			Vector2[] pos = new Vector2[max];
			float[] rot = new float[max];
			for (int i = 0; i < pos.Length; i++)
            {
				pos[i] = Projectile.Center - Projectile.velocity * i * 6;
				rot[i] = Projectile.rotation;
            }

			trail.DrawSpecific(pos, rot, Vector2.Zero, windTrail);

			return false;
        }
    }
}
