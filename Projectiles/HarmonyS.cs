using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class HarmonyS : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 46;
			Projectile.height = 46;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
            
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
		}

		public override void AI() {
            
            Player player = Main.player[Projectile.owner];
            Vector2 mountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.velocity = Main.MouseWorld - mountedCenter;
            }
            Vector2 center = new Vector2(93, 0).RotatedBy(Projectile.velocity.ToRotation());
            Projectile.Center = mountedCenter + center;
            if (!player.channel)
            {
                if (Projectile.ai[0] > 0)
                    Projectile.ai[0] -= 2;
                if (Projectile.ai[0] <= 0)
                    Projectile.Kill();
            }
            else
            {
                if (Projectile.ai[0] < 60)
                    Projectile.ai[0] += 0.5f;
            }
            player.itemAnimation = 2;
            player.itemTime = 2;

            player.direction = center.X >= 0 ? 1 : -1;
            Projectile.direction = player.direction;
            player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * player.direction, Projectile.velocity.X * player.direction);

            Projectile.timeLeft = 60;
            Projectile.rotation += (MathHelper.ToRadians(20) * (Projectile.ai[0] / 60)) * player.direction;
            if (Main.rand.Next((int)(30 - 30 * (Projectile.ai[0] / 60))) == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    float rotRand = Main.rand.NextFloat((float)Math.PI * 2);
                    Vector2 dustPos = new Vector2(23, 0).RotatedBy(rotRand);
                    Vector2 dustVel = new Vector2(0, 4).RotatedBy(rotRand);
                    Dust.NewDustPerfect(Projectile.Center + dustPos, 5, dustVel);
                }
            }

            Projectile.localAI[0]--;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
            Vector2 origin = new Vector2(Projectile.direction > 1 ? 93 : 23, 23);
            float rot = Projectile.velocity.ToRotation() + (Projectile.direction > 1 ? 0 : 3.14f);

            Main.EntitySpriteDraw(tex, position, tex.Frame(), lightColor, rot, origin, Projectile.scale, (SpriteEffects)Projectile.direction, 0);

            tex = Mod.Assets.Request<Texture2D>("Projectiles/HarmonySHead").Value;
            Main.EntitySpriteDraw(tex, position, tex.Frame(), lightColor, Projectile.rotation, origin, Projectile.scale, (SpriteEffects)Projectile.direction, 0);
            tex = Mod.Assets.Request<Texture2D>("Projectiles/HarmonySString").Value;
            Main.EntitySpriteDraw(tex, position, tex.Frame(), lightColor, rot, origin, Projectile.scale, (SpriteEffects)Projectile.direction, 0);

            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            foreach (Player p in Main.player)
            {
                if (p.active && (p.whoAmI == Projectile.owner || p.team == Main.player[Projectile.owner].team) && !p.dead)
                {
                    LobotomyModPlayer.ModPlayer(p).HarmonyTime += 5;
                    if (LobotomyModPlayer.ModPlayer(p).HarmonyTime > 600)
                        LobotomyModPlayer.ModPlayer(p).HarmonyTime = 600;
                    p.AddBuff(ModContent.BuffType<Buffs.MusicalAddiction>(), LobotomyModPlayer.ModPlayer(p).HarmonyTime, true);
                }
            }
            target.immune[Projectile.owner] = 15 - (int)(13 * (Projectile.ai[0] / 60f));
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(target.position, target.width, target.height, 5);
            }
            Vector2 spe = new Vector2(16f, 0).RotatedByRandom(6.28f);
            Main.item[Item.NewItem(Projectile.GetSource_DropAsItem(),target.getRect(), ModContent.ItemType<Items.Ruina.Technology.HarmonyNote>(), 1, true, 0)].velocity = spe;

            if (Projectile.localAI[0] <= 0)
            {
                SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Singing_Atk") with {Volume = 0.5f, PitchVariance = 0.3f}, Projectile.position);
                Projectile.localAI[0] = 60 - 30 * (Projectile.ai[0] / 60);
            }

        }
    }
}
