using LobotomyCorp.Config;
using LobotomyCorp.UI;
using LobotomyCorp.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
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

        public static Asset<Texture2D> CircleGlow = null;

        public static Asset<Texture2D> RedShoesGlittering = null;

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

        public const bool TestMode = true;

        public class WeaponSounds
        {
            public static SoundStyle Axe;
            public static SoundStyle BowGun;
            public static SoundStyle Cannon;
            public static SoundStyle Dagger;
            public static SoundStyle Fist;
            public static SoundStyle Gun;
            public static SoundStyle Hammer;
            public static SoundStyle Mace;
            public static SoundStyle Rapier;
            public static SoundStyle Revolver;
            public static SoundStyle Rifle;
            public static SoundStyle Spear;
        }

        public static Dictionary<string, CustomShaderData> LobcorpShaders = new Dictionary<string, CustomShaderData>();

        public static void RiskLevelResist(ref int damage, RiskLevel ego, RiskLevel risk)
        {

        }

        public override void Load()
        {
            SuppressionTextData.Initialize();

            if (!Main.dedServ)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    ArcanaSlaveLaser = Assets.Request<Texture2D>("Projectiles/QueenLaser/Laser", AssetRequestMode.ImmediateLoad);
                    ArcanaSlaveBackground = Assets.Request<Texture2D>("Projectiles/QueenLaser/CircleBackground", AssetRequestMode.ImmediateLoad);
                    KingPortal1 = Assets.Request<Texture2D>("Projectiles/KingPortal/KingPortal1", AssetRequestMode.ImmediateLoad);
                    KingPortal2 = Assets.Request<Texture2D>("Projectiles/KingPortal/KingPortal2", AssetRequestMode.ImmediateLoad);
                    KingPortal3 = Assets.Request<Texture2D>("Projectiles/KingPortal/KingPortal3", AssetRequestMode.ImmediateLoad);

                    MagicBulletBullet = Assets.Request<Texture2D>("Projectiles/MagicBulletBullet");

                    CircleGlow = Assets.Request<Texture2D>("Misc/CircleGlow", AssetRequestMode.ImmediateLoad);

                    RedShoesGlittering = Assets.Request<Texture2D>("Misc/RedShoesGlitter", AssetRequestMode.ImmediateLoad);

                    Main.QueueMainThreadAction(() =>
                    {
                        PremultiplyTexture(ArcanaSlaveLaser.Value);
                        PremultiplyTexture(ArcanaSlaveBackground.Value);

                        PremultiplyTexture(KingPortal1.Value);
                        PremultiplyTexture(KingPortal2.Value);
                        PremultiplyTexture(KingPortal3.Value);

                        PremultiplyTexture(MagicBulletBullet.Value);

                        PremultiplyTexture(CircleGlow.Value);

                        PremultiplyTexture(RedShoesGlittering.Value);

                        
                    });

                    //Ref<Effect> punishingRef = new Ref<Effect>(GetEffect("Effects/PunishingBird"));
                    Ref<Effect> TrailRef = new Ref<Effect>(Assets.Request<Effect>("Effects/SwordTrail", AssetRequestMode.ImmediateLoad).Value);
                    Ref<Effect> GlowTrail = new Ref<Effect>(Assets.Request<Effect>("Effects/GlowTrail", AssetRequestMode.ImmediateLoad).Value);
                    Ref<Effect> ArcanaSlaveRef = new Ref<Effect>(Assets.Request<Effect>("Effects/ArcanaSlave", AssetRequestMode.ImmediateLoad).Value);
                    Ref<Effect> FourthMatchFlame = new Ref<Effect>(Assets.Request<Effect>("Effects/FourthMatchFlame", AssetRequestMode.ImmediateLoad).Value);
                    Ref<Effect> GenericTrail = new Ref<Effect>(Assets.Request<Effect>("Effects/GenericTrail", AssetRequestMode.ImmediateLoad).Value);
                    Ref<Effect> BladeTrail = new Ref<Effect>(Assets.Request<Effect>("Effects/BladeTrail", AssetRequestMode.ImmediateLoad).Value);
                    Ref<Effect> BrokenScreen = new Ref<Effect>(Assets.Request<Effect>("Effects/BrokenShader", AssetRequestMode.ImmediateLoad).Value);
                    Ref<Effect> RedMistEffect = new Ref<Effect>(Assets.Request<Effect>("Effects/OverlayShader", AssetRequestMode.ImmediateLoad).Value);
                    //GameShaders.Misc["Punish"] = new MiscShaderData(punishingRef, "PunishingBird");

                    GameShaders.Misc["LobotomyCorp:Rotate"] = new MiscShaderData(new Ref<Effect>(ArcanaSlaveRef.Value), "ArcanaResize").UseSaturation(0f);

                    ScreenShaderData shaderData = new ScreenShaderData(BrokenScreen, "BrokenScreenShader");
                    shaderData.UseImage(Assets.Request<Texture2D>("Misc/CameraFilterPack_TV_BrokenGlass5", AssetRequestMode.ImmediateLoad).Value, 0, SamplerState.LinearWrap);
                    Filters.Scene["LobotomyCorp:BrokenScreen"] = new Filter(shaderData, EffectPriority.Medium);

                    shaderData = new ScreenShaderData(RedMistEffect, "OverlayRedMist");
                    shaderData.UseImage(Assets.Request<Texture2D>("Misc/Hexagons2", AssetRequestMode.ImmediateLoad).Value, 0, SamplerState.LinearWrap);
                    shaderData.UseImage(Assets.Request<Texture2D>("Misc/Hexagons", AssetRequestMode.ImmediateLoad).Value, 1, SamplerState.LinearWrap);
                    shaderData.UseImage(Assets.Request<Texture2D>("Misc/VignetteA", AssetRequestMode.ImmediateLoad).Value, 2, SamplerState.LinearWrap);
                    shaderData.UseColor(Color.Red);
                    shaderData.UseSecondaryColor(new Color(1, 0.8f, 0.8f));
                    Filters.Scene["LobotomyCorp:RedMistOverlay"] = new Filter(shaderData, EffectPriority.Medium);

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

                    shader = new CustomShaderData(BladeTrail, "BlaidTrail");
                    LobcorpShaders["SwingTrail"] = shader;

                    shader = new CustomShaderData(BladeTrail, "BlaidTrail");
                    shader.UseImage1(this, "Projectiles/Realized/RedEyesSlash");
                    shader.UseImage2(this, "Projectiles/Realized/RedEyesSlashA");
                    shader.UseImage3(this, "Misc/FX_Tex_Noise_Plasma1");
                    LobcorpShaders["RedEyesTrail"] = shader;

                    WeaponSounds.Axe = WeaponSound("axe", true, 2);
                    WeaponSounds.BowGun = WeaponSound("bowGun");
                    WeaponSounds.Cannon = WeaponSound("cannon");
                    WeaponSounds.Dagger = WeaponSound("dagger", true, 2);
                    WeaponSounds.Fist = WeaponSound("fist", true, 2);
                    WeaponSounds.Gun = WeaponSound("gun");
                    WeaponSounds.Hammer = WeaponSound("hammer");
                    WeaponSounds.Mace = WeaponSound("mace", true, 2);
                    WeaponSounds.Rapier = WeaponSound("rapier", true, 2);
                    WeaponSounds.Revolver = WeaponSound("revolver");
                    WeaponSounds.Rifle = WeaponSound("rifle");
                    WeaponSounds.Spear = WeaponSound("spear", true, 2);
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

        public static bool LamentValid(NPC t, Projectile p)
        {
            bool valid = true;
            float health = (float)t.life / (float)t.lifeMax;
            foreach (NPC n in Main.npc)
            {
                float health2 = (float)n.life / (float)n.lifeMax;
                if (n.active && !n.dontTakeDamage && !n.friendly && n.life > 0 && n.whoAmI != t.whoAmI && health2 < health && n.chaseable && n.CanBeChasedBy(p) && n.realLife < 0)
                {
                    valid = false;
                    //Main.NewText(n.type);
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
        
        public static SoundStyle WeaponSound(string name, bool PitchVariance = true, int Variance = 1)
        {
            float Volume = 0.1f;
            if (PitchVariance)
            {
                if (Variance > 1)
                    return new SoundStyle("LobotomyCorp/Sounds/Item/LWeapons/" + name, Variance) with { Volume = Volume, MaxInstances = 2,PitchVariance = 0.1f };
                else
                    return new SoundStyle("LobotomyCorp/Sounds/Item/LWeapons/" + name) with { Volume = Volume, MaxInstances = 2, PitchVariance = 0.1f };
            }
            else
            {
                if (Variance > 1)
                    return new SoundStyle("LobotomyCorp/Sounds/Item/LWeapons/" + name, Variance) with { Volume = Volume, MaxInstances = 2 };
                else
                    return new SoundStyle("LobotomyCorp/Sounds/Item/LWeapons/" + name) with { Volume = Volume, MaxInstances = 2 };
            }
        }

        /// <summary>
        /// NameLocation as FolderName/FileName
        /// </summary>
        /// <param name="NameLocation"></param>
        /// <param name="instances"></param>
        /// <param name="Variance"></param>
        /// <param name="Pitch"></param>
        /// <returns></returns>
        public static SoundStyle ItemLobSound(string NameLocation, int instances = 1, int Variance = 1, float Pitch = 0f, float Volume = 1f)
        {
            if (Variance == 1)
            {
                return new SoundStyle("LobotomyCorp/Sounds/Item/" + NameLocation) with { Volume = 0.5f * Volume, MaxInstances = instances, PitchVariance = Pitch };
            }
            else
            {
                return new SoundStyle("LobotomyCorp/Sounds/Item/" + NameLocation, Variance) with { Volume = 0.5f, MaxInstances = instances, PitchVariance = Pitch };
            }
        }

        public static void ScreenShake(int Time, float Intensity, float decay = 0, bool Forced = true)
        {
            ModContent.GetInstance<LobSystem>().ScreenShake(Time, Intensity, decay, Forced);
        }
    }

    struct LCMWorleyNoise
    {
        private float _variability;
        private int _columns;
        private int _rows;
        private int _width;
        private int _height;

        public LCMWorleyNoise(float randomSeed, int columns = 5, int rows = 5)
        {
            _variability = randomSeed;
            _columns = columns;
            _rows = rows;
            _width = 100;
            _height = 100;
        }

        public void CreateWorleyNoise(float time = 0)
        {
            Texture2D voronoise;
            Color[] data = new Color[_width * _height];
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    float result = GenerateWorley(new Vector2(x / (float)_width, y / (float)_height), _columns, _rows);
                    data[x + (y * 100)] = new Color(result, result, result);
                }
            }
            Main.QueueMainThreadAction(() =>
            {
                voronoise = new Texture2D(Main.graphics.GraphicsDevice, 100, 100);
                voronoise.SetData(data);
                voronoise.SaveAsPng(new FileStream(testFilePath, FileMode.Create), 100, 100);
            });
        }

        private static string cacheFolder = Path.Combine(Main.SavePath, "Mods", "Cache");
        private static string testFileName = "Worley.png";
        private static string testFilePath = Path.Combine(cacheFolder, testFileName);

        private float GenerateWorley(Vector2 uv, int columns, int rows)
        {
            uv.X *= columns;
            uv.Y *= rows;

            Vector2 uvFloor = new Vector2((float)Math.Floor(uv.X), (float)Math.Floor(uv.Y));
            uv -= uvFloor;

            float minimumDist = 1f;

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    Vector2 neighbor = new Vector2(x, y);
                    Vector2 Point = random(uvFloor + neighbor);

                    Vector2 diff = neighbor + Point - uv;
                    float dist = diff.Length();
                    minimumDist = (float)Math.Min(minimumDist, dist);
                }
            }

            return minimumDist;
        }

        private Vector2 random(Vector2 uv)
        {
            if (uv.X == 0)
            {
                uv += new Vector2((float)Math.Sin(_variability * 0.2f), (float)Math.Sin(_variability * 0.5f)) * 0.4f;
            }

            float dot = (float)Math.Sin(Vector2.Dot(uv, new Vector2(321.2f * _variability, 21.43f)));
            float fract = dot - (float)Math.Floor(dot);
            dot = (float)Math.Sin(Vector2.Dot(uv, new Vector2(142.7f * _variability * 0.84f, 421.3f * _variability * 0.2f)));
            float fract2 = dot - (float)Math.Floor(dot);
            return new Vector2(fract, fract2);
        }
    }

    class LobSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            if (Filters.Scene["LobotomyCorp:RedMistOverlay"].IsActive() && !NPC.AnyNPCs(ModContent.NPCType<NPCs.RedMist.RedMist>()))
            {
                Filters.Scene["LobotomyCorp:RedMistOverlay"].Deactivate();
            }

            base.PostUpdateEverything();
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup rec = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + "EvilPowder", new[]
            {
                (int)ItemID.ViciousPowder,
                (int)ItemID.VilePowder
            });
            RecipeGroup.RegisterGroup("LobotomyCorp:EvilPowder", rec);

            rec = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + "Butterflies", new[]
            {
                (int)ItemID.JuliaButterfly,
                ItemID.MonarchButterfly,
                ItemID.PurpleEmperorButterfly,
                ItemID.RedAdmiralButterfly,
                ItemID.SulphurButterfly,
                ItemID.TreeNymphButterfly,
                ItemID.UlyssesButterfly,
                ItemID.ZebraSwallowtailButterfly,
                ItemID.GoldButterfly
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

            rec = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + "Boss Masks", new[]
           {
                (int)ItemID.KingSlimeMask,
                (int)2112,
                2103,
                2111,
                2108,
                1281,
                5109,
                2105,
                4959,
                2113,
                2106,
                2107,
                2109,
                2110,
                4784,
                2588,
                3372,
                3373,
                3864,
                3865,
                3863
            });
            RecipeGroup.RegisterGroup("LobotomyCorp:BossMasks", rec);
        }

        public override void ModifyScreenPosition()
        {
            if (modifiedCamera)
            {
                Main.screenPosition = modifiedScreenPosition;
                modifiedCamera = false;
            }
            
            if (ScreenShakeTimer > 0)
            {
                Main.screenPosition.X += Main.rand.NextFloat(-ScreenShakeIntensity, ScreenShakeIntensity);
                Main.screenPosition.Y += Main.rand.NextFloat(-ScreenShakeIntensity, ScreenShakeIntensity);
                ScreenShakeIntensity -= ScreenShakeDecay;
                ScreenShakeTimer--;
            }
            base.ModifyScreenPosition();
        }

        private int ScreenShakeTimer;
        private float ScreenShakeIntensity;
        private float ScreenShakeDecay;

        public void ScreenShake(int Time, float Intensity, float decay = 0, bool Forced = true)
        {
            if (!ModContent.GetInstance<LobotomyConfig>().ScreenShakeEnabled || (!Forced && ScreenShakeTimer > 0))
                return;

            ScreenShakeTimer = Time;
            ScreenShakeIntensity = Intensity;
            ScreenShakeDecay = decay;
        }

        private bool modifiedCamera;
        private Vector2 modifiedScreenPosition;

        public Vector2 RedEyesSpecialCamera(Vector2 cameraCenter, Vector2? lerpTo = null, float lerp = 0f)
        {
            modifiedCamera = true;
            Vector2 position = cameraCenter + Vector2.UnitY * (Main.player[Main.myPlayer].gfxOffY - 1);
            if (lerpTo != null)
            {
                position = Vector2.Lerp(position, (Vector2)lerpTo, lerp);
            }
            modifiedScreenPosition.X = position.X - Main.screenWidth / 2;
            modifiedScreenPosition.Y = position.Y - Main.screenHeight / 2;
            Main.SetCameraLerp(0.1f, 15);
            return position;
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