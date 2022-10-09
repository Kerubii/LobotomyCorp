using LobotomyCorp.UI;
using LobotomyCorp.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace LobotomyCorp
{
    public enum RiskLevel
    {
        Zayin,
        Teth,
        He,
        Waw,
        Aleph,
        Unknown
    }
    public class LobotomyCorp : Mod
    {
        public static Asset<Texture2D> ArcanaSlaveLaser = null;
        public static Asset<Texture2D> ArcanaSlaveBackground = null;

        public static Asset<Texture2D> KingPortal1 = null;
        public static Asset<Texture2D> KingPortal2 = null;
        public static Asset<Texture2D> KingPortal3 = null;

        public static Asset<Texture2D> MagicBulletBullet = null;

        public static Color PositivePE => new Color(18, 255, 86);
        public static Color NegativePE => new Color(239, 77, 61);

        public static Color RedDamage => new Color(203, 39, 70);
        public static Color WhiteDamage => new Color(238, 230, 191);
        public static Color BlackDamage => new Color(127, 75, 129);
        public static Color PaleDamage => new Color(65, 244, 188);

        public static Color ZayinRarity => new Color(33, 249, 0);
        public static Color TethRarity => new Color(26, 161, 255);
        public static Color HeRarity => new Color(255, 250, 4);
        public static Color WawRarity => new Color(122, 48, 241);
        public static Color AlephRarity => new Color(255, 1, 0);

        public static ModKeybind SynchronizeEGO;
        public static ModKeybind PassiveShow;
        public static bool ExtraPassiveShow = true;

        public static Dictionary<string, CustomShaderData> LobcorpShaders = new Dictionary<string, CustomShaderData>();

        public static void RiskLevelResist(ref int damage, RiskLevel ego, RiskLevel risk)
        {

        }

        public override void Load()
        {
            SuppressionTextData.Initialize();

            if (!Main.dedServ)
            {
                ExtraPassiveShow = false;
                PassiveShow = KeybindLoader.RegisterKeybind(this, "Extend Passive", "F");

                if (Main.netMode != NetmodeID.Server)
                {
                    ArcanaSlaveLaser = Assets.Request<Texture2D>("Projectiles/QueenLaser/Laser", AssetRequestMode.ImmediateLoad);
                    ArcanaSlaveBackground = Assets.Request<Texture2D>("Projectiles/QueenLaser/CircleBackground", AssetRequestMode.ImmediateLoad);
                    KingPortal1 = Assets.Request<Texture2D>("Projectiles/KingPortal/KingPortal1", AssetRequestMode.ImmediateLoad);
                    KingPortal2 = Assets.Request<Texture2D>("Projectiles/KingPortal/KingPortal2", AssetRequestMode.ImmediateLoad);
                    KingPortal3 = Assets.Request<Texture2D>("Projectiles/KingPortal/KingPortal3", AssetRequestMode.ImmediateLoad);

                    MagicBulletBullet = Assets.Request<Texture2D>("Projectiles/MagicBulletBullet");

                    Main.QueueMainThreadAction(() =>
                    {
                        PremultiplyTexture(ArcanaSlaveLaser.Value);
                        PremultiplyTexture(ArcanaSlaveBackground.Value);

                        PremultiplyTexture(KingPortal1.Value);
                        PremultiplyTexture(KingPortal2.Value);
                        PremultiplyTexture(KingPortal3.Value);

                        PremultiplyTexture(MagicBulletBullet.Value);
                    });

                    //Ref<Effect> punishingRef = new Ref<Effect>(GetEffect("Effects/PunishingBird"));
                    Ref<Effect> TrailRef = new Ref<Effect>(Assets.Request<Effect>("Effects/SwordTrail", AssetRequestMode.ImmediateLoad).Value);
                    Ref<Effect> GlowTrail = new Ref<Effect>(Assets.Request<Effect>("Effects/GlowTrail", AssetRequestMode.ImmediateLoad).Value);
                    Ref<Effect> ArcanaSlaveRef = new Ref<Effect>(Assets.Request<Effect>("Effects/ArcanaSlave", AssetRequestMode.ImmediateLoad).Value);
                    Ref<Effect> FourthMatchFlame = new Ref<Effect>(Assets.Request<Effect>("Effects/FourthMatchFlame", AssetRequestMode.ImmediateLoad).Value);
                    Ref<Effect> GenericTrail = new Ref<Effect>(Assets.Request<Effect>("Effects/GenericTrail", AssetRequestMode.ImmediateLoad).Value);

                    //GameShaders.Misc["Punish"] = new MiscShaderData(punishingRef, "PunishingBird");

                    GameShaders.Misc["LobotomyCorp:Rotate"] = new MiscShaderData(new Ref<Effect>(ArcanaSlaveRef.Value), "ArcanaResize").UseSaturation(0f);

                    //Texture2D blankTexture = TextureManager.BlankTexture;

                    //TextureManager.BlankTexture = GetTexture("Projectiles/Help");
                    //GameShaders.Misc["TrailingShader"] = new MiscShaderData(TrailRef, "Trail").UseImage("f");

                    //LobcorpShaders["FairySlash"] = new CustomShaderData(TrailRef, "Trail").UseImage1(this, "Projectiles/Help");
                    //LobcorpShaders["TestSlash"] = new CustomShaderData(TestTrail, "TestTrail").UseImage1(this, "Projectiles/TestSlash");
                    //LobcorpShaders["TwilightSlash"] = new CustomShaderData(TrailRef, "Trail").UseImage1(this, "Projectiles/TwilightTrail");

                    CustomShaderData shader = new CustomShaderData(TrailRef, "Trail").UseImage1(this, "Projectiles/MimicrySBlur2A");
                    shader.UseImage2(this, "Projectiles/MimicrySBlur2");
                    LobcorpShaders["MimicrySlash"] = shader;

                    shader = new CustomShaderData(FourthMatchFlame, "FourthMatch").UseImage1(this, "Projectiles/FourthMatchFlameGigaSlashA");
                    shader.UseImage2(this, "Projectiles/FourthMatchFlameGigaSlashFire");
                    shader.UseImage3(this, "Projectiles/FourthMatchFlameGigaSlashB");
                    LobcorpShaders["FourthMatchFlame"] = shader;

                    shader = new CustomShaderData(TrailRef, "Trail").UseImage1(this, "Projectiles/WingbeatSlashA");
                    shader.UseImage2(this, "Projectiles/WingbeatSlashA");
                    shader.UseImage3(this, "Projectiles/WingbeatSlashB");
                    LobcorpShaders["WingbeatSlash"] = shader;

                    shader = new CustomShaderData(GlowTrail, "GlowTrail").UseImage1(this, "Misc/GenericWindTrail");
                    shader.UseImage2(this, "Misc/GenericWindTrailG");
                    LobcorpShaders["WindTrail"] = shader;

                    shader = new CustomShaderData(GenericTrail, "GenericTrail").UseImage1(this, "Misc/GenTrail");
                    LobcorpShaders["GenericTrail"] = shader;

                    shader = new CustomShaderData(TrailRef, "Trail").UseImage1(this, "Misc/MagicBulletTrailA");
                    shader.UseImage2(this, "Misc/MagicBulletTrailB");
                    shader.UseImage3(this, "Misc/MagicBulletTrailA");
                    LobcorpShaders["MagicBulletTrail"] = shader;

                    shader = new CustomShaderData(TrailRef, "Trail");
                    LobcorpShaders["TextureTrail"] = shader;
                    //TextureManager.BlankTexture = blankTexture;
                }
            }
        }

        public override void Unload()
        {
            SuppressionTextData.Unload();

            ArcanaSlaveLaser = null;
            ArcanaSlaveBackground = null;

            KingPortal1 = null;
            KingPortal2 = null;
            KingPortal3 = null;

            MagicBulletBullet = null;

            /*PositivePE = new Color();
            NegativePE = new Color();
            ZayinRarity = new Color();
            TethRarity = new Color();
            HeRarity = new Color();
            WawRarity = new Color();
            AlephRarity = new Color();*/
            PassiveShow = null;
        }
        /*
        public override void Close()
        {
            var slots = new int[] {
                MusicLoader.GetMusicSlot(this, "LobotomyCorp/Sounds/Music/PMSecondWarning"),
                MusicLoader.GetMusicSlot(this, "LobotomyCorp/Sounds/Music/TilaridsDistortedNight"),
                MusicLoader.GetMusicSlot(this, "LobotomyCorp/Sounds/Music/TilaridsInsigniaDecay")
            };

            foreach (var slot in slots)
            {
                if (((LegacyAudioSystem)Main.audioSystem).AudioTracks.IndexInRange(slot) && Main.audioSystem.IsTrackPlaying(slot) == true)
                {
                    ((LegacyAudioSystem)Main.audioSystem).AudioTracks[slot].Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions.Immediate);
                }
            }

            base.Close();
        }*/

        public override void AddRecipeGroups()/* tModPorter Note: Removed. Use ModSystem.AddRecipeGroups */
        {
            RecipeGroup rec = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + "EvilPowder", new[]
            {
                (int)ItemID.ViciousPowder,
                (int)ItemID.VilePowder
            });
            RecipeGroup.RegisterGroup("LobotomyCorp:EvilPowder", rec);

            rec = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + "Butterflies", new[]
            {
                (int)ItemID.GoldButterfly,
                (int)ItemID.JuliaButterfly,
                ItemID.MonarchButterfly,
                ItemID.PurpleEmperorButterfly,
                ItemID.RedAdmiralButterfly,
                ItemID.SulphurButterfly,
                ItemID.TreeNymphButterfly,
                ItemID.UlyssesButterfly,
                ItemID.ZebraSwallowtailButterfly
            });
            RecipeGroup.RegisterGroup("LobotomyCorp:Butterflies", rec);

            rec = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + "Dungeon Lanterns", new[]
            {
                (int)ItemID.AlchemyLantern,
                (int)ItemID.BrassLantern,
                ItemID.CagedLantern,
                ItemID.CarriageLantern,
                ItemID.AlchemyLantern,
                ItemID.DiablostLamp,
                ItemID.OilRagSconse
            });
            RecipeGroup.RegisterGroup("LobotomyCorp:DungeonLantern", rec);
        }

        public static bool LamentValid(NPC t, Projectile p)
        {
            bool valid = true;
            float health = (float)t.life / (float)t.lifeMax;
            foreach (NPC n in Main.npc)
            {
                float health2 = (float)t.life / (float)t.lifeMax;
                if (n.active && !n.dontTakeDamage && !n.friendly && n.life > 0 && n.whoAmI != t.whoAmI && health2 < health && n.chaseable && n.CanBeChasedBy(p) && n.realLife < 0)
                {
                    valid = false;
                    Main.NewText(n.type);
                    break;
                }
            }
            return valid;
        }

        public static void PremultiplyTexture(Texture2D texture)
        {
            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }
            texture.SetData(buffer);
        }

        public static float Lerp(float x, float x2, float progress, bool reverse = false)
        {
            if (progress < 0)
                progress = 0;
            if (progress > 1f)
                progress = 1f;
            if (reverse)
                return x2 * (1 - progress) + x * progress;
            else
                return x * (1 - progress) + x2 * progress;
        }
    
        public static Vector4 ShaderRotation(float progress)
        {
            float sin = (float)Math.Sin(6.28f * progress);
            float cos = (float)Math.Cos(6.28f * progress);
            return new Vector4(sin, cos, 0f, 0f);
        }
    }

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
                                filter.Draw(Main.spriteBatch);
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
        /// There are 5 Layers for screentextures, Replaces the layer so as a General rule of thumb, lets say this
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
                    if (screenFilters.GetType() == newLayer.GetType() || !screenFilters[i].Active)
                    {
                        screenFilters[i] = newLayer;
                        return;
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