using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Graphics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using LobotomyCorp;
using Terraria.UI;
using ReLogic.Graphics;
using System;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;

namespace LobotomyCorp.UI
{
    internal class LobWorkUI : UIState
    {
        public override void OnInitialize()
        {
            
        }
    }

    class LobWorkButton : UIElement
    {
        string FlavorText;
        WorkType workType;

        public override void OnActivate()
        {
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
        }
    }

    enum WorkType
    {
        Instinct,
        Insight,
        Attachment,
        Repression
    }
}