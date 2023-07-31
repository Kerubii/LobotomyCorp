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
	public class OurGalaxyComet : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }

        public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 1;

			Projectile.scale = 1.5f;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.extraUpdates = 2;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(5))
            {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SilverCoin);
            }

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public static void ApplyStoneBuff(Player reciever, int giver)
        {
			reciever.AddBuff(ModContent.BuffType<Buffs.OurGalaxyStoneBuff>(), 100);
			LobotomyModPlayer modReciever = LobotomyModPlayer.ModPlayer(reciever);
			modReciever.OurGalaxyStone = true;
			modReciever.OurGalaxyOwner = giver;
        }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			ApplyStoneBuff(Main.player[Projectile.owner], Projectile.owner);
			foreach (Player p in Main.player)
			{
				if (p.whoAmI != Projectile.owner && !p.dead && p.team == Main.player[Projectile.owner].team)
				{
					ApplyStoneBuff(p, Projectile.owner);
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				Vector2 velocity = new Vector2(Main.rand.NextFloat(6f, 16f), 0).RotatedByRandom(6.28);
				Dust.NewDustPerfect(Projectile.Center, DustID.SilverCoin).fadeIn = Main.rand.NextFloat(0.5f, 1.2f);
			}
		}

        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = tex.Frame();
			for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
				Vector2 pos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;

				float opacity = 0.8f * (1f - i / 4f);
				Main.EntitySpriteDraw(tex, pos, frame, Color.White * opacity, Projectile.rotation, new Vector2(53, 15), 1f, 0, 0);
			}

			Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, new Vector2(53, 15), 1f, 0, 0);
			return false;
        }
    }
}
