using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Chat;
using LobotomyCorp;
using LobotomyCorp.Utils;
using LobotomyCorp.UI;
using System.Collections.Generic;

namespace LobotomyCorp.Utils
{
    public class SkeletonBase
    {
        //For General Use
        //Unsure How good this is? compared to something that exists out there, probly more fun though
        public Dictionary<string, BonePart> BoneName;

        public SkeletonBase(Dictionary<string, BonePart> BoneList)
        {
            BoneName = BoneList;
        }

        public virtual void Record()
        {
            foreach (BonePart bp in BoneName.Values)
            {
                bp.Record();
            }
        }

        public virtual void Distance()
        {

        }

        public Vector2 ElbowIK(Vector2 startPoint, Vector2 endPoint, float length1, float length2, int dir = 1)
        {
            Vector2 elbow = startPoint;
            float Dist = Vector2.Distance(endPoint, startPoint);
            if (Dist > length1 + length2)
                Dist = length1 + length2;
            float Angle = (float)Math.Acos(Dist * Dist / ((length1 + length2) * Dist));
            float Rotation = (endPoint - elbow).ToRotation() + Angle * dir;
            elbow += new Vector2(length1, 0).RotatedBy(Rotation);
            return elbow;
        }
    }

    public class BonePart
    {
        Vector2[] Offset;
        bool InheritOffset;
        float[] Rotation;
        bool InheritRotation;
        float[] Scale;
        bool InheritScale;

        float Length;

        BonePart Parent;

        private Texture2D Texture;
        private Rectangle Frame;
        private Vector2 Origin;
        public bool Visible;

        public BonePart(Vector2 initOffset, float initRot, float initScale, float length, BonePart BoneParent = null, int oldRecord = 1)
        {
            Offset = new Vector2[oldRecord];
            Rotation = new float[oldRecord];
            Scale = new float[oldRecord];

            Offset[0] = initOffset;
            Rotation[0] = initRot;
            Scale[0] = initScale;
            Length = length;

            if (BoneParent != null)
            {
                Parent = BoneParent;
                InheritOffset = true;
                InheritRotation = true;
                InheritScale = true;
            }
            else
            {
                InheritOffset = false;
                InheritRotation = false;
                InheritScale = false;
            }
        }

        public void Record()
        {
            for (int i = Offset.Length - 1; i > 0; i--)
            {
                if (i > 0)
                {
                    if (Offset[i - 1] != null)
                        Offset[i] = Offset[i - 1];

                    Rotation[i] = Rotation[i - 1];
                    Scale[i] = Scale[i - 1];
                }
            }
        }

        public void ChangeIOffset(bool To)
        {
            if (To != InheritOffset)
            {
                Vector2 newPos;
                if (!To)
                    newPos = GetPosition();
                else
                    newPos = Parent.EndPoint() - GetPosition();
                Offset[0] = newPos;
                InheritOffset = To;
            }
        }

        public void ChangeIRotation(bool To)
        {
            InheritRotation = To;
        }

        public void ChangeIScale(bool To)
        {
            if (To != InheritScale)
            {
                float newScale;
                if (!To)
                    newScale = GetScale();
                else
                    newScale = GetScale() / Parent.GetScale();
                Scale[0] = newScale;
                InheritScale = To;
            }
        }

        public Vector2 GetPosition(int dir = 1, int i = 0)
        {
            Vector2 offset = new Vector2(Offset[i].X, Offset[i].Y * dir) * GetScale(i);
            if (InheritOffset)
                return offset.RotatedBy(Parent.GetRotation(dir, i)) + Parent.EndPoint(dir, i);
            return offset;
        }

        public Vector2 EndPoint(int dir = 1, int i = 0)
        {
            return GetPosition(dir, i) + new Vector2(Length * GetScale(i), 0).RotatedBy(GetRotation(dir, i));
        }

        public float GetRotation(int dir = 1, int i = 0)
        {
            Vector2 vec1 = new Vector2(1, 0).RotatedBy(Rotation[i]);
            vec1.X *= dir;
            if (InheritRotation)
                return vec1.ToRotation() + Parent.GetRotation();
            return vec1.ToRotation();
        }

        public float GetScale(int i = 0)
        {
            if (InheritScale)
                return Scale[i] * Parent.GetScale(i);
            return Scale[i];
        }

        public void SetDraw(Texture2D boneTexture, Rectangle texFrame, Vector2 texOrigin)
        {
            Texture = boneTexture;
            Frame = texFrame;
            Origin = texOrigin;
        }

        public void DrawBone(SpriteBatch sb, int dir = 1, int Trail = 0)
        {
            sb.Draw(Texture,
                    GetPosition(dir, Trail) - Main.screenPosition,
                    Frame,
                    Color.White,
                    GetRotation(dir, Trail),
                    Origin,
                    Scale[Trail],
                    dir < 0 ? SpriteEffects.FlipHorizontally : 0f,
                    0f);
        }
    }
}
