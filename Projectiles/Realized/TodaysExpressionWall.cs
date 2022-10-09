using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LobotomyCorp.Utils;
using Terraria.GameContent;
using System.Collections.Generic;

namespace LobotomyCorp.Projectiles.Realized
{
	public class TodaysExpressionWall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("What the fuck do you want from me?"); // The English name of the Projectile
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 128;
			Projectile.height = 128; 
			Projectile.aiStyle = -1;
			Projectile.scale = 1f;

			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.tileCollide = false;

			Projectile.penetrate = -1;
			Projectile.timeLeft = 30;

			ReflectedProjectiles = new List<int>();
		}

		private List<int> ReflectedProjectiles;

        public override void AI()
        {
			if (ReflectedProjectiles == null)
				ReflectedProjectiles = new List<int>();

			Projectile.rotation = Projectile.velocity.ToRotation();
			int ReflectionChance = 100;

			if (Projectile.ai[0] < 2)
			{
				Player owner = Main.player[Projectile.owner];
				Vector2 ownerMountedCenter = owner.RotatedRelativePoint(owner.MountedCenter, true);

				float length = Projectile.ai[1];
				Projectile.Center = ownerMountedCenter + new Vector2(length, 0).RotatedBy(Projectile.rotation);
				Projectile.ai[1] += Projectile.velocity.Length();
			}			

			if (Projectile.ai[0] == 2)
				ReflectionChance = 50;

			if (Projectile.ai[0] == 3)
				ReflectionChance = 10;

			if (Projectile.ai[0] == 4)
				Projectile.frame = 1;

			if (Projectile.ai[0] < 4)
			{
				Projectile.velocity *= 0.95f;
				foreach (Projectile proj in Main.projectile)
				{
					if (proj.active && !proj.friendly && !ReflectedProjectiles.Contains(proj.whoAmI) && Projectile.getRect().Intersects(proj.getRect()))
					{
						if (Projectile.ai[0] == 0)
						{
							proj.Kill();
							continue;
						}
						if (Main.rand.Next(100) < ReflectionChance)
						{
							float Speed = proj.velocity.Length();
							Vector2 delta = proj.Center - Projectile.Center;
							delta.Normalize();
							delta *= Speed;

							proj.velocity = delta;
						}
						ReflectedProjectiles.Add(proj.whoAmI);
					}
				}
			}
        }

        public override void ModifyDamageScaling(ref float damageScale)
        {
			float Distance = (Projectile.Center - Main.player[Projectile.owner].Center).Length();
			float damageBonus = 0f;
			if (Distance > 80)
				damageBonus = (Distance - 80) / 500;
			if (damageBonus > 0.5f)
				damageBonus = 0.5f;
			damageScale += damageBonus;
            base.ModifyDamageScaling(ref damageScale);
        }
    }
}
