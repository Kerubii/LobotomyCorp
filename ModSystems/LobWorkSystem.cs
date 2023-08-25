using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace LobotomyCorp.ModSystems
{
    class LobWorkSystem : ModSystem
    {
        internal UserInterface LobWork;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                LobWork = new UserInterface();
            }
        }

        public override void Unload()
        {
            LobWork = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (Main.LocalPlayer.controlInv)
            {
                LobWork.SetState(new UI.LobWorkUI());
            }
            if (LobWork != null)
            {
                LobWork.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "LobotomyCorp: WorkUI",
                    delegate
                    {
                        LobWork.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }

}
