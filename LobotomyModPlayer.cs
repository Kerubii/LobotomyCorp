using System;
using System.Collections.Generic;
using System.Linq;
using LobotomyCorp.Buffs;
using LobotomyCorp.Items.Aleph;
using LobotomyCorp.Items.Waw;
using LobotomyCorp.ModSystems;
using LobotomyCorp.NPCs.RedMist;
using LobotomyCorp.PlayerDrawEffects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace LobotomyCorp
{
    public class LobotomyModPlayer : ModPlayer
    {
        public int SynchronizedEGO = -1;
        public bool Desync = false;

        public int AttackComboOrder = 0;
        public int AttackComboOrderCooldown = 0;

        public int HeavyWeaponHelper = 0;
        public float ChargeWeaponHelper = 0;

        public bool RedShield = false;
        public bool WhiteShield = false;
        public bool BlackShield = false;
        public bool PaleShield = false;
        public bool CooldownShield = false;
        public int ShieldReapplyCooldown = 0;
        public int ShieldHP = 0;
        public int ShieldHPMax = 0;
        public int ShieldAnim = 0;

        public int statSanity = 100;
        public int statSanityMax = 100;

        public int statFortitude = 0;
        public int statPrudence = 0;
        public int statTemperance = 0;
        public int statJustice = 0;

        public bool RedMistMask = false;

        public bool Hopeless = false;

        public int BeakParry = 0;
        public int BeakPunish = 0;

        public int TwilightSpecial = 10;

        public int BlackSwanParryChance = 0;
        public float BlackSwanNettleClothing = 0;
        public static int BlackSwanNettleClothingMax = 6;
        public bool BlackSwanBrokenDream = false;

        /// <summary>
        /// Use 1 to empower Melee, Use 2 to empower Range
        /// </summary>
        public int CrimsonScarEmpower = 0;

        public int DaCapoSilentMusicPhase = 0;
        public bool DaCapoSilentMusic = false;
        public int DaCapoTotalDamage = 0;

        public float FaintAromaPetal = 0;
        public int FaintAromaPetalMax = 60;
        public float FaintAromaDecay = 0.1f;

        public int HarmonyTime = 0;
        public bool HarmonyAddiction = false;
        public bool HarmonyConnected = false;

        public int ForgottenAffection = -1;
        public float ForgottenAffectionResistance = 0;

        public bool giftFourthMatchFlame = false;
        public int FourthMatchFlameR = 0;
        public bool MatchstickBurn = false;
        public int MatchstickBurnTime = 0;

        public bool PleasureDebuff = false;
        public bool PleasureTail = false;

        public int RegretShockwave = 0;
        public bool RegretChainedWrath = false;
        public bool RegretBinded = false;

        public bool GrinderMk2Active = false;
        public int GrinderMk2Dash = 0;
        public int GrinderMk2BatteryMax = 1400;
        public int GrinderMk2Battery = 1400;
        public bool GrinderMk2Recharging = false;
        public int GrinderMk2AttackCooldown = 0;
        public int GrinderMk2AttackRandom = 0;

        public int LifeForADareDevilGift = 0;
        public bool LifeForADareDevilGiftActive = false;
        public bool LifeForADareDevilCounterStance = false;
        public int LifeForADareDevilCounterArea = 0;

        public int LoveAndHateHysteria = 0;

        public int LuminousGreed = 0;

        public int MagicBulletNthShot = 0;
        public int MagicBulletRequest = -1;
        public bool MagicBulletDarkFlame = false;

        public bool MimicryShell = false;
        public int MimicryShellHealth = 0;
        public float MimicryShellDamage = 1f;
        public int MimicryShellDefense = 0;
        public int MimicryShellTimerMax = 0;
        public bool MimicryHusk = false;
        public int MimicryHuskDeficit = 0;

        public bool NihilActive = false;
        public int NihilMode = 0;

        public bool OurGalaxyStone = false;
        public int OurGalaxyOwner = -1;

        public bool RedEyesAlerted = false;
        public float RedEyesOpacity = 0f;
        public int RedEyesMealMax = 60 * 8;

        public bool SmileDebuff = false;

        public bool SolemnSwitch = false;
        public int SolemnLamentDisableDamage = 0;
        public int SolemnLamentDisable = 0;
        public int SolemnLamentCooldown = 0;

        public int RealizedSword = 0;
        public bool RealizedSwordShoot = false;

        public int TodaysExpressionFace = 0;
        public int TodaysExpressionTimer = 0;
        public int TodaysExpressionTimerMax = 180;
        public bool TodaysExpressionActive = false;

        public bool WristCutterScars = false;

        public int RealizedWingbeatMeal = -1;
        public bool WingbeatFairyMeal = false;
        public bool WingbeatGluttony = false;

        private bool forcePlayerVelocity = false;
        private Vector2 forcePlayerVelocityValue;

        //Aura Vanity
        public AuraBehavior CurrentAura;
        public AuraParticle[] PlayerParticles = new AuraParticle[100];

        public static LobotomyModPlayer ModPlayer(Player Player)
        {
            return Player.GetModPlayer<LobotomyModPlayer>();
        }

        public override void ResetEffects()
        {
            Desync = false;

            if (HeavyWeaponHelper > 0)
                HeavyWeaponHelper--;

            if (Player.itemAnimation == 0)
                ChargeWeaponHelper = 0;

            ResetAttackCombo();

            RedShield = false;
            WhiteShield = false;
            BlackShield = false;
            PaleShield = false;
            CooldownShield = false;

            RedMistMask = false;

            Hopeless = false;

            statSanityMax = 17 + statPrudence;

            if (BeakPunish > 0)
                BeakPunish--;

            BlackSwanParryChance = 0;
            BlackSwanBrokenDream = false;

            if (FaintAromaPetal > 0)
            {
                FaintAromaPetal -= FaintAromaDecay;
            }

            if (ForgottenAffection >= 0)
            {
                if (Player.HeldItem.type != ModContent.ItemType<Items.Ruina.History.ForgottenR>() || !Main.npc[ForgottenAffection].active || Main.npc[ForgottenAffection].life <= 0 || (Main.npc[ForgottenAffection].Center - Player.Center).Length() > 960)
                {
                    ForgottenAffection = -1;
                    ForgottenAffectionResistance = 0;
                }
            }

            DaCapoSilentMusic = false;

            GrinderMk2Active = false;
            if (GrinderMk2AttackCooldown > 0)
                GrinderMk2AttackCooldown--;

            if (LifeForADareDevilGift > 0)
            {
                LifeForADareDevilGift--;
            }
            LifeForADareDevilGiftActive = false;

            giftFourthMatchFlame = false;
            MatchstickBurn = false;

            MimicryShell = false;
            MimicryHusk = false;
            MimicryHuskDeficit = 0;

            if (MagicBulletRequest >= 0 && Player.HeldItem.type != ModContent.ItemType<Items.Ruina.Technology.MagicBulletR>())
                MagicBulletRequest = -1;
            MagicBulletDarkFlame = false;

            NihilActive = false;

            if (!OurGalaxyStone)
                OurGalaxyOwner = -1;
            OurGalaxyStone = false;

            PleasureDebuff = false;

            PleasureTail = false;

            RegretChainedWrath = false;
            RegretBinded = false;

            RedEyesAlerted = false;
            RedEyesOpacity = 0f;

            SmileDebuff = false;

            TodaysExpressionTimerMax = 300;
            TodaysExpressionActive = false;

            RealizedSwordShoot = false;
            if (RealizedWingbeatMeal >= 0 && (!Main.npc[RealizedWingbeatMeal].active || Main.npc[RealizedWingbeatMeal].life <= 0))
                RealizedWingbeatMeal = -1;
            WingbeatFairyMeal = false;
            WingbeatGluttony = false;

            if (HarmonyTime > 0)
                HarmonyTime--;
            HarmonyAddiction = false;
            HarmonyConnected = false;

            WristCutterScars = false;

            //RemoveMaxFallSpeed = false;

            CurrentAura = null;
        }

        private void ResetAttackCombo()
        {
            if (AttackComboOrderCooldown <= 0)
            {
                AttackComboOrder = 0; return;
            }
            AttackComboOrderCooldown--;
        }

        public override void OnRespawn()
        {
            ForgottenAffection = -1;
            ForgottenAffectionResistance = 0;
        }

        public override void UpdateDead()
        {
            SynchronizedEGO = -1;
            Desync = false;

            BlackSwanNettleClothing = 0;

            ForgottenAffection = -1;
            ForgottenAffectionResistance = 0;

            GrinderMk2Recharging = false;


            HarmonyTime = 0;
            HarmonyAddiction = false;

            LifeForADareDevilGift = 0;
            LifeForADareDevilCounterStance = false;
            
            LuminousGreed = 0;

            SolemnLamentDisable = 0;

            FaintAromaPetal = 0;

            NihilActive = false;

            for(int i = 0; i < PlayerParticles.Length; i++)
            {
                if (PlayerParticles[i] != null && PlayerParticles[i].Active)
                    PlayerParticles[i].Active = false;
            }
        }

        public override void OnEnterWorld()
        {
            if (Player.HasBuff<Buffs.NettleClothing>())
                Player.ClearBuff(ModContent.BuffType<Buffs.NettleClothing>());

            if (Player.HasBuff<TodaysLook>())
                Player.ClearBuff(ModContent.BuffType<Buffs.TodaysLook>());

            if (Player.HasBuff<PleasureTail>())
                Player.ClearBuff(ModContent.BuffType<PleasureTail>());

            if (Player.HasBuff<SilentMusic>())
                Player.ClearBuff(ModContent.BuffType<SilentMusic>());

            if (Player.HasBuff<Buffs.Shell>())
                Player.ClearBuff(ModContent.BuffType<Buffs.Shell>());
            if (Player.HasBuff<Buffs.Husk>())
                Player.ClearBuff(ModContent.BuffType<Buffs.Husk>());

            AuraParticle[] AuraParticles = new AuraParticle[100];
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (!mediumCoreDeath)
            {
                return new[]
                {
                    new Item(ModContent.ItemType<Items.ItemTiles.BlackBox>()),
                    new Item(ModContent.ItemType<Items.Zayin.Penitence>())
                };
            }
            return Enumerable.Empty<Item>();
        }

        public override void PreUpdate()
        {
            if (BeakParry > 0)
            {
                BeakParry--;
            }
            //GrinderBatteryHandler
            if (GrinderMk2Recharging)
            {
                if (GrinderMk2Battery < 0)
                    GrinderMk2Battery = 0;
                GrinderMk2Battery += 8;
                if (GrinderMk2Battery > GrinderMk2BatteryMax)
                {
                    GrinderMk2Battery = GrinderMk2BatteryMax;
                    GrinderMk2Recharging = !GrinderMk2Recharging;
                }

                if (Main.rand.Next(8) == 0)
                {
                    Projectiles.Realized.GrinderMk2Cleaner2.SpawnTrailDust(Player.position, Player.width, Player.height, ModContent.DustType<Misc.Dusts.ElectricTrail>(), new Vector2(Main.rand.Next(-2, 3), -1f), 0.5f, 5);
                }
            }

            if (RealizedWingbeatMeal > -1)
            {
                Main.npc[RealizedWingbeatMeal].GetGlobalNPC<LobotomyGlobalNPC>().WingbeatTarget = Player.whoAmI;
            }

            if (ShieldActive)
            {
                ShieldAnim--;
                /*Main.NewText(ShieldHP);
                Main.NewText(ShieldAnim);*/
                if (ShieldHP <= ShieldHPMax / 2)
                {
                    if (ShieldAnim > 120)
                    {
                        ShieldAnim = 60;
                        //Dust Particles when Shield breaks a bit here
                    }
                    if (ShieldAnim <= 0)
                        ShieldAnim = 60;
                }
                else
                {
                    if (ShieldAnim <= 60)
                        ShieldAnim = 120;
                }
            }
        }

        public override void PreUpdateMovement()
        {
            if (forcePlayerVelocity)
            {
                Player.velocity = forcePlayerVelocityValue;
                forcePlayerVelocity = false;
            }
        }

        public override void SetControls()
        {
            if (Hopeless)
            {
                if (Player.velocity.Length() > 0)
                    Player.controlUseItem = false;

                if (Player.itemAnimation > 0)
                {
                    Player.controlJump = false;
                    Player.controlDown = false;
                    Player.controlLeft = false;
                    Player.controlRight = false;
                    Player.controlUp = false;
                }
            }

            if (GrinderMk2Recharging)
            {
                Player.controlMount = false;
                Player.controlHook = false;
                Player.controlUseItem = false;
                Player.controlRight = false;
                Player.controlJump = false;
                Player.controlDown = false;
                Player.controlLeft = false;
                Player.controlRight = false;
                Player.controlUp = false;
            }

            if (GrinderMk2Active && GrinderMk2Dash > 0 &&
                Player.HeldItem.type != ModContent.ItemType<Items.Ruina.Technology.GrinderMk52R>())
            {
                Player.controlUseItem = false;
            }
        }

        public override void PostUpdate()
        {
            if (statSanity > statSanityMax)
                statSanity = statSanityMax;

            if (TodaysExpressionActive)
            {
                TodaysExpressionTimer--;
                if (TodaysExpressionTimer <= 0)
                {
                    TodayExpressionChangeFace(Main.rand.Next(5));
                }
            }

            if (HarmonyAddiction && !HarmonyConnected)
            {
                Player.statDefense -= 15;
            }
        }

        public override void PostUpdateBuffs()
        {
            if (PleasureTail)
            {
                Player.manaRegenBuff = false;
                if (Player.manaRegenBonus > 0)
                    Player.manaRegenBonus = 0;
                Player.manaRegenBonus = -10;
                Player.manaRegenDelayBonus = 0;
                Player.wingAccRunSpeed += 0.15f;
            }
        }

        public override void PostUpdateMiscEffects()
        {
            /*
            if (Player.pulley)
                DashMovement();
            else if (Player.grappling[0] == -1 && !Player.tongued)
            {
                DashMovement();
            }*/

            if (RegretBinded)
            {
                Player.moveSpeed = 1f;
            }
            else if (Player.HeldItem.type == ModContent.ItemType<Items.Ruina.Technology.RegretR>() && !Player.mount.Active)
            {
                Player.GetDamage(DamageClass.Melee) += (Player.moveSpeed - 1) * 0.5f;
                RegretChainedWrath = true;
                //Main.NewText("test");
                Player.moveSpeed = 1f;
            }

            //Aura Effects
            if (CurrentAura != null)
            {
                if (Main.timeForVisualEffects % CurrentAura.intensity == 0)
                    LobotomyPlayerParticle.GenerateAuraParticle(this, CurrentAura);
            }
            for (int i = 0; i < PlayerParticles.Length; i++)
            {
                if (PlayerParticles[i] != null && PlayerParticles[i].Active)
                {
                    PlayerParticles[i].Update(Player, Player.direction, Player.gravDir, (float)Main.timeForVisualEffects);
                }
            }
        }

        public override void PostUpdateEquips()
        {
            if (TodaysExpressionActive && TodaysExpressionFace == 4)//If Angry, set def to 0
            {
                Player.statDefense -= 30;
                if (Player.statDefense > 0)
                    Player.statDefense *= 0;
            }
        }

        private void DashMovement()
        {
            if (GrinderMk2Active)
            {
                float grinderSpeed = 16f;

                if (GrinderMk2Dash > 0)
                {
                    int dir = Math.Sign(Player.velocity.X);
                    Player.dashDelay = 20;

                    Player.velocity.X = grinderSpeed * dir;

                    int Distance = 54;
                    if (Collision.SolidTiles(Player.position + Vector2.UnitY * Player.height, Player.width, Distance + 8, true))
                    {
                        Player.gravity = 0;
                        Player.velocity.Y = 0.00001f;
                        if (Collision.SolidTiles(Player.position + Vector2.UnitY * Player.height, Player.width, Distance, true))
                        {
                            Player.velocity.Y = -4f;
                        }
                    }
                }
                if (Player.dashDelay > 0)
                {
                    Player.velocity.X *= 0.98f;
                    GrinderMk2Dash--;
                }
                else if (!Player.mount.Active)
                {
                    int dir = 0;
                    bool dashing = false;
                    if (Player.dashTime > 0)
                    {
                        Player.dashTime--;
                    }
                    if (Player.dashTime < 0)
                    {
                        Player.dashTime++;
                    }
                    if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[2] < 15)
                    {
                        if (Player.dashTime > 0)
                        {
                            dir = 1;
                            dashing = true;
                            Player.dashTime = 0;
                            Player.timeSinceLastDashStarted = 0;
                        }
                        else
                        {
                            Player.dashTime = 15;
                        }
                    }
                    else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[3] < 15)
                    {
                        if (Player.dashTime < 0)
                        {
                            dir = -1;
                            dashing = true;
                            Player.dashTime = 0;
                            Player.timeSinceLastDashStarted = 0;
                        }
                        else
                        {
                            Player.dashTime = -15;
                        }
                    }

                    if (dashing)
                    {
                        Player.velocity.X = grinderSpeed * dir;
                        Point point = (Player.Center + new Vector2((float)(dir * Player.width / 2 + 2), Player.gravDir * (float)(-Player.height) / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                        Point point2 = (Player.Center + new Vector2((float)(dir * Player.width / 2 + 2), 0f)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y))
                        {
                            Player.velocity.X /= 2f;
                        }
                        Player.dashDelay = -1;
                        Player.direction = dir;
                        GrinderMk2Dash = 40;

                        SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Helper_Atk") with {Volume = 0.25f}, Player.Center);
                    }
                }
            }
            else
                GrinderMk2Dash = 0;
        }
            
        public void ForcePlayerVelocity(Vector2 vel)
        {
            forcePlayerVelocity = true;
            forcePlayerVelocityValue = vel;
        }

        public override void UpdateBadLifeRegen()
        {
            if (MatchstickBurn)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen = MatchstickBurnTime * 2;
            }
            if (PleasureDebuff)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen = -5;
            }
            if (SmileDebuff)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen = -30;
            }
            if (WingbeatGluttony && !WingbeatFairyMeal)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen = -15;
            }

            if (HarmonyAddiction && !HarmonyConnected)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen = -12;
            }

            if (WristCutterScars)
            {
                Player.lifeRegen = 0;
            }
        }

        public override void UpdateLifeRegen()
        {
            if (OurGalaxyStone && !(Player.HasBuff(BuffID.PotionSickness) || Player.potionDelay > 0))
            {
                int value = 5 + 5 * OurGalaxyStoneAllies();
                if (value > 30)
                    value = 30;
                Player.lifeRegen += value;
            }

            if (FaintAromaPetal > 0 && Player.lifeRegen < 0)
                Player.lifeRegen = 0;
        }

        private int OurGalaxyStoneAllies()
        {
            int count = 0;
            foreach (Player p in Main.player)
            {
                if (p.whoAmI != Player.whoAmI && p.team == Player.team && !p.dead)
                {
                    LobotomyModPlayer modPlayer = ModPlayer(p);
                    if (modPlayer.OurGalaxyStone && modPlayer.OurGalaxyOwner == OurGalaxyOwner)
                        count++;
                }
            }

            return count;
        }

        public override bool PreItemCheck()
        {
            if (SynchronizedEGO >= 0 && Player.HeldItem.type != SynchronizedEGO)
            {
                SynchronizedEGO = -1;
            }
            return base.PreItemCheck();
        }

        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            if (PleasureTail)
                healValue = 0;
            base.GetHealMana(item, quickHeal, ref healValue);
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            /*if (Player.HeldItem.type == ModContent.ItemType<Items.BeakS>())
            {
                if (BeakParry > 0)
                {
                    damage /= 2;
                    BeakParry = 0;
                }
                if (BeakPunish > 0)
                    damage /= 2;
                BeakPunish = 180;
                LobotomyGlobalNPC.LNPC(npc).BeakTarget = 180;
            }*/
            if (Player.HeldItem.type == ModContent.ItemType<BlackSwan>() && Main.rand.Next(100) < 10)
            {
                Player.ApplyDamageToNPC(npc, hurtInfo.Damage, 0, Player.direction, false);
            }
        }

        private void OnHitSolemnLament(double damage)
        {
            if (Player.HeldItem.type == ModContent.ItemType<Items.Ruina.Technology.SolemnLamentR>())
            {
                SolemnLamentDisableDamage += (int)damage;
                if (SolemnLamentDisableDamage < Player.statLifeMax2 * 0.25f)
                    return;
                SolemnLamentDisableDamage = 0;
                Player.AddBuff(ModContent.BuffType<Buffs.Lament>(), 300);
                if (SolemnLamentDisable == 0)
                    SolemnLamentDisable = Main.rand.Next(2) + 1;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (RedEyesOpacity > 0f)
            {
                float opacity = 1f - RedEyesOpacity;
                r *= opacity;
                g *= opacity;
                b *= opacity;
                a *= opacity;
            }

            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        }

        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (Player.HeldItem.type == ModContent.ItemType<Items.Ruina.LifeForADaredevilR>() && LifeForADareDevilCounterStance)
            {
                if (Player.channel)
                {
                    LifeForADareDevilCounterStance = false;
                    Player.channel = false;
                    /*
                    foreach (Projectile p in Main.projectile)
                    {
                        if (p.type == ModContent.ProjectileType<Projectiles.Realized.LifeForADaredevilR>() && p.owner == Player.whoAmI)
                        {
                            p.Kill();
                            break;
                        }
                    }*/
                }
                else
                {
                    Player.itemAnimation = Player.itemAnimationMax / 2;
                    Player.itemTime = Player.itemAnimationMax / 2;
                    LifeForADaredevilCounterAttack(Player.HeldItem.damage);
                    LifeForADareDevilGift = 1800;
                    Player.AddBuff(ModContent.BuffType<Buffs.InspiredBravery>(), 10);
                    return false;
                }
            }

            return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            // Pleasure has a bonus chance to dodge Projectiles when no enemies are near or a boss is active, should NOT prevent contact damage
            if (PleasureTail && info.Dodgeable && info.DamageSource.SourceProjectileType > 0 && Main.rand.NextFloat(1f) < PleasureDodgeChance())
            {
                for (int i = 0; i < 8; i++)
                {
                    Vector2 velocity = new Vector2(4, 0).RotatedBy(6.28f * i / 8f);
                    Gore.NewGore(Player.GetSource_FromThis(), Player.MountedCenter, velocity, 331, Main.rand.NextFloat(0.9f, 1.1f));
                }
                Player.immune = true;
                Player.AddImmuneTime(ImmunityCooldownID.General, 45);
                SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Art/Porccu_Nodmg") with { Volume = 0.5f }, Player.Center);
                return true;
            }
            return base.FreeDodge(info);
        }

        private float PleasureDodgeChance()
        {
            // Default Dodge Chance
            float dodge = .2f;

            // If a boss is alive at any point in time, Dodge chance is normal
            if (Main.CurrentFrameFlags.AnyActiveBossNPC)
                return dodge;


            // When an enemy is near, reduce chance to dodge which range from 80% to 20% (20% is impossible since they need to be inside you)
            // 15 Tiles?
            float maxDist = 15 * 16;
            float distance = maxDist;
            foreach (NPC n in Main.npc)
            {
                if (n.active && !n.friendly)
                {
                    float npcDist = n.Center.Distance(Player.Center);
                    if (npcDist < distance)
                    {
                        distance = npcDist;
                    }
                }
            }
            dodge += (0.8f - dodge) * distance / maxDist;

            return dodge;
        }

        public override bool ConsumableDodge(Player.HurtInfo info)
        {
            bool takeDamage = true;
            //Black Swan Nettle Damage Reduction
            if (BlackSwanNettleClothing > 0)
            {
                BlackSwanNettleRemove(1);
                if (BlackSwanNettleClothing < 0)
                {
                    BlackSwanNettleClothing = 0;
                    if (Player.HasBuff<Buffs.NettleClothing>())
                        Player.ClearBuff(ModContent.BuffType<Buffs.NettleClothing>());
                    Player.AddBuff(ModContent.BuffType<Buffs.BrokenDreams>(), 30 * 60);
                    SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Sis_Trans") with { Volume = 0.25f }, Player.Center);
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        int dustType = Main.rand.Next(2, 4);
                        Vector2 dustVel = new Vector2(3f * (float)Math.Cos(6.28f * (i / 8f)), 3f * (float)Math.Sin(6.28f * (i / 8f)));

                        Dust.NewDustPerfect(Player.MountedCenter, dustType, dustVel, 0, default, 1.5f).noGravity = true;
                    }

                    int reflectDamage = info.Damage;

                    if (Player.HeldItem.type == ModContent.ItemType<Items.Ruina.Literature.BlackSwanR>())
                    {
                        if (BlackSwanNettleClothing >= 3 - 1 || BlackSwanBrokenDream)
                        {
                            reflectDamage = (int)(reflectDamage * 1.2f);
                        }
                        if (BlackSwanNettleClothing >= 6 - 1)
                        {
                            Player.immune = true;
                            Player.immuneTime = 30;
                            takeDamage = false;
                        }
                    }

                    if (info.DamageSource.SourceNPCIndex >= 0)
                    {
                        Player.ApplyDamageToNPC(Main.npc[info.DamageSource.SourceNPCIndex], reflectDamage, 0f, Player.direction, false);
                    }
                    if (!takeDamage)
                        return takeDamage;
                    info.Damage = (int)(info.Damage * 0.25f);
                }
            }

            return base.ConsumableDodge(info);
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (info.DamageSource.SourceNPCIndex >= 0)
            {
                if (RedShield || BlackShield && !Player.immune)
                {
                    info.Damage = ShieldDamage(info.Damage);
                }
            }

            if (info.DamageSource.SourceProjectileLocalIndex >= 0)
            {
                if (WhiteShield || BlackShield && !Player.immune)
                {
                    info.Damage = ShieldDamage(info.Damage);
                }
            }

            if (PleasureTail)
            {
                float lifePercent = (float)info.Damage * 2 / Player.statLifeMax2;
                int manaLost = (int)(lifePercent * Player.statManaMax2);
                int max = info.Damage * 2;
                if (manaLost < max)
                {
                    manaLost = max;
                }

                //Main.NewText(manaLost);
                if (!Player.CheckMana(manaLost, true, true))
                {
                    Player.statMana = Player.statManaMax2;
                    int damage = Player.statLifeMax2 / 2;
                    Player.statLife -= damage;
                    CombatText.NewText(Player.getRect(), CombatText.DamagedHostile, damage, true);
                    SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Art/Porccu_Special") with { Volume = 0.5f }, Player.Center);
                }
                else
                {
                    Player.manaRegenDelay = 900;
                }
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Player.HeldItem.type == ModContent.ItemType<Items.Ruina.History.ForgottenR>() && modifiers.DamageSource.SourceNPCIndex == ForgottenAffection)
            {
                modifiers.FinalDamage -= ForgottenAffectionResistance;
                SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Teddy_Guard") with { Volume = 0.5f }, Player.Center);
                modifiers.DisableSound();
            }            

            if (Player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.GreenStemArea>()] >= 1)
            {
                modifiers.FinalDamage += 0.3f;
            }

            if (PleasureTail)
            {
                modifiers.FinalDamage *= 0.33f;
            }
        }

        public void LifeForADaredevilCounterAttack(int damage)
        {
            SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Armor_HeadOff"), Player.position);

            int Distance = 16 * 20;//Radius
            //Additional Damage
            int diff = Player.statLifeMax2 - Player.statLife;
            damage += (int)Player.GetDamage(DamageClass.Melee).ApplyTo(diff);

            foreach (Projectile p in Main.projectile)
            {
                if (p.active && p.hostile && !p.netImportant && p.damage > 0)
                {
                    if (p.Center.Distance(Player.Center) < Distance)
                    {
                        LifeForADareDevilPierceEffect(Player, p.Center, p.width, p.height);
                        p.Kill();
                    }
                }
            }

            Dictionary<int, int> SegmentList = new Dictionary<int, int>();
            float dmgMult = 1f;
            bool NearNPC = false;

            if (LifeForADareDevilGift < 1200)
                dmgMult += .15f;
            else
                dmgMult += 0.1f;
            foreach (NPC n in Main.npc)
            {
                if (n.active && !n.friendly)
                {
                    float npcDist = Distance * (n.boss ? 2 : 1);
                    bool segmentLimit = false;
                    if (n.realLife > -1 && SegmentList.ContainsKey(n.realLife) && SegmentList[n.realLife] > 5)
                    {
                        segmentLimit = true;
                    }
                    float distance = n.Center.Distance(Player.Center);
                    float extra = + (n.width > n.height ? n.width / 2 : n.height / 2);
                    if (distance < npcDist + extra && !segmentLimit)
                    {
                        if (distance < 30 + (n.width > n.height ? n.width / 2 : n.height / 2))
                            NearNPC = true;
                        int x = Math.Sign(Player.position.X - n.position.X) * -1;
                        LifeForADareDevilPierceEffect(Player, n.Center, n.width, n.height);
                        int hitDamage = (int)(damage * dmgMult);
                        Player.ApplyDamageToNPC(n, hitDamage, 8f, x, true);
                        n.immune[Player.whoAmI] = 15;
                        if (LifeForADareDevilGift <= 1200)
                        {
                            int slashes = 2;
                            if (LifeForADareDevilGift <= 600)
                                slashes += 2;
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Realized.LifeForADareDevilEffectsSlashes>(), hitDamage, 0, Player.whoAmI, 60, slashes, n.whoAmI);
                        }
                        if (n.realLife > -1)
                        {
                            if (SegmentList.ContainsKey(n.realLife))
                            {
                                SegmentList[n.realLife]++;
                            }
                            else
                            {
                                SegmentList.Add(n.realLife, 1);
                            }
                        }
                    }
                }
            }

            float scale = 1f + 1.5f * (1f - Player.statLife / (float)Player.statLifeMax2);
            if (Main.myPlayer == Player.whoAmI)
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Realized.LifeForADareDevilEffects>(), 0, 0, Player.whoAmI, scale, Player.direction);

            Player.immune = true;
            Player.immuneTime = 45;
            if (NearNPC)
                Player.immuneTime += 45;
            //Player.immuneNoBlink = true;
            LifeForADareDevilCounterStance = false;
        }

        public static void LifeForADareDevilPierceEffect(Player Player, Vector2 pos, int width, int height)
        {
            if (height > width)
                width = height;

            if (width < 16)
                width = 16;

            float scale = 1f + 1.5f * (1f - Player.statLife / (float)Player.statLifeMax2);

            if (Main.myPlayer == Player.whoAmI)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), pos, Vector2.Zero, ModContent.ProjectileType<Projectiles.Realized.LifeForADareDevilEffects>(), 0, 0, Player.whoAmI, -Main.rand.NextFloat(6.28f), width * scale);
            }
        }

        /// <summary>
        /// True Nettle amount
        /// </summary>
        /// <param name="player"></param>
        /// <param name="NettleAmount"></param>
        /// <returns></returns>
        public bool IsNettleOver(int NettleAmount, bool BrokenDreams = true)
        {
            if (BrokenDreams)
                return BlackSwanNettleClothing >= NettleAmount || BlackSwanBrokenDream;
            return BlackSwanNettleClothing >= NettleAmount;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            OnHitSolemnLament(info.Damage);

            int heldItem = Player.HeldItem.type;
            if (heldItem == ModContent.ItemType<Items.Aleph.Censored>())
            {
                float healing = (float)info.Damage * 0.4f;
                if (info.Damage > 1)
                {
                    Player.statLife += (int)healing;
                    Player.HealEffect((int)healing);
                }
            }
            else if (heldItem == ModContent.ItemType<Items.Ruina.Literature.TodaysExpressionR>() && Main.myPlayer == Player.whoAmI)
            {
                float velocityMult = 1f;
                float knockback = 6f;
                float projDamage = Player.HeldItem.damage;
                switch (TodaysExpressionFace)
                {
                    case 0://Happy
                        velocityMult *= 0.1f;
                        knockback *= 3f;
                        projDamage = (int)(info.Damage * 0.05f);
                        break;
                    case 1://Smile
                        velocityMult *= 0.4f;
                        knockback *= 2f;
                        projDamage = (int)(info.Damage * 0.5f);
                        break;
                    default://Neutral
                        break;
                    case 3://Sad
                        velocityMult *= 1.5f;
                        knockback *= 0.5f;
                        projDamage = (int)(info.Damage * 1.1f);
                        break;
                    case 4://Angry
                        velocityMult *= 2.2f;
                        knockback *= 0f;
                        projDamage = (int)(info.Damage * 1.5f);
                        break;
                }
                
                for (int i = 0; i < 6; i++)
                {
                    Vector2 vel = new Vector2(16f, 0).RotatedBy(MathHelper.ToRadians(60 * i));

                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, vel * velocityMult, ModContent.ProjectileType<Projectiles.Realized.TodaysExpressionWall>(), (int)projDamage, knockback, Player.whoAmI, TodaysExpressionFace);
                }

                SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Literature/Shy_Strong_Guard") with { Volume = 0.5f }, Player.position);
            }
        }

        /*public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            //MimicryShell Attack Boost
            if (MimicryShell)
            {
                modifiers.SourceDamage += MimicryBonusDamage;
            }
        }*/

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (FaintAromaPetal > 0)
            {
                if (FaintAromaPetal < FaintAromaPetalMax)
                    modifiers.FinalDamage *= (1.1f + ((float)FaintAromaPetal / (float)FaintAromaPetalMax));
                else
                    modifiers.FinalDamage *= 1.2f;
            }
        }

        /*public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (WhiteShield || BlackShield && !Player.immune)
            {
                damage = ShieldDamage(damage);
            }
        }*/

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (FaintAromaPetal > 0 && Player.HeldItem.type == ModContent.ItemType<Items.Ruina.Art.FaintAromaS>() && Player.itemAnimation > 0 && npc.immune[Player.whoAmI] > 0)
                return false;
            return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }
        /*
        public override void FrameEffects()
        {
            if (Player.HeldItem.type == ModContent.ItemType<Items.Ruina.History.WingbeatS>())
            {
                //Main.NewText(EquipLoader.GetEquipSlot(Mod, "LobotomyCorp:WingBeatBlades", EquipType.HandsOn));
                Player.han = EquipLoader.GetEquipSlot(Mod, "LobotomyCorp:WingBeatBlades", EquipType.HandsOn);
            }
        }*/

        public void FourthMatchExplode(bool forced = false)
        {
            if (giftFourthMatchFlame)
                return;
            Player.AddBuff(BuffID.OnFire, 120);
            if (Main.myPlayer == Player.whoAmI && forced)// || (Main.rand.Next(100) == 0 && (Player.statLife == Player.statLifeMax2 || (float)(Player.statLife / (float)Player.statLifeMax2) < 0.25f)))
            {
                int i = Projectile.NewProjectile(Player.GetSource_Misc("ItemUse_FourthMatchFlameExplosion"), Player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.FourthMatchFlameSelfExplosion>(), 1600, 10f, Player.whoAmI);

                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, i);
            }
        }

        public void TodayExpressionChangeFace(int face)
        {
            TodaysExpressionTimer = TodaysExpressionTimerMax;
            TodaysExpressionFace = face;
        }

        public float TodaysExpressionDamage()
        {
            switch (TodaysExpressionFace)
            {
                case 0://Happy
                    return Buffs.TodaysLook.TODAYDAMAGEHAPPY;
                case 1://Smile
                    return Buffs.TodaysLook.TODAYDAMAGESMILE;
                default://Neutral
                    return Buffs.TodaysLook.TODAYDAMAGENEUTRAL;
                case 3://Sad
                    return Buffs.TodaysLook.TODAYDAMAGESAD;
                case 4://Angry
                    return Buffs.TodaysLook.TODAYDAMAGEANGRY;
            }
        }

        public void BlackSwanNettleAdd(float val)
        {
            int nextVal = (int)Math.Ceiling(BlackSwanNettleClothing);
            float oldVal = BlackSwanNettleClothing;
            BlackSwanNettleClothing += val;
            if (nextVal != 0 && BlackSwanNettleClothing > nextVal && oldVal < nextVal)
            {
                //SpecialEffects with Nettle Clothing
                Vector2 dustPos = Player.MountedCenter + new Vector2(30 + 3 * (float)Math.Sin(MathHelper.ToRadians(5 * (float)Main.timeForVisualEffects)), 0).RotatedBy(MathHelper.ToRadians(60 + 60 * nextVal - 1));
                for (int i = 0; i < 8; i++)
                {
                    int dustType = Main.rand.Next(2, 4);
                    Vector2 dustVel = new Vector2(1f * (float)Math.Cos(6.28f * (i / 8f)), 1f * (float)Math.Sin(6.28f * (i / 8f)));

                    Dust.NewDustPerfect(dustPos, dustType, dustVel).noGravity = true;
                }

                SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/BlackSwan_Revive") with { Volume = 0.1f }, Player.Center);
            }
        }

        /*
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (damageSource.SourceCustomReason != null)
                return;

            if (damageSource.SourceProjectileType == ModContent.ProjectileType<Projectiles.DespairSword>())
            {
                if (Main.rand.Next(2) == 0)
                    damageSource.SourceCustomReason = Player.name + " fell into Despair.";
                else
                    damageSource.SourceCustomReason = Player.name + " was stricken with Grief.";
            }
            if (damageSource.SourceProjectileType == ModContent.ProjectileType<Projectiles.FourthMatchFlameExplosion>())
            {
                damageSource.SourceCustomReason = Player.name + " was reduced to ashes...";
            }
            if (WingbeatGluttony && damageSource.SourceOtherIndex == 8)
            {
                damageSource.SourceCustomReason = Player.name + " was starved of its prey";
            }
            if (LuminousGreed > 1200 && damageSource.SourceOtherIndex == 8)
            {
                damageSource.SourceCustomReason = Player.name + "'s body turned into lumps of overgrown meat";
            }
            if (Main.npc[damageSource.SourceNPCIndex].type == ModContent.NPCType<NPCs.RedMist.RedMist>() ||
                damageSource.SourceProjectileType == ModContent.ProjectileType<NPCs.RedMist.RedMistMelee>() ||
                damageSource.SourceProjectileType == ModContent.ProjectileType<NPCs.RedMist.RedMistMimicry>() ||
                damageSource.SourceProjectileType == ModContent.ProjectileType<NPCs.RedMist.RedMistDaCapo>() ||
                damageSource.SourceProjectileType == ModContent.ProjectileType<NPCs.RedMist.DaCapoThrow>() ||
                damageSource.SourceProjectileType == ModContent.ProjectileType<NPCs.RedMist.DaCapoLegato>() ||
                damageSource.SourceProjectileType == ModContent.ProjectileType<NPCs.RedMist.SwitchMimicryThrow>() ||
                damageSource.SourceProjectileType == ModContent.ProjectileType<NPCs.RedMist.SwitchDaCapoThrow>() ||
                damageSource.SourceProjectileType == ModContent.ProjectileType<NPCs.RedMist.HeavenBoss>() ||
                damageSource.SourceProjectileType == ModContent.ProjectileType<NPCs.RedMist.JustitiaSlashBoss>())
            {
                damageSource.SourceCustomReason = Player.name + "'s body was reduced to a red mist";
            }
            //could not finish their performance
            //"Goodbye"
            //'s head bursted from pleasure
            //tried to forcefully escape their established role
            //'s malice overtook their body
            //'s role as a villain has ended
            //thirst went unquenched
            //was judged to be sinful
            //disrespected the fairy's care
            //'s unbearable loneliness crushed them
            //averted their gaze 
            
            
            //was overthrown
            //was consumed by Malice
        
            //could not satiate their addiction
            //'s soul has fallen to hell

            //could not protect their beloved family
            base.Kill(damage, hitDirection, pvp, damageSource);
        }*/

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (OurGalaxyStone)
            {
                foreach (Player p in Main.player)
                {
                    if (p.whoAmI != Player.whoAmI && p.team == Player.team && !p.dead)
                    {
                        LobotomyModPlayer modPlayer = ModPlayer(p);
                        if (modPlayer.OurGalaxyStone && modPlayer.OurGalaxyOwner == OurGalaxyOwner)
                        {
                            damageSource.SourceCustomReason = p.name + "'s friends has died";
                            p.KillMe(damageSource, p.statLifeMax2 * 4, 1);
                        }
                    }
                }
            }

            if (NPC.AnyNPCs(ModContent.NPCType<RedMist>()))
            {
                LobEventFlags.killedByRedMist = true;
            }

            base.Kill(damage, hitDirection, pvp, damageSource);
        }

        public static float Lerp(float x, float x2, float progress, bool reverse = false)
        {
            if (progress < 0)
                progress = 0;
            if (progress > 1f)
                progress = 1f;
            if (reverse)
                return x2 * (1 - progress) + x * progress;
            else
                return x * (1 - progress) + x2 * progress;
        }

        #region R EGO STUFF
        /// <summary>
        /// Soft Limits are HP = 500, Damage = 50%, Defense = 30
        /// </summary>
        /// <param name="n"></param>
        public void MimicryWearShell(NPC n)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust.NewDust(Player.position, Player.width, Player.height, DustID.Blood);
            }

            if (Player.HasBuff<Husk>())
            {
                Player.ClearBuff(ModContent.BuffType<Husk>());
            }
            int time = Math.Min(n.lifeMax * 30, 300 * 60);
            Player.AddBuff(ModContent.BuffType<Shell>(), time);
            MimicryShell = true;
            MimicryShellDamage = Math.Min(n.damage / 10f, .8f);
            MimicryShellDefense = Math.Min(n.defense, 30);
            MimicryShellHealth = Math.Min((int)(n.lifeMax * 0.1f), 500);
            MimicryShellTimerMax = time;
        }

        public void MimicryIncreaseShellTime(int Time)
        {
            int index = Player.FindBuffIndex(ModContent.BuffType<Shell>());
            Player.buffTime[index] += Time;
            if (Player.buffTime[index] > MimicryShellTimerMax)
                Player.buffTime[index] = MimicryShellTimerMax;
        }

        public float MimicryShellPercent()
        {
            int index = -1;
            if (Player.HasBuff<Shell>())
                index = Player.FindBuffIndex(ModContent.BuffType<Shell>());
            if (index < 0)
                return 0;

            float percent = (float)Player.buffTime[index] / MimicryShellTimerMax;
            if (percent > 1f)
                percent = 1f;
            return percent;
        }
        public int MimicryBonusHealth { get { return (int)(MimicryShellHealth * MimicryShellPercent()); } }
        public int MimicryBonusDamage { get { return (int)(MimicryShellDamage * MimicryShellPercent()); } }
        public int MimicryBonusDefense { get { return (int)(MimicryShellDefense * MimicryShellPercent()); } }

        public static void PleasureManaConsume(Player player, int cost)
        {
            bool Mana = player.CheckMana(Main.LocalPlayer.statManaMax2 / 5, true, true);

            if (!Mana)
            {
                player.statMana = player.statManaMax2;
                player.ManaEffect(player.statManaMax2);
                player.statLife -= player.statLifeMax / 2;
                player.HealEffect(-player.statLifeMax / 2);
            }
        }

        public void BlackSwanNettleRemove(int val)
        {
            BlackSwanNettleClothing = (int)Math.Floor(BlackSwanNettleClothing - val);
        }

        public bool NihilCheckActive()
        {
            bool checkActive = false;

            for (int slot = 0; slot < Main.InventorySlotsTotal; slot++)
            {
                if (!Player.inventory[slot].IsAir && Player.inventory[slot].type != ModContent.ItemType<Items.Ruina.Natural.NihilR>())
                {
                    checkActive = true;
                }
            }

            for (int slot = 0; slot < 8 + Player.extraAccessorySlots; slot++)
            {
                if (!Player.armor[slot].IsAir && Player.armor[slot].type != ModContent.ItemType<Items.Ruina.Natural.NihilR>())
                {
                    checkActive = true;
                }
            }

            NihilActive = checkActive;
            return NihilActive;
        }
        #endregion

        #region SHIELD STUFF
        /// <summary>
        /// Apply the RWBP shields, use the buff IDs.
        /// </summary>
        public void ApplyShield(int type, int time, int shieldHP, bool forceApply = false)
        {
            if (!forceApply && ShieldActive)
                return;

            ShieldReset(ShieldActive, forceApply);
            Player.AddBuff(type, time);
            ShieldHP = shieldHP;
            ShieldHPMax = ShieldHP;
            ShieldAnim = 120;

            int dustType = 62;
            if (type == ModContent.BuffType<Buffs.ShieldR>())
                dustType = 60;
            else if (type == ModContent.BuffType<Buffs.ShieldW>())
                dustType = 63;
            else if (type == ModContent.BuffType<Buffs.ShieldP>())
                dustType = 59;
            for (int i = 0; i < 10; i++)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Player.RotatedRelativePoint(Player.MountedCenter, true) - new Vector2(33, 33);
                dust = Main.dust[Terraria.Dust.NewDust(position, 66, 66, dustType, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
            }
        }

        /// <summary>
		/// Use "R" "W" "B" "P" for type for this one, only works if buff name is "Shield" + Color.
		/// </summary>
        public void ApplyShield(string type, int time, int shieldHP, bool forceApply = false)
        {
            ApplyShield(Mod.Find<ModBuff>("Shield" + type).Type, time, shieldHP, forceApply);
        }

        public bool ShieldActive => RedShield || WhiteShield || BlackShield || PaleShield || CooldownShield;

        /// <summary>
		/// Manually Reset Shields, True for break for shield breaking particles. 
		/// </summary>
        public void ShieldReset(bool broke = false, bool forceApply = false)
        {
            ShieldHP = 0;
            ShieldHPMax = 0;
            ShieldAnim = 0;
            int remainingTime = 0;
            for (int i = 0; i < 4 + forceApply.ToInt(); i++)
            {
                string letter = "R";
                switch (i)
                {
                    case 0:
                        letter = "R";
                        break;
                    case 1:
                        letter = "W";
                        break;
                    case 2:
                        letter = "B";
                        break;
                    case 3:
                        letter = "P";
                        break;
                    default:
                        letter = "Cooldown";
                        break;
                }
                int ModBuff = Mod.Find<ModBuff>("Shield" + letter).Type;
                if (Player.HasBuff(ModBuff))
                {
                    if (letter != "Cooldown")
                        remainingTime += Player.buffTime[Player.FindBuffIndex(ModBuff)];
                    Player.buffTime[Player.FindBuffIndex(ModBuff)] = 0;
                }
            }

            if (remainingTime > 0 && !forceApply)
                Player.AddBuff(ModContent.BuffType<Buffs.ShieldCooldown>(), remainingTime);

            //if (broke) //Dust Particles when breaking
        }

        /// <summary>
		/// Returns leftovers
		/// </summary>
        public int ShieldDamage(int damage)
        {
            Player.immune = true;
            Player.immuneTime = 40;
            int damageAbsorb = Main.DamageVar(damage);
            ShieldHP -= damageAbsorb;
            if (ShieldHP <= 0)
            {
                int leftover = ShieldHP * -1;
                ShieldReset(true);
                return leftover;
            }
            return 0;
        }

        public int shakeTimer = 0;
        public int shakeIntensity = 0;

        public override void ModifyScreenPosition()
        {
            if (shakeTimer > 0)
            {
                shakeTimer--;

                Main.screenPosition += new Vector2(shakeIntensity * Main.rand.NextFloat(), shakeIntensity * Main.rand.NextFloat());

                if (shakeTimer % 5 == 0 && shakeIntensity > 1)
                    shakeIntensity--;
            }
            else
                shakeIntensity = 0;
        }

        #endregion
    }
}