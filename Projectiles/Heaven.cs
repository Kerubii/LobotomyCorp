using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class Heaven : LobcorpSpear
	{
        protected override float HoldoutRangeMax => base.HoldoutRangeMax + 8;

        public override void ProjectileSpawn(int duration)
        {
            if (Projectile.timeLeft == duration * 2 / 3 && Main.myPlayer == Projectile.owner)
            {
                for (int i = -1; i < 2; i += 2)
                {
                    Vector2 vel = Projectile.velocity.RotatedBy(MathHelper.ToRadians(45 * i));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, ModContent.ProjectileType<HeavenBranchPlayer>(), (int)(Projectile.damage * 1.5f), Projectile.knockBack, Projectile.owner, Projectile.whoAmI);
                }
            }
        }
    }
}
