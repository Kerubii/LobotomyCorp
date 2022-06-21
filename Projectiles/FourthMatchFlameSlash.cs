using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class FourthMatchFlameSlash : ModProjectile
	{
		public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Spear");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 100;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }    
            
        public override void AI() {
            Player player = Main.player[Projectile.owner];
            //player.heldProj = Projectile.whoAmI;
            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
            Projectile.position += Projectile.velocity * 113f;
            Projectile.direction = player.direction;
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.direction == 1 ? 0 : MathHelper.ToRadians(180));
            
            if (Projectile.frameCounter++ >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
            }

            if (Projectile.frame > 5 || player.dead)
                Projectile.Kill();

            if (Projectile.frame != 5)
            {
                Rectangle dustRect = new Rectangle((int)(Projectile.position.X), (int)Projectile.position.Y, 100, 100);
                if (Projectile.frame == 0 || Projectile.frame == 4)
                {
                    Vector2 pos = new Vector2(-134, Projectile.frame == 0 ? 0 : 50 * Projectile.direction).RotatedBy(Projectile.rotation + (Projectile.direction == 1 ? 0 : MathHelper.ToRadians(180)));
                    dustRect = new Rectangle((int)(Projectile.position.X + pos.X), (int)(Projectile.position.Y + pos.Y), 70, 50);
                }
                else if (Projectile.frame == 1 || Projectile.frame == 3)
                {
                    Vector2 pos = new Vector2(-84, Projectile.frame == 1 ? 0 : 50 * Projectile.direction).RotatedBy(Projectile.rotation + (Projectile.direction == 1 ? 0 : MathHelper.ToRadians(180)));
                    dustRect = new Rectangle((int)(Projectile.position.X + pos.X), (int)(Projectile.position.Y + pos.Y), 100, 50);
                }

                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDust(dustRect.TopLeft(), dustRect.Width, dustRect.Height, 6, Projectile.velocity.X, Projectile.velocity.Y, 50, new Color(), Main.rand.NextFloat(1.2f, 2.0f));
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.HasBuff(ModContent.BuffType<Buffs.Matchstick>()))
                target.buffTime[target.FindBuffIndex(ModContent.BuffType<Buffs.Matchstick>())] += 300;
            else
                target.AddBuff(ModContent.BuffType<Buffs.Matchstick>(), 300);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (target.HasBuff(ModContent.BuffType<Buffs.Matchstick>()))
                target.buffTime[target.FindBuffIndex(ModContent.BuffType<Buffs.Matchstick>())] += 300;
            else
                target.AddBuff(ModContent.BuffType<Buffs.Matchstick>(), 300);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            if (Projectile.frame == 0 || Projectile.frame == 4 || Projectile.frame == 5)
            {
                Vector2 pos = new Vector2(-134, 0).RotatedBy(Projectile.rotation + (Projectile.direction == 1 ? 0 : MathHelper.ToRadians(180)));
                hitbox = new Rectangle(hitbox.X + (int)pos.X, hitbox.Y + (int)pos.Y, 70, 100);
            }
            if (Projectile.frame == 1 || Projectile.frame == 3)
            {
                Vector2 pos = new Vector2(-84, 0).RotatedBy(Projectile.rotation + (Projectile.direction == 1 ? 0 : MathHelper.ToRadians(180)));
                hitbox = new Rectangle(hitbox.X + (int)pos.X, hitbox.Y + (int)pos.Y, 100, 100);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 pos = Projectile.Center - Main.screenPosition;
            Vector2 origin = new Vector2(Projectile.direction == 1 ? 184 : 50, 35);
            Rectangle frame = new Rectangle(0, Projectile.frame * 70, 234, 70);
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, pos, new Rectangle?(frame), Color.White, Projectile.rotation, origin, Projectile.scale, Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            return false;
        }
    }

}
