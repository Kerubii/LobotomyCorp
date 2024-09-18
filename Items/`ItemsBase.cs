using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Audio;
using LobotomyCorp.Configs;
using log4net.Util;
using Steamworks;

namespace LobotomyCorp.Items
{
    public abstract class SEgoItem : ModItem
	{
        //public override bool CloneNewInstances => true;

        /// <summary>
		/// Remember to seperate the Negative part with '|'. 
		/// </summary>
        public string PassiveText = "Nihil - All starts from nothing" + 
                                    "|Void - ...And all returns to nothing\n" +
                                    "Test - These are test messages, if these showed up then I fucked up\n" +
                                    "This Item is incomplete and unobtainable";

        /// <summary>
		/// Use the thing you setup on the Mod cs pls. 
		/// </summary>
        public Color EgoColor = LobotomyCorp.ZayinRarity;

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault(GetTooltip());
        }

        public sealed override bool CanUseItem(Player player)
        {
            LobotomyModPlayer.ModPlayer(player).SynchronizedEGO = Item.type;
            bool CanUse = SafeCanUseItem(player);
            return !LobotomyModPlayer.ModPlayer(player).Desync && CanUse;
        }

        public sealed override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var RealizedEGOTooltip = new TooltipLine(Mod, "PositivePassive", $"{Language.GetTextValue("Mods.LobotomyCorp.EgoItemTooltip.RealizedEgo")}"){ OverrideColor = Color.Lerp(Color.Yellow, Color.Cyan, 0.5f + 0.5f * (float)Math.Sin(6.28f * (Main.timeForVisualEffects % 120 / 120f))) };
            int index = tooltips.FindIndex(x => x.Mod == "Terraria" && x.Name == "Tooltip0");
            tooltips.Insert(index, RealizedEGOTooltip);

            bool ExtraShow = ModContent.GetInstance<LobotomyConfig>().ExtraPassivesShow;
            //int tooltipIndex = tooltips.IndexOf()
            var Passive = new TooltipLine(Mod, "PositivePassive", $"{PassiveInitialize(ExtraShow)}"
                ) { OverrideColor =  LobotomyCorp.PositivePE};
            tooltips.Add(Passive);

            Passive = new TooltipLine(Mod, "NegativePassive", $"{PassiveInitialize(ExtraShow, true)}") { OverrideColor = LobotomyCorp.NegativePE };
            tooltips.Add(Passive);

            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "ItemName")
                {
                    line.OverrideColor = EgoColor;
                }
            }
        }

        private string PassiveInitialize(bool Extra, bool Negative = false)
        {
            string[] Passive = GetPassiveList().Split('|');// PassiveText.Split('|');
            string result = "Voided";
            if (!Negative)
            {
                Passive = Passive[0].Split(new[] { "\n" }, StringSplitOptions.None);
            }
            else
            {
                Passive = Passive[1].Split(new[] { "\n" }, StringSplitOptions.None);
            }

            if (!Extra && Passive.Length > 0)
            {
                for (int i = 0; i < Passive.Length; i++)
                {
                    int dashLocation = Passive[i].IndexOf(" - ");
                    if (dashLocation <= 0)
                        continue;
                    Passive[i] = Passive[i].Substring(0, dashLocation);
                }
            }
            result = string.Join("\n", Passive);
            if (result.EndsWith("\n"))
                result = result.Substring(0, result.Length - 1);
            
            /*
            if (Negative)
            {
                result += "\nThese Items are incomplete";
            }*/
            return result;
        }

        public virtual bool SafeCanUseItem(Player player)
        {
            return true;
        }

        public string ItemName()
        {
            return Name;//.Remove(Name.Length - 1);
        }

        public string GetTooltip()
        {
            return Language.GetTextValue("Mods.LobotomyCorp.EgoItemTooltip.RealizedEgo") + "\n\"" + Language.GetTextValue("Mods.LobotomyCorp.EgoItemTooltip." + ItemName() + ".ItemTooltip") + "\"";
        }

        public virtual string GetPassiveList()
        {
            string key = "Mods.LobotomyCorp.Items." + ItemName() + ".PassiveList";
            string list = Language.GetTextValue(key);
            if (list == key)
                list = PassiveText;
            return list;
        }

        public static Condition RedMistCond = new Condition("Mods.LobotomyCorp.LobotomyRedMistRequirement", () => ModSystems.LobEventFlags.downedRedMist);
    }

    public abstract class LobItemBase : ModItem
    {
        public RiskLevel EGORiskLevel;

        public bool RedMistMaskUpgrade(Player player)
        {
            if (LobotomyModPlayer.ModPlayer(player).RedMistMask)
            {
                switch (EGORiskLevel)
                {
                    case RiskLevel.Zayin:
                    case RiskLevel.Teth:
                        return true;
                    case RiskLevel.He:
                        if (NPC.downedMechBossAny) return true;
                        break;
                    case RiskLevel.Waw:
                        if (NPC.downedPlantBoss) return true;
                        break;
                    case RiskLevel.Aleph:
                        if (NPC.downedGolemBoss) return true;
                        break;
                    default:
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// Return Base to keep Red Mist Mask Upgrade tooltip :) 
        /// </summary>
        /// <param name="tooltips"></param>
        public virtual void LobModifyTooltips(List<TooltipLine> tooltips)
        {
            if (RedMistMaskUpgrade(Main.LocalPlayer))
            {
                string text = Language.GetTextValue("Mods.LobotomyCorp.Items." + Name + ".Tooltip2");
                TooltipLine tooltip2 = new TooltipLine(Mod, "RedMistMask", text);
                tooltip2.OverrideColor = Color.Maroon;
                tooltips.Add(tooltip2);
            }
        }

        public sealed override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            LobModifyTooltips(tooltips);
        }
    }

    /// <summary>
    /// Use useStyle = 15 to use the light animation :)
    /// </summary>
    public abstract class LobCorpLight : LobItemBase
    {
        public sealed override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (Item.useStyle == 15)
            {
                float rotation = ItemRotation(player);
                PseudoUseStyleSwing(player, heldItemFrame, rotation);
            }
            UseStyleAlt(player, heldItemFrame);
            base.UseStyle(player, heldItemFrame);
        }

        /// <summary>
        /// Just to use as a base for other weapons for a smoother time, Use Degrees for rotation just to maintain my SP
        /// </summary>
        /// <param name="player"></param>
        /// <param name="heldItemFrame"></param>
        /// <param name="rotation"></param>
        public static void PseudoUseStyleSwing(Player player, Rectangle heldItemFrame, float rotation)
        {
            player.itemLocation = LobItemLocation(player, heldItemFrame, rotation - 90);

            player.itemRotation = MathHelper.ToRadians(rotation - 45) * player.direction;
        }
        
        /// <summary>
        /// No need for reversed rotation, if using reversed rotation, change Direction
        /// </summary>
        /// <param name="player"></param>
        /// <param name="heldItemFrame"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Vector2 LobItemLocation(Player player, Rectangle heldItemFrame, float rotation, int direction = 1, int Xoffset = 0)
        {
            rotation = Math.Clamp(rotation, -180, 180);
            rotation = MathHelper.ToRadians(rotation);
            float x = (float)Math.Cos(rotation) * direction;
            float y = (float)Math.Sin(rotation);

            Vector2 location = new Vector2();
            if (y < 0)
            {
                if (x < 0)
                {
                    float num33 = 6f;
                    if (heldItemFrame.Width > 32)
                    {
                        num33 = 14f;
                    }
                    if (heldItemFrame.Width >= 48)
                    {
                        num33 = 18f;
                    }
                    if (heldItemFrame.Width >= 52)
                    {
                        num33 = 24f;
                    }
                    if (heldItemFrame.Width >= 64)
                    {
                        num33 = 28f;
                    }
                    if (heldItemFrame.Width >= 92)
                    {
                        num33 = 38f;
                    }
                    location.X = player.position.X + (float)player.width * 0.5f - ((float)heldItemFrame.Width * 0.5f - (num33 + Xoffset)) * (float)player.direction;
                    num33 = 10f;
                    if (heldItemFrame.Height > 32)
                    {
                        num33 = 10f;
                    }
                    if (heldItemFrame.Height > 52)
                    {
                        num33 = 12f;
                    }
                    if (heldItemFrame.Height > 64)
                    {
                        num33 = 14f;
                    }
                    location.Y = player.position.Y + num33 + player.HeightOffsetHitboxCenter;
                }
                else
                {
                    float num32 = 10f;
                    if (heldItemFrame.Width > 32)
                    {
                        num32 = 18f;
                    }
                    if (heldItemFrame.Width >= 52)
                    {
                        num32 = 24f;
                    }
                    if (heldItemFrame.Width >= 64)
                    {
                        num32 = 28f;
                    }
                    if (heldItemFrame.Width >= 92)
                    {
                        num32 = 38f;
                    }
                    location.X = player.position.X + (float)player.width * 0.5f + ((float)heldItemFrame.Width * 0.5f - (num32 + Xoffset)) * (float)player.direction;
                    num32 = 10f;
                    if (heldItemFrame.Height > 32)
                    {
                        num32 = 8f;
                    }
                    if (heldItemFrame.Height > 52)
                    {
                        num32 = 12f;
                    }
                    if (heldItemFrame.Height > 64)
                    {
                        num32 = 14f;
                    }
                    location.Y = player.position.Y + num32 + player.HeightOffsetHitboxCenter;
                }
            }
            else
            {
                if (x > 0)
                {
                    float num31 = 14f;
                    if (heldItemFrame.Width > 32)
                    {
                        num31 = 18f;
                    }
                    if (heldItemFrame.Width >= 52)
                    {
                        num31 = 28f;
                    }
                    if (heldItemFrame.Width >= 64)
                    {
                        num31 = 32f;
                    }
                    if (heldItemFrame.Width >= 92)
                    {
                        num31 = 42f;
                    }
                    location.X = player.position.X + (float)player.width * 0.5f + ((float)heldItemFrame.Width * 0.5f - (num31 + Xoffset)) * (float)player.direction;
                    location.Y = player.position.Y + 26f + player.HeightOffsetHitboxCenter;
                }
                else
                {
                    float num33 = 6f;
                    if (heldItemFrame.Width > 32)
                    {
                        num33 = 14f;
                    }
                    if (heldItemFrame.Width >= 48)
                    {
                        num33 = 18f;
                    }
                    if (heldItemFrame.Width >= 52)
                    {
                        num33 = 24f;
                    }
                    if (heldItemFrame.Width >= 64)
                    {
                        num33 = 28f;
                    }
                    if (heldItemFrame.Width >= 92)
                    {
                        num33 = 38f;
                    }
                    location.X = player.position.X + (float)player.width * 0.5f - ((float)heldItemFrame.Width * 0.5f - (num33 + Xoffset)) * (float)player.direction;
                    location.Y = player.position.Y + 24f + player.HeightOffsetHitboxCenter;
                }
            }
            return location;
        }

        public virtual void UseStyleAlt(Player player, Rectangle heldItemFrame)
        {
        }

        public sealed override void UseItemFrame(Player player)
        {
            if (Item.useStyle == 15)
            {
                float rotation = ItemRotation(player);
                LobItemFrame(player, rotation - 90);
            }
            base.UseItemFrame(player);
        }

        /// <summary>
        /// [OLDER VERSION]Just to use as a base for other weapons for a smoother time, Use Degrees for rotation just to maintain my SP
        /// </summary>
        /// <param name="player"></param>
        /// <param name="rotation"></param>
        public static void PseudoUseItemFrame(Player player, float rotation)
        {
            if ((player.direction == 1 && rotation < 0) ||
                    (player.direction == -1 && rotation > 0))
            {
                player.bodyFrame.Y = player.bodyFrame.Height * 1;
            }
            else if ((player.direction == 1 && rotation < 90) ||
                     (player.direction == -1 && rotation > -90))
            {
                player.bodyFrame.Y = player.bodyFrame.Height * 2;
            }
            else if ((player.direction == 1 && rotation < 180) ||
                     (player.direction == -1 && rotation > -180))
            {
                player.bodyFrame.Y = player.bodyFrame.Height * 4;
            }
            else
            {
                player.SetCompositeArmFront(enabled: true, Player.CompositeArmStretchAmount.Full, MathHelper.ToRadians(45 - (player.direction < 0 ? 90 : 0)));
            }
        }

        /// <summary>
        /// Base for other weapons to use, Use Degrees for rotation just to maintain my SP, if using reversed rotation, change Direction
        /// </summary>
        /// <param name="player"></param>
        /// <param name="rotation"></param>
        public static void LobItemFrame(Player player, float rotation, int direction = 1)
        {
            rotation = Math.Clamp(rotation, -180, 180);
            rotation = MathHelper.ToRadians(rotation);
            float x = (float)Math.Cos(rotation) * direction;
            float y = (float)Math.Sin(rotation);
            
            if (y < 0)
            {
                if (x < 0)
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 1;
                }
                else
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 2;
                }
            }
            else
            {
                if (x > 0)
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 4;
                }
                else
                {
                    player.SetCompositeArmFront(enabled: true, Player.CompositeArmStretchAmount.Full, MathHelper.ToRadians(45 - (player.direction < 0 ? 90 : 0)));
                }
            }
        }

        public sealed override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (Item.useStyle == 15)
            {
                hitbox = new Rectangle((int)player.itemLocation.X, (int)player.itemLocation.Y, 32, 32);
                if (!Main.dedServ)
                {
                    Rectangle hitboxSize = Item.GetDrawHitbox(Item.type, player);
                    hitbox = new Rectangle((int)player.itemLocation.X, (int)player.itemLocation.Y, hitboxSize.Width, hitboxSize.Height);
                }
                float adjustedItemScale = player.GetAdjustedItemScale(Item);
                hitbox.Width = (int)((float)hitbox.Width * adjustedItemScale);
                hitbox.Height = (int)((float)hitbox.Height * adjustedItemScale);
                if (player.direction == -1)
                {
                    hitbox.X -= hitbox.Width;
                }
                if (player.gravDir == 1f)
                {
                    hitbox.Y -= hitbox.Height;
                }

                float prog = 1f - player.itemAnimation / (float)player.itemAnimationMax;
                if (prog < .2f)
                {
                    if (player.direction == 1)
                    {
                        hitbox.X -= (int)(hitbox.Width * 1);
                    }
                    hitbox.Width *= 2;
                    hitbox.Y -= (int)((hitbox.Height * 1.4 - hitbox.Height) * player.gravDir);
                    hitbox.Height = (int)(hitbox.Height * 1.4);
                }
                else if (prog < .4f)
                {
                    if (player.direction == -1)
                    {
                        hitbox.X -= (int)((double)hitbox.Width * 1.4 - (double)hitbox.Width);
                    }
                    hitbox.Width = (int)((double)hitbox.Width * 1.4);
                    hitbox.Y += (int)((double)hitbox.Height * 0.5 * (double)player.gravDir);
                    hitbox.Height = (int)((double)hitbox.Height * 1.4);
                }
                else
                    noHitbox = true;
            }
            UseItemHitboxAlt(player, ref hitbox, ref noHitbox);
            base.UseItemHitbox(player, ref hitbox, ref noHitbox);
        }

        public virtual void UseItemHitboxAlt(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
        }

        public override bool? UseItem(Player player)
        {
            if (Item.useStyle == 15)
            {
                //This is a jank way of changing item's attack cooldown, thought it would better fit at onhit same as immune but I guess not since that ones before they change the immune time and attackCD
                ResetPlayerAttackCooldown(player);
            }
            return UseItemAlt(player);
        }

        /// <summary>
        /// Use when overriding UseItem since its there :(, shouldn't really be used... unless
        /// Put it on UseStyleAlt
        /// Im really angry they dont let you change this when you actually hit the fucking doods
        /// </summary>
        /// <param name="player"></param>
        public static void ResetPlayerAttackCooldown(Player player, double percent = 0.1)
        {
            int cooldown = Math.Max(1, (int)(player.itemAnimationMax * percent));
            if (player.attackCD > cooldown)
                player.attackCD = cooldown;
        }

        /// <summary>
        /// Changes IFrames, also changes AttackCooldown
        /// Put it on UseStyleAlt or HoldItem maybe...
        /// Jank, might cause problems
        /// Im really angry they dont let you change this when you actually hit the fucking doods
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        /// <param name=""></param>
        public static void ResetPlayerImmuneHit(Player player, ref int target, int immuneLimit, double percent = 0.1)
        {
            NPC npc = Main.npc[target];
            if (player.itemAnimation > immuneLimit)
            {
                player.SetMeleeHitCooldown(target, player.itemAnimation - immuneLimit);// player.itemAnimation - immuneLimit);
                //npc.immune[player.whoAmI] = player.itemAnimation - immuneLimit;
            }

            target = -1;
            ResetPlayerAttackCooldown(player, percent);
        }

        /// <summary>
        /// ResetPlayerMeleeCooldown, but as a number
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        /// <param name="immuneLimit"></param>
        /// <param name="percent"></param>
        public static void SetPlayerMeleeCooldown(Player player, ref int target, int immuneTime, double percent = 0.1)
        {
            NPC npc = Main.npc[target];
            if (player.itemAnimation > immuneTime)
            {
                player.SetMeleeHitCooldown(target, 0);// player.itemAnimation - immuneLimit);
                //npc.immune[player.whoAmI] = immuneTime;
            }

            target = -1;
            ResetPlayerAttackCooldown(player, percent);
        }

        public virtual bool? UseItemAlt(Player player)
        {
            return null;
        }

        /// <summary>
        /// Gives raw rotation in form of degrees, flip when nescessary 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static float ItemRotation(Player player)
        {
            float prog = 1f - player.itemAnimation / (float)player.itemAnimationMax;
            float rotation = 0;
            
            if (prog < 0.4f)
            {
                prog = prog / 0.4f;
                rotation = (-60 + 200 * (float)Math.Sin(1.57f * prog));// * player.direction;
            }
            else if (prog < 0.5f)
            {
                rotation = 140;// * player.direction;
            }
            else
            {
                prog = (prog - 0.5f) / 0.5f;
                rotation = (140 - 45 * prog);// * player.direction;
            }
            return rotation;
        }

        public override void ModifyItemScale(Player player, ref float scale)
        {

            base.ModifyItemScale(player, ref scale);
        }
    }

    /// <summary>
    /// Use useStyle = 15 to use the animation :)
    /// </summary>
    public abstract class LobCorpHeavy : LobItemBase
    {
        public SoundStyle? SwingSound = null;

        public sealed override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (Item.useStyle == 15)
            {
                if (player.itemAnimation == (int)(player.itemAnimationMax * 0.6f) && SwingSound != null)
                    SoundEngine.PlaySound(SwingSound, player.Center);
                float rotation = ItemRotation(player);
                PseudoUseStyleSwing(player, heldItemFrame, rotation);
            }
            UseStyleAlt(player, heldItemFrame);
            base.UseStyle(player, heldItemFrame);
        }

        /// <summary>
        /// Just to use as a base for other weapons for a smoother time, Use Degrees for rotation just to maintain my SP
        /// </summary>
        /// <param name="player"></param>
        /// <param name="heldItemFrame"></param>
        /// <param name="rotation"></param>
        public static void PseudoUseStyleSwing(Player player, Rectangle heldItemFrame, float rotation)
        {
            player.itemLocation = LobCorpLight.LobItemLocation(player, heldItemFrame, rotation - 90);

            player.itemRotation = MathHelper.ToRadians(rotation - 45) * player.direction;
        }

        public virtual void UseStyleAlt(Player player, Rectangle heldItemFrame)
        {
        }

        public sealed override void UseItemFrame(Player player)
        {
            if (Item.useStyle == 15)
            {
                float rotation = ItemRotation(player);
                //PseudoUseItemFrame(player, rotation);
                LobCorpLight.LobItemFrame(player, rotation - 90);
            }
            base.UseItemFrame(player);
        }

        /*
        /// <summary>
        /// [Older Version?]Just to use as a base for other weapons for a smoother time, Use Degrees for rotation just to maintain my SP
        /// </summary>
        /// <param name="player"></param>
        /// <param name="rotation"></param>
        public static void PseudoUseItemFrame(Player player, float rotation)
        {
            if ((player.direction == 1 && rotation < 0) ||
                    (player.direction == -1 && rotation > 0))
            {
                player.bodyFrame.Y = player.bodyFrame.Height * 1;
            }
            else if ((player.direction == 1 && rotation < 90) ||
                     (player.direction == -1 && rotation > -90))
            {
                player.bodyFrame.Y = player.bodyFrame.Height * 2;
            }
            else if ((player.direction == 1 && rotation < 180) ||
                     (player.direction == -1 && rotation > -180))
            {
                player.bodyFrame.Y = player.bodyFrame.Height * 4;
            }
            else
            {
                player.SetCompositeArmFront(enabled: true, Player.CompositeArmStretchAmount.Full, 90);
            }
        }*/

        public sealed override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (Item.useStyle == 15)
            {
                hitbox = new Rectangle((int)player.itemLocation.X, (int)player.itemLocation.Y, 32, 32);
                if (!Main.dedServ)
                {
                    Rectangle hitboxSize = Item.GetDrawHitbox(Item.type, player);
                    hitbox = new Rectangle((int)player.itemLocation.X, (int)player.itemLocation.Y, hitboxSize.Width, hitboxSize.Height);
                }
                float adjustedItemScale = player.GetAdjustedItemScale(Item);
                hitbox.Width = (int)((float)hitbox.Width * adjustedItemScale);
                hitbox.Height = (int)((float)hitbox.Height * adjustedItemScale);
                if (player.direction == -1)
                {
                    hitbox.X -= hitbox.Width;
                }
                if (player.gravDir == 1f)
                {
                    hitbox.Y -= hitbox.Height;
                }

                float prog = 1f - player.itemAnimation / (float)player.itemAnimationMax;
                if (prog > 0.4f && prog < 0.7f)
                {
                    if (prog < .5f)
                    {
                        if (player.direction == 1)
                        {
                            hitbox.X -= (int)(hitbox.Width * 1);
                        }
                        hitbox.Width *= 2;
                        hitbox.Y -= (int)((hitbox.Height * 1.4 - hitbox.Height) * player.gravDir);
                        hitbox.Height = (int)(hitbox.Height * 1.4);
                    }
                    else if (prog < .6f)
                    {
                        if (player.direction == -1)
                        {
                            hitbox.X -= (int)((double)hitbox.Width * 1.4 - (double)hitbox.Width);
                        }
                        hitbox.Width = (int)((double)hitbox.Width * 1.4);
                        hitbox.Y += (int)((double)hitbox.Height * 0.5 * (double)player.gravDir);
                        hitbox.Height = (int)((double)hitbox.Height * 1.4);
                    }
                }
                else
                    noHitbox = true;
            }
            UseItemHitboxAlt(player, ref hitbox, ref noHitbox);
            base.UseItemHitbox(player, ref hitbox, ref noHitbox);
        }

        public virtual void UseItemHitboxAlt(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
        }

        public override bool? UseItem(Player player)
        {
            if (Item.useStyle == 15)
            {
                //This is a jank way of changing item's attack cooldown, thought it would better fit at onhit same as immune but I guess not since that ones before they change the immune time and attackCD
                ResetPlayerAttackCooldown(player);
            }
            return UseItemAlt(player);
        }

        /// <summary>
        /// Use when overriding UseItem since its there :(, shouldn't really be used... unless
        /// Put it on UseStyleAlt
        /// </summary>
        /// <param name="player"></param>
        public void ResetPlayerAttackCooldown(Player player)
        {
            int cooldown = Math.Max(1, (int)((double)player.itemAnimationMax * 0.05f));
            if (player.attackCD > cooldown)
                player.attackCD = cooldown;
        }

        public virtual bool? UseItemAlt(Player player)
        {
            return null;
        }

        public static float ItemRotation(Player player)
        {
            float prog = 1f - player.itemAnimation / (float)player.itemAnimationMax;
            float rotation = 0;

            if (prog < 0.4f)
            {
                prog = prog / 0.4f;
                rotation = (90 - 150 * prog);// * player.direction;
            }
            else if (prog < 0.6f)
            {
                prog = (prog - 0.4f) / 0.2f;
                rotation = (-60 + 200 * (float)Math.Sin(1.57f * prog));// * player.direction;
            }
            else if (prog < 0.7f)
            {
                rotation = 140;// * player.direction;
            }
            else
            {
                prog = (prog - 0.7f) / 0.3f;
                rotation = (140 - 45 * prog);// * player.direction;
            }

            return rotation;
        }

        public override void ModifyItemScale(Player player, ref float scale)
        {

            base.ModifyItemScale(player, ref scale);
        }
    }
}