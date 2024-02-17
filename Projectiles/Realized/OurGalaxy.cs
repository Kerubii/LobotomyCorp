﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using LobotomyCorp.Utils;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.CodeAnalysis;

namespace LobotomyCorp.Projectiles.Realized
{
	public class OurGalaxyComet : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
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
			Projectile.extraUpdates = 3;
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

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			Player owner = Main.player[Projectile.owner];
			if (Projectile.position.Y < owner.position.Y - 80)
				return false;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			ApplyStoneBuff(Main.player[Projectile.owner], Projectile.owner);
			int hp = -1;
			int healTarget = -1;
			foreach (Player p in Main.player)
			{
				if (p.whoAmI != Projectile.owner && !p.dead && p.team == Main.player[Projectile.owner].team)
				{
					if (hp == -1 || p.statLife < hp)
					{
						hp = p.statLife;
						healTarget = p.whoAmI;
					}
				}
			}
			if (healTarget >= 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<OurGalaxyHeal>(), 0, 0, Projectile.owner, healTarget, (int)(damageDone / 10));
                //ApplyStoneBuff(p, Projectile.owner);
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = new Vector2(Main.rand.NextFloat(6f, 16f), 0).RotatedByRandom(6.28);
                Dust.NewDustPerfect(Projectile.Center, DustID.SilverCoin).fadeIn = Main.rand.NextFloat(0.5f, 2f);
			}

			SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Art/Galaxy_Strong_Big_Boom") with { Volume = 0.25f });
		}

        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = tex.Frame();
			int max = Projectile.oldPos.Length;
            for (int i = 0; i < max; i++)
            {
				Vector2 pos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;

				float opacity = 0.5f * (1f - i / (float)max);
				Main.EntitySpriteDraw(tex, pos, frame, Color.White * opacity, Projectile.rotation, new Vector2(53, 15), 1f, 0, 0);
			}

			tex = Mod.Assets.Request<Texture2D>("Projectiles/Realized/OurGalaxyStone").Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, new Vector2(53, 15), 1f, 0, 0);
			return false;
        }
    }
}
