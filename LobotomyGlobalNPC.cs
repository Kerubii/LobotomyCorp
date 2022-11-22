using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp
{
	public class LobotomyGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public int BeakTarget = 0;
		public bool BODExecute = false;

        public bool HarmonyMusicalAddiction = false;

        public int LaetitiaGiftDamage = 0;
        public int LaetitiaGiftOwner = -1;

        public bool MatchstickBurn = false;
        public int MatchstickBurnTime = 0;

        public bool PleasureDebuff = false;

        public bool QueenBeeSpore = false;
        public bool QueenBeeLarva = true;

        public bool RedEyesCocoon = false;
        private int RedEyesMeal = 0;
        private int RedEyesMealDecay = 0;
        public int RedEyesCocoonPlayer = -1;
        public int RedEyesCocoonCooldown = 0;

        public bool SanguineDesireGlitter = false;
        public int SanguineDesireGlitterTarget = -1;
        public bool SanguineDesireExtensiveBleeding = false;
        public float SanguineDesireExtensiveBleedAmount = 0;

        public bool WingbeatFairyMeal = false;
        public int WingbeatFairyHeal = 0;
        public int WingbeatRotation = Main.rand.Next(360);
        public int WingbeatTarget = -1;
        public int WingbeatIndicator = 0;
        public int RiskLevel = 0;

        public bool WristCutterScars = false;

        public static LobotomyGlobalNPC LNPC (NPC npc)
        {
            return npc.GetGlobalNPC<LobotomyGlobalNPC>();
        }

        public override void ResetEffects(NPC npc)
        {
            if (BeakTarget > 0)
                BeakTarget--;

            HarmonyMusicalAddiction = false;

            MatchstickBurn = false;

            PleasureDebuff = false;

            QueenBeeSpore = false;

            if (!RedEyesCocoon)
                RedEyesCocoonPlayer = -1;
            RedEyesCocoon = false;
            if (RedEyesMealDecay <= 0)
            {
                if (RedEyesMeal > 0)
                    RedEyesMeal--;
            }
            else
                RedEyesMealDecay--;

            if (!SanguineDesireGlitter)
                SanguineDesireGlitterTarget = -1;
            SanguineDesireGlitter = false;
            SanguineDesireExtensiveBleeding = false;

            if (RedEyesCocoonCooldown > 0)
                RedEyesCocoonCooldown--;

            if (!WingbeatFairyMeal)
            {
                WingbeatFairyHeal = 0;
                WingbeatRotation = Main.rand.Next(360);
            }
            if (WingbeatTarget > -1 && LobotomyModPlayer.ModPlayer(Main.player[WingbeatTarget]).RealizedWingbeatMeal != npc.whoAmI)
                WingbeatTarget = -1;
            WingbeatFairyMeal = false;

            WristCutterScars = false;
        }

		public override bool PreAI(NPC npc)
		{
			return !BODExecute;
		}

        public override void AI(NPC npc)
        {
            if (QueenBeeSpore)
            {
                Dust.NewDust(npc.Center, 1, 1, ModContent.DustType<Misc.Dusts.HornetDust>());
            }

            if (WingbeatFairyMeal)
            {
                WingbeatRotation += 3;
                WingbeatFairyHeal -= 3;
                if (WingbeatRotation > 360)
                    WingbeatRotation -= 360;
                if (Main.rand.Next(30) == 0)
                {
                    Dust dust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 89)];
                    dust.velocity *= 0;
                    dust.noGravity = true;
                    dust.fadeIn = 1f;
                }
                if (WingbeatFairyHeal <= 0)
                {
                    WingbeatFairyHeal = 120;
                    npc.life += 10;
                    if (npc.life > npc.lifeMax)
                        npc.life = npc.lifeMax;
                    npc.HealEffect(10, true);
                    for (int i = 0; i < 10; i++)
                    {
                        Dust dust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 89)];
                        dust.velocity *= 0;
                        dust.noGravity = true;
                        dust.fadeIn = 1f;
                    }
                }
            }
            if (WingbeatFairyMeal && WingbeatNear(npc))
            {
                if (WingbeatIndicator < 5)
                    WingbeatIndicator++;
            }
            else if (WingbeatIndicator > 0)
                WingbeatIndicator--;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (HarmonyMusicalAddiction)
            {
                npc.lifeRegen = -12;
                damage += 5;
            }

            if (MatchstickBurn)
            {
                npc.lifeRegen -= MatchstickBurnTime * 2;
                damage += MatchstickBurnTime;
            }
            if (PleasureDebuff)
            {
                npc.lifeRegen -= 10;
                damage += 5;
            }
            if (QueenBeeSpore)
            {
                npc.lifeRegen -= 30;
                damage += 10;
            }

            if (WristCutterScars)
            {
                npc.lifeRegen -= 10;
                damage += 2;
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
        {
            if (HarmonyMusicalAddiction)
            {
                damage = (int)(damage * 1.1f);
            }

            base.ModifyHitPlayer(npc, target, ref damage, ref crit);
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            if (SanguineDesireExtensiveBleeding && !npc.immortal)
            {
                int bleedDamage = (int)SanguineDesireExtensiveBleedAmount;
                
                CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), CombatText.DamagedHostile, (int)bleedDamage, false);
                if (npc.realLife >= 0)
                {
                    Main.npc[npc.realLife].life -= (int)bleedDamage;
                    npc.life = Main.npc[npc.realLife].life;
                    npc.lifeMax = Main.npc[npc.realLife].lifeMax;
                }
                else
                {
                    npc.life -= (int)bleedDamage;
                }
                npc.HitEffect(0, bleedDamage);

                for (int i = 0; i < bleedDamage / 2; i++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood);
                }

                SanguineDesireExtensiveBleedAmount /= 3;
                if (SanguineDesireExtensiveBleedAmount < 1f)
                {
                    int type = ModContent.BuffType<Buffs.ExtensiveBleeding>();
                    if (npc.HasBuff<Buffs.ExtensiveBleeding>())
                    {
                        int i = npc.FindBuffIndex(type);
                        npc.DelBuff(i);
                    }
                }
            }
            base.OnHitPlayer(npc, target, damage, crit);
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (LaetitiaGiftOwner >= 0)
            {
                LaetitiaGiftDamage += damage;
                if (LaetitiaGiftDamage > 1700f)
                {
                    Main.player[LaetitiaGiftOwner].ApplyDamageToNPC(npc, LaetitiaGiftDamage, 0f, 1, false);
                    Projectile.NewProjectile(Main.player[LaetitiaGiftOwner].GetSource_FromThis(), npc.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Realized.LaetitiaFriend>(), 60, 3.5f, LaetitiaGiftOwner);

                    for (int i = 0; i < 16; i++)
                    {
                        Vector2 velocity = (Vector2.UnitX * Main.rand.Next(5)).RotateRandom(6.28f);

                        Dust.NewDustPerfect(npc.Center, 69, velocity).noGravity = true;
                    }

                    LaetitiaGiftDamage = 0;
                    LaetitiaGiftOwner = -1;
                }
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (BeakTarget > 0)
                drawColor = Color.Red;

            if (MatchstickBurn)
            {
                if (MatchstickBurnTime < 20)
                {
                    if (Main.rand.Next(25 - MatchstickBurnTime) == 0)
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.Torch);
                }
                else
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        float scale = 0.5f + MatchstickBurnTime / 40f;
                        if (scale > 4f)
                            scale = 4f;
                        Dust d = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, DustID.Torch)];
                        d.scale = scale;
                        d.noGravity = true;
                    }
                }
            }

            if (npc.HasBuff(ModContent.BuffType<Buffs.CrookedNotes>()) && Main.rand.NextBool(3))
            {
                Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Misc.Dusts.NoteDust>())].velocity.Y -= 1f;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                if (RedEyesMealAmount > 0 && Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Items.Ruina.Literature.RedEyesR>())
                {
                    int RedEyesMax = LobotomyModPlayer.ModPlayer(Main.LocalPlayer).RedEyesMealMax;
                    if (RedEyesMealAmount > RedEyesMax)
                    {
                        drawColor = Color.Lerp(Color.Red, Color.Pink, (float)Math.Sin(6.28f * ((float)Main.timeForVisualEffects % 30) / 30f));
                    }
                    else
                    {
                        float amount = (float)RedEyesMealAmount / RedEyesMax;
                        if (amount > 1)
                            amount = 1;
                        Color red = Color.Red * 0.8f;
                        red.A = drawColor.A;
                        drawColor = Color.Lerp(drawColor, red, amount);
                    }
                }
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (WingbeatFairyMeal)
            {
                Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/WingbeatFairy").Value;
                Vector2 position = npc.Center - Main.screenPosition + new Vector2(npc.width, 0).RotatedBy(MathHelper.ToRadians(WingbeatRotation));

                Rectangle? frame = new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, texture.Height / 6 * (int)(WingbeatRotation / 60f), texture.Width, texture.Height / 6));

                spriteBatch.Draw(texture, position, frame, drawColor, 0, new Vector2(15, 12), 1f, SpriteEffects.None, 0f);
            }

            if (Main.netMode != NetmodeID.Server)
            {
                if (WingbeatIndicator > 0)
                {
                    Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/WingbeatTarget").Value;
                    Vector2 position = npc.Center - Main.screenPosition;
                    Rectangle? frame = new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height));
                    float rotation = (Main.LocalPlayer.Center - npc.Center).ToRotation();
                    Vector2 scale = new Vector2(((npc.width < npc.height ? npc.height * 2 : npc.width * 2) / 60f), 0.5f);
                    Color color = Color.White * (WingbeatIndicator / 10f);
                    color.A = (byte)(color.A * 0.8f);
                    spriteBatch.Draw(texture, position, frame, color, rotation, new Vector2(20, 15), scale, SpriteEffects.None, 0f);
                }

                if (LobotomyModPlayer.ModPlayer(Main.LocalPlayer).ForgottenAffectionResistance >= 0.03f && LobotomyModPlayer.ModPlayer(Main.LocalPlayer).ForgottenAffection == npc.whoAmI)
                {
                    Texture2D texture = Mod.Assets.Request<Texture2D>("Misc/HappyMemories").Value;
                    Vector2 position = npc.position + new Vector2(npc.width / 2, npc.gfxOffY) - Main.screenPosition;
                    position.Y -= texture.Height - 4;
                    Vector2 origin = new Vector2(23, 24);

                    int time = (int)Main.timeForVisualEffects % 180;
                    int dir = 0;
                    if (time < 60)
                        dir = 1;
                    if (time >= 120)
                        dir = -1;
                    float rotation = 0.174f * dir;

                    float opacity = LobotomyModPlayer.ModPlayer(Main.LocalPlayer).ForgottenAffectionResistance / 0.4f;

                    spriteBatch.Draw(texture, position, null, Color.White * opacity, rotation + 0.03f, origin, 1f, 0, 0);

                    if (LobotomyModPlayer.ModPlayer(Main.LocalPlayer).ForgottenAffectionResistance >= 0.4f)
                    {
                        texture = Mod.Assets.Request<Texture2D>("Misc/HappyMemoriesW").Value;
                        opacity = (float)Math.Sin(((int)Main.timeForVisualEffects % 60) / 60f * 3.14) * 0.5f;

                        spriteBatch.Draw(texture, position, null, Color.White * opacity, rotation + 0.03f, origin, 1f, 0, 0);
                    }
                }
                
                if (LobotomyModPlayer.ModPlayer(Main.LocalPlayer).MagicBulletRequest == npc.whoAmI)
                {
                    Texture2D texture = Mod.Assets.Request<Texture2D>("Misc/MagicBulletCircle").Value;
                    Vector2 position = npc.Center + new Vector2(0, npc.gfxOffY) - Main.screenPosition;
                    float scale = npc.width > npc.height ? npc.width / 122f : npc.height / 122f;
                    if (scale > 0.5f)
                        scale = 0.5f;

                    float rotation = MathHelper.ToRadians((5 + 20f * scale) * (float)Main.timeForVisualEffects);

                    spriteBatch.Draw(texture, position, null, Color.White * 0.7f, rotation, texture.Size() / 2, scale, 0, 0);
                }
            }
        
            if (LaetitiaGiftOwner >= 0)
            {
                float progress = Math.Clamp(LaetitiaGiftDamage / 1600f, 0f, 1f);

                Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Realized/LaetitiaR").Value;
                Vector2 position = npc.Center - Main.screenPosition + new Vector2(0, -npc.height / 2 - 30);
                float scale = 1f + 0.2f * (float)Math.Sin(6.28f * ((Main.timeForVisualEffects * (8 * progress)) / 60));
                if (scale < 1f)
                    scale = 1f;
                Rectangle frame = texture.Frame();

                spriteBatch.Draw(texture, position, frame, drawColor, 0f, frame.Size()/2, scale, SpriteEffects.None, 0f);
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (HornetTarget(npc))
            {
                SpawnHornet(npc);
            }
            return true;
        }

        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
            if (npc.life <= 0 && LobotomyModPlayer.ModPlayer(Main.LocalPlayer).MagicBulletRequest == npc.whoAmI)
            {
                LobotomyModPlayer.ModPlayer(Main.LocalPlayer).MagicBulletRequest = -1;
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.QueenBee)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Hornet>(), 2));
                }
                if (npc.type == NPCID.WallofFlesh)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType < Items.Censored>(), 4));
                }
                if (npc.type == NPCID.EyeofCthulhu)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType < Items.Syrinx>(), 5));
                }
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Merchant)
            {
                if (Main.bloodMoon)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType < Items.WristCutter>());
                    nextSlot++;
                }
            }
            if (type == NPCID.Dryad)
            {
                if (NPC.downedQueenBee || NPC.downedBoss3)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType <Items.Hypocrisy>());
                    shop.item[nextSlot].shopCustomPrice = 100000;
                    nextSlot++;
                }
            }
        }

        public bool WingbeatNear(NPC npc)
        {
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Items.Ruina.History.WingbeatR>())
                return Vector2.Distance(npc.Center, Main.LocalPlayer.Center) - (npc.width < npc.height ? npc.height/2 : npc.width/2) < 200;
            return false;
        }

        public static bool HornetTarget(NPC npc)
        {
            return LNPC(npc).QueenBeeSpore;
        }

        public static void SpawnHornet(NPC npc, int player = -1, int damage = 0, float knockBack = 0)
        {
            Mod Mod = ModLoader.GetMod("LobotomyCorp");
            if (LNPC(npc).QueenBeeLarva)
            {
                if (player <= -1)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player p = Main.player[i];
                        if (p.active && !p.dead && p.HeldItem.type == ModContent.ItemType<Items.Ruina.History.HornetR>())
                        {
                            Item item = p.HeldItem;
                            player = i;
                            damage = item.damage / 2;
                            knockBack = item.knockBack;
                            break;
                        }
                    }

                    if (player <= -1)
                        return;
                }

                int proj = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.WorkerBee>(), damage, knockBack, player);
                Main.projectile[proj].originalDamage = damage;
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Misc.Dusts.HornetDust>());
                }
                LNPC(npc).QueenBeeLarva = false;
            }
        }
    
        public void RedEyesApplyMeal(int amount, int decayTimer = 600)
        {
            if (RedEyesCocoon)
            {
                RedEyesMeal = 0;
                RedEyesMealDecay = 0;
                return;
            }
            RedEyesMeal += amount;
            RedEyesMealDecay = decayTimer;
        }
        public int RedEyesMealAmount { get { return RedEyesMeal; } }

        public static void SanguineDesireApplyBleed(NPC npc, float amount, int amountMax, int time, int timeMax)
        {
            LNPC(npc).SanguineDesireExtensiveBleedAmount += amount;
            if (LNPC(npc).SanguineDesireExtensiveBleedAmount > amountMax)
                LNPC(npc).SanguineDesireExtensiveBleedAmount = amountMax;
            if (npc.HasBuff<Buffs.ExtensiveBleeding>())
            {
                int i = npc.FindBuffIndex(ModContent.BuffType<Buffs.ExtensiveBleeding>());
                int buffTime = npc.buffTime[i] += time;
                if (buffTime > timeMax)
                    buffTime = timeMax;
                npc.AddBuff(ModContent.BuffType<Buffs.ExtensiveBleeding>(), buffTime);
            }
            else
                npc.AddBuff(ModContent.BuffType<Buffs.ExtensiveBleeding>(), time);
        }
    }
}