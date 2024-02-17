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
using LobotomyCorp.ModSystems;
using static LobotomyCorp.ModSystems.LobWorkSystem;
using System.Diagnostics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.UI.Chat;
using System.Text.RegularExpressions;
using System.Drawing.Text;
using System.Security.Cryptography;

namespace LobotomyCorp.UI
{
    internal class LobWorking : UIState
    {
        private AbnormalityData Abnormality;

        public UIElement UIRoot;

        public UIPanel ImagePanel;
        public UIImage AbnoImage;
        public LobQlipothUI QlipothCounter;

        public LobAbnormalityNameUI AbnoName;
        public LobAbnormalityFlavourText AbnoFlavorText;

        public UIImage ThreatLevel;
        public UIPanel TotalAmount;
        public LobEnkephalinBar EnkephalinBar;

        public UIElement WorkPanelRoot;
        public LobWorkTimerUI WorkTimer;
        public UIPanel WorkPanel;
        public LobWorkChoiceUI[] WorkChoice;
        public LobGearImageUI GearWork;

        private int TimeElapsed;
        private WorkType currentWorkType;

        public override void OnInitialize()
        {
            TimeElapsed = 0;

            UIRoot = new UIElement();
            SetRectangle(UIRoot, left: 525, top: 200f, width: 1000f, height: 1000f);

            EnkephalinBar = new LobEnkephalinBar();
            SetRectangle(EnkephalinBar, -76, 44, 96, 500);
            UIRoot.Append(EnkephalinBar);

            WorkPanelRoot = new UIElement();
            SetRectangle(WorkPanelRoot, 0, 275, 700, 300);
            UIRoot.Append(WorkPanelRoot);

            int length = (int)WorkPanelRoot.Width.Pixels;
            int height = (int)WorkPanelRoot.Height.Pixels;
            GearWork = new LobGearImageUI(length + 5, height / 2 - 25);
            WorkPanelRoot.Append(GearWork);

            WorkPanel = LobPanelCreate(4);
            SetRectangle(WorkPanel, 0, 0, 700, 300);
            WorkPanelRoot.Append(WorkPanel);

            WorkChoice = new LobWorkChoiceUI[4];
            CreateChoices(WorkPanel, length, height);
            
            WorkTimer = new LobWorkTimerUI();
            SetRectangle(WorkTimer, length / 2 - 16, height / 2 - 16, 32, 32);
            WorkPanel.Append(WorkTimer);

            ImagePanel = LobPanelCreate(0);
            ImagePanel.SetPadding(0);
            SetRectangle(ImagePanel, 0, 0, width: 250f, height: 250f);
            UIRoot.Append(ImagePanel);

            AbnoImage = new UIImage(ModContent.Request<Texture2D>("LobotomyCorp/AbnormalityData/Images/OneBadManyGood"));
            SetRectangle(AbnoImage, 25 + 6f, 25 + 6f, 200f, 200f);
            ImagePanel.Append(AbnoImage);

            QlipothCounter = new LobQlipothUI(-1);
            QlipothCounter.Top.Set(-64f, 0f);
            ImagePanel.Append(QlipothCounter);

            AbnoFlavorText = new LobAbnormalityFlavourText();
            UIRoot.Append(AbnoFlavorText);
            AbnoFlavorText.Left.Set(250, 0f);
            AbnoFlavorText.Top.Set(50, 0f);
            AbnoFlavorText.ChangeLength(400);

            Append(UIRoot);
        }

        private void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
        {
            uiElement.Left.Set(left, 0f);
            uiElement.Top.Set(top, 0f);
            uiElement.Width.Set(width, 0f);
            uiElement.Height.Set(height, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Main.playerInventory)
            {
                ModContent.GetInstance<LobWorkSystem>().UnloadWorkUI();

                //Main.NewText("Unloaded!");
            }

            if (Main.gameInactive)
                return;

            AbnoFlavorText.UpdateProgress(EnkephalinBar.CurrentEnkephalin);
            AbnoFlavorText.Update(gameTime);

            if (EnkephalinBar.IsFull())
                return;

            GearWork.Update(gameTime);
            WorkTimer.Update(gameTime);

            //TimeElapsed++;
            if (WorkTimer.CurrentTime <= 0)//TimeElapsed % 200 == 0)
            {
                if (CheckSelectedWork(currentWorkType))
                    EnkephalinBar.AddPEBox();
                else
                    EnkephalinBar.AddNEBox();
                UpdateChoices();
            }
        }

