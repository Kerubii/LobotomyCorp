using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Chat;
using LobotomyCorp;


namespace LobotomyCorp.NPCs.RedMist
{
	class RedMistMelee : ModProjectile
    {
        public override string Texture => "LobotomyCorp/Projectiles/KingPortal/KingPortal1";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("E.G.O");
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;

            Projectile.hostile = true;
            Projectile.friendly = false;
        }

        public override void AI()
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }

    class RedMistMimicry : ModProjectile
    {
        public override string Texture => "LobotomyCorp/Projectiles/KingPortal/KingPortal1";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mimicry");
        }

        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;

            Projectile.hostile = true;
            Projectile.friendly = false;
        }

        public override void AI()
        {
            Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood)];
            d.noGravity = true;
            d.fadeIn = 1.2f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }

    class RedMistDaCapo : ModProjectile
    {
        public override string Texture => "LobotomyCorp/Projectiles/KingPortal/KingPortal1";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DaCapo");
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;

            Projectile.hostile = true;
            Projectile.friendly = false;
        }

        public override void AI()
        {
            Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Wraith)];
            d.noGravity = true;
            d.fadeIn = 1.2f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
