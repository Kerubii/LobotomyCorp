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
using LobotomyCorp.Items.Zayin;
using System.Threading;
using LobotomyCorp.Items.Teth;

namespace LobotomyCorp.Items
{
    class PEBox : ModItem
    {
        public static Dictionary<int, int[]> EgoSets;

        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<Configs.LobotomyServerConfig>().TestItemEnable;
        }

        public override void Load()
        {
            EgoSets = new Dictionary<int, int[]>();
        }

        public override void Unload()
        {
            EgoSets = null;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Lens);
        }

        public override bool CanRightClick()
        {
            return (EgoSets.ContainsKey(Main.mouseItem.type));
        }

        public override void RightClick(Player player)
        {
            int type = Main.mouseItem.type;
            CreateEgoArmor(type);
            base.RightClick(player);
        }

        private bool CreateEgoArmor(int type)
        {
            if (EgoSets.ContainsKey(type))
            {
                int[] itemSets = EgoSets[type];
                for (int i = 0; i < itemSets.Length; i++)
                {
                    Main.LocalPlayer.QuickSpawnItem(null, itemSets[i]);
                }
                return true;
            }
            return false;
        }
    }
}