        public void StartTest()
        {
            TimeElapsed = 0;
            //EnkephalinBar.Reinitialize(25);
            AbnoFlavorText.StartSlideOut();
        }

        public void CloseTest()
        {
            AbnoFlavorText.ResetTimer();
        }

        public void ApplyData(AbnormalityData data, WorkType type = WorkType.Instinct)
        {
            currentWorkType = type;

            Abnormality = data;
            //AbnoName.SetAbnoName(data.AbnormalityName);
            AbnoImage.SetImage(data.Image);
            AbnoFlavorText.upcomingFlavourList = data.GetFlavourText;

            WorkTimer.SetTimer(data.MaximumWorkTime);

            EnkephalinBar.Reinitialize(data.MaximumBoxes, 500);

            GearWork.SetWorkType(type);
            AbnoFlavorText.ChangeFlavourText(0);
            UpdateChoices();
        }

        public UIPanel LobPanelCreate(int BorderType, bool Borderless = false)
        {
            Asset<Texture2D> border;// = LobWorkSystem.LobPanelBorder1;
            Asset<Texture2D> background;// = LobWorkSystem.LobPanelBackground1;
            switch (BorderType)
            {
                case 0:
                    border = LobWorkSystem.LobPanelBorder1;
                    background = LobWorkSystem.LobPanelBackground1;
                    break;
                case 1:
                    border = LobWorkSystem.LobPanelBorder2;
                    background = LobWorkSystem.LobPanelBackground2;
                    break;
                case 2:
                    border = LobWorkSystem.LobPanelBorder3;
                    background = LobWorkSystem.LobPanelBackground3;
                    break;
                default:
                    border = LobWorkSystem.LobPanelBorder4;
                    background = LobWorkSystem.LobPanelBackground4;
                    break;
            }
            UIPanel lobPanel = new UIPanel(background, border);
            lobPanel.BackgroundColor = Color.White;
            lobPanel.BorderColor = LobYellow;
            if (Borderless)
                lobPanel.BorderColor = Color.Black;

            return lobPanel;
        }
    
        private void CreateChoices(UIPanel root, int rootLength, int rootHeight)
        {
            int padding = 10;

            int panelHeight = rootHeight / 2 - padding * 2 - 4;
            int panelLength = rootLength / 2 - padding * 2 - 4;

            for (int i = 0; i < 4; i++)
            {
                WorkChoice[i] = LobWorkChoiceUI.LobWorkChoiceCreate(3);
                int x = padding;
                int y = padding;

                if (i % 2 == 1)
                    x += padding + panelLength;
                if (i >= 2)
                    y += padding + panelHeight;

                SetRectangle(WorkChoice[i], x, y, panelLength, panelHeight);
                WorkChoice[i].OnLeftClick += WorkButtonClicked;
                root.Append(WorkChoice[i]);
            }
        }

        private void WorkButtonClicked(UIMouseEvent evt, UIElement listentingElement)
        {
            for (int i = 0; i < 4; i++)
            {
                if (evt.Target == WorkChoice[i])
                    WorkChoice[i].SelectWorkButton(true);
                else
                    WorkChoice[i].SelectWorkButton(false);
            }
        }

