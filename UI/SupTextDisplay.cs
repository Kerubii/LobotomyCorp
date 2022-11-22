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

namespace LobotomyCorp.UI
{
    class SupTextDisplay
    {
        public SuppressionText[] Text;

        public SupTextDisplay()
        {
            Text = new SuppressionText[10];
            for (int i = 0; i < Text.Length; i++)
            {
                Text[i] = new SuppressionText("", Vector2.Zero, 0, 0, Color.White, 0, 0, 0, 0f);
                Text[i].active = false;
            }
        }

        public void Unload()
        {
            Text = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (SuppressionText tex in Text)
            {
                tex.Draw(spriteBatch);
            }
        }

        public void Update()
        {
            foreach (SuppressionText tex in Text)
            {
                tex.Update();
            }
        }

        public bool IsActive()
        {
            foreach (SuppressionText tex in Text)
            {
                if (tex.active)
                    return true;
            }
            return false;
        }
    }

    class SuppressionText
    {
        public string text;
        public string subText;
        public float rotation;
        public float speed;
        public Vector2 position;
        public float scale;
        public int dir;
        public byte alpha;
        public Color color;
        public bool active;
        public float depth;

        float progress;
        int linger;

        public SuppressionText(string Text, Vector2 TextPosition, float TextRotation, float TextScale, Color Color, float TextSpeed, int fadeTime, int Direction, float Depth)
        {
            text = Text;
            subText = "";
            rotation = TextRotation;
            speed = TextSpeed;
            position = TextPosition;
            scale = TextScale;
            dir = Direction;
            color = Color;
            active = true;
            alpha = 255;
            depth = Depth;

            progress = 0f;
            linger = fadeTime;
        }

        public void Update()
        {
            if (!active)
                return;

            progress += speed;
            int length = (int)Math.Floor(progress);
            int maxLength = text.Length;
            if (length > maxLength)
                length = maxLength;
            subText = text.Substring(0, length);
            //Main.NewText(progress);

            float fade = maxLength + linger * speed;
            if (progress > fade)
            {
                int a = (int)(255 * ((progress - fade) / (linger * 0.5f * speed)));
                if (a > 255)
                    a = 255;
                alpha = (byte)(a);
            }
            else
            {
                if (alpha > 0)
                {
                    int i = alpha - 9;
                    alpha = (byte)i;
                    if (i <= 0)
                        alpha = 0;
                }
            }

            if (alpha >= 255)
            {
                active = false;
                //Main.NewText("Done!");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!active)
                return;

            //Main.NewText("Draw");

            Vector2 pos = position - Main.screenPosition;
            Vector2 origin = FontAssets.CombatText[1].Value.MeasureString(subText);
            origin.Y *= 0.5f;
            if (dir > 0)
                origin.X = 0;
            else if (dir == 0)
                origin.X *= 0.5f;
            float tScale = CombatText.TargetScale;
            float textScale = scale * tScale;
            Vector2 offset = Vector2.Zero;
            if (depth > 0f && Main.LocalPlayer != null)
            {
                offset = position - Main.LocalPlayer.Center;
                offset *= 0.3f * depth;
            }
            Color textColor = color * (1f - alpha / 255f) * 0.4f;
            textColor.A = (byte)(255 * (1f - alpha / 255f));

            for (int i = 0; i < 6; i++)
            {
                switch(i)
                {
                    case 0:
                        pos.X -= CombatText.TargetScale;
                        break;
                    case 1:
                        pos.X += CombatText.TargetScale;
                        break;
                    case 2:
                        pos.Y -= CombatText.TargetScale;
                        break;
                    case 3:
                        pos.Y += CombatText.TargetScale;
                        break;
                    default:
                        if (Main.rand.Next(30) == 0)
                        {
                            offset.X += Main.rand.NextFloat(-1.0f, 1.0f) * scale;
                            offset.Y += Main.rand.NextFloat(-1.0f, 1.0f) * scale;
                        }
                        textColor = color * (1f - alpha / 255f);
                        break;
                }
                textColor *= 0.9f;
                DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.CombatText[1].Value, subText, pos + offset, textColor, rotation, origin, textScale, 0f, 0f);
            }
        }

        /// <summary>
        /// Adds wacky suppression text owo
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="dir"></param>
        public static int AddText(string text, Vector2 position, float Rotation, float scale, Color color, float speed = 0.2f, int fadeTime = 240,int dir = 0, float depth = 0)
        {
            SuppressionTextSystem System = ModContent.GetInstance<SuppressionTextSystem>();

            if (System.SupText == null)
            {
                ModContent.GetInstance<LobotomyCorp>().Logger.Info("Suppression text failed to Initialize");
                return -1;
            }

            SuppressionText[] Text = System.SupText.Text;//Mod.SupText.Text;
            for (int i = 0; i < Text.Length; i++)
            {
                if (!Text[i].active)
                {
                    Text[i] = new SuppressionText(text, position, Rotation, scale, color, speed, fadeTime, dir, depth);
                    //Main.NewText("Added");

                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Get Sephirah text from Localization files
        /// </summary>
        /// <param name="Sephirah"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetSephirahText(string Sephirah, int id)
        {
            return Language.GetTextValue("Mods.LobotomyCorp.SuppressionTexts." + Sephirah + "." + id);
        }

        /// <summary>
        /// Get Sephirah text from Localization files
        /// </summary>
        /// <param name="Sephirah"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetSephirahText(string Sephirah, string id)
        {
            return Language.GetTextValue("Mods.LobotomyCorp.SuppressionTexts." + Sephirah + "." + id);
        }

        /// <summary>
        /// So yeah lol
        /// </summary>
        /// <param name="mod"></param>
        public static void Clear()
        {
            SuppressionTextSystem System = ModContent.GetInstance<SuppressionTextSystem>();

            if (System.SupText == null)
            {
                ModContent.GetInstance<LobotomyCorp>().Logger.Info("Suppression text failed to Initialize");
                return;
            }

            SuppressionText[] Text = System.SupText.Text;
            for (int i = 0; i < Text.Length; i++)
            {
                Text[i].active = false;
            }
        }
    }
}