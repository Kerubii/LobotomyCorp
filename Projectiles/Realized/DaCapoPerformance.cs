using LobotomyCorp.Buffs;
using LobotomyCorp.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static LobotomyCorp.LobotomyCorp;

namespace LobotomyCorp.Projectiles.Realized
{
	public class DaCapoPerformance : ModProjectile
	{
        public override string Texture => "LobotomyCorp/Projectiles/Realized/1stMovement";

        public static Asset<Texture2D> fourthMovement = null;

        public override void Load()
        {
            if (!Main.dedServ && Main.netMode != NetmodeID.Server)
            {
                fourthMovement = Mod.Assets.Request<Texture2D>("Projectiles/Realized/4thmovement", AssetRequestMode.ImmediateLoad);
                Main.QueueMainThreadAction(() =>
                {
                    PremultiplyTexture(fourthMovement.Value);
                });
            }
        }

        public override void Unload()
        {
            fourthMovement = null;
        }

        public override void SetDefaults() {
            Projectile.height = 16;
            Projectile.width = 16;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;

            Projectile.timeLeft = 6252;
			Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
		}

        // Timestamps
        // Clap         -        6.839s/            ( 6.839)     - 410.34
        // Movement 1   -       29.873s/            (23.034)     - 1,792.38
        // Movement 2   -       46.733s/            (16.860)     - 2803.98
        // Movement 3   -       52.498s/            ( 5.765)     - 3149.88
        // Movement 4   - 1m     6.133s/    66.133s (13.635)     - 3967.98
        // Movement 5   - 1m    10.009s/    70.009s (3.876)      - 4200.54
        // Finale       - 1m    14.914s/    74.914s ()           - 4494.84
        //Applause      - 1m    44.205s/   104.205s ()           - 6252.3

        private int Time
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        const int move1 = 410;
        const int move2 = 1792;
        const int move3 = 2804;
        const int move4 = 3150;
        const int move5 = 3968;
        const int movefinale = 4201;
        const int moveclap = 4495;

        public override void AI()
        {
			Player owner = Main.player[Projectile.owner];

            if (Time == 0)
            {
                SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Art/Sym_movment_0_clap"));
            }

            Projectile.ai[0]++;

            string sound = "";
            bool playSound = false;
            switch (Time)
            {
                case move1:
                    sound = "Sym_movment_1_mov1";
                    playSound = true;
                    break;
                case move2:
                    sound = "Sym_movment_2_mov2";
                    playSound = true;
                    break;
                case move3:
                    sound = "Sym_movment_3_mov3";
                    playSound = true;
                    break;
                case move4:
                    sound = "Sym_movment_4_mov4";
                    playSound = true;
                    break;
                case move5:
                    sound = "Sym_movment_5";
                    playSound = true;
                    break;
                case movefinale:
                    sound = "Sym_movment_5_finale";
                    playSound = true;
                    break;
                case moveclap:
                    sound = "Sym_movment_5_clap";
                    playSound = true;
                    break;
                case 6252 - (move1 * 2 / 3):
                    Time = move1 - 1;
                    Projectile.ai[1]++;
                    Projectile.timeLeft += 6252;
                    break;
            }
            if (playSound)
                SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Art/" + sound));

            if (Time < movefinale)
            {
                Projectile.rotation = (movefinale - Time + 10) * MathHelper.ToRadians(0.5f);
            }

            float soundRange = 1000;
            int phase = currentPhase(Time);

            // Apply Debuffs to Nearby Enemies
            foreach (NPC n in Main.npc)
            {
                if (n.active && !n.friendly && !n.dontTakeDamage)
                {
                    float dist = n.Distance(Projectile.Center);
                    if (dist < soundRange)
                    {
                        // Apply Debuffs to here
                        n.AddBuff(ModContent.BuffType<SilentMusic>(), 60, true);
                        n.GetGlobalNPC<LobotomyGlobalNPC>().DaCapoSilentMusicPhase = phase;
                    }
                }
            }

            foreach (Player p in Main.player)
            {
                if (p.active && !p.dead)
                {
                    float dist = p.Distance(Projectile.Center);
                    if (dist < soundRange)
                    {
                        // Apply Debuffs to here
                        p.AddBuff(ModContent.BuffType<SilentMusic>(), 60, true);
                        p.GetModPlayer<LobotomyModPlayer>().DaCapoSilentMusicPhase = phase;
                    }
                }
            }

            Projectile.Center = owner.MountedCenter;
		}

