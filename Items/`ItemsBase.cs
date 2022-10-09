using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

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
                                    "Test - These are test messages, if these showed up then I fucked up";

        /// <summary>
		/// Use the thing you setup on the Mod cs pls. 
		/// </summary>
        public Color EgoColor = LobotomyCorp.ZayinRarity;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(GetTooltip());
        }

        public sealed override bool CanUseItem(Player player)
        {
            LobotomyModPlayer.ModPlayer(player).SynchronizedEGO = Item.type;
            bool CanUse = SafeCanUseItem(player);
            return !LobotomyModPlayer.ModPlayer(player).Desync && CanUse;
        }

        public sealed override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            bool ExtraShow = LobotomyCorp.ExtraPassiveShow;
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
            return Name.Remove(Name.Length - 1);
        }

        public string GetTooltip()
        {
            return Language.GetTextValue("Mods.LobotomyMod.EgoItemTooltip.RealizedEgo") + "\n\"" + Language.GetTextValue("Mods.LobotomyMod.EgoItemTooltip." + ItemName() + ".ItemTooltip") + "\"";
        }

        public virtual string GetPassiveList()
        {
            string key = "Mods.LobotomyMod.EgoItemTooltip." + ItemName() + ".PassiveList";
            string list = Language.GetTextValue(key);
            if (list == key)
                list = PassiveText;
            return list;
        }
    }

    //Old Heavy
    public abstract class LobCorpHeavyOld : ModItem
    {
        public sealed override bool CanUseItem(Player player)
        {
            LobotomyModPlayer.ModPlayer(player).HeavyWeaponHelper = (int)(player.GetAttackSpeed(DamageClass.Melee) * Item.useAnimation * 3);// TotalMeleeTime((float)Item.useAnimation * player.meleeSpeed, player, Item) * 3;
            Item.noUseGraphic = true;

            return SafeCanUseItem(player);
        }

        public virtual bool SafeCanUseItem(Player player)
        {
            return true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.altFunctionUse != 2)
            {
                float anim = player.itemAnimationMax;
                int helper = LobotomyModPlayer.ModPlayer(player).HeavyWeaponHelper;
                if (helper <= 2)
                {
                    player.itemAnimation = helper;
                    return;
                }
                if (helper > anim)
                {
                    player.itemAnimation = (int)(player.itemAnimationMax * ((0.5f * Math.Cos((float)Math.PI * (((float)helper - anim) / (anim * 2))) + 0.5f) / 1f));
                }
                else
                {
                    player.itemAnimation = (int)(player.itemAnimationMax * ((float)helper / anim));
                }
                if (player.itemAnimation <= 2)
                    player.itemAnimation = 3;
                if (helper < player.itemAnimationMax * 3)
                    Item.noUseGraphic = false;
            }
        }
        
        public override bool? CanHitNPC(Player player, NPC target)
        {
            if (LobotomyModPlayer.ModPlayer(player).HeavyWeaponHelper <= player.itemAnimationMax)
            {
                return base.CanHitNPC(player, target);
            }
            else
                return false;
        }

        public override bool CanHitPvp(Player player, Player target)
        {
            if (LobotomyModPlayer.ModPlayer(player).HeavyWeaponHelper <= player.itemAnimationMax)
            {
                return base.CanHitPvp(player, target);
            }
            else
                return false;
        }
    }
}