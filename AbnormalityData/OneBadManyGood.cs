using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Graphics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using LobotomyCorp;
using LobotomyCorp.UI;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.LootSimulation;

namespace LobotomyCorp
{
    public class OneBadManyGood : AbnormalityData
    {
        public override int npcType => 3;

        public override void SetDefault()
        {
            
        }

        public override List<string> GetFlavourText(int progress, bool SpecialWorkType)
        {
            List<string> list = new List<string>();
            float prog = (float)progress / MaximumBoxes;

            if (SpecialWorkType)
            {

            }

            if (prog < 0.2f)
            {
                list.Add(GetLocalizedFilePlayer("FlavourText.Start.1"));
                list.Add(GetLocalizedFileAbno("FlavourText.Start.2"));
                list.Add(GetLocalizedFileAbno("FlavourText.Start.3"));
                list.Add(GetLocalizedFileAbno("FlavourText.Start.4"));
            }
            else if (prog < 0.4)
            {
                list.Add(GetLocalizedFileAbno("FlavourText.Mid1.1"));
                list.Add(GetLocalizedFileAbnoPlayer("FlavourText.Mid1.2"));
                list.Add(GetLocalizedFileAbno("FlavourText.Mid1.3"));
                list.Add(GetLocalizedFileAbno("FlavourText.Mid1.4"));
            }
            else if (prog < 0.6f)
            {
                list.Add(GetLocalizedFileAbnoPlayer("FlavourText.Mid2.1"));
                list.Add(GetLocalizedFileAbnoPlayer("FlavourText.Mid2.2"));
                list.Add(GetLocalizedFileAbnoPlayer("FlavourText.Mid2.3"));
            }
            else if (prog < 0.8f)
            {
                list.Add(GetLocalizedFileAbnoPlayer("FlavourText.Mid3.1"));
                list.Add(GetLocalizedFileAbnoPlayer("FlavourText.Mid3.2"));
                list.Add(GetLocalizedFileAbnoPlayer("FlavourText.Mid3.3"));
            }
            else
            {
                list.Add(GetLocalizedFile("FlavourText.Mid4.1"));
                list.Add(GetLocalizedFile("FlavourText.Mid4.2"));
                list.Add(GetLocalizedFile("FlavourText.Mid4.3"));
            }

            return list;
        }
    }
}