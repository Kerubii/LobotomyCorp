using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Chat;
using Terraria.Audio;
using LobotomyCorp;
using LobotomyCorp.Utils;
using LobotomyCorp.UI;
using System.Collections.Generic;

namespace LobotomyCorp.NPCs.RedMist
{
    [AutoloadBossHead]
    class RedMist : ModNPC
    {
        public override void Load()
        {
            string tex = "LobotomyCorp/NPCs/RedMist/RedMist_Head_Boss";
            Mod.AddBossHeadTexture(tex + "_2", -1);
            BossHead2 = ModContent.GetModBossHeadSlot(tex + "_2");
            Mod.AddBossHeadTexture(tex + "_3", -1);
            BossHead3 = ModContent.GetModBossHeadSlot(tex + "_3");
            Mod.AddBossHeadTexture(tex + "_4", -1);
            BossHead4 = ModContent.GetModBossHeadSlot(tex + "_4");
        }

        public static int BossHead2 = -1;
        public static int BossHead3 = -1;
        public static int BossHead4 = -1;
        
        public override void BossHeadSlot(ref int index)
        {
            switch(NPC.ai[0])
            {
                case 1:
                    index = BossHead2;
                    break;
                case 2:
                    index = BossHead3;
                    break;
                case 3:
                    index = BossHead4;
                    break;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Red Mist");
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 100;
            NPC.lifeMax = 26000;
            //NPC.noTileCollide = true;
            //NPC.noGravity = true;
            //NPC.damage = 240;
            NPC.defense = 12;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0.0f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.boss = true;
            NPC.timeLeft *= 10000;
            LobotomyGlobalNPC.LNPC(NPC).RiskLevel = (int)RiskLevel.Aleph;
            GoldRushCount = 0;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/PMSecondWarning");
        }

        private RedMistSkeletonHelper skelly;
        private Projectile HeldProjectile => Main.projectile[(int)NPC.ai[3]];

        public int GoldRushCount = 0;

        public override void AI()
        {
            if (skelly == null)
            {
                skelly = new RedMistSkeletonHelper
                        (new Vector2(NPC.Center.X, NPC.position.Y + NPC.height),
                         new Vector2(0, -72),
                         new Vector2(26, -14),
                         new Vector2(26, 2),
                         12,
                         16,
                         new Vector2(-4, -10),
                         new Vector2(-4, 2),
                         30,
                         42);
            }
            if (!NPC.HasValidTarget)
            {
                NPC.TargetClosest();
            }
            Vector2 target = NPC.Center;/*
            if (Main.player[NPC.target].dead && NPC.timeLeft > 2)
            {
                NPC.timeLeft = 2;
                NPC.position = Vector2.Zero;
                return;
            }*/
            target = NPC.GetTargetData().Center;
            float healthPercent = NPC.life / (float)NPC.lifeMax;
            
            //NPC.spriteDirection = 1;
            //ChangeAnimation(AnimationState.Phase3Transition);
            //return;
            //return;
            /*if (NPC.ai[0] == 0)
            {
                NPC.ai[0] = 2;
                NPC.ai[1] = 10;
                NPC.ai[3] = -1;
                NPC.localAI[1] = 2;
                ChangeAnimation(AnimationState.Phase4Transition);
                //Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<RedMistEye>(), 0, 0, 0, NPC.whoAmI);
            }*/

            //First Phase - Lobcorp Phase
            if (NPC.ai[0] == 0)
            {
                if (Main.expertMode)
                    foreach (Player p in Main.player)
                    {
                        if (p.active && !p.dead)
                        {
                            //p.AddBuff(ModContent.BuffType<Buffs.Hopeless>(), 60, true);
                        }
                    }

                //Follow Mode
                if (NPC.ai[1] == 0) 
                {
                    if (NPC.velocity.Y == 0)
                    {
                        if (NPC.velocity.X == 0)
                            ChangeAnimation(AnimationState.Idle1);
                        else
                            ChangeAnimation(AnimationState.Walk1);
                    }
                    else
                        ChangeAnimation(AnimationState.MidAir);

                    fighterAI(NPC.GetTargetData(), 64f, 0.08f, 2f);
                    Vector2 delta = NPC.GetTargetData().Center - NPC.Center;

                    if (NPC.ai[2] > 0)
                        NPC.ai[2]--;

                    if (NPC.ai[2] <= 0)
                    {
                        if (delta.Length() < 128f)
                        {
                            if (Main.rand.Next(30) == 0)
                            {
                                int atk = Main.rand.Next(3);
                                switch (atk)
                                {
                                    case 0:
                                        Talk("RedEyes");
                                        break;
                                    case 1:
                                        Talk("Penitence");
                                        break;
                                    case 2:
                                        Talk("Both");
                                        break;
                                }

                                NPC.ai[1] = 1 + atk; //Swings takes 50 frames 
                            }
                        }
                        else if (delta.Length() > 2000f && Main.rand.Next(360) == 0)
                        {
                            NPC.ai[1] = 8;
                        }
                    }

                    if ((healthPercent <= .91f && GoldRushCount <= 0) ||
                        (healthPercent <= .83f && GoldRushCount <= 1) ||
                        (healthPercent <= .75f && GoldRushCount <= 2))
                    {
                        NPC.ai[1] = 4;
                        NPC.ai[2] = 0;
                        Talk("GoldRush1");
                    }

                    if (healthPercent <= .75f && GoldRushCount >= 3)
                    {
                        NPC.ai[1] = 10;
                        NPC.ai[2] = 0;
                    }
                }
                //EGO Swing
                else if (0 < NPC.ai[1] && NPC.ai[1] <= 3)
                {
                    NPC.velocity.X *= 0;
                    switch (NPC.ai[1])
                    {
                        case 1:
                            ChangeAnimation(AnimationState.SwingRedEyes);
                            break;
                        case 2:
                            ChangeAnimation(AnimationState.SwingPenitence);
                            break;
                        case 3:
                            ChangeAnimation(AnimationState.SwingBoth);
                            break;
                    }
                    NPC.ai[2]++;
                    if (20 < NPC.ai[2] && NPC.ai[2] < 30)
                    {
                        if (NPC.ai[1] != 2)
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), skelly.Weapon1.EndPoint(NPC.spriteDirection), Vector2.Zero, ModContent.ProjectileType<RedMistMelee>(), 25, 2);
                        if (NPC.ai[1] != 1)
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), skelly.Weapon2.EndPoint(NPC.spriteDirection), Vector2.Zero, ModContent.ProjectileType<RedMistMelee>(), 25, 2);
                    }
                    if (NPC.ai[2] >= 50)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 30;
                    }
                    if (NPC.ai[2] == 20)
                    {
                        SoundEngine.PlaySound(SoundID.Item1, NPC.Center);
                    }
                }
                //GoldRush Start
                else if (NPC.ai[1] == 4)
                {
                    GoldRushCount++;
                    NPC.velocity *= 0;
                    NPC.spriteDirection *= -1;
                    NPC.ai[1]++;
                    ChangeAnimation(AnimationState.GoldRushIntro);
                    NPC.noGravity = true;
                    NPC.noTileCollide = true;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(320 * NPC.spriteDirection, 0), Vector2.Zero, ModContent.ProjectileType<Projectiles.KingPortal.GoldRushPortal>(), 0, 0, 0, -1, 5);
                }
                //GoldRush Charging
                else if (NPC.ai[1] == 5)
                {
                    NPC.ai[2]++;
                    if (NPC.ai[2] > 30)
                    {
                        NPC.ai[2] = 30 * 5 + 15;
                        NPC.ai[1]++;
                        NPC.velocity.X = 24f * NPC.spriteDirection;
                        NPC.velocity.Y = 0;
                    }
                }
                //GoldRush Charge
                else if (NPC.ai[1] == 6)
                {
                    NPC.damage = 65;
                    NPC.ai[2]--;
                    if (NPC.ai[2] <= 0)
                    {
                        ChangeAnimation(AnimationState.GoldRushEnd);
                        NPC.ai[1]++;
                        NPC.ai[2] = 0;
                        NPC.noGravity = false;
                        NPC.noTileCollide = false;
                    }

                    Dust d = Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(NPC.position.X, NPC.position.X + NPC.width), NPC.position.Y + NPC.height), 57);
                    d.fadeIn = 1.4f;
                    d.noGravity = true;
                }
                //GoldRush End
                else if (NPC.ai[1] == 7)
                {
                    NPC.damage = 0;
                    NPC.velocity.X *= 0.9f;
                    NPC.ai[2]++;
                    if (NPC.ai[2] > 120)
                    {
                        ChangeAnimation(AnimationState.Idle1);
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;
                    }
                }
            
                //Mini GoldRush
                else if (NPC.ai[1] == 8)
                {
                    NPC.velocity *= 0;
                    NPC.ai[1]++;
                    ChangeAnimation(AnimationState.GoldRushIntro);
                    NPC.noGravity = true;
                    NPC.noTileCollide = true;
                    int i = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.GetTargetData().Center + new Vector2(400 * NPC.spriteDirection, Main.LocalPlayer.height / 2 - 50), new Vector2(-22f * NPC.spriteDirection, 0), ModContent.ProjectileType<Projectiles.KingPortal.GoldRushPortal>(), 0, 0, 0, -2);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(320 * NPC.spriteDirection, 0), Vector2.Zero, ModContent.ProjectileType<Projectiles.KingPortal.GoldRushPortal>(), 0, 0, 0, i);
                }
                //Mini GoldRush Charge
                else if (NPC.ai[1] == 9)
                {
                    NPC.ai[2]++;
                    if (NPC.ai[2] > 30)
                    {
                        NPC.ai[2] = 45;
                        NPC.ai[1] = 6;
                        NPC.velocity.X = 24f * NPC.spriteDirection;
                    }
                }

                //Transition to Phase 2
                else if (NPC.ai[1] == 10)
                {
                    ChangeAnimation(AnimationState.Phase2Transition);
                    NPC.ai[2]++;
                    NPC.velocity.X = 0;

                    if (NPC.ai[2] == 30)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<RedMistEye>(), 0, 0, 0, NPC.whoAmI);
                    }

                    if (NPC.ai[2] > 60)
                    {
                        NPC.ai[0]++;
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;
                        NPC.ai[3] = -1;
                        NPC.netUpdate = true;
                    }
                }
            }
            //Second Phase
            //Changes to Mimicry and DaCapo
            //Allows Heaven Strikes as anti air
            //Focused on closing her distance between you and her
            //Allows DaCapo toss as anti range
            //Dashes through incoming Projectiles, gaining Projectile invincibility
            //Teleports to DaCapo and unleashes a 180 degree slash depending on player direction
            //Range needed to deal full damage shrinks
            else if (NPC.ai[0] == 1)
            {
                if (NPC.ai[1] == 0)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        if (NPC.velocity.X == 0)
                            ChangeAnimation(AnimationState.Idle2);
                        else
                            ChangeAnimation(AnimationState.Walk2);
                    }
                    else
                        ChangeAnimation(AnimationState.MidAir);

                    fighterAI(NPC.GetTargetData(), 64f, 0.08f, 5f);
                    Vector2 delta = NPC.GetTargetData().Center - NPC.Center;

                    if (NPC.ai[2] > 0)
                        NPC.ai[2]--;

                    if (NPC.ai[2] <= 0)
                    {
                        if (delta.Length() < 128f && Main.rand.Next(30) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Talk("Mimicry" + (1 + Main.rand.Next(2)));
                                NPC.ai[1] = 1;
                                ChangeAnimation(AnimationState.SwingMimicry);
                                NPC.velocity.X = 0;
                            }
                            else if (NPC.ai[3] < 0)
                            {
                                Talk("DaCapo" + (1 + Main.rand.Next(2)));
                                NPC.ai[1] = 2;
                                ChangeAnimation(AnimationState.SwingDaCapo);
                                NPC.velocity.X = 0;
                            }
                        }

                        if ((delta.Length() > 600 && Main.rand.Next(240) == 0) ||
                            (Main.rand.Next(2400) == 0) ||
                            (delta.Y < -150 && Main.rand.Next(200) == 0))
                        {
                            NPC.ai[1] = 4;
                            ChangeAnimation(AnimationState.HeavenThrow);
                            Talk("Heaven");
                            NPC.velocity.X = 0;
                        }
                    }

                    if (delta.Length() > 460)
                    {
                        if (delta.Length() < 600 && Main.rand.Next(200) == 0)
                        {
                            NPC.ai[1] = 6;
                            NPC.ai[2] = 30;
                            NPC.velocity.Y *= 0;
                            NPC.velocity.X = 18 * NPC.spriteDirection;
                            ChangeAnimation(AnimationState.Dash);
                        }

                        if (IncomingProjectile())
                        {
                            NPC.ai[1] = 5;
                            NPC.ai[2] = 30;
                            NPC.velocity.Y *= 0;
                            NPC.velocity.X = 18 * NPC.spriteDirection;
                            ChangeAnimation(AnimationState.Dash);
                        }
                    }

                    if (NPC.ai[3] < 0 &&
                        ((healthPercent <= .66f && GoldRushCount <= 3) ||
                        (healthPercent <= .58f && GoldRushCount <= 4) ||
                        (healthPercent <= .50f && GoldRushCount >= 5)))
                    {
                        NPC.ai[1] = 7;
                        NPC.ai[2] = 0;
                        NPC.velocity *= 0;
                        if (healthPercent <= .50f)
                        {
                            NPC.ai[1] = 10;
                            Talk("Shift1");
                        }
                        else
                        {
                            GoldRushCount++;
                            Talk("Teleport1");
                        }
                    }
                }
                //Mimicry Swing
                else if (NPC.ai[1] == 1)
                {
                    ChangeAnimation(AnimationState.SwingMimicry);
                    NPC.velocity.X *= 0.9f;
                    NPC.ai[2]++;
                    if (60 < NPC.ai[2] && NPC.ai[2] < 67)
                    {
                        Vector2 position = skelly.Weapon1.Position(NPC.spriteDirection) + new Vector2(100, 0).RotatedBy(skelly.Weapon1.GetRotation(NPC.spriteDirection));

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), position, Vector2.Zero, ModContent.ProjectileType<RedMistMimicry>(), 50, 2);
                    }

                    if (NPC.ai[2] == 60)
                    {
                        SoundEngine.PlaySound(SoundID.Item1, NPC.Center);
                    }

                    if (NPC.ai[2] > 90)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 60;
                    }
                }
                //DaCapo Swing
                else if (NPC.ai[1] == 2)
                {
                    NPC.ai[2]++;
                    NPC.velocity.X *= 0.9f;
                    if (NPC.ai[2] > 20 && NPC.ai[2] < 90 && NPC.ai[2] % 30 < 10)
                    {
                        Vector2 position = skelly.Weapon2.Position(NPC.spriteDirection) + new Vector2(40, 0).RotatedBy(skelly.Weapon2.GetRotation(NPC.spriteDirection));

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), position, Vector2.Zero, ModContent.ProjectileType<RedMistDaCapo>(), 15, 0);
                    }
                    if (NPC.ai[2] == 90) //Throw DaCapo when enemy is at certain distance
                    {
                        Vector2 delta = NPC.GetTargetData().Center - NPC.Center;
                        if (delta.Length() > 260)
                        {
                            NPC.ai[1] = 3;
                            ChangeAnimation(AnimationState.DaCapoThrow);
                            NPC.ai[2] = 0;
                        }
                    }
                    if (NPC.ai[2] > 100)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 30;
                    }

                    if (NPC.ai[2] == 1 || NPC.ai[2] == 30 || NPC.ai[2] == 60)
                    {
                        SoundEngine.PlaySound(SoundID.Item1, NPC.Center);
                    }
                }
                //DaCapo Normal Throw
                else if (NPC.ai[1] == 3)
                {
                    NPC.velocity.X *= 0.9f;
                    Vector2 delta = NPC.GetTargetData().Center - NPC.Center;
                    NPC.ai[2]++;
                    if (NPC.ai[2] == 50)
                    {
                        NPC.ai[3] = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Normalize(delta) * 24f, ModContent.ProjectileType<DaCapoThrow>(), 20, 2, 0, NPC.whoAmI);
                        SoundEngine.PlaySound(SoundID.Item19, NPC.Center);
                    }
                    if (NPC.ai[2] > 100)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 30;
                        ChangeAnimation(AnimationState.Idle2);
                        skelly.Weapon1.Visible = true;
                    }
                }
                //Heaven Throw
                else if (NPC.ai[1] == 4)
                {
                    NPC.ai[2]++;
                    NPC.velocity.X *= 0.9f;
                    if (NPC.ai[2] == 50)
                    {
                        Vector2 delta = NPC.GetTargetData().Center - NPC.Center + new Vector2(0, -13);
                        if (Math.Sign(delta.X) != Math.Sign(NPC.spriteDirection) || delta.Y > 0)
                        {
                            delta = new Vector2(1 * NPC.spriteDirection, 0);
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -13), Vector2.Normalize(delta) * 12f, ModContent.ProjectileType<HeavenBoss>(), 25, 2, 0, NPC.whoAmI);
                        SoundEngine.PlaySound(SoundID.Item19, NPC.Center);
                    }
                    if (NPC.ai[2] > 80)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 30;
                    }
                }
                //Dash
                else if (NPC.ai[1] == 5)
                {
                    ChangeAnimation(AnimationState.Dash);
                    NPC.ai[2]--;
                    NPC.noTileCollide = true;
                    NPC.noGravity = true;
                    if (NPC.ai[2] < 10)
                    {
                        NPC.velocity.X *= 0.9f;
                    }
                    if (NPC.ai[2] <= 0)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;

                        NPC.noTileCollide = false;
                        NPC.noGravity = false;
                    }
                }            
                //Dash To Mimicry
                else if (NPC.ai[1] == 6)
                {
                    ChangeAnimation(AnimationState.Dash);
                    NPC.ai[2]--;
                    NPC.noTileCollide = true;
                    NPC.noGravity = true;
                    if (NPC.ai[2] < 10)
                    {
                        NPC.velocity.X *= 0.9f;
                    }
                    if (NPC.ai[2] <= 0)
                    {
                        NPC.ai[1] = 1;
                        NPC.ai[2] = 0;

                        NPC.noTileCollide = false;
                        NPC.noGravity = false;
                    }
                }

                //DaCapo Teleport Throw
                else if (NPC.ai[1] == 7)
                {
                    ChangeAnimation(AnimationState.DaCapoThrow);
                    NPC.velocity.X *= 0.9f;
                    Vector2 delta = NPC.GetTargetData().Center - NPC.Center;
                    NPC.ai[2]++;
                    if (NPC.ai[2] == 50)
                        NPC.ai[3] = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, delta /= 30, ModContent.ProjectileType<DaCapoLegato>(), 20, 2, 0, NPC.whoAmI);
                    if (NPC.ai[2] > 100)
                    {
                        NPC.ai[1] = 8;
                        NPC.ai[2] = 0;
                        skelly.Weapon1.Visible = true;
                        Talk("Teleport2");
                    }
                }
                //Mimicry Prepare
                else if (NPC.ai[1] == 8)
                {
                    ChangeAnimation(AnimationState.MimicryPrepare);
                    NPC.ai[2]++;
                    if (NPC.ai[2] >= 120)
                    {
                        NPC.ai[1] = 9;
                        NPC.ai[2] = 0;
                        Projectile p = Main.projectile[(int)NPC.ai[3]];
                        NPC.Center = p.Center;// + new Vector2(200, 0).RotateRandom(6.28f);
                        p.Kill();
                        NPC.ai[3] = -1;
                        Talk("Teleport3");
                    }
                }
                //Mimicry GreaterSplit!
                else if (NPC.ai[1] == 9)
                {
                    NPC.velocity *= 0;
                    NPC.noGravity = true;
                    ChangeAnimation(AnimationState.MimicryGreaterSplitV);
                    NPC.ai[2]++;
                    if (NPC.ai[2] >= 60)
                    {
                        NPC.noGravity = false;
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;
                        NPC.velocity.Y = -8f;
                    }
                }
                //Transition Phase
                else if (NPC.ai[1] == 10)
                {
                    ChangeAnimation(AnimationState.Phase3Transition);
                    NPC.ai[2]++;
                    if (NPC.ai[2] == 60)
                    {
                        //Shoot Mimicry forwards and Dacapo backwards
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(6 * NPC.spriteDirection, 0), ModContent.ProjectileType<SwitchMimicryThrow>(), 20, 0);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(-6 * NPC.spriteDirection, 0), ModContent.ProjectileType<SwitchDaCapoThrow>(), 20, 0);
                    }
                    if (NPC.ai[2] == 130)
                    {
                        Talk("Shift2");
                    }
                    if (NPC.ai[2] > 160)
                    {
                        NPC.ai[0]++;
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;
                    }
                }
            }
            //Third Phase
            //Throw Mimicry and DaCapo forward and equip Smile and Justitia
            //Back to normal walking speed
            //Smile forces all players to plummet down if in midair or go through platforms if above
            //Smile inflicts "Horrifying Screech", disabling wings and lowers movement speed by 8%
            //Smile second hit releases shockwaves with infinite range that is blocked by direct line of sight, signified by black dust hitting tiles
            //Getting hit by Smile shockwave reduces movement by 92% for 2 seconds
            //Justitia Releases Energy Slashes, sometimes two or three hit combos, has a higher end lag when doing three hit combos
            //Gold Rush has random center positions with different angles, ending with a strike from above
            //Range needed to deal full damage expands
            else if (NPC.ai[0] == 2)
            {
                if (NPC.ai[1] == 0)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        if (NPC.velocity.X == 0)
                            ChangeAnimation(AnimationState.Idle3);
                        else
                            ChangeAnimation(AnimationState.Walk3);
                    }
                    else
                        ChangeAnimation(AnimationState.MidAir);

                    fighterAI(NPC.GetTargetData(), 90, 0.08f, 2f);

                    Vector2 delta = NPC.GetTargetData().Center - NPC.Center;

                    if (NPC.ai[2] > 0)
                        NPC.ai[2]--;

                    if (NPC.ai[2] <= 0 && Main.rand.Next(60) == 0)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            NPC.velocity.X *= 0;
                            //Justitia Attack, up to three just to catch people offguard
                            NPC.ai[1] = 1;
                            NPC.ai[2] = 0;
                            ChangeAnimation(AnimationState.JustitiaSwing);

                            Talk("Justitia" + Main.rand.Next(1, 3));
                        }
                        else
                        {
                            //Smile Attack
                            NPC.velocity.X *= 0;
                            NPC.ai[1] = 4;
                            NPC.ai[2] = 0;
                            ChangeAnimation(AnimationState.SmileSwing);
                            Talk("Smile" + Main.rand.Next(1, 3));
                        }

                        //Heaven Attack
                        if ((delta.Length() > 600 && Main.rand.Next(240) == 0) ||
                            (Main.rand.Next(2400) == 0) ||
                            (delta.Y < -150 && Main.rand.Next(200) == 0))
                        {
                            NPC.ai[1] = 4;
                            ChangeAnimation(AnimationState.HeavenThrow);
                            Talk("Heaven");
                            NPC.velocity.X = 0;
                        }
                    }

                    //Gold Rush!
                    
                    if ((healthPercent <= .42f && GoldRushCount <= 5) ||
                        (healthPercent <= .33f && GoldRushCount <= 6))
                    {
                        NPC.ai[1] = 6;//Gold Rush!
                        NPC.ai[2] = 0;
                        NPC.velocity *= 0;
                        GoldRushCount++;
                        Talk("GoldRush1");
                        ChangeAnimation(AnimationState.GoldRushIntro);
                    }

                    if (healthPercent <= .25f && GoldRushCount <= 7)
                    {
                        NPC.ai[1] = 10;
                        NPC.ai[2] = 0;
                        GoldRushCount++;
                        Talk("Shift3");
                        ChangeAnimation(AnimationState.Phase4Transition);
                    }
                }
                //Justitia Slashes
                else if (NPC.ai[1] > 0 && NPC.ai[1] <= 3)
                {
                    NPC.ai[2]++;
                    if (NPC.ai[2] > 30 && NPC.ai[2] % 45 == 0)
                    {
                        //Shoot Justitia Slashes
                        Vector2 delta = NPC.GetTargetData().Center - NPC.Center;
                        delta.Normalize();
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, 8 * delta, ModContent.ProjectileType<JustitiaSlashBoss>(), 15, 0);
                        if (NPC.ai[1] > 1)
                            NPC.ai[1]--;
                    }
                    if (NPC.ai[1] == 1)
                    {
                        if (NPC.ai[2] % 90 >= 59)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 100 * (int)(NPC.ai[2] / 60);
                        }
                    }
                }
                //Smile Quake
                else if (NPC.ai[1] == 4)
                {
                    NPC.ai[2]++;
                    if (NPC.ai[2] == 40)
                    {
                        //Push down all players and apply debuff
                    }
                    else if (NPC.ai[2] > 90)
                    {
                        if (NPC.ai[2] % 2 == 0)
                        {
                            //Damage Everything Nearby
                        }
                    }

                    if (NPC.ai[2] == 70)
                    {
                        Vector2 hammerPos = skelly.Weapon2.Position(NPC.spriteDirection) + new Vector2(70, 0).RotatedBy(skelly.Weapon2.GetRotation(NPC.spriteDirection));
                        foreach (Player p in Main.player)
                        {
                            if (p.active && !p.dead && Collision.CanHit(hammerPos, 1, 1, p.position, p.width, p.height))
                            {
                                p.AddBuff(ModContent.BuffType<Buffs.Scream>(), 300);
                                p.velocity.Y = 6;
                            }
                        }
                        
                        for (int i = 0; i < 24; i++)
                        {
                            Vector2 vel = new Vector2(Main.rand.NextFloat(10f, 20f), 0).RotatedBy(Main.rand.NextFloat(0.34f) + MathHelper.ToRadians(-90 + 45 * NPC.spriteDirection));
                            Dust d = Dust.NewDustPerfect(hammerPos, DustID.Wraith, vel);
                            d.noGravity = true;
                        }
                    }
                    else if (NPC.ai[2] >= 160 && NPC.ai[2] < 240)
                    {
                        Vector2 hammerPos = skelly.Weapon2.Position(NPC.spriteDirection) + new Vector2(70, 0).RotatedBy(skelly.Weapon2.GetRotation(NPC.spriteDirection));
                        /*if (NPC.ai[2] % 10 == 0)
                        {
                            foreach (Player p in Main.player)
                            {
                                if (p.active && !p.dead && Collision.CanHit(hammerPos, 1, 1, p.position, p.width, p.height))
                                    p.AddBuff(ModContent.BuffType<Buffs.Vomit>(), 60);
                            }
                        }*/

                        if (NPC.ai[2] % 5 == 0)
                        {
                            float random = Main.rand.NextFloat(1.00f);
                            for (int i = 0; i < 28; i++)
                            {
                                /*Vector2 vel = new Vector2(16, 0).RotatedBy(random + MathHelper.ToRadians(11.25f * i));
                                Vector2 dustPos = new Vector2(70, 0).RotatedBy(skelly.Weapon2.GetRotation(NPC.spriteDirection));
                                Dust d = Dust.NewDustPerfect(hammerPos, DustID.Wraith, vel);
                                d.noGravity = true;*/

                                Vector2 vel = new Vector2(8, 0).RotatedBy(random + MathHelper.ToRadians(11.25f * i));
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), hammerPos, vel, ModContent.ProjectileType<Projectiles.SmileBits>(), 16, 0, 255, Main.rand.NextFloat(0.26f));
                            }
                        }
                    }

                    if (NPC.ai[2] > 270)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 180;
                    }
                }
                //Heaven Throw
                else if (NPC.ai[1] == 5)
                {
                    ChangeAnimation(AnimationState.HeavenThrow);
                    NPC.ai[2]++;
                    NPC.velocity.X *= 0.9f;
                    if (NPC.ai[2] == 50)
                    {
                        Vector2 delta = NPC.GetTargetData().Center - NPC.Center + new Vector2(0, -13);
                        if (Math.Sign(delta.X) != Math.Sign(NPC.spriteDirection) || delta.Y > 0)
                        {
                            delta = new Vector2(1 * NPC.spriteDirection, 0);
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -13), Vector2.Normalize(delta) * 12f, ModContent.ProjectileType<HeavenBoss>(), 25, 2, 0, NPC.whoAmI);
                        SoundEngine.PlaySound(SoundID.Item19, NPC.Center);
                    }
                    if (NPC.ai[2] > 80)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 30;
                    }
                }
                //GoldRush Start
                else if (NPC.ai[1] == 6)
                {
                    //GoldRushCount++;
                    NPC.velocity *= 0;
                    NPC.spriteDirection *= -1;
                    NPC.ai[1]++;
                    ChangeAnimation(AnimationState.GoldRushIntro);
                    NPC.noGravity = true;
                    NPC.noTileCollide = true;
                    //Spawn Portals MANUALLY AAAAAA
                    int portals = 8;
                    int currentPortal = -1;
                    float currentRotation = 0;
                    //Doesnt work because of portal intersecting :(
                    Vector2 targetPos = NPC.Center;
                    /*
                    for (int i = 0; i < portals; i++)
                    {
                        if (currentPortal == -1)
                        {
                            currentRotation = Main.rand.NextFloat((float)Math.PI * 2);
                            currentPortal = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.GetTargetData().Center + new Vector2(400, 0).RotatedBy(currentRotation), new Vector2(-22, 0).RotatedBy(currentRotation), ModContent.ProjectileType<GoldRushPortal>(), 0, 0, 0, -1, 5);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPos + new Vector2(320 * NPC.spriteDirection, 0), Vector2.Zero, ModContent.ProjectileType<GoldRushPortal>(), 0, 0, 0, currentPortal, 5);
                            continue;
                        }
                        if (i == portals - 1)
                        {
                            int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.GetTargetData().Center + new Vector2(-400, 0).RotatedBy(currentRotation), new Vector2(-22, 0).RotatedBy(currentRotation), ModContent.ProjectileType<GoldRushPortal>(), 0, 0, 0, -2, 5);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.GetTargetData().Center + new Vector2(0, -320), new Vector2(0, -22), ModContent.ProjectileType<GoldRushPortal>(), 0, 0, 0, -2, 5);
                            Main.Projectile[p].ai[0] = currentPortal;
                            Main.Projectile[p].netUpdate = true;
                        }
                        else
                        {
                            int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.GetTargetData().Center + new Vector2(-400, 0).RotatedBy(currentRotation), new Vector2(-22, 0).RotatedBy(currentRotation), ModContent.ProjectileType<GoldRushPortal>(), 0, 0, 0, -2, 5);
                            currentRotation = Main.rand.NextFloat((float)Math.PI * 2);
                            currentPortal = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.GetTargetData().Center + new Vector2(400, 0).RotatedBy(currentRotation), new Vector2(-22, 0).RotatedBy(currentRotation), ModContent.ProjectileType<GoldRushPortal>(), 0, 0, 0, currentPortal, 5);
                            Main.Projectile[p].ai[0] = currentPortal;
                            Main.Projectile[p].netUpdate = true;
                        }
                    }*/
                    //One at a time version
                    /*
                    Also Kinda bad
                    currentRotation = Main.rand.NextFloat((float)Math.PI * 2);
                    currentPortal = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.GetTargetData().Center + new Vector2(320, 0).RotatedBy(currentRotation), new Vector2(-22, 0).RotatedBy(currentRotation), ModContent.ProjectileType<GoldRushPortal>(), 0, 0, 0, currentPortal, 5);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPos + new Vector2(320 * NPC.spriteDirection, 0), Vector2.Zero, ModContent.ProjectileType<GoldRushPortal>(), 0, 0, 0, currentPortal, 5);
                    */
                    //Default
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(320 * NPC.spriteDirection, 0), Vector2.Zero, ModContent.ProjectileType<Projectiles.KingPortal.GoldRushPortal>(), 0, 0, 0, -3, 8);
                }
                //GoldRush Charging
                else if (NPC.ai[1] == 7)
                {
                    NPC.ai[2]++;
                    if (NPC.ai[2] > 30)
                    {
                        NPC.ai[2] = 30 * 8;// + 15;
                        NPC.ai[1]++;
                        NPC.velocity.X = 24f * NPC.spriteDirection;
                        NPC.velocity.Y = 0;
                    }
                }
                //GoldRush Charge
                else if (NPC.ai[1] == 8)
                {
                    NPC.damage = 65;
                    NPC.ai[2]--;
                    if (NPC.ai[2] <= 0)
                    {
                        ChangeAnimation(AnimationState.GoldRushEnd);
                        NPC.ai[1]++;
                        NPC.ai[2] = 0;
                        NPC.noGravity = false;
                        NPC.noTileCollide = false;
                    }
                    /*
                    if (NPC.ai[2] % 45 == 0)
                    {
                        float currentRotation = Main.rand.NextFloat((float)Math.PI * 2);
                        int currentPortal = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.GetTargetData().Center + new Vector2(600, 0).RotatedBy(currentRotation), new Vector2(-22, 0).RotatedBy(currentRotation), ModContent.ProjectileType<GoldRushPortal>(), 0, 0, 0, -1, 5);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(400, 0).RotatedBy(NPC.velocity.ToRotation()), new Vector2(-22, 0).RotatedBy(NPC.velocity.ToRotation()), ModContent.ProjectileType<GoldRushPortal>(), 0, 0, 0, currentPortal, 5);
                    }*/

                    Dust d = Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(NPC.position.X, NPC.position.X + NPC.width), NPC.position.Y + NPC.height), 57);
                    d.fadeIn = 1.4f;
                    d.noGravity = true;
                }
                //GoldRush End
                else if (NPC.ai[1] == 9)
                {
                    NPC.damage = 0;
                    NPC.velocity.X *= 0.9f;
                    NPC.ai[2]++;
                    if (NPC.ai[2] > 120)
                    {
                        ChangeAnimation(AnimationState.Idle1);
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;
                    }
                }
            
                //Phase4Transition
                else if (NPC.ai[1] == 10)
                {
                    NPC.velocity.X = 0;
                    NPC.ai[2]++;
                    if (NPC.ai[2] == 120)
                    {
                        Talk("Shift4");
                    }
                    if (NPC.ai[2] > 130)
                    {
                        NPC.ai[0] = 3;
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;
                    }    
                }
            }
            //Instead of using Gold Rush a third time before transitioning, begins transition immediently
            //Smile gets used one last time, Justitia transforms into Twilight
            //Gold Rush gets summoned, and thrown as a Projectile with half the speed of red mist, creating random barriers all around the player
            //Red Mist gains semi flight with low gravity, runs as normal, can dash
            //Red Mist leaves behind numerous slashes when dashing
            //Red Mist deals NO contact damage at all
            //Red Mist relies on passing through players
            //Red Mist sometimes enters a portal to pass through it
            //Red Mist sometimes stops and redirects her velocity towards the player
            //After 25 seconds, she stops, falls to the ground, and lays motionless for 10 seconds with increased damage taken, before attacking again
            else if (NPC.ai[0] == 3)
            {

            }
            //Hidden Red Mist Phase??
            //Hold Apocalypse up high, envelopes the area in darkness as it dissipates, red mist suddenly gets a burst of red mist, as Mimicry is summoned and blazes of red flames replaces her hair and gains a burning red cloak
            //Can throw mimicry spear form multiple times when player is away
            //Can Transform mimicry into a scythe to for wide slashes,
            //Can Transform mimicry spear form to poke
            //Can Transform mimicry hammer form as heavy hitter


            //Music Changer???
            if (NPC.ai[0] > 0)
            {
                if (NPC.ai[0] < 3)
                    Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/TilaridsDistortedNight");
                else
                    Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/TilaridsInsigniaDecay");
            }

            //Suppresion Text Maker
            if (Main.rand.Next(300) == 0)
            {
                List<string> possibleText = new List<string>();
                if (Main.rand.Next(2) == 0)
                {
                    possibleText.Add(SuppressionTextData.Gebura[0]);
                    possibleText.Add(SuppressionTextData.Gebura[1]);
                    possibleText.Add(SuppressionTextData.Gebura[2]);
                    possibleText.Add(SuppressionTextData.Gebura[3]);
                }

                if (NPC.ai[0] == 0)
                {
                    possibleText.Add(SuppressionTextData.Gebura[4]);
                    possibleText.Add(SuppressionTextData.Gebura[5]);
                }

                else if (NPC.ai[0] == 1)
                {
                    possibleText.Add(SuppressionTextData.Gebura[7]);
                    possibleText.Add(SuppressionTextData.Gebura[8]);
                }

                else if (NPC.ai[0] == 2)
                {
                    possibleText.Add(SuppressionTextData.Gebura[9]);
                    possibleText.Add(SuppressionTextData.Gebura[10]);
                }

                else if (NPC.ai[0] == 3)
                {
                    possibleText.Add(SuppressionTextData.Gebura[11]);
                }
                Vector2 offsetRandom = new Vector2(Main.rand.Next(-1000, 1000), Main.rand.Next(-1000, 1000));
                Color textColor = Color.Red;
                textColor *= 0.5f;
                SuppressionText.AddText(possibleText[Main.rand.Next(possibleText.Count)], NPC.Center + offsetRandom, Main.rand.NextFloat(-0.5000f, 0.5000f), 2f, textColor, 0.2f, 240, Main.rand.Next(-1, 2), Main.rand.NextFloat(1.000f));
            }
        }

        public override void FindFrame(int frameHeight)
        {
            //The Animation part for pure pain
            if (skelly != null)
            {
                Vector2 origin = new Vector2(NPC.Center.X, NPC.position.Y + NPC.height);
                skelly.Origin.BoneOffset = origin;
                //skelly.CalculateHandIK();
                bool LookAtPlayer = true;
                NPC.frameCounter++;
                if (animState == AnimationState.Idle1)
                {
                    skelly.Weapon1.Visible = true;
                    skelly.Weapon2.Visible = true;
                    skelly.Gauntlet.Visible = false;

                    skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                    skelly.CalculateLegIK();

                    skelly.HandLIK.ChangeBoneOffset(new Vector2(-4, skelly.ArmLength() - 1));
                    skelly.Weapon1.rotation = skelly.LowerArmL.rotation + MathHelper.ToRadians(-75);// * NPC.spriteDirection);
                    skelly.HandRIK.ChangeBoneOffset(new Vector2(4, skelly.ArmLength() - 1));
                    skelly.Weapon2.rotation = skelly.LowerArmR.rotation + MathHelper.ToRadians(-45);// * NPC.spriteDirection);

                    skelly.CalculateHandIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                    skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90 + 2 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(90));
                }
                else if (animState == AnimationState.Walk1)
                {
                    float x = 16 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4));// * NPC.spriteDirection;
                    float y = 6 * (float)Math.Cos(MathHelper.ToRadians((float)NPC.frameCounter * 4));
                    if (y < 0)
                        y = 0;
                    skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12 + x, y * -1));
                    y = 6 * (float)Math.Cos(MathHelper.ToRadians((float)NPC.frameCounter * 4));
                    if (y > 0)
                        y = 0;
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0 - x, y));

                    skelly.HandLIK.ChangeBoneOffset(new Vector2(-4, skelly.ArmLength() - 1));
                    skelly.Weapon1.rotation = skelly.LowerArmL.rotation + MathHelper.ToRadians(-75);// * NPC.spriteDirection);
                    skelly.HandRIK.ChangeBoneOffset(new Vector2(4, skelly.ArmLength() - 1));
                    skelly.Weapon2.rotation = skelly.LowerArmR.rotation + MathHelper.ToRadians(-45);// * NPC.spriteDirection);

                    skelly.CalculateHandIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -74 + 1f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 8))));
                    skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90));

                    skelly.CalculateLegIK();
                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(100));
                }
                else if (animState == AnimationState.MidAir)
                {
                    if (NPC.velocity.Y > 0)
                    {
                        Vector2 originLeg = skelly.UpperLegL.Position() - skelly.Origin.Position();
                        skelly.FeetLIK.ChangeBoneOffset(originLeg + new Vector2(skelly.LegLength() - 6, 0).RotatedBy(MathHelper.ToRadians(80)), 0.5f);
                        skelly.FeetRIK.ChangeBoneOffset(originLeg + new Vector2(skelly.LegLength() * 0.66f, 0).RotatedBy(MathHelper.ToRadians(80)), 0.5f);

                        skelly.HandLIK.ChangeBoneOffset(new Vector2(-16, skelly.ArmLength() - 24), 0.5f);
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(-12, skelly.ArmLength() - 24), 0.5f);

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-80));
                    }
                    else
                    {
                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-8, 0), 0.5f);
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(4, 0), 0.5f);

                        skelly.HandLIK.ChangeBoneOffset(new Vector2(-16, skelly.ArmLength() - 12));
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(-12, skelly.ArmLength() - 12));

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90));

                        LookAtPlayer = false;
                        skelly.Head.ChangeBoneRotation(-1.57f + 0.3f * NPC.spriteDirection);
                    }

                    skelly.CalculateHandIK();
                    skelly.CalculateLegIK();
                }
                else if (animState == AnimationState.SwingRedEyes)
                {
                    if (NPC.frameCounter < 20)
                    {
                        float angle = 1.57f - ((float)NPC.frameCounter / 19f) * 3.14f;// * NPC.spriteDirection;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 6);

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90 - 6)); // * NPC.spriteDirection));
                    }
                    else if (NPC.frameCounter < 30)
                    {
                        float angle = -1.57f + (((float)NPC.frameCounter - 20) / 10) * 4.71f;// * NPC.spriteDirection;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-80));// * NPC.spriteDirection));
                    }
                    else if (NPC.frameCounter < 50)
                    {
                        float angle = 3.14f;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);
                    }
                    else
                    {
                        ChangeAnimation(0);
                    }
                    skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                    skelly.HandRIK.ChangeBoneOffset(new Vector2(4, skelly.ArmLength() - 1));

                    skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.rotation - MathHelper.ToRadians(45));// * NPC.spriteDirection);
                    skelly.Weapon1.ChangeBoneRotation(skelly.LowerArmL.rotation - 1.309f, 1.57f);// * NPC.spriteDirection, 1.57f);

                    skelly.CalculateLegIK();
                    skelly.CalculateHandIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(90));
                }
                else if (animState == AnimationState.SwingPenitence)
                {
                    if (NPC.frameCounter < 20)
                    {
                        float angle = 1.57f - ((float)NPC.frameCounter / 19f) * 3.14f;
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 6);

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90 - 6));// * NPC.spriteDirection));
                    }
                    else if (NPC.frameCounter < 30)
                    {
                        float angle = -1.57f + (((float)NPC.frameCounter - 20) / 10) * 4.71f;// * NPC.spriteDirection;
                        //Main.NewText(MathHelper.ToDegrees(angle));
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-80));// * NPC.spriteDirection));
                    }
                    else if (NPC.frameCounter < 50)
                    {
                        float angle = 3.14f;
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);
                    }
                    else
                    {
                        ChangeAnimation(0);
                    }
                    skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                    skelly.HandLIK.ChangeBoneOffset(new Vector2(-4, skelly.ArmLength() - 1));

                    skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.rotation - MathHelper.ToRadians(45), 1.57f);
                    skelly.Weapon1.ChangeBoneRotation(skelly.LowerArmL.rotation - 1.309f, 1.57f);

                    skelly.CalculateLegIK();
                    skelly.CalculateHandIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(90));
                }
                else if (animState == AnimationState.SwingBoth)
                {
                    if (NPC.frameCounter < 20)
                    {
                        float angle = 1.57f - ((float)NPC.frameCounter / 19f) * 3.14f;// * NPC.spriteDirection;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 6);
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 6);

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90 - 6));// * NPC.spriteDirection));
                    }
                    else if (NPC.frameCounter < 30)
                    {
                        float angle = -1.57f + (((float)NPC.frameCounter - 20) / 10) * 4.71f;// * NPC.spriteDirection;
                        //Main.NewText(MathHelper.ToDegrees(angle));
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-80));// * NPC.spriteDirection));
                    }
                    else if (NPC.frameCounter < 50)
                    {
                        float angle = 3.14f;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);
                    }
                    else
                    {
                        ChangeAnimation(0);
                    }
                    skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                    skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.rotation - MathHelper.ToRadians(45), 1.57f);
                    skelly.Weapon1.ChangeBoneRotation(skelly.LowerArmL.rotation - 1.309f, 1.57f);

                    skelly.CalculateLegIK();
                    skelly.CalculateHandIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(90));
                }
                else if (animState == AnimationState.GoldRushIntro)
                {
                    skelly.Weapon1.Visible = false;
                    skelly.Weapon2.Visible = false;
                    skelly.Gauntlet.Visible = true;
                    if (NPC.frameCounter == 1)
                    {
                        skelly.Gauntlet.scale = 1f;
                    }
                    if (skelly.Gauntlet.scale < 2f)
                        skelly.Gauntlet.scale = 1f + 1f * ((float)NPC.frameCounter / 30f);

                    skelly.FeetLIK.ChangeBoneOffset(new Vector2(-54, 0));
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(40, 0));
                    skelly.CalculateLegIK();

                    skelly.HandLIK.ChangeBoneOffset(new Vector2(-16 + Main.rand.NextFloat(-1, 1), 6 + Main.rand.NextFloat(-1, 1)));
                    skelly.HandRIK.ChangeBoneOffset(new Vector2(-16, 6));
                    skelly.CalculateHandIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -50));
                    skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-75 + 2 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));


                    skelly.Head.rotation = NPC.spriteDirection == 1 ? -1.57f : -1.57f;

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(110));

                    if (NPC.frameCounter > 30)
                    {
                        ChangeAnimation(AnimationState.GoldRushLoop);
                        skelly.Gauntlet.scale = 2f;
                    }
                }
                else if (animState == AnimationState.GoldRushLoop)
                {
                    float x = 102 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 16));// * NPC.spriteDirection;
                    float y = 20 * (float)Math.Cos(MathHelper.ToRadians((float)NPC.frameCounter * 16));
                    if (y < 0)
                        y = 0;
                    skelly.FeetLIK.ChangeBoneOffset(new Vector2(-18 + x, y * -1), 12);
                    y = 20 * (float)Math.Cos(MathHelper.ToRadians((float)NPC.frameCounter * 16));
                    if (y > 0)
                        y = 0;
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0 - x, y), 12);
                    skelly.CalculateLegIK();

                    float rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X * NPC.spriteDirection);
                    skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(rotation), 16);
                    skelly.HandRIK.ChangeBoneOffset(new Vector2(-skelly.UpperArmR.length, skelly.LowerArmL.length));
                    skelly.CalculateHandIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -50 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 16))));
                    skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-50 + 2 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(140));
                }
                else if (animState == AnimationState.GoldRushLoopFall)
                {

                }
                else if (animState == AnimationState.GoldRushEnd)
                {
                    skelly.Weapon1.Visible = false;
                    skelly.Weapon2.Visible = false;
                    skelly.Gauntlet.Visible = true;
                    skelly.Gauntlet.scale = 1f;

                    skelly.HandLIK.ChangeBoneOffset(new Vector2(-14, 26));
                    skelly.HandRIK.ChangeBoneOffset(new Vector2(-16, 6));
                    skelly.CalculateHandIK();

                    skelly.FeetLIK.ChangeBoneOffset(new Vector2(skelly.UpperLegL.length - 6, 0));
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(-skelly.LowerArmL.length - 20, 0));
                    skelly.CalculateLegIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -skelly.UpperLegL.length - 4));
                    skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-73 + 3 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 10))));

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(100));
                }
                else if (animState == AnimationState.Phase2Transition)
                {
                    skelly.Weapon1.Visible = true;
                    skelly.Weapon2.Visible = true;
                    skelly.Gauntlet.Visible = false;
                    if (NPC.frameCounter < 30)
                    {
                        float factor = ((float)NPC.frameCounter / 30f);
                        float angle = 1.57f - factor * 3.14f;// * NPC.spriteDirection;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-16, 0));
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-94));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -80));
                    }
                    else if (NPC.frameCounter < 40)
                    {
                        NPC.localAI[1] = 1;

                        float factor = (((float)NPC.frameCounter - 30) / 10f);
                        float angle = -1.57f + factor * 4.17f;// * NPC.spriteDirection;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-26, 0));

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-80));
                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -80 + factor * 2));
                    }
                    else if (NPC.frameCounter < 60)
                    {
                        NPC.localAI[1] = 1;

                        float angle = 3.14f;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 6);
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 6);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-26, 0));
                    }
                    else
                    {
                        NPC.localAI[1] = 1;
                        ChangeAnimation(AnimationState.Idle2);
                    }

                    skelly.Weapon1.rotation = skelly.LowerArmL.rotation + MathHelper.ToRadians(-75);// * NPC.spriteDirection);
                    skelly.Weapon2.rotation = skelly.LowerArmR.rotation + MathHelper.ToRadians(-45);// * NPC.spriteDirection);
                    skelly.CalculateHandIK();

                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                    skelly.CalculateLegIK();

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(90));
                }
                else if (animState == AnimationState.Idle2)
                {
                    NPC.localAI[1] = 1;
                    skelly.Weapon1.Visible = true;
                    //skelly.Weapon2.Visible = true;
                    skelly.Gauntlet.Visible = false;

                    skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                    skelly.CalculateLegIK();

                    skelly.HandLIK.ChangeBoneOffset(new Vector2(-4, skelly.ArmLength() - 1));
                    skelly.Weapon1.rotation = skelly.LowerArmL.rotation + MathHelper.ToRadians(-50);// * NPC.spriteDirection);
                    skelly.HandRIK.ChangeBoneOffset(new Vector2(4, skelly.ArmLength() - 1));
                    skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.rotation + MathHelper.ToRadians(135), 0.3f);// * NPC.spriteDirection);

                    skelly.CalculateHandIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                    skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90 + 2 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(90));
                }
                else if (animState == AnimationState.Walk2)
                {
                    skelly.Weapon1.Visible = true;

                    float speed = 10;// * (Math.Abs(NPC.velocity.X) / 5f);
                    if (speed > 10)
                        speed = 10;

                    float x = 30 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * speed));// * NPC.spriteDirection;
                    float y = 20 * (float)Math.Cos(MathHelper.ToRadians((float)NPC.frameCounter * speed - 20));
                    if (y < 0)
                        y = 0;
                    skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12 + x, y * -1));
                    y = 14 * (float)Math.Cos(MathHelper.ToRadians((float)NPC.frameCounter * speed - 20));
                    if (y > 0)
                        y = 0;
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0 - x, y));

                    skelly.HandLIK.ChangeBoneOffset(new Vector2(-16, skelly.ArmLength() - 12));
                    skelly.Weapon1.ChangeBoneRotation(skelly.LowerArmL.rotation + MathHelper.ToRadians(-50), 0.2f);
                    skelly.HandRIK.ChangeBoneOffset(new Vector2(-12, skelly.ArmLength() - 12));
                    skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.rotation + MathHelper.ToRadians(135), 0.2f);
                    skelly.CalculateHandIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(5, -72 + 1f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * speed * 2))));
                    skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-70 + 3 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));

                    skelly.CalculateLegIK();
                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(120));
                }
                else if (animState == AnimationState.Dash)
                {
                    skelly.HandLIK.ChangeBoneOffset(new Vector2(-skelly.ArmLength(), 0));
                    skelly.Weapon1.ChangeBoneRotation(MathHelper.ToRadians(105), 0.12f);
                    skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(MathHelper.ToRadians(135)));
                    skelly.Weapon2.rotation = skelly.LowerArmR.rotation + MathHelper.ToRadians(180);
                    skelly.CalculateHandIK();

                    Vector2 point = skelly.Origin.DifferenceBone(skelly.UpperLegL) + new Vector2(skelly.LegLength() - 2f, 0).RotatedBy(MathHelper.ToRadians(165));
                    skelly.FeetLIK.ChangeBoneOffset(point, 32);
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, point.Y), 16);
                    skelly.CalculateLegIK();

                    skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-30));
                    skelly.Pelvis.ChangeBoneOffset(new Vector2(5, -40));

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(160));
                }
                else if (animState == AnimationState.SwingMimicry)
                {
                    skelly.Weapon1.Visible = true;
                    if (NPC.frameCounter < 20)
                    {
                        float angle = 1.57f - ((float)NPC.frameCounter / 19f) * 3.14f;// * NPC.spriteDirection;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 6);
                        skelly.Weapon1.ChangeBoneRotation(skelly.LowerArmL.rotation - MathHelper.ToRadians(45), 1.57f);// * NPC.spriteDirection);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90 - 6)); // * NPC.spriteDirection));
                    }
                    else if (NPC.frameCounter < 60)
                    {
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength() + Main.rand.NextFloat(-0.5000f, 0.5000f), Main.rand.NextFloat(-0.5000f, 0.5000f)).RotatedBy(-1.57f), 6);
                        skelly.Weapon1.scale = 1f + 1f * ((float)NPC.frameCounter - 20) / 30f;
                        skelly.Weapon1.ChangeBoneRotation(skelly.LowerArmL.rotation - MathHelper.ToRadians(45), 1.57f);// * NPC.spriteDirection);

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                    }
                    else if (NPC.frameCounter < 70)
                    {
                        float angle = -1.57f + (((float)NPC.frameCounter - 60) / 10) * 5.06f;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);
                        skelly.Weapon1.ChangeBoneRotation(skelly.LowerArmL.rotation - MathHelper.ToRadians(15), 1.57f);// * NPC.spriteDirection);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-28, 0));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(-10, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-80));// * NPC.spriteDirection));
                    }
                    else if (NPC.frameCounter < 90)
                    {
                        skelly.Weapon1.scale = 2f - 1f * ((float)NPC.frameCounter - 70) / 10f;
                        if (skelly.Weapon1.scale < 1f)
                            skelly.Weapon1.scale = 1f;
                        skelly.Weapon1.ChangeBoneRotation(skelly.LowerArmL.rotation - MathHelper.ToRadians(15), 1.57f);

                        float angle = 3.49f;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-28, 0));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(-10, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                    }
                    else
                    {
                        ChangeAnimation(AnimationState.Idle2);
                    }
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                    skelly.HandRIK.ChangeBoneOffset(new Vector2(4, skelly.ArmLength() - 1));

                    skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.rotation + MathHelper.ToRadians(135), 1.57f);// * NPC.spriteDirection, 1.57f);

                    skelly.CalculateLegIK();
                    skelly.CalculateHandIK();


                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(90));
                }
                else if (animState == AnimationState.SwingDaCapo)
                {
                    float progress = ((float)NPC.frameCounter % 30) / 10f;
                    if (progress > 1f)
                        progress = 1f;
                    if (NPC.frameCounter < 30)
                    {
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(1.57f - 3.14f * progress), 16);
                        skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.GetRotation(), 1.2f);

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-96));
                    }
                    else if (NPC.frameCounter < 40)
                    {
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(-1.57f + 4.71239f * progress), 16);
                        skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.GetRotation() - MathHelper.ToRadians(60), 1.2f);

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-84));
                    }
                    else if (NPC.frameCounter < 60)
                    {
                        progress = (((float)NPC.frameCounter - 40) / 20f);

                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(3.14f - 0.6f * progress));
                        skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.GetRotation() - MathHelper.ToRadians(60), 1.2f);

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-84));
                    }
                    else if (NPC.frameCounter < 90)
                    {
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(3.14f - 4.71239f * progress), 16);
                        skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.GetRotation(), 1.2f);

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-96));
                    }
                    else if (NPC.frameCounter < 100)
                    {
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(-1.57f + 3.14f * progress), 5);
                        skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.rotation + MathHelper.ToRadians(135), 0.2f);

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90));
                    }

                    skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                    skelly.HandLIK.ChangeBoneOffset(new Vector2(-4, skelly.ArmLength() - 1));
                    skelly.Weapon1.ChangeBoneRotation(skelly.LowerArmL.rotation + MathHelper.ToRadians(-50), 0.3f);

                    skelly.CalculateHandIK();
                    skelly.CalculateLegIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(90));
                }
                else if (animState == AnimationState.DaCapoThrow)
                {
                    if (NPC.frameCounter < 15)
                    {
                        float angle = 1.57f - ((float)NPC.frameCounter / 15f) * 3.14f;
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 6);
                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                        skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.GetRotation() + MathHelper.ToRadians(60));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))), 1);
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90 - 6));
                    }
                    else if (NPC.frameCounter < 50)
                    {
                        float prog = ((float)NPC.frameCounter - 15) / 35f;
                        float angle = -1.57f - (prog * MathHelper.ToRadians(10));
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 6);
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                        skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.GetRotation() + MathHelper.ToRadians(60));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))), 1);
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-96 - 3 * prog));
                    }
                    else if (NPC.frameCounter < 60)
                    {
                        skelly.Weapon2.Visible = false;

                        float prog = ((float)NPC.frameCounter - 50) / 10f;
                        float angle = -1.57f - MathHelper.ToRadians(10) + (prog * MathHelper.ToRadians(270));
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(14, 0), 4);
                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-36, 0), 4);

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(-10, -65 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))), 1);
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-60), 0.32f);
                    }
                    else
                    {
                        float angle = -1.57f - MathHelper.ToRadians(10) + MathHelper.ToRadians(270);
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 6);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-36, 0), 4);
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(14, 0), 4);

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(-10, -65 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))), 1);
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-60), 0.32f);
                    }

                    skelly.CalculateLegIK();
                    skelly.CalculateHandIK();

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(110));
                }
                else if (animState == AnimationState.HeavenThrow)
                {
                    skelly.Weapon2.Visible = false;
                    if (NPC.frameCounter < 50)
                    {
                        skelly.Weapon1.Visible = true;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(-skelly.UpperArmL.length, -skelly.LowerArmL.length));
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0));
                        skelly.Weapon1.ChangeBoneRotation(Main.rand.NextFloat(-0.08f, 0.08f), 0.5f);
                        skelly.Weapon1.scale = 0.2f + 0.8f * ((float)NPC.frameCounter/50f);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12 - 10 - skelly.LowerLegL.length, 0), 5);
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(-12 + 2 + skelly.UpperLegL.length, 0), 5);

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(-12, -46));
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-100));

                        skelly.CalculateHandIK(-1);

                        skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(90));
                    }
                    else
                    {
                        skelly.Weapon1.Visible = false;
                        if (NPC.frameCounter < 60)
                        {
                            float progress = ((float)NPC.frameCounter - 50)/9f;
                            skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(MathHelper.ToRadians(-90 + 270 * progress)), 16);
                            skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(MathHelper.ToRadians(110)), 8);

                            skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-100 + 40 * progress), 1.2f);

                            skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(180), 0.12f);
                        }
                        else
                        {
                            skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(130));
                        }
                        
                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(12 + skelly.LowerLegL.length, 0), 16);
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(-6 - skelly.UpperLegL.length, 0), 16);
                        skelly.Pelvis.ChangeBoneOffset(new Vector2(20, -46));
                        LookAtPlayer = false;
                        skelly.Head.ChangeBoneRotation(MathHelper.ToRadians(NPC.spriteDirection == 1 ? -45 : -135), 0.8f);

                        skelly.CalculateHandIK();
                    }
                    skelly.CalculateLegIK();
                }
                else if (animState == AnimationState.MimicryPrepare)
                {
                    skelly.Weapon2.Visible = false;
                    if (NPC.frameCounter < 30)
                    {
                        float prog = (float)NPC.frameCounter / 30f;

                        Vector2 handR = new Vector2(skelly.ArmLength() - 6, 0).RotatedBy(MathHelper.ToRadians(-80 + 170 * (1f - prog)));
                        skelly.HandLIK.ChangeBoneOffset(handR, 8f);
                        skelly.Weapon1.ChangeBoneRotation(handR.ToRotation() - MathHelper.ToRadians(90 + Main.rand.NextFloat(-1f, 1f)), 1.2f);

                        skelly.HandRIK.ChangeBoneOffset(handR + new Vector2(-10, 0).RotatedBy(skelly.Weapon1.GetRotation()), 8f);
                    }
                    else
                    {
                        Vector2 handR = new Vector2(skelly.ArmLength() - 6, 0).RotatedBy(MathHelper.ToRadians(-80));
                        skelly.Weapon1.ChangeBoneRotation(handR.ToRotation() - MathHelper.ToRadians(90 + Main.rand.NextFloat(-1f, 1f)), 1.2f);
                        skelly.HandRIK.ChangeBoneOffset(handR + new Vector2(-10, 0).RotatedBy(skelly.Weapon1.GetRotation()), 8f);
                    }

                    if (NPC.frameCounter < 50)
                    {
                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-42, 0));
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(16, 0));
                    }

                    if (NPC.frameCounter < 60)
                    {
                        float weaponScale = (float)NPC.frameCounter / 60f;

                        skelly.Weapon1.scale = 1f + 1f * (float)Math.Sin(1.57f * weaponScale);
                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -68 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-95));
                    }
                    else if (NPC.frameCounter >= 70)
                    {
                        float weaponScale = ((float)NPC.frameCounter - 70f)/ 60f;
                        if (weaponScale > 1f)
                            weaponScale = 1f;

                        skelly.Weapon1.scale = 2f + 2f * (float)Math.Sin(1.57f * weaponScale);
                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -68 + (weaponScale * 12f) + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));

                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-80));
                    }

                    skelly.CalculateHandIK();
                    skelly.CalculateLegIK();
                }
                else if (animState == AnimationState.MimicryGreaterSplitV)
                {
                    Vector2 originLeg;
                    if (NPC.frameCounter == 1)
                    {
                        Vector2 handR = new Vector2(skelly.ArmLength(), 0).RotatedBy(MathHelper.ToRadians(-80));
                        skelly.HandLIK.BoneOffset = handR;

                        skelly.Weapon1.rotation = handR.ToRotation() - MathHelper.ToRadians(90);
                        skelly.HandRIK.BoneOffset = handR + new Vector2(-10, 0).RotatedBy(skelly.Weapon1.GetRotation());

                        originLeg = skelly.UpperLegL.Position() - skelly.Origin.Position();
                        skelly.FeetLIK.BoneOffset = originLeg + new Vector2(skelly.LegLength() - 6, 0).RotatedBy(MathHelper.ToRadians(60));
                        skelly.FeetRIK.BoneOffset = originLeg + new Vector2(skelly.LegLength()/2, 0).RotatedBy(MathHelper.ToRadians(60));

                        skelly.Pelvis.rotation = MathHelper.ToRadians(-100);
                        skelly.Pelvis.BoneOffset = new Vector2(0, -72);
                    }
                    if (NPC.frameCounter >= 15)
                    {
                        float scale = (((float)NPC.frameCounter - 15) / 20);
                        if (scale > 1f)
                            scale = 1f;

                        skelly.Weapon1.scale = 4f - 3f * scale;
                    }

                    float prog = (float)NPC.frameCounter / 10f;
                    if (prog > 1f)
                        prog = 1f;

                    Vector2 hand = new Vector2(skelly.ArmLength() - 6 * prog, 0).RotatedBy(MathHelper.ToRadians(-80 + 320f * (float)Math.Sin(1.57f * prog)));
                    skelly.HandLIK.BoneOffset = hand;

                    skelly.Weapon1.rotation = hand.ToRotation() - MathHelper.ToRadians(90);
                    skelly.HandRIK.BoneOffset = hand + new Vector2(-10, 0).RotatedBy(skelly.Weapon1.GetRotation());

                    originLeg = skelly.UpperLegL.Position() - skelly.Origin.Position();
                    skelly.FeetLIK.BoneOffset = originLeg + new Vector2(skelly.LegLength() - 6, 0).RotatedBy(MathHelper.ToRadians(60 + 15 * prog));
                    skelly.FeetRIK.BoneOffset = originLeg + new Vector2(skelly.LegLength() / 2, 0).RotatedBy(MathHelper.ToRadians(60 + 15 * prog));

                    skelly.Pelvis.rotation = MathHelper.ToRadians(-100 + 20 * prog);

                    skelly.CalculateHandIK();
                    skelly.CalculateLegIK();
                }
                else if (animState == AnimationState.Phase3Transition)
                {
                    if (NPC.frameCounter < 60)
                    {
                        float angle;
                        if (NPC.frameCounter < 20)
                        {
                            angle = 1.57f - ((float)NPC.frameCounter / 20f) * 3.14f;
                            skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);

                            skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.GetRotation() + MathHelper.ToRadians(60));

                        }
                        angle = 1.57f - ((float)NPC.frameCounter / 60f) * 2.35619f;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))), 1);
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-96));

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));
                    }
                    else if (NPC.frameCounter < 70)
                    {
                        skelly.Weapon1.Visible = false;
                        skelly.Weapon2.Visible = false;

                        float prog = (((float)NPC.frameCounter - 60f) / 10f);
                        float angle = -1.57f + 4.71239f * prog;
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);
                        angle = -0.786f + 3.92699f;
                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(angle), 16);

                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(14, 0), 4);
                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-36, 0), 4);

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(-10, -65 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))), 1);
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-60), 0.32f);
                    }
                    else if (NPC.frameCounter >= 130)
                    {
                        NPC.localAI[1] = 2;
                        skelly.Weapon1.Visible = true;
                        skelly.Weapon2.Visible = true;

                        /*float prog = (((float)NPC.frameCounter - 130f) / 30f);
                        if (prog > 1f)
                            prog = 1f;*/

                        skelly.HandLIK.ChangeBoneOffset(new Vector2(-4, skelly.ArmLength() - 1));
                        skelly.Weapon1.rotation = skelly.LowerArmL.rotation + MathHelper.ToRadians(-75);// * NPC.spriteDirection);

                        skelly.HandRIK.ChangeBoneOffset(new Vector2(8, 0).RotatedBy(-0.523599f));
                        skelly.Weapon2.rotation = skelly.LowerArmR.rotation + MathHelper.ToRadians(-60);// * NPC.spriteDirection);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))), 1);
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-96));
                    }

                    skelly.CalculateHandIK();
                    skelly.CalculateLegIK();
                }
                else if (animState == AnimationState.Idle3)
                {
                    NPC.localAI[1] = 2;

                    skelly.Weapon1.Visible = true;
                    skelly.Weapon2.Visible = true;
                    skelly.Gauntlet.Visible = false;

                    

                    skelly.CalculateLegIK();

                    skelly.HandLIK.ChangeBoneOffset(new Vector2(-4, skelly.ArmLength() - 1));
                    skelly.Weapon1.rotation = skelly.LowerArmL.rotation + MathHelper.ToRadians(-75);// * NPC.spriteDirection);

                    skelly.HandRIK.ChangeBoneOffset(new Vector2(8, 0).RotatedBy(-0.523599f));
                    skelly.Weapon2.rotation = skelly.LowerArmR.rotation + MathHelper.ToRadians(-60);// * NPC.spriteDirection);

                    skelly.CalculateHandIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                    skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90 + 2 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(90));
                }
                else if (animState == AnimationState.Walk3)
                {
                    NPC.localAI[1] = 2;

                    float x = 16 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4));// * NPC.spriteDirection;
                    float y = 6 * (float)Math.Cos(MathHelper.ToRadians((float)NPC.frameCounter * 4));
                    if (y < 0)
                        y = 0;
                    skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12 + x, y * -1));
                    y = 6 * (float)Math.Cos(MathHelper.ToRadians((float)NPC.frameCounter * 4));
                    if (y > 0)
                        y = 0;
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0 - x, y));

                    skelly.HandLIK.ChangeBoneOffset(new Vector2(-4, skelly.ArmLength() - 1));
                    skelly.Weapon1.rotation = skelly.LowerArmL.rotation + MathHelper.ToRadians(-75);// * NPC.spriteDirection);

                    skelly.HandRIK.ChangeBoneOffset(new Vector2(8, 0).RotatedBy(-0.523599f));
                    skelly.Weapon2.rotation = skelly.LowerArmR.rotation + MathHelper.ToRadians(-60);// * NPC.spriteDirection);

                    skelly.CalculateHandIK();

                    skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -74 + 1f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 8))));
                    skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90));

                    skelly.CalculateLegIK();
                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(100));
                }
                else if (animState == AnimationState.JustitiaSwing)
                {
                    if (NPC.frameCounter < 45)
                    {
                        float prog = (float)NPC.frameCounter / 20;
                        if (prog > 1)
                            prog = 1;

                        Vector2 position = new Vector2( 0, skelly.ArmLength() - 1 - 6 * prog).RotatedBy(-1.57f * prog);
                        float rotation = (position).ToRotation() - 1.57f;
                        skelly.Weapon1.ChangeBoneRotation(rotation, 0.8f);

                        if (prog == 1)
                            position += new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                        skelly.HandLIK.ChangeBoneOffset(position);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                    }
                    else
                    {
                        float prog = ((float)NPC.frameCounter - 45)/ 6;
                        if (prog > 1)
                            prog = 1;

                        Vector2 position = new Vector2(-4, skelly.ArmLength() - 1).RotatedBy(-1.57f + 3.3f * (float)Math.Sin(1.57f * prog));
                        skelly.HandLIK.BoneOffset = position;
                        skelly.Weapon1.ChangeBoneRotation(skelly.LowerArmL.rotation + MathHelper.ToRadians(-75 - 15 * prog), 1.2f);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-24, 0));
                        skelly.Pelvis.ChangeBoneOffset(new Vector2(-4, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                    }

                    skelly.Weapon1.Visible = true;
                    skelly.Weapon2.Visible = true;
                    skelly.Gauntlet.Visible = false;

                    
                    skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                    skelly.CalculateLegIK();

                    //skelly.HandLIK.ChangeBoneOffset(new Vector2(-4, skelly.ArmLength() - 1));
                    //skelly.Weapon1.rotation = skelly.LowerArmL.rotation + MathHelper.ToRadians(-75);// * NPC.spriteDirection);

                    skelly.HandRIK.ChangeBoneOffset(new Vector2(8, 0).RotatedBy(-0.523599f));
                    skelly.Weapon2.rotation = skelly.LowerArmR.rotation + MathHelper.ToRadians(-60);// * NPC.spriteDirection);

                    skelly.CalculateHandIK();

                    //skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90 + 2 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));

                    skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(90));
                }
                else if (animState == AnimationState.SmileSwing)
                {
                    //skelly.Weapon1.Visible = false;
                    Vector2 justitaPlacement = skelly.Origin.Position() + new Vector2(-36, -90);
                    if (NPC.frameCounter < 60)
                    {
                        float prog = (float)NPC.frameCounter / 60;
                        skelly.Weapon2.scale = 1f + (1f * prog);

                        skelly.HandLIK.ChangeBoneOffset(justitaPlacement - skelly.UpperArmL.Position());
                        skelly.Weapon1.ChangeBoneRotation(MathHelper.ToRadians(90));

                        Vector2 position = new Vector2(8 + 20 * prog, 0).RotatedBy(MathHelper.ToRadians(-30 - 30 * prog));
                        skelly.HandRIK.ChangeBoneOffset(position, 8f);

                        Vector2 offset = new Vector2(36 * prog, 0).RotatedBy(skelly.Weapon2.GetRotation() - skelly.LowerArmR.GetRotation());
                        skelly.Weapon2.BoneOffset = offset;
                        //float rotation = (position).ToRotation() + MathHelper.ToRadians(60);
                        //skelly.Weapon2.ChangeBoneRotation(rotation, 0.8f);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76));
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90));

                        skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(100));
                    }
                    else if (NPC.frameCounter < 90)
                    {
                        float prog = ((float)NPC.frameCounter - 60) / 10;
                        if (prog > 1)
                            prog = 1;

                        skelly.HandLIK.ChangeBoneOffset(new Vector2(-12, skelly.ArmLength() - 4));

                        Vector2 position = new Vector2(28 - 16 * prog, 0).RotatedBy(MathHelper.ToRadians(-60 + 180 * prog));
                        skelly.HandRIK.ChangeBoneOffset(position, 8f);

                        float rotation = (position).ToRotation() - MathHelper.ToRadians(80);
                        skelly.Weapon2.ChangeBoneRotation(rotation,0.8f);

                        Vector2 offset = new Vector2(36, 0).RotatedBy(skelly.Weapon2.GetRotation() - skelly.LowerArmR.GetRotation());
                        skelly.Weapon2.BoneOffset = offset;

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-48, 0), 8);
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(2, 0), 8);

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -62), 8);
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-45), 0.8f);

                        skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(140));
                    }
                    else if (NPC.frameCounter < 150)
                    {
                        float prog = ((float)NPC.frameCounter - 90) / 60;
                        if (prog > 1)
                            prog = 1;

                        Vector2 position = new Vector2(12 + 16 * prog, 0).RotatedBy(MathHelper.ToRadians(120 - 210 * prog));
                        skelly.HandRIK.ChangeBoneOffset(position, 8f);

                        float rotation = (position).ToRotation() - MathHelper.ToRadians(80);
                        skelly.Weapon2.ChangeBoneRotation(rotation, 0.8f);

                        position += (skelly.UpperArmR.Position() - skelly.UpperArmL.Position()) + new Vector2(-16, 0).RotatedBy(skelly.Weapon2.GetRotation());
                        skelly.HandLIK.ChangeBoneOffset(position, 8f);

                        Vector2 offset = new Vector2(36, 0).RotatedBy(skelly.Weapon2.GetRotation() - skelly.LowerArmR.GetRotation());
                        skelly.Weapon2.BoneOffset = offset;

                            
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-45 - 10f * prog), 0.8f);

                        skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(110));
                    }
                    else if (NPC.frameCounter < 240)
                    {
                        float prog = ((float)NPC.frameCounter - 150) / 10;
                        if (prog > 1)
                            prog = 1;

                        LookAtPlayer = false;
                        skelly.Head.ChangeBoneRotation(-1.57f + 0.875f * NPC.spriteDirection);

                        Vector2 position = new Vector2(28 - 8 * prog, 0).RotatedBy(MathHelper.ToRadians(-90 + 145 * prog));
                        skelly.HandRIK.ChangeBoneOffset(position, 8f);

                        float rotation = (position).ToRotation() - MathHelper.ToRadians(50);
                        skelly.Weapon2.ChangeBoneRotation(rotation, 0.8f);

                        position += (skelly.UpperArmR.Position() - skelly.UpperArmL.Position()) + new Vector2(-16, 0).RotatedBy(skelly.Weapon2.GetRotation());
                        skelly.HandLIK.ChangeBoneOffset(position, 8f);

                        Vector2 offset = new Vector2(36, 0).RotatedBy(skelly.Weapon2.GetRotation() - skelly.LowerArmR.GetRotation());
                        skelly.Weapon2.BoneOffset = offset;

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -42));
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-10 + Main.rand.NextFloat(-5, 5)));

                        skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(210));
                    }
                    else// if (NPC.frameCounter < 210)
                    {
                        float prog = ((float)NPC.frameCounter - 240) / 30;
                        if (prog > 1)
                            prog = 1;

                        skelly.Weapon2.scale = 2f - (1f * prog);

                        Vector2 position = new Vector2(20 - 12 * prog, 0).RotatedBy(MathHelper.ToRadians(55 - 85 * prog));
                        skelly.HandRIK.ChangeBoneOffset(position);
                        skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmR.rotation + MathHelper.ToRadians(-60), 0.4f);

                        Vector2 offset = new Vector2(36 - 36 * prog, 0).RotatedBy(skelly.Weapon2.GetRotation() - skelly.LowerArmR.GetRotation());
                        skelly.Weapon2.BoneOffset = offset;

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76));
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90));

                        skelly.Hair.ChangeBoneRotation(MathHelper.ToRadians(90));

                        skelly.HandLIK.ChangeBoneOffset(justitaPlacement - skelly.UpperArmL.Position());
                    }

                    skelly.CalculateLegIK();
                    skelly.CalculateHandIK();

                    if (NPC.frameCounter >= 60 && NPC.frameCounter < 260)
                    {
                        skelly.Weapon1.BoneOffset = (justitaPlacement - skelly.LowerArmL.EndPoint()).RotatedBy(-skelly.LowerArmL.GetRotation());
                    }
                    else
                    {
                        skelly.Weapon1.BoneOffset = Vector2.Zero;
                    }

                    //LookingAt(skelly.Weapon1.Position(), 64);
                    //LookingAt(justitaPlacement, 64);
                }
                else if (animState == AnimationState.Phase4Transition)
                {
                    if (NPC.frameCounter < 30)
                    {
                        float prog = ((float)NPC.frameCounter) / 30f;
                        skelly.HandRIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(MathHelper.ToRadians(-90 * prog)));
                        skelly.Weapon2.ChangeBoneRotation(skelly.LowerArmL.GetRotation() - MathHelper.ToRadians(90), 0.3f);

                        skelly.HandLIK.ChangeBoneOffset(new Vector2(0, skelly.ArmLength() - 1));
                        skelly.Weapon1.rotation = skelly.LowerArmL.rotation + MathHelper.ToRadians(-75);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));
                    }
                    else if (NPC.frameCounter < 75)
                    {
                        LookAtPlayer = false;
                        skelly.Head.ChangeBoneRotation(-1.57f + MathHelper.ToRadians(45 * NPC.spriteDirection));
                        float prog = ((float)NPC.frameCounter - 30f) / 10f;
                        if (prog > 1f)
                            prog = 1f;

                        Vector2 offset = new Vector2(38 * prog, 0).RotatedBy(skelly.Weapon2.GetRotation() - skelly.LowerArmR.GetRotation());
                        skelly.Weapon2.BoneOffset = offset;
                        skelly.Weapon2.scale = 1f + 0.5f * prog;

                        Vector2 position = new Vector2(skelly.ArmLength() - 8, 0).RotatedBy(MathHelper.ToRadians(-90 + 90 * prog));
                        Vector2 position2 = position + new Vector2(0, 100);
                        if (prog >= 1f)
                        {
                            position.X += Main.rand.NextFloat(-1, 1);
                            position.Y += Main.rand.NextFloat(-1, 1);
                        }
                        skelly.HandRIK.ChangeBoneOffset(position);
                        skelly.Weapon2.ChangeBoneRotation((position2 - position).ToRotation(), 0.3f);

                        skelly.HandLIK.ChangeBoneOffset(new Vector2(-48, skelly.ArmLength() - 8));
                        skelly.Weapon1.rotation = skelly.LowerArmL.rotation + MathHelper.ToRadians(-75);

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-14, 0));
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(2, 0));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -62), 8);
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90), 0.8f);
                    }
                    else if (NPC.frameCounter < 90)
                    {
                        skelly.Weapon2.Visible = false;

                        skelly.HandLIK.ChangeBoneOffset(new Vector2(-4, skelly.ArmLength() - 1));
                        skelly.Weapon1.rotation = skelly.LowerArmL.rotation + MathHelper.ToRadians(-75);

                        skelly.HandRIK.ChangeBoneOffset(new Vector2(4, skelly.ArmLength() - 1));

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90 + 2 * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                    }
                    else if (NPC.frameCounter < 120)
                    {
                        float prog = ((float)NPC.frameCounter - 90f) / 30f;

                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(1.57f - prog * 3.14f), 6);
                        skelly.Weapon1.ChangeBoneRotation(skelly.LowerArmL.rotation - 1.309f, 1.57f);

                        skelly.HandRIK.ChangeBoneOffset(new Vector2(4, skelly.ArmLength() - 1));

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90 - 6));
                    }
                    else if (NPC.frameCounter < 130)
                    {
                        NPC.localAI[1] = 3;
                        float prog = ((float)NPC.frameCounter - 120f) / 10f;

                        skelly.HandLIK.ChangeBoneOffset(new Vector2(skelly.ArmLength(), 0).RotatedBy(-1.57f + prog * MathHelper.ToRadians(280)), 16);
                        skelly.Weapon1.ChangeBoneRotation(skelly.LowerArmL.rotation - 1.309f, 1.57f);

                        skelly.HandRIK.ChangeBoneOffset(new Vector2(4, skelly.ArmLength() - 1));

                        skelly.FeetLIK.ChangeBoneOffset(new Vector2(-12, 0));
                        skelly.FeetRIK.ChangeBoneOffset(new Vector2(0, 0));

                        skelly.Pelvis.ChangeBoneOffset(new Vector2(0, -76 + 0.25f * (float)Math.Sin(MathHelper.ToRadians((float)NPC.frameCounter * 4))));
                        skelly.Pelvis.ChangeBoneRotation(MathHelper.ToRadians(-90));
                    }
                    skelly.CalculateHandIK();
                    skelly.CalculateLegIK();
                }

                //Draw Afterimages
                if (animState == AnimationState.GoldRushLoop ||
                    animState == AnimationState.Dash ||
                    animState == AnimationState.MimicryGreaterSplitV)
                    NPC.localAI[2] = 1f;
                else if (NPC.localAI[2] > 0f)
                    NPC.localAI[2] -= 0.1f;


                if (NPC.ai[0] == 1 && NPC.ai[3] < 0 && 
                  !(animState == AnimationState.HeavenThrow || 
                    animState == AnimationState.MimicryPrepare || 
                    animState == AnimationState.MimicryGreaterSplitV || 
                    animState == AnimationState.Phase3Transition))
                {
                    skelly.Weapon2.Visible = true;
                }

                if (LookAtPlayer)
                {
                    Vector2 delta = (NPC.GetTargetData().Center - skelly.Head.Position());
                    float headRot = (float)(Math.Atan2(delta.Y * NPC.spriteDirection, delta.X * NPC.spriteDirection));

                    if (headRot > MathHelper.ToRadians(25))
                        headRot = MathHelper.ToRadians(25);
                    else if (headRot < -MathHelper.ToRadians(30))
                        headRot = -MathHelper.ToRadians(30);

                    if (Math.Sign(delta.X) == Math.Sign(NPC.spriteDirection))
                        skelly.Head.ChangeBoneRotation(headRot - 1.57f);
                    else
                        skelly.Head.ChangeBoneRotation(-1.57f);
                }

                /*if (NPC.ai[0] > 0 && (skelly.Head.Position() - skelly.Head.OldPosition(1)).Length() > 0.05f)
                {
                    Vector2 EyePosition = skelly.Head.Position(NPC.spriteDirection) + new Vector2(-2,8 * NPC.spriteDirection).RotatedBy(skelly.Head.GetRotation());
                    Dust d = Dust.NewDustPerfect(EyePosition, Mod.DustType("RedMistEye"));
                    d.velocity *= 0;
                    d.fadeIn = 0.1f;
                    d.noGravity = true;
                }*/

                skelly.UpdateOldBones();
                //LookingAt(skelly.HandLIK.Position(), 15);
                //LookingAt(skelly.HandRIK.Position(), 6);

                //LookingAt(skelly.Weapon2.Position(), 6);

                //LookingAt(skelly.FeetLIK.Position(), 6);
                //LookingAt(skelly.FeetRIK.Position(), 15);
            }
        }

        public override void ModifyHitByProjectile(Projectile Projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if ((NPC.Center - Main.player[Projectile.owner].Center).Length() > 300)
                    damage /= 3;
        }

        private void fighterAI(Terraria.DataStructures.NPCAimedTarget target, float EffectiveRange, float speed, float maxSpeed)
        {
            Vector2 delta = target.Center - NPC.Center;
            bool moveLeft = false;
            bool moveRight = false;

            if (delta.X + EffectiveRange < 0)
                moveLeft = true;
            if (delta.X - EffectiveRange > 0)
                moveRight = true;

            if (moveLeft)
            {
                NPC.velocity.X -= speed;
                if (NPC.velocity.X < -maxSpeed)
                    NPC.velocity.X = -maxSpeed;
                if (NPC.velocity.X > 0)
                    NPC.velocity.X -= speed * 0.5f;

                NPC.spriteDirection = -1;
            }
            else if (moveRight)
            {
                NPC.velocity.X += speed;
                if (NPC.velocity.X > maxSpeed)
                    NPC.velocity.X = maxSpeed;
                if (NPC.velocity.X < 0)
                    NPC.velocity.X += speed * 0.5f;
                NPC.spriteDirection = 1;
            }
            else
            {
                NPC.velocity.X *= 0.8f;
                if (NPC.velocity.X < speed || NPC.velocity.X > -speed)
                    NPC.velocity.X = 0;
            }

            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY, 1, false);

            int x = (int)NPC.Center.X, y = (int)(NPC.position.Y + NPC.height);
            if (moveLeft)
                x -= (int)(NPC.width / 2) - 1;
            if (moveRight)
                x += (int)(NPC.width / 2) + 1;

            x /= 16;
            y /= 16;

            //LookingAt(new Vector2(x * 16, y * 16));

            if (NPC.velocity.Y == 0)
            {
                bool IsBelow = NPC.position.Y + NPC.height > target.Position.Y + target.Height;
                if (!IsBelow)
                {

                }
            }
        }

        private AnimationState animState
        {
            get { return (AnimationState)NPC.localAI[0]; } 
            set { NPC.localAI[0] = (int) value; }
        }

        private enum AnimationState
        {
            Idle1,
            Walk1,
            SwingRedEyes,
            SwingPenitence,
            SwingBoth,
            GoldRushIntro,
            GoldRushLoop,
            GoldRushLoopFall,
            GoldRushEnd,
            Phase2Transition,
            Idle2,
            Walk2,
            Dash,
            Run,
            SwingDaCapo,
            SwingMimicry,
            DaCapoThrow,
            MimicryPrepare,
            MimicryGreaterSplitV,
            MidAir,
            HeavenThrow,
            Phase3Transition,
            Walk3,
            Idle3,
            JustitiaSwing,
            SmileSwing,
            Phase4Transition,
            Idle4,
            GoldRushThrow,
            TwilightChase,
            TwilightDashSlash,
            TwilightFinisher,
            TwilightDrift,
            TwilightEnd
        }

        private void ChangeAnimation(AnimationState i)
        {
            if (animState != i)
                NPC.frameCounter = 0;
            animState = i;
        }

        public void LookingAt(Vector2 pos, int type)
        {
            Dust d = Dust.NewDustPerfect(pos, type);
            d.noGravity = true;
            d.velocity *= 0;
        }

        public bool IncomingProjectile()
        {
            foreach (Projectile p in Main.projectile)
            {
                if (p.active && p.friendly && p.damage > 0)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        Rectangle hb = p.getRect();
                        hb.X = (int)p.position.X + p.width; hb.Y = (int)p.position.Y + p.height;
                        if (hb.Intersects(NPC.getRect()))
                            return true;
                    }
                }
            }
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile Projectile)
        {
            if (Projectile.ai[0] == 1 && Projectile.ai[1] == 5)
                return false;
            return null;
        }

        public void Talk(string id)
        {
            string text = SuppressionTextData.GeburaBark[id];
            int dir = NPC.spriteDirection;
            Vector2 pos = new Vector2(NPC.Center.X, NPC.position.Y - NPC.height / 2);
            if (dir < 0)
                pos.X = NPC.position.X + NPC.width * 0.25f;
            else if (dir > 0)
                pos.X = NPC.position.X + NPC.width * 0.75f;
            SuppressionText.AddText(text, pos, Main.rand.NextFloat(-0.12f, 0.12f), 0.5f, Color.Red, 0.75f, 30, dir, 0);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (skelly == null)
            {
                return false;
            }
            //Should probably add Keter Suppresion Red filter when player is outside effective range
            if (NPC.localAI[2] > 0)
            {
                int Factor = (int)(10 * (NPC.localAI[2] / 1f));
                for (int i = 0; i < Factor; i++)
                {
                    Color color = drawColor * (0.3f * (1f - (float)i / (float)Factor));
                    DrawRedMist(spriteBatch, color, i);
                }
            }
            DrawRedMist(spriteBatch, drawColor);
            return false;
        }

        public void GetEye(ref Vector2 position, ref float rotation)
        {
            position = skelly.Head.Position(NPC.spriteDirection) + new Vector2(-2, 8 * NPC.spriteDirection).RotatedBy(skelly.Head.GetRotation());
            rotation = skelly.Head.GetRotation();
        }

        private void DrawRedMist(SpriteBatch spriteBatch, Color lightColor, int i = -1)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("NPCs/RedMist/RedMistAssembled").Value;
            Vector2 origin;
            Vector2 position;
            float rot;
            Color color = lightColor;
            Rectangle frame = new Rectangle(0, 0, 48, 66);

            //Hair
            origin = new Vector2(27, 5);
            position = skelly.Head.Position(NPC.spriteDirection, i);
            rot = skelly.Hair.GetRotation(NPC.spriteDirection, i) - 1.57f;//MathHelper.ToRadians(90);//skelly.Head.rotation + 1.57f;
            frame.Y = frame.Height * 11;
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            //Lower Layer Arm
            position = skelly.UpperArmR.Position(NPC.spriteDirection, i);
            rot = skelly.UpperArmR.GetRotation(NPC.spriteDirection, i) - (NPC.spriteDirection == 1 ? 0.785f : 2.355f);
            origin = new Vector2(3, 3);
            frame.Y = frame.Height * 4;
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            position = skelly.LowerArmR.Position(NPC.spriteDirection, i);
            rot = skelly.LowerArmR.GetRotation(NPC.spriteDirection, i) - (NPC.spriteDirection == 1 ? 0.785f : 2.355f);
            origin = new Vector2(3, 3);
            frame.Y = frame.Height * 5;
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            position = skelly.HandR.Position(NPC.spriteDirection, i);
            frame.Y = frame.Height * 6;
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            //Weapon 2
            if (skelly.Weapon2.Visible && NPC.localAI[1] < 3)
            {
                int dir = NPC.spriteDirection;
                Texture2D weapon = Mod.Assets.Request<Texture2D>("Items/Penitence").Value;
                Vector2 weaponOrigin = new Vector2(4, 49);
                if (NPC.localAI[1] == 1)
                {
                    dir *= -1;
                    weapon = Mod.Assets.Request<Texture2D>("Items/DaCapo").Value;
                    weaponOrigin = new Vector2(45, 63);
                }
                if (NPC.localAI[1] == 2)
                {
                    weapon = Mod.Assets.Request<Texture2D>("Items/Smile").Value;
                    weaponOrigin = new Vector2(39, weapon.Height - 39);
                }
                position = skelly.Weapon2.Position(NPC.spriteDirection, i);
                rot = skelly.Weapon2.GetRotation(NPC.spriteDirection, i) + 0.785f;

                if (animState == AnimationState.SwingDaCapo && 30 <= NPC.frameCounter && NPC.frameCounter < 60)
                {
                    dir *= -1;
                }

                Draw(spriteBatch, weapon, position, weapon.Frame(), color, rot, weaponOrigin, skelly.Weapon2.scale, dir, true);
            }

            //Lower Layer Leg
            origin = new Vector2(5, 9);
            position = skelly.UpperLegR.Position(NPC.spriteDirection, i);
            rot = skelly.UpperLegR.GetRotation(NPC.spriteDirection, i) - 1.57f;
            frame.Y = frame.Height * 9;
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            origin = new Vector2(7, 5);
            position = skelly.LowerLegR.Position(NPC.spriteDirection, i);
            rot = skelly.LowerLegR.GetRotation(NPC.spriteDirection, i) - 1.57f;
            frame.Y = frame.Height * 10;
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            //Body
            origin = new Vector2(17, 41);
            position = skelly.Pelvis.Position(NPC.spriteDirection, i);
            rot = skelly.Pelvis.GetRotation(NPC.spriteDirection, i) + 1.57f;
            frame.Y = frame.Height * 0;
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            int x = (int)NPC.ai[0];
            origin = new Vector2(13, 21);
            position = skelly.Head.Position(NPC.spriteDirection, i);
            rot = skelly.Head.GetRotation(1, i) + 1.57f;
            frame.Y = frame.Height * (12 + x);
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            //Top Layer Leg
            origin = new Vector2(11, 9);
            position = skelly.UpperLegL.Position(NPC.spriteDirection, i);
            rot = skelly.UpperLegL.GetRotation(NPC.spriteDirection, i) - 1.57f;
            frame.Y = frame.Height * 7;
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            origin = new Vector2(11, 5);
            position = skelly.LowerLegL.Position(NPC.spriteDirection, i);
            rot = skelly.LowerLegL.GetRotation(NPC.spriteDirection, i) - 1.57f;
            frame.Y = frame.Height * 8;
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            //Weapon 1
            if (skelly.Weapon1.Visible)
            {
                Texture2D weapon = Mod.Assets.Request<Texture2D>("Items/RedEyes").Value;
                Vector2 weaponOrigin = new Vector2(5, weapon.Height - 5);
                if (NPC.localAI[1] == 1)
                {
                    weapon = Mod.Assets.Request<Texture2D>("Items/Mimicry").Value;
                    weaponOrigin = new Vector2(9, weapon.Height - 9);
                }
                if (NPC.localAI[1] == 2)
                {
                    weapon = Mod.Assets.Request<Texture2D>("Items/Justitia").Value;
                    weaponOrigin = new Vector2(15, weapon.Height - 15);
                }
                if (NPC.localAI[1] == 3)
                {
                    weapon = Mod.Assets.Request<Texture2D>("Items/Twilight").Value;
                    weaponOrigin = new Vector2(15, weapon.Height - 15);
                }
                if (animState == AnimationState.HeavenThrow)
                {
                    weapon = Mod.Assets.Request<Texture2D>("NPCs/RedMist/HeavenBoss").Value;
                    weaponOrigin = new Vector2(44, 44);
                }
                position = skelly.Weapon1.Position(NPC.spriteDirection, i);
                rot = skelly.Weapon1.GetRotation(NPC.spriteDirection, i) + 0.785f;
                Draw(spriteBatch, weapon, position, weapon.Frame(), color, rot, weaponOrigin, skelly.Weapon1.scale, NPC.spriteDirection, true);
            }

            //Top Layer Arm
            position = skelly.UpperArmL.Position(NPC.spriteDirection, i);
            rot = skelly.UpperArmL.GetRotation(NPC.spriteDirection, i) - (NPC.spriteDirection == 1 ? 0.785f : 2.355f);
            origin = new Vector2(3, 3);
            frame.Y = frame.Height * 1;
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            position = skelly.LowerArmL.Position(NPC.spriteDirection, i);
            rot = skelly.LowerArmL.GetRotation(NPC.spriteDirection, i) - (NPC.spriteDirection == 1 ? 0.785f : 2.355f);
            origin = new Vector2(3, 3);
            frame.Y = frame.Height * 2;
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            position = skelly.HandL.Position(NPC.spriteDirection, i);
            frame.Y = frame.Height * 3;
            Draw(spriteBatch, tex, position, frame, color, rot, origin, 1f, NPC.spriteDirection);

            //---Gauntlet
            if (skelly.Gauntlet.Visible)
            {
                Texture2D weapon = Mod.Assets.Request<Texture2D>("Items/GoldRush").Value;
                position = skelly.Gauntlet.Position(NPC.spriteDirection, i);
                rot = skelly.LowerArmL.GetRotation(NPC.spriteDirection, i) + (NPC.spriteDirection == -1 ? 0.785f : 0.785f + 1.57f);
                Vector2 weaponOrigin = new Vector2(3, 28);
                Draw(spriteBatch, weapon, position, weapon.Frame(), color, rot, weaponOrigin, skelly.Gauntlet.scale, NPC.spriteDirection * -1);
            }
        }

        private void Draw(SpriteBatch spriteBatch, Texture2D tex, Vector2 position, Rectangle frame, Color color, float rot, Vector2 origin, float scale, int dir, bool Weapon = false)
        {
            SpriteEffects speffect = SpriteEffects.None;
            if (dir == -1)
            {
                origin.X = frame.Width - origin.X;
                speffect = SpriteEffects.FlipHorizontally;
                if (Weapon)
                    rot += 1.57f;
            }
            spriteBatch.Draw(tex, position - Main.screenPosition, frame, color, rot, origin, scale, speffect, 0f);
        }
    }    

    class RedMistSkeletonHelper
    {
        //I hate myself
        //Specialied for Red Mist since I think its the only thing that will use this boy sure I hope so :D
        //Rotation Based because math hurts my brain so I just let the computer do it :V
        //Remember its original rotation is 0 which is ->
        //Origin makes everything move, its all parented on Origin
        //Origin doesnt make stuff rotate tho so fuck me I guess
        public Bone Origin;

        public Bone Pelvis;
        public Bone Head;
        public Bone Hair;

        public Bone UpperArmL;
        public Bone UpperArmR;

        public Bone LowerArmL;
        public Bone LowerArmR;

        public Bone HandR;
        public Bone HandL;

        public Bone Weapon1;
        public Bone Weapon2;

        public Bone Gauntlet;

        public Bone HandRIK;
        public Bone HandLIK;

        public Bone UpperLegL;
        public Bone UpperLegR;

        public Bone LowerLegL;
        public Bone LowerLegR;

        public Bone FeetRIK;
        public Bone FeetLIK;

        public RedMistSkeletonHelper(Vector2 origin, Vector2 pelvis, Vector2 shoulderOffsetL, Vector2 shoulderOffsetR, float upperArmLength, float lowerArmLength, Vector2 legOffsetL, Vector2 legOffsetR, float upperLegLength, float lowerLegLength,float StartRot = 1.57f)
        {
            Origin = new Bone(origin, 0, 1, null, false, true);
            Pelvis = new Bone(pelvis, StartRot - 3.14f, 0, Origin, false, true, 10, true);
            Head = new Bone(new Vector2(44, -6), StartRot - 3.14f, 0, Pelvis);
            Hair = new Bone(Vector2.Zero, StartRot, 0, Head);

            UpperArmL = new Bone(shoulderOffsetL, StartRot + MathHelper.ToRadians(90), upperArmLength, Pelvis);
            UpperArmR = new Bone(shoulderOffsetR, StartRot - MathHelper.ToRadians(90), upperArmLength, Pelvis);

            LowerArmL = new Bone(Vector2.Zero, StartRot + MathHelper.ToRadians(90), lowerArmLength, UpperArmL);
            LowerArmR = new Bone(Vector2.Zero, StartRot - MathHelper.ToRadians(90), lowerArmLength, UpperArmR);

            HandL = new Bone(Vector2.Zero, StartRot, 2, LowerArmL);
            HandR = new Bone(Vector2.Zero, StartRot, 2, LowerArmR);

            Weapon1 = new Bone(Vector2.Zero, StartRot, 52, LowerArmL);
            Weapon1.scale = 1.3f;
            Weapon2 = new Bone(Vector2.Zero, StartRot, 52, LowerArmR);

            Gauntlet = new Bone(Vector2.Zero, StartRot, 8, UpperArmL);
            Gauntlet.scale = 0.1f;
            //Gauntlet.Visible = false;

            HandLIK = new Bone(Vector2.Zero, 0, 1, UpperArmL, true);
            HandRIK = new Bone(Vector2.Zero, 0, 1, UpperArmR, true);

            UpperLegL = new Bone(legOffsetL, StartRot, upperLegLength, Pelvis);
            UpperLegR = new Bone(legOffsetR, StartRot, upperLegLength, Pelvis);

            LowerLegL = new Bone(Vector2.Zero, StartRot, lowerLegLength, UpperLegL);
            LowerLegR = new Bone(Vector2.Zero, StartRot, lowerLegLength, UpperLegR);

            FeetLIK = new Bone(Vector2.Zero, 0, 1, Origin, true);
            FeetRIK = new Bone(Vector2.Zero, 0, 1, Origin, true);
        }

        public void UpdateOldBones()
        {
            Origin.Record();

            Pelvis.Record();
            Head.Record();
            Hair.Record();

            UpperArmL.Record();
            UpperArmR.Record();

            LowerArmL.Record();
            LowerArmR.Record();

            HandR.Record();
            HandL.Record();

            Weapon1.Record();
            Weapon2.Record();
            Gauntlet.Record();

            HandRIK.Record();
            HandLIK.Record();

            UpperLegL.Record();
            UpperLegR.Record();

            LowerLegL.Record();
            LowerLegR.Record();

            FeetRIK.Record();
            FeetLIK.Record();
        }

        public enum Part
        {
            O,
            P,
            H,
            UAL,
            UAR,
            FAL,
            FAR,
            HL,
            HLIK,
            HR,
            HRIK,
            ULL,
            ULR,
            UFL,
            UFR,
            LLIK,
            LRIK
        }

        /// <summary>
        /// Direction 1 = Clockwise, Direction -1 = CounterClockwise
        /// </summary>
        /// <param name="dir"></param>
        public void CalculateHandIK(int dir = 1)
        {
            //Left Side
            Vector2 startPoint = UpperArmL.Position();
            Vector2 endPoint = HandLIK.Position();
            Vector2 elbow = ElbowIK(startPoint, endPoint, UpperArmL.length, LowerArmL.length, dir);
            //Main.NewText(elbow);
            UpperArmL.rotation = (elbow - startPoint).ToRotation();
            LowerArmL.rotation = (endPoint - elbow).ToRotation();

            //Right Side
            startPoint = UpperArmR.Position();
            endPoint = HandRIK.Position();
            elbow = ElbowIK(startPoint, endPoint, UpperArmR.length, LowerArmR.length, dir);
            //Main.NewText(elbow);
            UpperArmR.rotation = (elbow - startPoint).ToRotation();
            LowerArmR.rotation = (endPoint - elbow).ToRotation();
        }

        /// <summary>
        /// Direction 1 = Clockwise, Direction -1 = CounterClockwise
        /// </summary>
        /// <param name="dir"></param>
        public void CalculateLegIK(int dir = 1)
        {
            //Left Side
            Vector2 startPoint = UpperLegL.Position();
            Vector2 endPoint = FeetLIK.Position();
            Vector2 elbow = ElbowIK(startPoint, endPoint, UpperLegL.length, LowerLegL.length, -1 * dir);
            //Main.NewText(elbow);
            UpperLegL.rotation = (elbow - startPoint).ToRotation();
            LowerLegL.rotation = (endPoint - elbow).ToRotation();

            //Right Side
            startPoint = UpperLegR.Position();
            endPoint = FeetRIK.Position();
            elbow = ElbowIK(startPoint, endPoint, UpperLegR.length, LowerLegR.length, -1 * dir);
            //Main.NewText(elbow);
            UpperLegR.rotation = (elbow - startPoint).ToRotation();
            LowerLegR.rotation = (endPoint - elbow).ToRotation();
        }

        public Vector2 ElbowIK(Vector2 startPoint, Vector2 endPoint, float length1, float length2, int dir = 1)
        {
            Vector2 elbow = startPoint;
            float Dist = Vector2.Distance(endPoint, startPoint);
            if (Dist > length1 + length2)
                Dist = length1 + length2;
            float Angle = (float)Math.Acos(Dist * Dist / ((length1 + length2) * Dist));
            float Rotation = (endPoint - elbow).ToRotation() + Angle * dir;
            elbow += new Vector2(length1, 0).RotatedBy(Rotation);
            return elbow;
        }
    
        public float ArmLength()
        {
            return UpperArmL.length + LowerArmL.length;
        }

        public float LegLength()
        {
            return UpperLegL.length + LowerLegL.length;
        }
    }

    class Bone
    {
        public Vector2 BoneOffset;
        public Vector2[] oldOffset;

        public float rotation;
        public float[] oldRot;

        Bone Parent;
        public float length;
        public float scale;
        public bool Origin;
        public bool Visible;
        public bool Pelvis;

        bool IKBone;

        public Bone(Vector2 pos, float rot = 0, float Length = 1, Bone parent = null, bool iKBone = false, bool origin = false, int record = 10, bool pelvis = false)
        {
            BoneOffset = pos;
            rotation = rot;
            length = Length;
            Parent = parent;
            IKBone = iKBone;
            Origin = origin;
            Visible = true;
            scale = 1f;
            Pelvis = pelvis;

            oldOffset = new Vector2[record];
            oldRot = new float[record];
        }

        public void Record()
        {
            for (int i = oldOffset.Length - 1; i >= 0; i--)
            {
                if (i > 0)
                {
                    if (oldOffset[i - 1] != null)
                        oldOffset[i] = oldOffset[i - 1];

                    oldRot[i] = oldRot[i - 1];
                }
                else
                {
                    oldOffset[i] = BoneOffset;
                    oldRot[i] = rotation;
                }
            }
        }

        public float GetRotation(int dir = 1, int old = -1)
        {
            if (old >= 0)
                return GetOldRotation(old, dir);
            Vector2 vec1 = new Vector2(1, 0).RotatedBy(rotation);
            vec1.X *= dir;
            return vec1.ToRotation();
        }

        public float GetOldRotation(int i,int dir = 1)
        {
            Vector2 vec1 = new Vector2(1, 0).RotatedBy(oldRot[i]);
            vec1.X *= dir;
            return vec1.ToRotation();
        }

        public Vector2 Position(int dir = 1, int old = -1)
        {
            if (old >= 0)
                return OldPosition(old, dir);
            if (IKBone)
                return BoneOffset + Parent.Position();
            if (Parent != null)
            {
                if (Pelvis)
                    return new Vector2(BoneOffset.X * dir, BoneOffset.Y).RotatedBy(Parent.GetRotation()) + Parent.EndPoint();
                return new Vector2(BoneOffset.X, BoneOffset.Y * dir).RotatedBy(Parent.GetRotation(dir)) + Parent.EndPoint(dir);
            }
            else
                return new Vector2(BoneOffset.X, BoneOffset.Y * dir);
        }

        public Vector2 OldPosition(int i, int dir = 1)
        {
            if (IKBone)
                return oldOffset[i] + Parent.OldPosition(i);
            if (Parent != null)
            {
                if (Pelvis)
                    return new Vector2(oldOffset[i].X * dir, oldOffset[i].Y).RotatedBy(Parent.GetOldRotation(i)) + Parent.OldEndPoint(i);
                return new Vector2(oldOffset[i].X, oldOffset[i].Y * dir).RotatedBy(Parent.GetOldRotation(i, dir)) + Parent.OldEndPoint(i, dir);
            }
            else
                return new Vector2(oldOffset[i].X, oldOffset[i].Y * dir);
        }

        public void AssignParent(Bone bone)
        {
            Parent = bone;
        }

        public float InheritRotation()
        {
            if (Parent != null)
                return rotation + Parent.InheritRotation();
            else
                return rotation;
        }

        public Vector2 EndPoint(int dir = 1)
        {
            if (Pelvis)
                return Position(dir) + new Vector2(length, 0).RotatedBy(GetRotation(dir));
            if (Origin)
                return Position() + new Vector2(length, 0).RotatedBy(rotation);
            return Position(dir) + new Vector2(length, 0).RotatedBy(GetRotation(dir));
        }

        public Vector2 OldEndPoint(int i, int dir = 1)
        {
            if (Pelvis)
                return OldPosition(i, dir) + new Vector2(length, 0).RotatedBy(GetRotation(dir));
            if (Origin)
                return OldPosition(i) + new Vector2(length, 0).RotatedBy(oldRot[i]);
            return OldPosition( i, dir) + new Vector2(length, 0).RotatedBy(GetOldRotation(i, dir));
        }

        public void ChangeBone(Vector2 newPos, float speed, float rot, float rotSpeed)
        {
            if (speed > 0)
            {
                Vector2 bonePos = BoneOffset;
                Vector2 delta = newPos - bonePos;

                if (delta.Length() > speed)
                {
                    delta.Normalize();
                    delta *= speed;
                }
                BoneOffset += delta;
            }

            if (rotSpeed > 0)
                rotation = Terraria.Utils.AngleLerp(rotation, rot, rotSpeed);
        }

        public void ChangeBoneOffset(Vector2 newPos, float speed = 3)
        {
            ChangeBone(newPos, speed, 0, 0);
        }

        public void ChangeBoneRotation(float rot, float rotSpeed = 0.0872f)
        {
            ChangeBone(Vector2.Zero, 0, rot, rotSpeed);
        }
    
        public Vector2 DifferenceBone(Bone bone)
        {
            return bone.Position() - Position();
        }

        public void ChangeBoneScale(float Scale, float speed = 0.15f)
        {
            if (scale < Scale)
            {
                scale += speed;
                if (scale > Scale)
                    scale = Scale;
            }
            else if (scale > Scale)
            {
                scale -= speed;
                if (scale < Scale)
                    scale = Scale;
            }
        }
    }

    class RedMistEye : ModProjectile
    {
        public override string Texture => "LobotomyCorp/Misc/Dusts/RedMistEye";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye");
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 10;

            Projectile.hostile = true;
            Projectile.friendly = false;
        }

        private Trailhelper trail;

        public override void AI()
        {
            NPC n = Main.npc[(int)Projectile.ai[0]];
            if (!n.active || n.life <= 0 || n.type != ModContent.NPCType<RedMist>())
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 10;

            if (trail == null)
                trail = new Trailhelper(30);
            RedMist redmist = (RedMist)n.ModNPC;
            Vector2 pos = Projectile.Center; float rot = 0;
            redmist.GetEye(ref pos, ref rot);
            trail.TrailUpdate(pos, rot + 1.57f);
            Projectile.Center = pos;
            Projectile.rotation = rot;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CustomShaderData shader = LobotomyCorp.LobcorpShaders["GenericTrail"];
            TaperingTrail Trail = new TaperingTrail();
            Trail.ColorStart = Color.Red;
            Trail.ColorEnd = Color.Red;
            Trail.width = 3;

            Trail.Draw(trail.TrailPos, trail.TrailRotation, Vector2.Zero, shader);
            return true;
        }
    }
}
