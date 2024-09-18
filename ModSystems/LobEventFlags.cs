using System.Collections.Generic;
using LobotomyCorp.UI;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;

namespace LobotomyCorp.ModSystems
{
    class LobEventFlags : ModSystem
    {
        public static bool downedRedMist = false;
        public static bool binahIntroTalk = false;
        public static bool binahRedmistTalk = false;
        public static bool binahDoneTalk = false;
        public static bool downedAnArbiter = false;
        public static bool killedByRedMist = true;

        public override void ClearWorld()
        {
            downedRedMist = false;
            binahIntroTalk = false;
            binahRedmistTalk = false;
            binahDoneTalk = false;
            downedAnArbiter = false;
            killedByRedMist = true;
        }

        
        public override void SaveWorldData(TagCompound tag)
        {
            if (downedRedMist)
            {
                tag["downedRedMist"] = true;
            }

            if (binahIntroTalk)
            {
                tag["binahIntroTalk"] = true;
            }

            if (binahDoneTalk)
            {
                tag["binahDoneTalk"] = true;
            }

            if (downedAnArbiter)
            {
                tag["downedAnArbiter"] = true;
            }
        }

        
        public override void LoadWorldData(TagCompound tag)
        {
            downedRedMist = tag.ContainsKey("downedRedMist");
            binahIntroTalk = tag.ContainsKey("binahIntroTalk");
            binahDoneTalk = tag.ContainsKey("binahDoneTalk");
            downedAnArbiter = tag.ContainsKey("downedAnArbiter");
        }

        public override void NetSend(BinaryWriter writer)
        {
            // Order of operations is important and has to match that of NetReceive
            var flags = new BitsByte();
            flags[0] = downedRedMist;
            flags[1] = downedAnArbiter;
            // flags[1] = downedOtherBoss;
            writer.Write(flags);

        }

        public override void NetReceive(BinaryReader reader)
        {
            // Order of operations is important and has to match that of NetSend
            BitsByte flags = reader.ReadByte();
            downedRedMist = flags[0];
            downedAnArbiter = flags[1];
        }

        public static void debugEventReset()
        {
            downedRedMist = false;
            binahIntroTalk = false;
            binahRedmistTalk = false;
            binahDoneTalk = false;
            downedAnArbiter = false;
        }
    }
}
