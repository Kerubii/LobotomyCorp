using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Graphics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using LobotomyCorp;

namespace LobotomyCorp
{
    abstract class AbnormalityData
    {
        public string AbnormalityName;
        public int npcType;

        public RiskLevel riskLevel;

        public int BoxResult;

        public int minGoodRange;
        public int minNormRange;
        public int minBadRange;

        public int QlipothCounter;

        //Work Favor
        public int Instinct;
        public int Insight;
        public int Attachment;
        public int Repression;

        //Flavour Texts
        public string Story;

        public string[] ManagerialTips;

        public bool EscapeInformation;

        public string[] FlavourText;
    }
}