        private bool CheckSelectedWork(WorkType currentWorktype)
        {
            for (int i = 0; i < 4; i++)
            {
                if (WorkChoice[i].IsSelected && WorkChoice[i].ButtonWorkType == currentWorktype)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateChoices()
        {
            for (int i = 0; i < 4; i++)
            {
                WorkChoice[i].SetRandomTextType((WorkType)Main.rand.Next(4));
                WorkChoice[i].SelectWorkButton(false);
            }
        }
    }

    public class LobWorkChoiceUI : UIPanel
    {
        string displayedText;

        bool selected;
        public bool IsSelected { get { return selected; } }

        bool isHighlighted;

        WorkType currentWork;
        public WorkType ButtonWorkType { get { return currentWork; } }

        public LobWorkChoiceUI(Asset<Texture2D> customBackground, Asset<Texture2D> customborder, int customCornerSize = 12, int customBarSize = 4) : base(customBackground, customborder, customCornerSize, customBarSize)
        {
            selected = false;
        }

        public void SelectWorkButton(bool Selected)
        {
            selected = Selected;
            if (selected)
                BorderColor = LobOrange;
            else
                BorderColor = LobYellow;
        }

        public void SetRandomTextType(WorkType work)
        {
            string[] text = GetWorkProccessText(work);
            SetTextTo(work, text[Main.rand.Next(text.Length)], false);
        }

        public void SetTextTo(WorkType work, string text, bool highlighted)
        {
            currentWork = work;
            displayedText = text;
            isHighlighted = highlighted;
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            BorderColor = LobOrange;
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            BorderColor = LobYellow;
            if (selected)
                BorderColor = LobOrange;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (displayedText != null)
            {
                DrawFlavourText(spriteBatch);
            }
        }

        private void DrawFlavourText(SpriteBatch sp)
        {
            CalculatedStyle dimensions = GetInnerDimensions();
            Vector2 position = dimensions.Position();
            int width = (int)(dimensions.Width * 0.75);
            int height = (int)(dimensions.Height * 0.75);
            position.X += dimensions.Width / 2;
            position.Y += dimensions.Height / 2;

            string currentText = displayedText;
            DynamicSpriteFont value = FontAssets.MouseText.Value;
            Vector2 vector = value.MeasureString(currentText);
            Color baseColor = LobOrange;
            Vector2 origin = vector / 2;
            Vector2 baseScale = new Vector2(1f);

            TextSnippet[] snippets = ChatManager.ParseMessage(currentText, baseColor).ToArray();
            ChatManager.ConvertNormalSnippets(snippets);
            //ChatManager.DrawColorCodedString(sp, value, snippets, position, Color.White, 0f, origin, baseScale, out var _, width);
            LobAbnormalityFlavourText.CreateCenteredString(sp, value, snippets, position, baseColor, baseScale, width, height);
        }

        public static LobWorkChoiceUI LobWorkChoiceCreate(int BorderType, WorkType workType = WorkType.None)
        {
            Asset<Texture2D> border;// = LobWorkSystem.LobPanelBorder1;
            Asset<Texture2D> background;// = LobWorkSystem.LobPanelBackground1;
            switch (BorderType)
            {
                case 0:
                    border = LobPanelBorder1;
                    background = LobPanelBackground1;
                    break;
                case 1:
                    border = LobPanelBorder2;
                    background = LobPanelBackground2;
                    break;
                case 2:
                    border = LobPanelBorder3;
                    background = LobPanelBackground3;
                    break;
                default:
                    border = LobPanelBorder4;
                    background = LobPanelBackground4;
                    break;
            }
            LobWorkChoiceUI lobPanel = new LobWorkChoiceUI(background, border);
            lobPanel.BackgroundColor = Color.White;
            lobPanel.BorderColor = LobYellow;
            lobPanel.SetTextTo(WorkType.None, "", false);

            return lobPanel;
        }
    }

    public class LobAbnormalityNameUI : UIElement
    {
        private string nameDisplay;
        private float textScale = 1f;
        private Vector2 textSize = Vector2.Zero;

        private Color mainColor = Color.White;
        private Color backColor = Color.Black;

        private bool isWrapped;

        public string Text => nameDisplay;

        public LobAbnormalityNameUI()
        {
            nameDisplay = "Nihil";
        }

        public void SetAbnoName(string AbnoName)
        {
            nameDisplay = AbnoName;
        }
    }

    public class LobAbnormalityFlavourText : UIElement
    {
        //private List<string> upcomingFlavourList;
        private List<string> flavourList;

        private int currentFlavourText;
        private Vector2 textScale;

        private float _textScale = 1f;

        private int timer;
        private int timerMax = 300;

        private int progress;
        private bool special;

        private int slideDirection;

        private float slideAnimation;

        public LobAbnormalityFlavourText()
        {
            flavourList = new List<string>();
            timer = 300;
            slideDirection = 1;
            slideAnimation = 0;
            Height.Set(200, 0f);
            currentFlavourText = 0;
            flavourList.Add("This is a test message");
            flavourList.Add("This is a longer test message to check how long shit can go");
        }

        public override void Update(GameTime gameTime)
        {
            if (slideDirection > 0)
            {
                slideAnimation = MathHelper.Lerp(slideAnimation, 1f, 0.25f);
                if (slideAnimation > 0.98f)
                {
                    slideAnimation = 1f;
                    slideDirection = 0;
                }
            }
            else if (slideDirection < 0)
            {
                slideAnimation = MathHelper.Lerp(slideAnimation, 0f, 0.25f);
                if (slideAnimation < 0.02f)
                {
                    slideAnimation = 1f;
                    slideDirection = 0;
                }
            }

            if (slideDirection != 0)
                return;
            timer--;
            if (timer < 0)
            {
                ChangeFlavourText(progress, special);
                ResetTimer();
            }
        }

        public void UpdateProgress(int currentProg, bool Special = false)
        {
            progress = currentProg;
            special = Special;
        }

        public void ResetTimer()
        {
            timer = timerMax;
        }

        // Play animation to make it slide to the right
        public void StartSlideOut()
        {
            slideDirection = 1;
            slideAnimation = 0;
        }

        // Play animation to make it slide to the left
        public void StartSlideIn()
        {
            slideDirection = -1;
            slideAnimation = 1f;
        }

        // Use to initialize first Abno text length, might not be used at all
        public void LoadTextList(List<string> list)
        {
            flavourList = list;
            timer = 300;

            DynamicSpriteFont value = FontAssets.MouseText.Value;
            int length = 0;
            for (int i = 0; i < list.Count; i++)
            {
                int currentLength = (int)value.MeasureString(list[i]).X;
                if (currentLength > length)
                    length = currentLength;
            }
            ChangeLength(length);
        }

        public void ChangeLength(int length)
        {
            length -= length % 64;
            length += 64;

            Width.Set(length, 0f);
        }

        public delegate List<string> NextList(int progress, bool SpecialWorkType);
        public NextList upcomingFlavourList;

        public void ChangeFlavourText(int i = 0, bool special = false)
        {
            List<string> newList = upcomingFlavourList(i, special);
            flavourList = newList;

            currentFlavourText = Main.rand.Next(flavourList.Count);
        }

        private void DrawFlavourText(SpriteBatch sp)
        {
            CalculatedStyle dimensions = GetInnerDimensions();
            Vector2 position = dimensions.Position();
            int width = (int)(dimensions.Width * 0.75);
            int height = (int)(dimensions.Height * 0.75);
            position.X += dimensions.Width / 2;
            position.Y += dimensions.Height / 2;

            float TextOpacity = 1f;
            if (timer > timerMax - 60)
            {
                TextOpacity = (timerMax - timer) / 60f;
            }
            else if (timer <= 60)
            {
                TextOpacity = timer / 60f;
            }

            string currentText = flavourList[currentFlavourText];
            DynamicSpriteFont value = FontAssets.MouseText.Value;
            Vector2 vector = value.MeasureString(currentText);
            Color baseColor = LobOrange * TextOpacity;
            Vector2 origin = vector / 2;
            Vector2 baseScale = new Vector2(1f);

            TextSnippet[] snippets = ChatManager.ParseMessage(currentText, baseColor).ToArray();
            ChatManager.ConvertNormalSnippets(snippets);
            //ChatManager.DrawColorCodedString(sp, value, snippets, position, Color.White, 0f, origin, baseScale, out var _, width);
            CreateCenteredString(sp, value, snippets, position, baseColor, baseScale, width, width);
        }

        public static void CreateCenteredString(SpriteBatch sp, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 centerPoint, Color color, Vector2 baseScale, float maxWidth, float maxHeight)
        {
            // Initial values :V
            float heightText = 0;
            float lengthText = 0;
            //float x = font.MeasureString(" ").X;
            List<string> BatchDraw = new List<string>();
            string currentBatch = "";

            float snippetScale = 1f;
            for (int i = 0; i < snippets.Length; i++)
            {
                // Load current Snippet
                TextSnippet textSnippet = snippets[i];
                string text = textSnippet.Text;
                textSnippet.Update();

                snippetScale = textSnippet.Scale;

                // Split textSnippets
                string[] array = textSnippet.Text.Split('\n');
                array = Regex.Split(textSnippet.Text, "(\n)");
                bool flag = true;
                string[] array2 = array;
                foreach (string obj in array2)
                {
                    string[] array3 = Regex.Split(obj, "( )");
                    array3 = obj.Split(' ');

                    for (int k = 0; k < array3.Length; k++)
                    {
                        string Text = array3[k];
                        // If MaxWidth has length
                        if (maxWidth > 0f)
                        {
                            // Measure current snippet's length
                            float currentSnippetLength = font.MeasureString(Text).X * baseScale.X * snippetScale;

                            // Get current sentence length, if its going to be higher than max length
                            // Add current sentence to draw for later, increase text height size
                            // Reset current sentence and length
                            if (lengthText + currentSnippetLength > maxWidth)
                            {
                                BatchDraw.Add(currentBatch);
                                lengthText = 0;
                                currentBatch = string.Empty;
                                heightText += (float)font.LineSpacing * snippetScale * baseScale.Y;
                            }
                        }
                        if (lengthText > 0)
                        {
                            currentBatch += " ";
                        }

                        // Measure current snippet and add it to current sentence and its length
                        Vector2 snippetSize = font.MeasureString(Text);
                        lengthText += snippetSize.X * baseScale.X * snippetScale;
                        currentBatch += Text;

                        if (k == array3.Length - 1)
                        {
                            BatchDraw.Add(currentBatch);
                            heightText += (float)font.LineSpacing * snippetScale * baseScale.Y;
                        }
                    }
                }
            }

            // Draw each line in BatchDraw one by one
            for (int i = 0; i < BatchDraw.Count; i++)
            {
                string draw = BatchDraw[i];
                
                // Get Values for drawing
                Vector2 position = centerPoint;
                Vector2 size = font.MeasureString(draw);
                Vector2 origin = new Vector2(0.5f, 0f) * size;

                // Get length for current scale, get individual Height for each text
                float height = heightText / BatchDraw.Count;
                //
                position.Y -= (heightText / 2);
                position.Y += height * i;
                sp.DrawString(font, draw, position, color, 0f, origin, baseScale * snippetScale, SpriteEffects.None, 0f);
            }
            //sp.DrawString(font, "test", centerPoint, color, 0f, Vector2.Zero, 1f, 0, 0);
        }

        private void DrawBackground(SpriteBatch sp)
        {
            Texture2D uiTex = LobWorkSystem.LobotomyUIGetTexture;

            // Frame for the slashes Top and bottom border, the /// parts
            Rectangle borderFrame = new Rectangle(320, 424, 64, 24);

            CalculatedStyle dimensions = GetDimensions();
            int borderHeight = borderFrame.Height;

            int topBorder = (int)dimensions.Y;
            int bottomBorder = topBorder + (int)dimensions.Height - borderHeight;

            int currentLength = (int)(dimensions.Width * slideAnimation);

            // Draw the Top and Bottom part
            int lengthForLoop = currentLength;

            // Draw Expand Button (does not work)
            Rectangle ArrowFrame = new Rectangle(480, 384, 10, 24);
            Rectangle ArrowDestination = new Rectangle((int)dimensions.X + currentLength, topBorder, ArrowFrame.Width, ArrowFrame.Height);
            sp.Draw(uiTex, ArrowDestination, ArrowFrame, LobOrange);
            ArrowFrame.Y = 424;
            ArrowDestination.Y = bottomBorder;
            sp.Draw(uiTex, ArrowDestination, ArrowFrame, LobOrange);
            ArrowFrame.Height = 12;
            ArrowDestination.Y = topBorder + borderHeight;
            ArrowDestination.Height = (int)dimensions.Height - borderHeight * 2;
            sp.Draw(uiTex, ArrowDestination, ArrowFrame, LobOrange);
            ArrowFrame.Y = 410;
            ArrowDestination.Height = ArrowFrame.Height;
            ArrowDestination.Y = topBorder + (int)dimensions.Height / 2;
            sp.Draw(uiTex, ArrowDestination, ArrowFrame, LobOrange);

            if (lengthForLoop == 0)
                return;
            do
            {
                lengthForLoop -= borderFrame.Width;
                int TopLeft = (int)dimensions.X + lengthForLoop;
                if (lengthForLoop < 0)
                {
                    borderFrame.X -= lengthForLoop;
                    borderFrame.Width += lengthForLoop;
                    TopLeft = (int)dimensions.X;
                }
                // Top Border
                Rectangle targetFrame = new Rectangle(TopLeft, topBorder, borderFrame.Width, borderHeight);
                sp.Draw(uiTex, targetFrame, borderFrame, LobOrange);

                // Bottom Border
                targetFrame.Y = bottomBorder;
                sp.Draw(uiTex, targetFrame, borderFrame, LobOrange);
            } while (lengthForLoop > 0);

            // Draw Main Fill
            Rectangle destinationFrame = new Rectangle((int)dimensions.X, topBorder + borderHeight, currentLength, (int)(dimensions.Height - borderHeight * 2));
            sp.Draw(uiTex, destinationFrame, new Rectangle(0, 0, 1, 1), Color.White);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            
            DrawFlavourText(spriteBatch);
        }
    }

    public class LobQlipothUI : UIElement
    {
        private int currentCounter;

        public float ImageScale = 1f;

        /// <summary>
        /// -2 for ?, -1 for X
        /// </summary>
        /// <param name="time"></param>
        public LobQlipothUI(int time = 0)
        {
            currentCounter = -1;
            Width.Set(72, 0f);
            Height.Set(72, 0f);
        }

        public void SetCounter(int time)
        {
            currentCounter = time;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Texture2D texSheet = LobWorkSystem.LobotomyUIGetTexture;

            Rectangle destination = dimensions.ToRectangle();
            Rectangle counterFrame = new Rectangle(218, 26, 76, 76);
            spriteBatch.Draw(texSheet, destination, counterFrame, Color.White);

            destination.X += destination.Width/2 - 14;
            destination.Y += destination.Height/2 - 14;
            destination.Width = 28;
            destination.Height = 28;
            Rectangle numberFrame = new Rectangle(320, 0, 28, 28);
            if (currentCounter == -1)
            {
                numberFrame.Y += 32;
            }
            else if (currentCounter == -2)
            {
                numberFrame.X += 32;
                numberFrame.Y += 32;
            }
            else
            {
                numberFrame.X += 32 * currentCounter;
            }

            spriteBatch.Draw(texSheet, destination, numberFrame, LobYellow);
        }
    }

    public class LobWorkTimerUI : UIElement
    {
        private int maxTime;
        private int timer;
        public int CurrentTime { get { return timer; } }

        public void SetTimer(int MaximumTime)
        {
            maxTime = MaximumTime;
            TimerOver();
        }

        public void LinkTimertoHandler()
        {

        }

        public override void Update(GameTime gameTime)
        {
            timer--;
            if (timer < 0)
            {
                TimerOver();
            }
        }

        private void TimerOver()
        {
            timer = maxTime;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Texture2D texSheet = LobWorkSystem.LobotomyUIGetTexture;

            Rectangle destination = dimensions.ToRectangle();
            Rectangle counterFrame = new Rectangle(192, 128, 128, 128);

            spriteBatch.Draw(texSheet, new Rectangle((int)(dimensions.X + dimensions.Width / 2 - counterFrame.Width / 2), (int)(dimensions.Y + dimensions.Height / 2 - counterFrame.Height / 2), counterFrame.Width, counterFrame.Height), counterFrame, Color.White);

            Rectangle numberFrame = new Rectangle(320, 64, 32, 32);

            int Time = (int)Math.Floor((maxTime - timer) / (maxTime / 9f));
            numberFrame.X += 32 * Time;

            spriteBatch.Draw(texSheet, destination, numberFrame, Color.White);
        }
    }

    public class LobEnkephalinBar : UIElement
    {
        private int TotalE;
        private int TotalPE;
        private int TotalNE;

        private int MaximumPEBox;
        public int CurrentEnkephalin => TotalE;

        public LobEnkephalinBar()
        {
            Reinitialize(10);
        }

        public void Reinitialize(int MaxBox, int MaximumHeight = 500)
        {
            MaximumPEBox = MaxBox;
            TotalPE = 0;
            TotalNE = 0;
            TotalE = 0;

            int height = MaxBox * 20;
            if (MaximumHeight < height)
                height = MaximumHeight;

            Height.Set(height, 0f);
        }

        public void AddPEBox()
        {
            TotalPE++;
            TotalE++;
        }

        public void AddNEBox()
        {
            TotalNE++;
            TotalE++;
        }

        public bool IsFull()
        {
            return TotalE == MaximumPEBox;
        }

        private void DrawBar(SpriteBatch sp)
        {
            Texture2D tex = LobotomyUIGetTexture;

            CalculatedStyle dimensions = GetDimensions();
            Vector2 position = dimensions.Position();

            Rectangle TopFrame = new Rectangle(0, 256, 96, 64);
            Rectangle BodyFrame = new Rectangle(0, 322, 96, 124);
            Rectangle BottomFrame = new Rectangle(0, 480, 96, 32);
            Rectangle PEBox = new Rectangle(128, 384, 28, 20);
            Rectangle NEBox = new Rectangle(160, 384, 28, 20);

            sp.Draw(tex, new Rectangle((int)position.X, (int)position.Y, (int)dimensions.Width, TopFrame.Height), TopFrame, Color.White);

            int topHeight = TopFrame.Height;
            int bodyHeight = (int)dimensions.Height - topHeight;
            position.Y += topHeight;
            sp.Draw(tex, new Rectangle((int)position.X, (int)position.Y, (int)dimensions.Width, bodyHeight), BodyFrame, Color.White);

            int padding = 8;
            int trueBarHeight = (int)dimensions.Height;
            trueBarHeight -= padding;
            position.Y = dimensions.Y + padding;
            int boxHeight = trueBarHeight / (MaximumPEBox);
            Rectangle boxLocation = new Rectangle((int)position.X + 34, (int)position.Y, NEBox.Width, boxHeight);

            // Draw Negative Boxes, Up to Bottom
            if (TotalNE > 0)
                for (int i = 0; i < TotalNE; i++)
                {
                    sp.Draw(tex, boxLocation, NEBox, Color.White);
                    boxLocation.Y += boxHeight;
                }

            boxLocation.Y = (int)(position.Y + boxHeight * (MaximumPEBox - 1));
            // Draw Positive Boxes, Bottom to Up
            if (TotalPE > 0)
                for (int i = 0; i < TotalPE; i++)
                {
                    sp.Draw(tex, boxLocation, PEBox, Color.White);
                    boxLocation.Y -= boxHeight;
                }

            int BottomBarPosition = (int)(position.Y + boxHeight * MaximumPEBox);
            sp.Draw(tex, new Rectangle((int)position.X, (int)BottomBarPosition, (int)dimensions.Width, BottomFrame.Height), BottomFrame, Color.White);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            DrawBar(spriteBatch);
        }
    }

    public class LobGearImageUI : UIElement
    {
        WorkType workType;
        float gearRotation;
        public override void Update(GameTime gameTime)
        {
            gearRotation += 1f;
            base.Update(gameTime);
        }

        public LobGearImageUI(int x, int y)
        {
            workType = WorkType.Instinct;
            Left.Set(x, 0f);
            Top.Set(y, 0f);
            Width.Set(50, 0f);
            Height.Set(50, 0f);
        }

        public void SetWorkType(WorkType Type)
        {
            workType = Type;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Texture2D texSheet = LobWorkSystem.LobotomyUIGetTexture;

            Rectangle WorkFrame = new Rectangle(320, 576, 50, 50);
            Rectangle GearFrame = new Rectangle(352, 160, 192, 192);
            WorkFrame.X += 64 * (int)workType;

            float framePercent = (float)dimensions.Width / (float)WorkFrame.Width;

            Vector2 position = dimensions.Position() + new Vector2(dimensions.Width / 2, dimensions.Height / 2);
            Vector2 origin = GearFrame.Size() / 2;

            spriteBatch.Draw(texSheet, position, GearFrame, Color.White, MathHelper.ToRadians(gearRotation * 0.5f), origin, framePercent, 0, 0);

            spriteBatch.Draw(texSheet, dimensions.ToRectangle(), WorkFrame, Color.White);
        }
    }

    public class LobPanelTest : UIElement
    {
        private int cornerType;
        private bool borderLess;

        private Color panelColor;

        /// <summary>
        /// Corner type is bends. Default nu, it might get fucky fucky idk
        /// </summary>
        /// <param name="size"></param>
        public LobPanelTest(int CornerType, bool Borderless = false, Color Color = default)
        {
            cornerType = CornerType;
            borderLess = Borderless;
            if (Color == default);
                panelColor = Color.White;
            panelColor = Color;
        }

        private void DrawPanel(SpriteBatch spriteBatch, Color color)
        {
            CalculatedStyle dimensions = GetDimensions();
            Rectangle frame = new Rectangle(0, 0, 14, 14);
            int cornerSize = 4;

            switch (cornerType)
            {
                case 1:
                    cornerSize = 6;
                    frame.X = 12;
                    break;
                case 2:
                    cornerSize = 8;
                    frame.Y = 14;
                    break;
                case 3:
                    cornerSize = 10;
                    frame.X = 28;
                    break;
                case 4:
                    cornerSize = 12;
                    frame.X = 52;
                    break;
            }
            if (borderLess)
                frame.X += 160;
            frame.Width = cornerSize;
            frame.Height = cornerSize;

            Point Base = new Point((int)dimensions.X, (int)dimensions.Y);
            Point Edge = new Point(Base.X + (int)dimensions.Width - cornerSize, Base.Y + (int)dimensions.Height - cornerSize);
            int width = Edge.X - Base.X - cornerSize;
            int height = Edge.Y - Base.Y - cornerSize;

            for (int i = 0; i < 4; i++)
            {
                // Draw corners from Upper Left, Upper Right, Lower Left, Lower Right
                Rectangle destinationCorner = new Rectangle(Base.X, Base.Y, cornerSize, cornerSize);
                Rectangle frameCorner = frame;
                if (i % 2 == 1)
                {
                    destinationCorner.X = Edge.X;
                    frameCorner.X += 2 + cornerSize;
                }
                if (i > 1)
                {
                    destinationCorner.Y = Edge.Y;
                    frameCorner.Y += 2 + cornerSize;
                }
                spriteBatch.Draw(LobWorkSystem.LobotomyUIGetTexture, destinationCorner, frameCorner, color);

                // Draw edges from Up, Right, Down, Left
                Rectangle destinationBorder = new Rectangle(destinationCorner.X, destinationCorner.Y, width, cornerSize);
                Rectangle frameBorder = new Rectangle(frameCorner.X + cornerSize, frameCorner.Y, 2, cornerSize);
                if (i % 2 == 1)
                {
                    destinationBorder.Width = cornerSize;
                    destinationBorder.Height = cornerSize;

                    frameBorder.Width = cornerSize;
                    frameBorder.Height = 2;
                }
                if (i == 1)
                {
                    frameBorder.X += 2;
                    frameBorder.Y += cornerSize;
                }
                if (i == 2)
                {
                    frameBorder.Y += cornerSize + 2;
                }
                if (i == 3)
                {
                    frameBorder.X -= cornerSize;
                    frameBorder.Y += cornerSize;
                }
                spriteBatch.Draw(LobWorkSystem.LobotomyUIGetTexture, destinationBorder, frameBorder, color);
            }
            //Draw background
            Rectangle destinationBase = new Rectangle(Base.X + cornerSize, Base.Y + cornerSize, width, height);
            Rectangle frameBase = new Rectangle(0, 0, 1, 1);
            spriteBatch.Draw(LobWorkSystem.LobotomyUIGetTexture, destinationBase, frameBase, color);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            DrawPanel(spriteBatch, panelColor);
        }
    }
}