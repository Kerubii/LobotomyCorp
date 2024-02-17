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

namespace LobotomyCorp
{
    public class AbnormalityData
    {
        public string AbnormalityName;
        public string AbnormalityNameSecond;
        public virtual int npcType => 0;
        public string CodeNo;
        public Asset<Texture2D> Image;

        public RiskLevel riskLevel;

        public int MaximumWorkTime;

        public int MaximumBoxes;

        public int minGoodRange;
        public int maxBadRange;

        public int QlipothCounter;

        //Work Favor
        public WorkData Instinct;
        public WorkData Insight;
        public WorkData Attachment;
        public WorkData Repression;

        //Flavour Texts

        private string[] Story;

        public string[] ManagerialTips;

        public bool EscapeInformation;

        public AbnormalityData()
        {
            AbnormalityName = GetLocalizedFile("Name");
            AbnormalityNameSecond = GetLocalizedFile("NameOld");
            Image = ModContent.Request<Texture2D>("LobotomyCorp/AbnormalityData/Images/" + GetType().Name);
            minGoodRange = 8;
            maxBadRange = 3;
            MaximumBoxes = 10;
            QlipothCounter = -1;
            CreateWorkData(WorkType.Instinct, 0.5f, 0.4f, 0.3f, 0.3f, 0.3f);
            CreateWorkData(WorkType.Insight, 0.7f, 0.7f, 0.5f, 0.5f, 0.5f);
            CreateWorkData(WorkType.Attachment, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f);
            CreateWorkData(WorkType.Repression, 0.5f, 0.4f, 0.3f, 0.3f, 0.3f);
            Story = GetLocalizedFile("Story", 10).ToArray();
            ManagerialTips = GetLocalizedFile("Tips", 10).ToArray();
            CodeNo = "O-00-00";
            MaximumWorkTime = 180;
            SetDefault();
        }

        /// <summary>
        /// List to change in SetDefaults: riskLevel = Zayin, BoxResult = 10, minGoodRange = 8, maxBadRange = 3, QlipothCounter = -1, Set WorkData with Create Work Data, Default Data is OneBad
        /// </summary>
        public virtual void SetDefault()
        {
            
        }

        //
        public virtual List<string> GetFlavourText(int progress, bool SpecialWorkType)
        {
            return GetLocalizedFileAbnoPlayer("FlavourText", 10);
        }

        public List<string> GetLocalizedFile(string key, int amount)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < amount; i++)
            {
                string text = GetLocalizedFile(key + "." + (i + 1));
                if (text == "Undefined")
                    break;
                list.Add(text);
            }
            return list;
        }

        // Returns Undefined if it doesnt exist in Localization File
        public string GetLocalizedFile(string key)
        {
            string ValueKey = "Mods.LobotomyCorp.AbnormalityData." + npcType + "." + key;
            string Text = Language.GetTextValue(ValueKey);
            if (Text == ValueKey)
                return "Undefined";
            return Text;
        }

        public List<string> GetLocalizedFileAbnoPlayer(string key, int amount)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < amount; i++)
            {
                string text = GetLocalizedFileAbnoPlayer(key + "." + (i + 1));
                if (text == "Undefined")
                    break;
                list.Add(text);
            }
            return list;
        }

        public string GetLocalizedFileAbnoPlayer(string key)
        {
            string ValueKey = "Mods.LobotomyCorp.AbnormalityData." + npcType + "." + key;
            string Text = Language.GetTextValue(ValueKey, AbnormalityName, Main.LocalPlayer.name);
            if (Text == ValueKey)
                return "Undefined";
            return Text;
            //return Language.GetTextValue("Mods.LobotomyCorp.AbnormalityData." + npcType + "." + key, AbnormalityName, Main.LocalPlayer.name);
        }

        public string GetLocalizedFilePlayer(string key)
        {
            string ValueKey = "Mods.LobotomyCorp.AbnormalityData." + npcType + "." + key;
            string Text = Language.GetTextValue(ValueKey, Main.LocalPlayer.name);
            if (Text == ValueKey)
                return "Undefined";
            return Text;
            //return Language.GetTextValue("Mods.LobotomyCorp.AbnormalityData." + npcType + "." + key, AbnormalityName, Main.LocalPlayer.name);
        }

        public string GetLocalizedFileAbno(string key)
        {
            string ValueKey = "Mods.LobotomyCorp.AbnormalityData." + npcType + "." + key;
            string Text = Language.GetTextValue(ValueKey, AbnormalityName);
            if (Text == ValueKey)
                return "Undefined";
            return Text;
            //return Language.GetTextValue("Mods.LobotomyCorp.AbnormalityData." + npcType + "." + key, AbnormalityName, Main.LocalPlayer.name);
        }

        public static string GetLocalizedFileNonAbno(string key)
        {
            string ValueKey = "Mods.LobotomyCorp.AbnormalityData." + key;
            return Language.GetTextValue(ValueKey);
        }

        public AbnormalityData LoadWorktype(WorkType work)
        {
            return this;
        }

        /// <summary>
        /// Use WholeNumbers, like 50 for 50%
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="value4"></param>
        /// <param name="value5"></param>
        public void CreateWorkData(WorkType type, float value1, float value2, float value3, float value4, float value5)
        {
            WorkData work = new WorkData(value1, value2, value3, value4, value5);
            switch (type)
            {
                case WorkType.Instinct:
                    Instinct = work;
                    return;
                case WorkType.Insight:
                    Insight = work;
                    break;
                case WorkType.Attachment:
                    Attachment = work;
                    break;
                case WorkType.Repression:
                    Repression = work;
                    break;
            }
        }
    }

    public class WorkData
    {
        private float Level1;
        private float Level2;
        private float Level3;
        private float Level4;
        private float Level5;

        public WorkData(float one, float two, float three, float four, float five)
        {
            Level1 = one; Level2 = two; Level3 = three; Level4 = four; Level5 = five;
        }

        public float GetChanceTemperance(int level, int Temperance)
        {
            return GetChance(level) + Temperance;
        }

        public float GetChance(int level)
        {
            switch (level)
            {
                default:return Level1;
                case 1: return Level2;
                case 2: return Level3;
                case 3: return Level4;
                case 4: return Level5;
            }
        }
    }
}