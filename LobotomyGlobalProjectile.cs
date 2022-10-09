using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using LobotomyCorp.Utils;

namespace LobotomyCorp
{
	public class LobotomyGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        //public override bool CloneNewInstances => true;

        public byte Lament = 0;

        public bool BlackSwanReflected = false;

        public override bool PreAI(Projectile projectile)
        {
            if (BlackSwanReflected)
            {
                projectile.rotation += 0.12f;
                return false;
            }
            return base.PreAI(projectile);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (Lament > 0 && LobotomyCorp.LamentValid(target, projectile) && target.CanBeChasedBy(projectile))
            {
                int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), Main.player[projectile.owner].Center, new Vector2(6, 0).RotateRandom(6.28f), ModContent.ProjectileType<Projectiles.Kaleidoscope>(), projectile.damage, projectile.knockBack, projectile.owner, target.whoAmI);
                Main.projectile[p].localAI[0] = Lament;
                SoundStyle ding = new SoundStyle("LobotomyCorp/Sounds/Item/ButterFlyMan_StongAtk_Black");
                int dustType = 91;
                ScreenFilter screenFilter = new Items.Ruina.Technology.SolemnLamentWhite();

                if (Lament == 1)
                {
                    ding = new SoundStyle("LobotomyCorp/Sounds/Item/ButterFlyMan_StongAtk_White");
                    dustType = 109;

                    screenFilter = new Items.Ruina.Technology.SolemnLamentBlack();
                }
                ding.Volume = 0.5f;
                //SoundEngine.PlaySound(ding, target.Center);

                if (projectile.owner == Main.myPlayer)
                    LobCustomDraw.Instance().AddFilter(screenFilter);

                for (int i = 0; i < 3; i++)
                {
                    Vector2 pos = target.position;
                    pos.X += Main.rand.Next(target.width);
                    pos.Y += Main.rand.Next(target.height);
                    int limit = 16;

                    float speed = Main.rand.NextFloat(8f);
                    for (int a = 0; a < limit; a++)
                    {
                        float angle = (float)a / limit * 6.34f;

                        Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                        Dust d = Dust.NewDustPerfect(pos + velocity * speed, dustType, velocity * 2);
                        d.noGravity = true;
                        d.scale = 0.5f;
                    }
                }
            }
        }

        public override void ModifyDamageScaling(Projectile projectile, ref float damageScale)
        {
            if (projectile.owner >= 0 && projectile.owner < Main.maxPlayers)
            {
                LobotomyModPlayer modPlayer = LobotomyModPlayer.ModPlayer(Main.player[projectile.owner]); 
                if (modPlayer.TodaysExpressionActive)
                    damageScale *= modPlayer.TodaysExpressionDamage();
            }
            base.ModifyDamageScaling(projectile, ref damageScale);
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Lament > 0 && LobotomyCorp.LamentValid(target, projectile) && target.CanBeChasedBy(projectile))
            {
                damage = (int)(damage * 1.15f);
            }
        }
    }
}