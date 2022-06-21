using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class HomingInstinct : ModProjectile
	{
		public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Spear");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 180;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
        }    
            
        public override void AI() {
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 57, 0, 0);
            }
            int x = (int)(Projectile.Center.X / 16), y = (int)(Projectile.Center.Y / 16);
            Tile tile = Main.tile[x, y];
            if (!tile.HasTile)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(x * 16,y * 16), Projectile.velocity, ModContent.ProjectileType<HomingInstinctBlock>(), 0, 0);
                /*WorldGen.PlaceObject(x, y, Mod.TileType("YellowBrickRoad"));
                if (!(Projectile.velocity.ToRotation() == 0 || Projectile.velocity.ToRotation() == 180))
                    WorldGen.PoundPlatform(x, y);*/
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
    }
}
