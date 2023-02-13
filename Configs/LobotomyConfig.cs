using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace LobotomyCorp.Config
{
    public class LobotomyConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("General Configuration")]
        [Label("Expanded Realized EGO Tooltips")]
        [DefaultValue(true)]
        public bool ExtraPassivesShow;

        [Label("Screenshake Enabled")]
        [DefaultValue(true)]
        public bool ScreenShakeEnabled;
    }
}