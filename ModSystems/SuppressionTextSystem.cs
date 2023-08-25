using System.Collections.Generic;
using LobotomyCorp.UI;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;

namespace LobotomyCorp.ModSystems
{
    class SuppressionTextSystem : ModSystem
    {
        internal SupTextDisplay SupText;

        public override void OnWorldLoad()
        {
            SupText = new SupTextDisplay();
        }

        public override void OnWorldUnload()
        {
            SupText.Unload();
        }

        public override void PostUpdateDusts()
        {
            SupText.Update();
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Tile Grid Option"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "LobotomyCorp: Suppression Text",
                    delegate
                    {
                        if (SupText.IsActive())
                        {
                            SupText.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.Game)
                );
            }

            base.ModifyInterfaceLayers(layers);
        }
    }
}
