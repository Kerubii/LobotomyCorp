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
	public class RegretR2 : ModProjectile
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

		private Vector2 OldMouse;
		private ChainPhysics Chain;

        public override void AI()
        {
			Player owner = Main.player[Projectile.owner];
			Vector2 mountedCenter = owner.RotatedRelativePoint(owner.MountedCenter);

			bool ownerIsMyPlayer = Main.myPlayer == Projectile.owner;
			int ChainAmount = 30;

			if (Chain == null)
			{
				OldMouse = Main.MouseWorld;
				Chain = new ChainPhysics(ChainAmount, 8);
			}

			Vector2 velocity = new Vector2(0, 0);

			if (owner.channel)
            {
				owner.itemTime = 6;
				owner.itemAnimation = 6;

				//if (Main.MouseWorld != OldMouse)
				//{
					velocity = Main.MouseWorld - mountedCenter;
					velocity.Normalize();
					velocity *= 18f;
				//}
				//else
					//velocity = Vector2.UnitY * 1.2f;
			}
			else
            {
				Projectile.Kill();
            }

			for (int i = 0; i < 5; i++)
			{
				float percent = 1f - ((float)i / ChainAmount);
				Chain.ApplyPhysic(mountedCenter, i, velocity * percent);
			}

			Chain.ApplyPhysics(mountedCenter, Vector2.UnitY * 8f);

			Vector3 ChainEnd = Chain.GetChainEnd();
			Projectile.Center = new Vector2(ChainEnd.X, ChainEnd.Y);
			Projectile.rotation = ChainEnd.Z - MathHelper.ToRadians(135);

			if (ownerIsMyPlayer)
				OldMouse = Main.MouseWorld;
        }

        public override bool PreDraw(ref Color lightColor)
        {
			if (Chain != null)
			{
				Texture2D tex = Mod.Assets.Request<Texture2D>("Projectiles/Realized/RegretRChain").Value;
				Rectangle frame = Terraria.Utils.Frame(tex, 1, 2);
				Vector2 origin = new Vector2(5, 7);

				for (int j = 1; j >= 0; j--)
				{
					float t = 0;
					for (int i = 0; i < Chain.ChainAmount(); i++)
					{
						if (i % 2 == j)
						{
							frame.Y = ((i + 1) % 2) * frame.Height;

							Main.EntitySpriteDraw((DrawData)Chain.DrawChain(i, tex, frame, origin, 0));
						}
					}
				}
			}

			Vector2 position = Projectile.Center - Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;

			Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, null, lightColor, Projectile.rotation, new Vector2(5, 27), Projectile.scale, 0, 0);

			return false;
        }

        public override bool ShouldUpdatePosition()
        {
			return false;
        }
    }
}
