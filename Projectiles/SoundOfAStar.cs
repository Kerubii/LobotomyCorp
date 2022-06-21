using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class SoundOfAStar : ModProjectile
	{
		public override void SetStaticDefaults() {
            DisplayName.SetDefault("Anime");
        }

		public override void SetDefaults() {
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 1;
			Projectile.scale = 1f;
            Projectile.timeLeft = 80;

            Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.friendly = true;
		}

        public override void AI() {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);

            Projectile.ai[0]++;

            if (Projectile.ai[0] < 70)
            {
                Projectile.position = ownerMountedCenter + Projectile.velocity * Projectile.ai[1];
                if (Projectile.ai[0] < 60)
                    Projectile.ai[1] += 0.5f * (Projectile.ai[0] > 20 ? 1f - ((Projectile.ai[0] - 20f ) / 40f) : 1f);
            }
            else if (Projectile.ai[0] == 70)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 targetPos = Main.MouseWorld;// + new Vector2(Main.rand.Next(32), Main.rand.Next(32));
                    Vector2 vel = Vector2.Normalize(targetPos - Projectile.Center);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel * 4, ModContent.ProjectileType<SoundOfAStarShoot>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Projectile.velocity = vel * -4f;

                    SoundEngine.PlaySound(SoundID.Item72, Projectile.Center);
                }
                for (int i = 0; i < 8; i++)
                {
                    Vector2 speed = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(i * 45));
                    Dust.NewDustPerfect(Projectile.Center, 91, speed, 0, default(Color), 0.5f).noGravity = true;
                }
            }
            else
            {
                Projectile.scale -= 0.1f;
                return;
            }

            if (Projectile.ai[0] > 30)
            {
                projOwner.itemTime = 2;
                projOwner.itemAnimation = 2;
                Projectile.direction = projOwner.direction;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw(
                TextureAssets.Projectile[Projectile.type].Value,
                Projectile.Center - Main.screenPosition + (Projectile.ai[0] > 45 && Projectile.ai[0] < 60 ? new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)) : Vector2.Zero),
                TextureAssets.Projectile[Projectile.type].Frame(),
                lightColor,
                Projectile.rotation,
                TextureAssets.Projectile[Projectile.type].Size()/2,
                Projectile.scale * 0.5f, 
                0f, 0);
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool CanHitPvp(Player target)
        {
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return Projectile.ai[0] > 70;
        }
    }
    
    public class SoundOfAStarShoot : ModProjectile
    {
        public override string Texture => "LobotomyCorp/Projectiles/SoundOfAStar";

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;
            Projectile.tileCollide = true;

            //Projectile.usesLocalNPCImmunity = true;
            //Projectile.localNPCHitCooldown = 5;

            Projectile.extraUpdates = 50;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            Dust.NewDustPerfect(Projectile.Center, 91).noGravity = true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 180; i++)
            {
                Vector2 speed = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(i * 2));
                Dust.NewDustPerfect(Projectile.Center, 91, speed).noGravity = true;
            }
        }
    }
}
