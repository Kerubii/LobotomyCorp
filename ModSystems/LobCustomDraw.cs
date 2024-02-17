using LobotomyCorp.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace LobotomyCorp.ModSystems
{
    class LobCustomDraw : ModSystem
    {
        private ScreenFilter[] screenFilters = new ScreenFilter[5];

        public static LobCustomDraw Instance()
        {
            return ModContent.GetInstance<LobCustomDraw>();
        }

        public override void OnWorldLoad()
        {
            screenFilters = new ScreenFilter[5];
            for (int i = 0; i < 5; i++)
            {
                screenFilters[i] = new ScreenFilter();
            }

            //ModContent.GetInstance<LobotomyCorp>().Logger.Info("ScreenFilterInitialized");
        }

        public override void OnWorldUnload()
        {
            screenFilters = null;

            //ModContent.GetInstance<LobotomyCorp>().Logger.Info("ScreenFilterDeinitialized");
        }

        public override void PostUpdateEverything()
        {
            foreach (ScreenFilter ol in screenFilters)
            {
                if (ol.Active)
                {
                    ol.Update();
                    ol.Active = !ol.DeActive();
                }
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 1"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "LobotomyCorp: ScreenFilter",
                    delegate
                    {
                        foreach (ScreenFilter filter in screenFilters)
                        {
                            if (filter.Active)
                            {
                                filter.Draw(Main.spriteBatch, ModContent.GetInstance<Configs.LobotomyConfig>().ScreenEffectOpacity);
                            }
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }

            base.ModifyInterfaceLayers(layers);
        }

        /// <summary>
        /// There are 5 Layers for screentextures, Replaces the layer as inteded limitation so as a General rule of thumb, lets say this
        /// 0-2 Lower Layers, Used as visual effects for players, will occupy a spot inactive, if all slots active replaces preferred layer
        /// 3   Used by NPCs to provide information, preferably bosses or special npcs
        /// 4   Special Cases
        /// Force to replace a specific layer, used for 0-2
        /// </summary>
        /// <param name="newLayer"></param>
        /// <param name="layer"></param>
        public void AddFilter(ScreenFilter newLayer, int layer = 0, bool force = false)
        {
            if (!force && layer < 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (screenFilters.GetType() == newLayer.GetType())
                    {
                        screenFilters[i] = null;
                        screenFilters[i] = newLayer;
                        return;
                    }
                }

                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (!screenFilters[i].Active)
                        {
                            screenFilters[i] = newLayer;
                            return;
                        }
                    }
                }
            }

            screenFilters[layer] = newLayer;
        }

        /// <summary>
        /// There are 5 Layers for screentextures, Replaces the layer so as a General rule of thumb, lets say this
        /// 0-2 Lower Layers, Used as visual effects for players, will occupy a spot inactive, if all slots active replaces preferred layer
        /// 3   Used by NPCs to provide information, preferably bosses or special npcs
        /// 4   Special Cases
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public bool IsLayerActive(int layer)
        {
            return screenFilters[layer].Active;
        }

        public override void Unload()
        {
            screenFilters = null;
        }
    }
}
