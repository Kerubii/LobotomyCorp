using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool Hopeless = false;

        public int BeakParry = 0;
        public int BeakPunish = 0;

        public int TwilightSpecial = 10;

        public int BlackSwanParryChance = 0;
        public float BlackSwanNettleClothing = 0;
        public static int BlackSwanNettleClothingMax = 6;
        public bool BlackSwanBrokenDream = false;

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

        public bool GrinderMk2Active = false;
        public int GrinderMk2Dash = 0;
        public int GrinderMk2BatteryMax = 1400;
        public int GrinderMk2Battery = 1400;
        public bool GrinderMk2Recharging = false;

        public int LoveAndHateHysteria = 0;

        public int LuminousGreed = 0;

        public int MagicBulletNthShot = 0;
        public int MagicBulletRequest = -1;
        public bool MagicBulletDarkFlame = false;

        public bool NihilActive = false;
        public int NihilMode = 0;

        public bool RedEyesActive = false;
        public float RedEyesOpacity = 0f;
        public int RedEyesMealMax = 60 * 8;

        public bool SmileDebuff = false;

        public bool SolemnSwitch = false;
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

            RedShield = false;
            WhiteShield = false;
            BlackShield = false;
            PaleShield = false;
            CooldownShield = false;

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

            GrinderMk2Active = false;

            giftFourthMatchFlame = false;
            MatchstickBurn = false;

            if (MagicBulletRequest >= 0 && Player.HeldItem.type != ModContent.ItemType<Items.Ruina.Technology.MagicBulletR>())
                MagicBulletRequest = -1;
            MagicBulletDarkFlame = false;

            NihilActive = false;

            PleasureDebuff = false;

            RedEyesActive = false;
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
        }

        public override void OnRespawn(Player player)
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
            LuminousGreed = 0;
            FaintAromaPetal = 0;

            NihilActive = false;
        }

        public override void OnEnterWorld(Player player)
        {
            if (player.HasBuff<Buffs.NettleClothing>())
                player.ClearBuff(ModContent.BuffType<Buffs.NettleClothing>());
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (!mediumCoreDeath)
            {
                return new[]
                {
                    new Item(ModContent.ItemType<Items.BlackBox>()),
                    new Item(ModContent.ItemType<Items.Penitence>())
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

        public override void PostUpdateMiscEffects()
        {
            if (Player.pulley)
                DashMovement();
            else if (Player.grappling[0] == -1 && !Player.tongued)
            {
                DashMovement();
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
                    if (Player.controlRight && Player.releaseRight)
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
                    else if (Player.controlLeft && Player.releaseLeft)
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
            if (WingbeatGluttony)
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
            if (WingbeatFairyMeal)
            {
                Player.lifeRegen += 10;
            }
            if (FaintAromaPetal > 0 && Player.lifeRegen < 0)
                Player.lifeRegen = 0;
        }

        public override bool PreItemCheck()
        {
            if (SynchronizedEGO >= 0 && Player.HeldItem.type != SynchronizedEGO)
            {
                SynchronizedEGO = -1;
            }
            return base.PreItemCheck();
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
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
            if (BlackSwanParryChance > 0 && Main.rand.Next(100) < BlackSwanParryChance && Player.CanParryAgainst(Player.Hitbox, npc.Hitbox, npc.velocity))
            {
                Player.ApplyDamageToNPC(npc, damage, 0, Player.direction, false);
            }
        }

        private void OnHitSolemnLament()
        {
            if (Player.HeldItem.type == ModContent.ItemType<Items.Ruina.Technology.SolemnLamentR>())
            {
                Player.AddBuff(ModContent.BuffType<Buffs.Lament>(), 180);
                if (SolemnLamentDisable == 0)
                    SolemnLamentDisable = Main.rand.Next(2) + 1;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Player.whoAmI == Main.myPlayer && LobotomyCorp.PassiveShow.JustPressed)
            {
                LobotomyCorp.ExtraPassiveShow = !LobotomyCorp.ExtraPassiveShow;
            }
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

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            bool takeDamage = true;

            if (damageSource.SourceNPCIndex >= 0)
            {
                if (RedShield || BlackShield && !Player.immune)
                {
                    damage = ShieldDamage(damage);
                    if (damage <= 0)
                        return false;
                }
            }

            if (damageSource.SourceProjectileIndex >= 0)
            {
                if (WhiteShield || BlackShield && !Player.immune)
                {
                    damage = ShieldDamage(damage);
                    if (damage <= 0)
                        return false;
                }
            }

            if (Player.HeldItem.type == ModContent.ItemType<Items.Ruina.History.ForgottenR>() && damageSource.SourceNPCIndex == ForgottenAffection)
            {
                damage = damage - (int)(damage * ForgottenAffectionResistance);
                SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Teddy_Guard") with { Volume = 0.5f }, Player.Center);
                playSound = false;
            }

            if (Player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.GreenStemArea>()] >= 1)
            {
                damage += (int)(damage * 0.3f);
            }

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

                    int reflectDamage = damage;

                    if (Player.HeldItem.type == ModContent.ItemType<Items.Ruina.Literature.BlackSwanR>())
                    {
                        if (BlackSwanNettleClothing >= 3 - 1)
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

                    if (damageSource.SourceNPCIndex >= 0)
                    {                        
                        Player.ApplyDamageToNPC(Main.npc[damageSource.SourceNPCIndex], reflectDamage, 0f, Player.direction, false);
                    }
                    if (!takeDamage)
                        return takeDamage;
                    damage = (int)(damage * 0.25f);
                }
            }

            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            OnHitSolemnLament();

            if (Player.HeldItem.type == ModContent.ItemType<Items.Censored>())
            {
                float healing = (float)damage * 0.4f;
                if (damage > 1)
                {
                    Player.statLife += (int)healing;
                    Player.HealEffect((int)healing);
                }
            }

            base.PostHurt(pvp, quiet, damage, hitDirection, crit, cooldownCounter);
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (FaintAromaPetal > 0)
            {
                if (FaintAromaPetal < FaintAromaPetalMax)
                    damage = (int)(damage * (1.1f + ((float)FaintAromaPetal / (float)FaintAromaPetalMax)));
                else
                    damage = (int)(damage * 1.2f);
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
            if (forced)// || (Main.rand.Next(100) == 0 && (Player.statLife == Player.statLifeMax2 || (float)(Player.statLife / (float)Player.statLifeMax2) < 0.25f)))
            {
                Projectile.NewProjectile(Player.GetSource_Misc("ItemUse_FourthMatchFlameExplosion"), Player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.FourthMatchFlameSelfExplosion>(), 500, 10f, Player.whoAmI);
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
                    return 0.25f;
                case 1://Smile
                    return 0.5f;
                default://Neutral
                    return 1f;
                case 3://Sad
                    return 1.2f;
                case 4://Angry
                    return 1.5f;
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
            //could not satiate their addiction
            //'s head bursted from pleasure
            //tried to forcefully escape their established role
            //'s malice overtook their body
            //'s role as a villain has ended
            //thirst went unquenched
            //was judged to be sinful
            //disrespected the fairy's care
            //'s unbearable loneliness crushed them
            //averted their gaze 
            base.Kill(damage, hitDirection, pvp, damageSource);
        }*/

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

        /// <summary>
		/// Apply the RWBP shields use the buffs.
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
    }

    public class DrawLobWeaponFront : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HeldItem);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.HeldItem.type == ModContent.ItemType<Items.Ruina.Technology.SolemnLamentR>() || player.HeldItem.type == ModContent.ItemType<Items.ParadiseLost>();
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player Player = drawInfo.drawPlayer;
            if (!Player.HeldItem.IsAir && Player.itemAnimation != 0 && Player.HeldItem.type == ModContent.ItemType< Items.Ruina.Technology.SolemnLamentR >())
            {
                LobotomyGlobalItem item = Player.HeldItem.GetGlobalItem<LobotomyGlobalItem>();

                if (!item.CustomDraw)
                    return;

                Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Ruina/Technology/SolemnLamentS2").Value;

                Color color = Lighting.GetColor((int)(Player.Center.X / 16f), (int)(Player.Center.Y / 16f));

                Vector2 position = drawInfo.ItemLocation - Main.screenPosition;
                Vector2 origin = new Vector2(Player.direction == 1 ? 0 : texture.Width, texture.Height);
                float rot = Player.itemRotation;

                if (Player.HeldItem.useStyle == 5)
                {
                    Vector2 textureCenter = new Vector2((float)(texture.Width / 2f), (float)(texture.Height / 2f));

                    float num = 10f;
                    Vector2 result = textureCenter;
                    result.X = num;
                    ItemLoader.HoldoutOffset(Player.gravDir, Player.HeldItem.type, ref result);


                    Vector2 PlayerItemPos = result;

                    int x = (int)PlayerItemPos.X;
                    textureCenter.Y = PlayerItemPos.Y;
                    origin = new Vector2(-x, texture.Height / 2);
                    if (Player.direction == -1)
                    {
                        origin = new Vector2(texture.Width + x, texture.Height / 2);
                    }
                    position.X += textureCenter.X + (Player.GetModPlayer<LobotomyModPlayer>().SolemnSwitch ? -6 : 0) * Player.direction; ;
                    position.Y += textureCenter.Y;

                    if (Player.GetModPlayer<LobotomyModPlayer>().SolemnSwitch)
                    {
                        rot -= MathHelper.ToRadians(30 + 75 * (1 - (float)Player.itemAnimation / (float)Player.itemAnimationMax)) * Player.direction;
                    }
                }

                drawInfo.DrawDataCache.Add(
                    new DrawData(
                        texture, //pass our glowmask's texture
                        position, //pass the position we should be drawing at from the PlayerDrawInfo we pass into this method. Always use this and not Player.itemLocation.
                        texture.Frame(), //our source rectangle should be the entire frame of our texture. If our mask was animated it would be the current frame of the animation.
                        color, //since we want our glowmask to glow, we tell it to draw with Color.White. This will make it ignore all lighting
                        rot, //the rotation of the Player's item based on how they used it. This allows our glowmask to rotate with swingng swords or guns pointing in a direction.
                        origin, //the origin that our mask rotates about. This needs to be adjusted based on the Player's direction, thus the ternary expression.
                        Player.HeldItem.scale, //scales our mask to match the item's scale
                        drawInfo.playerEffect, //the PlayerDrawInfo that was passed to this will tell us if we need to flip the sprite or not.
                        0 //we dont need to worry about the layer depth here
                    ));
            }

            if (!Player.HeldItem.IsAir && Player.itemAnimation != 0 && Player.HeldItem.type == ModContent.ItemType<Items.ParadiseLost>())
            {
                Texture2D tex = TextureAssets.Item[Player.HeldItem.type].Value;

                float OffsetY = -46;
                Vector2 position = Player.MountedCenter - Main.screenPosition + new Vector2(5 * Player.direction, OffsetY + Player.gfxOffY);
                Vector2 origin = new Vector2(23, 65);
                if (Player.direction < 0)
                    origin.X = 39;


                Color color = Lighting.GetColor((int)(Player.Center.X / 16f), (int)(Player.Center.Y / 16f));

                drawInfo.DrawDataCache.Add(
                    new DrawData(
                        tex,
                        position,
                        tex.Frame(),
                        color,
                        MathHelper.ToRadians(-30 * Player.direction),
                        tex.Size() / 2,
                        Player.HeldItem.scale,
                        drawInfo.playerEffect,
                        0
                    ));
            }
        }
    }

    public class DrawLobWeaponBack : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BackAcc);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player Player = drawInfo.drawPlayer;
            if (!Player.HeldItem.IsAir && Player.itemAnimation != 0 && Player.HeldItem.type == ModContent.ItemType<Items.Ruina.Technology.SolemnLamentR>())
            {
                LobotomyGlobalItem item = Player.HeldItem.GetGlobalItem<LobotomyGlobalItem>();

                if (!item.CustomDraw)
                    return;

                Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Ruina/Technology/SolemnLamentS1").Value;

                Color color = Lighting.GetColor((int)(Player.Center.X / 16f), (int)(Player.Center.Y / 16f));

                Vector2 position = drawInfo.ItemLocation - Main.screenPosition;
                Vector2 origin = new Vector2(Player.direction == 1 ? 0 : texture.Width, texture.Height);
                float rot = Player.itemRotation;

                if (Player.HeldItem.useStyle == 5)
                {
                    Vector2 textureCenter = new Vector2((float)(texture.Width / 2f), (float)(texture.Height / 2f));

                    float num = 10f;
                    Vector2 result = textureCenter;
                    result.X = num;
                    ItemLoader.HoldoutOffset(Player.gravDir, Player.HeldItem.type, ref result);

                    /*if (Player.GetModPlayer<LobotomyModPlayer>().SolemnSwitch)
                    {
                        Player.itemRotation -= MathHelper.ToRadians(45) * Player.direction;
                        rot += MathHelper.ToRadians(45) * Player.direction;
                    }*/

                    Vector2 PlayerItemPos = result;

                    int x = (int)PlayerItemPos.X;
                    textureCenter.Y = PlayerItemPos.Y;
                    origin = new Vector2(-x, texture.Height / 2);
                    if (Player.direction == -1)
                    {
                        origin = new Vector2(texture.Width + x, texture.Height / 2);
                    }
                    position.X += textureCenter.X + (!Player.GetModPlayer<LobotomyModPlayer>().SolemnSwitch ? 6 : 3) * Player.direction;
                    position.Y += textureCenter.Y;

                    if (!Player.GetModPlayer<LobotomyModPlayer>().SolemnSwitch)
                    {
                        rot -= MathHelper.ToRadians(30 + 75 * (1 - (float)Player.itemAnimation/(float)Player.itemAnimationMax)) * Player.direction;
                    }
                }

                drawInfo.DrawDataCache.Add(
                    new DrawData(
                        texture, //pass our glowmask's texture
                        position, //pass the position we should be drawing at from the PlayerDrawInfo we pass into this method. Always use this and not Player.itemLocation.
                        texture.Frame(), //our source rectangle should be the entire frame of our texture. If our mask was animated it would be the current frame of the animation.
                        color, //since we want our glowmask to glow, we tell it to draw with Color.White. This will make it ignore all lighting
                        rot, //the rotation of the Player's item based on how they used it. This allows our glowmask to rotate with swingng swords or guns pointing in a direction.
                        origin, //the origin that our mask rotates about. This needs to be adjusted based on the Player's direction, thus the ternary expression.
                        Player.HeldItem.scale, //scales our mask to match the item's scale
                        drawInfo.playerEffect, //the PlayerDrawInfo that was passed to this will tell us if we need to flip the sprite or not.
                        0 //we dont need to worry about the layer depth here
                    ));
            }

            if (!Player.HeldItem.IsAir && Player.itemAnimation != 0 && Player.HeldItem.type == ModContent.ItemType<Items.Ruina.Art.FaintAromaS>() && Player.heldProj > -1 && Main.projectile[Player.heldProj].type == ModContent.ProjectileType<Projectiles.FaintAromaS>() )
            {
                Projectile projectile = Main.projectile[Player.heldProj];
                
                Texture2D tex = TextureAssets.Projectile[projectile.type].Value;
                float rot = projectile.ai[1];
                Vector2 ownerMountedCenter = Player.RotatedRelativePoint(Player.MountedCenter, true) + new Vector2(8, 0).RotatedBy(rot);
                Vector2 position = ownerMountedCenter - Main.screenPosition;
                position.X += 8f * Player.direction;
                Vector2 origin = new Vector2(2, 42);

                drawInfo.DrawDataCache.Add(
                    new DrawData(tex, position, tex.Frame(), Lighting.GetColor((int)Player.position.X/16, (int)Player.position.Y/16), rot + MathHelper.ToRadians(45), origin, projectile.scale * 1.2f, 0, 0));
            }
        }
    }

    public class DrawFrontMiscellaneous : PlayerDrawLayer
    {
        private static Asset<Texture2D> AlriunePetal;
        private static Asset<Texture2D> MusicalAddiction;
        private static Asset<Texture2D> TodaysLook;
        private static Asset<Texture2D> BlackSwan;

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            LobotomyModPlayer ModPlayer = LobotomyModPlayer.ModPlayer(drawInfo.drawPlayer);
            return !drawInfo.drawPlayer.dead && (
                ModPlayer.FaintAromaPetal > 0 || 
                ModPlayer.HarmonyAddiction || 
                ModPlayer.TodaysExpressionActive ||
                ModPlayer.BlackSwanNettleClothing > 0);
        }

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.FrontAccFront);

        public override void Load()
        {
            AlriunePetal = Mod.Assets.Request<Texture2D>("Misc/AlriunePetal");
            MusicalAddiction = Mod.Assets.Request<Texture2D>("Misc/MusicalAddiction");
            TodaysLook = Mod.Assets.Request<Texture2D>("Misc/TodaysExpression");
            BlackSwan = Mod.Assets.Request<Texture2D>("Misc/Nettle");
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            /*
            if (AlriunePetal == null)
            {
                AlriunePetal = Mod.Assets.Request<Texture2D>("Misc/AlriunePetal");
            }
            if ()*/

            Player Player = drawInfo.drawPlayer;
            LobotomyModPlayer ModPlayer = LobotomyModPlayer.ModPlayer(Player);
            if (ModPlayer.FaintAromaPetal > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (ModPlayer.FaintAromaPetal <= ModPlayer.FaintAromaPetalMax * i)
                        continue;

                    Texture2D texture = AlriunePetal.Value;
                    int drawX = (int)(drawInfo.Position.X + Player.width / 2f - Main.screenPosition.X);
                    int drawY = (int)(drawInfo.Position.Y + Player.height / 2f - Main.screenPosition.Y);

                    float rot = 0f;
                    if (i == 1)
                        rot += 90;
                    else if (i == 2)
                        rot += 45;
                    Vector2 Offset = new Vector2(-24 * Player.direction, 0).RotatedBy(MathHelper.ToRadians(rot * Player.direction));
                    Offset.Y -= 14;

                    rot = MathHelper.ToRadians(rot * Player.direction + (Player.direction == 1 ? 0 : 180)) - MathHelper.ToRadians(135);

                    float alpha = Terraria.Utils.GetLerpValue(0, 1f, ((ModPlayer.FaintAromaPetal - ModPlayer.FaintAromaPetalMax * i) / ModPlayer.FaintAromaPetalMax));
                    Color color = Lighting.GetColor((int)(Player.position.X / 16), (int)(Player.position.Y / 16)) * alpha;

                    DrawData data = new DrawData(texture, new Vector2(drawX, drawY) + Offset, null, color, rot, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, 0, 0);
                    drawInfo.DrawDataCache.Add(data);
                }
            }

            if (ModPlayer.HarmonyAddiction)
            {
                Texture2D texture = MusicalAddiction.Value;
                int drawX = (int)(drawInfo.Position.X + Player.width / 2f - Main.screenPosition.X) + Main.rand.Next(2);
                int drawY = (int)(drawInfo.Position.Y + Player.height / 2f - Main.screenPosition.Y) - 32;

                Color color = Color.White;

                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, color, 0, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, 0, 0);
                drawInfo.DrawDataCache.Add(data);
            }

            if (ModPlayer.TodaysExpressionActive)
            {
                Texture2D texture = TodaysLook.Value;
                int drawX = (int)(drawInfo.Position.X + Player.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.Position.Y + Player.height / 2f - Main.screenPosition.Y) - 56;

                Color color = Color.White;

                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), texture.Frame(5, 1, ModPlayer.TodaysExpressionFace), color, 0, new Vector2(texture.Width / 10f, texture.Height / 2f), 0.75f, 0, 0);
                drawInfo.DrawDataCache.Add(data);
            }
        
            if (ModPlayer.BlackSwanNettleClothing > 0)
            {
                float clothing = ModPlayer.BlackSwanNettleClothing;
                int currentActive = (int)Math.Ceiling(clothing);
                Texture2D texture = BlackSwan.Value;
                int drawX = (int)(drawInfo.Position.X + Player.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.Position.Y + Player.height / 2f - Main.screenPosition.Y);
                for (int i = 0; i < currentActive; i++)
                {
                    Vector2 pos = new Vector2(drawX, drawY) + new Vector2(30 + 3 * (float)Math.Sin(MathHelper.ToRadians(5 * (float)Main.timeForVisualEffects)), 0).RotatedBy(MathHelper.ToRadians(120 + 60 * i));

                    float rotation = (float)Math.Atan2(pos.Y - drawY, pos.X - drawX) + 2.35619f;

                    Color color = Color.White;
                    if (clothing - i < 1f)
                    {
                        float opacity = clothing - i / 1f;
                        color *= 0.5f * opacity;
                    }

                    DrawData data = new DrawData(texture, pos, null, color, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, 0, 0);
                    drawInfo.DrawDataCache.Add(data);
                }
            }
        }
    }

    public class LobShields : PlayerDrawLayer
    {
        public static Asset<Texture2D> RedShield;
        public static Asset<Texture2D> WhiteShield;
        public static Asset<Texture2D> BlackShield;
        public static Asset<Texture2D> PaleShield;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                WhiteShield = Mod.Assets.Request<Texture2D>("Misc/BulletShield/WhiteShield");
                RedShield = Mod.Assets.Request<Texture2D>("Misc/BulletShield/RedShield");
                BlackShield = Mod.Assets.Request<Texture2D>("Misc/BulletShield/BlackShield");
                PaleShield = Mod.Assets.Request<Texture2D>("Misc/BulletShield/PaleShield");

                Main.QueueMainThreadAction(() =>
                {
                    LobotomyCorp.PremultiplyTexture(RedShield.Value);
                    LobotomyCorp.PremultiplyTexture(WhiteShield.Value);
                    LobotomyCorp.PremultiplyTexture(BlackShield.Value);
                    LobotomyCorp.PremultiplyTexture(PaleShield.Value);
                });
            }
        }

        public override void Unload()
        {
            WhiteShield = null;
            RedShield = null;
            BlackShield = null;
            PaleShield = null;
        }

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            LobotomyModPlayer ModPlayer = LobotomyModPlayer.ModPlayer(drawInfo.drawPlayer);
            return ModPlayer.ShieldActive;
        }

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.FrontAccFront);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player Player = drawInfo.drawPlayer;
            LobotomyModPlayer ModPlayer = LobotomyModPlayer.ModPlayer(Player);

            Texture2D shieldTex = RedShield.Value;
            if (ModPlayer.WhiteShield)
                shieldTex = WhiteShield.Value;
            if (ModPlayer.BlackShield)
                shieldTex = BlackShield.Value;
            if (ModPlayer.PaleShield)
                shieldTex = PaleShield.Value;

            //Current Shield state
            bool broken = ModPlayer.ShieldAnim <= 60;

            //Static positions etc
            Rectangle frame = new Rectangle(0, shieldTex.Height / 2 * broken.ToInt(), shieldTex.Width, shieldTex.Height / 2);
            Vector2 origin = frame.Size() / 2;
            Vector2 drawPos = new Vector2((drawInfo.Position.X + Player.width / 2f - Main.screenPosition.X), (int)(drawInfo.Position.Y + Player.height / 2f - Main.screenPosition.Y));

            //Shield%
            float shieldHealth = ((float)ModPlayer.ShieldHP / (float)ModPlayer.ShieldHPMax);
            //Color - Become less visible the lower the health
            float colorOpacity = 0.6f + 0.2f * shieldHealth;
            if (broken)
                colorOpacity = 0.4f + 0.2f * shieldHealth;
            Color color = Color.White * colorOpacity;
            color = Player.GetImmuneAlpha(color, drawInfo.shadow);
            color.A = (byte)(color.A * 0.7f);

            //Scale - slowly beating, shrinks a bit when damaged
            float progress = ((float)ModPlayer.ShieldAnim - (broken ? 0 : 60)) / 60f;
            float scale = 0.8f + 0.2f * shieldHealth + 0.05f * (float)Math.Sin(2f * (float)Math.PI * progress);

            DrawData data = new DrawData(
                shieldTex,
                drawPos,
                frame,
                color,
                0f,
                origin,
                scale,
                0,
                0);
            drawInfo.DrawDataCache.Add(data);
        }
    }
}