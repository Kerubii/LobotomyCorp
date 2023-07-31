using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class HarmonyShot : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 1;
			Projectile.scale = 1f;

            Projectile.alpha = 255;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
		}

		public override void AI() {
			for (int i = 0; i < 3; i++)
            {
				int n = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Misc.Dusts.NoteDust>());
				Main.dust[n].scale = 0.5f;
				Main.dust[n].noGravity = true;
			}
			if (Main.rand.Next(3) == 0)
			{
				int i = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Misc.Dusts.NoteDust>());
				Main.dust[i].scale = 1f;
				Main.dust[i].noGravity = true;
			}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(target.position, target.width, target.height, ModContent.DustType<Misc.Dusts.NoteDust>())].velocity.Y -= 1f;
			}
		}
    }

	public class HarmonyShotR : ModProjectile
	{
		public override string Texture => "LobotomyCorp/Projectiles/HarmonyShot";

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 1;
			Projectile.scale = 1f;

			Projectile.alpha = 255;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
		}

		public override void AI()
		{
			if (Projectile.ai[0] != 0)
            {
				float rotation = Projectile.velocity.ToRotation();
				float wave = 160 * (float)Math.Sin(Projectile.ai[1] * 0.1f * Projectile.ai[0]);
				Vector2 wavePosition = new Vector2(0, wave).RotatedBy(rotation);
				Projectile.position -= wavePosition;

				Projectile.ai[1]++;
				wave = 160 * (float)Math.Sin(Projectile.ai[1] * 0.1f * Projectile.ai[0]);
				wavePosition = new Vector2(0, wave).RotatedBy(rotation);
				Projectile.position += wavePosition;
			}

			for (int i = 0; i < 3; i++)
			{
				int n = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Misc.Dusts.NoteDust>());
				Main.dust[n].scale = 0.5f;
				Main.dust[n].noGravity = true;
			}
			if (Main.rand.Next(3) == 0)
			{
				int i = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Misc.Dusts.NoteDust>());
				Main.dust[i].scale = 1f;
				Main.dust[i].noGravity = true;
			}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			foreach (Player p in Main.player)
			{
				if (p.active && (p.whoAmI == Projectile.owner || p.team == Main.player[Projectile.owner].team) && !p.dead)
				{
					LobotomyModPlayer.ModPlayer(p).HarmonyTime += 30;
					if (LobotomyModPlayer.ModPlayer(p).HarmonyTime > 600)
						LobotomyModPlayer.ModPlayer(p).HarmonyTime = 600;
					p.AddBuff(ModContent.BuffType<Buffs.MusicalAddiction>(), LobotomyModPlayer.ModPlayer(p).HarmonyTime, true);
				}
			}
			foreach (NPC n in Main.npc)
			{
				if (n.active && !n.friendly && !n.dontTakeDamage && n.Center.Distance(target.Center) < 500)
				{
					n.AddBuff(ModContent.BuffType<Buffs.CrookedNotes>(), 300);
				}
			}

			target.AddBuff(ModContent.BuffType<Buffs.CrookedNotes>(), 600);

			for (int i = 0; i < 3; i++)
			{
				Vector2 spe = new Vector2(16f, 0).RotatedByRandom(6.28f);
				Main.item[Item.NewItem(Projectile.GetSource_DropAsItem(), target.getRect(), ModContent.ItemType<Items.Ruina.Technology.HarmonyNote>(), 1, true, 0)].velocity = spe;
			}

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(target.position, target.width, target.height, ModContent.DustType<Misc.Dusts.NoteDust>())].velocity.Y -= 1f;
			}
		}
	}
}