        private int currentPhase(int time)
        {
            int phase = (int)Projectile.ai[1] * 5;
            if (time < move2)
                return phase;
            else if (time < move3)
                return phase + 1;
            else if (time < move4)
                return phase + 2;
            else if (time < move5)
                return phase + 3;
            return phase + 4;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player projOwner = Main.player[Projectile.owner];

            Texture2D currentTex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 baseScale = new Vector2(1f, 1f);
            float rotation = Projectile.rotation;
            Rectangle frame = currentTex.Frame();
            Color baseColor = Color.White * 0.8f;
            
            if (Time > moveclap)
            {
                return false;
            }
            if (Time > movefinale)
            {
                float factor = (Time - movefinale) / ((float)moveclap - movefinale);
                baseScale *= 1f + factor * 8f;
                if (factor < 0.05f)
                {
                    factor = factor / 0.05f;
                    baseScale.X *= 1f + 0.5f * factor;
                    baseScale.Y *= 1f - 0.5f * factor;
                }
                else if (factor < 0.1f)
                {
                    factor = (factor - 0.05f) / 0.05f;
                    baseScale.X *= 1.5f - 0.5f * factor;
                    baseScale.Y *= 0.5f + 0.5f * factor;
                }
                else if (factor < 0.15f)
                {
                    factor = (factor - 0.1f) / 0.05f;
                    baseScale.X *= 1f - 0.9f * factor;
                    baseScale.Y *= 1f + 0.9f * factor;
                }
                else if (factor < 0.2f)
                {
                    factor = (factor - 0.15f) / 0.05f;
                    baseScale.X *= 0.1f + 0.9f * factor;
                    baseScale.Y *= 1.9f - 0.9f * factor;
                }
                else if (factor > 0.22f)
                {
                    baseScale.X *= 1.1f;
                    baseScale.Y *= 0.8f;
                    factor = 1f - ((factor - 0.22f) / 0.2f);
                    baseColor *= Math.Max(factor, 0f);
                }
            }            

            if (Time > move1)
            {
                Color color = baseColor;
                Vector2 scale = baseScale;
                if (Time < move2)
                {
                    float factor = (Time - move1) / ((float)move2 - move1);
                    color *= factor;
                    scale *= 0.5f + 0.5f * factor;
                }
                Main.EntitySpriteDraw(currentTex, Projectile.Center - Main.screenPosition, frame, color, rotation, frame.Size() / 2, scale, 0);
            }
            if (Time > move2)
            {
                Color color = baseColor;
                currentTex = Mod.Assets.Request<Texture2D>("Projectiles/Realized/2ndMovement").Value;
                frame = currentTex.Frame();
                Vector2 scale = baseScale;
                if (Time < 2804)
                {
                    float factor = (Time - move2) / ((float)move3 - move2);
                    color *= factor;
                    scale *= 0.6f + 0.4f * factor;
                }
                Main.EntitySpriteDraw(currentTex, Projectile.Center - Main.screenPosition, frame, color, -rotation, frame.Size() / 2, scale, 0);
            }
            if (Time > move3)
            {
                Color color = baseColor;
                currentTex = Mod.Assets.Request<Texture2D>("Projectiles/Realized/3rdMovement").Value;
                frame = currentTex.Frame();
                Vector2 scale = baseScale * 1.25f;
                if (Time < move4)
                {
                    float factor = (Time - move3) / (move4 - (float)move3);
                    color *= factor;
                    scale *= 0.75f + 0.25f * factor;
                }
                Main.EntitySpriteDraw(currentTex, Projectile.Center - Main.screenPosition, frame, color, rotation, frame.Size() / 2, scale, 0);
            }
            if (Time > move4)
            {
                Color color = baseColor;
                currentTex = fourthMovement.Value;
                frame = currentTex.Frame();
                Vector2 scale = baseScale * 2;
                if (Time < move5)
                {
                    float factor = (Time -move4) / ((float)move5 - move4);
                    color *= factor;
                    scale *= 0.6f + 0.4f * factor;
                }
                Main.EntitySpriteDraw(currentTex, Projectile.Center - Main.screenPosition, frame, color, -rotation, frame.Size() / 2, scale, 0);
            }

            return false;
        }
    }
}