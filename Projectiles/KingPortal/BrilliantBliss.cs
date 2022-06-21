using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles.KingPortal
{
	public class BrilliantBliss : ModProjectile
	{
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Brilliant Bliss");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 26;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 60;

            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }    
            
        public override void AI() {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0]++;

            }
        }
    }
}
