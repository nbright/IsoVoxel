﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IsoVoxel
{
    enum Outlining
    {
        Full, Light, Partial, None
    }
    enum Direction
    {
        SE, SW, NW, NE
    }
    enum OrthoDirection
    {
        S, W, N, E
    }

    public struct MagicaVoxelData
    {
        public byte x;
        public byte y;
        public byte z;
        public byte color;

        public MagicaVoxelData(BinaryReader stream, bool subsample)
        {
            x = stream.ReadByte(); //(byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
            y = stream.ReadByte(); //(byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
            z = stream.ReadByte(); //(byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
            color = stream.ReadByte();
        }
        public MagicaVoxelData(int x, int y, int z, int color)
        {
            this.x = (byte)x;
            this.y = (byte)y;
            this.z = (byte)z;
            this.color = (byte)color;
        }
    }
    class PaletteDraw
    {
        public static float[][] colors = new float[][]
        {
            new float[]{1.0F, 1.0F, 1.0F, 1.0F},
            new float[]{1.0F, 1.0F, 0.8F, 1.0F},
            new float[]{1.0F, 1.0F, 0.6F, 1.0F},
            new float[]{1.0F, 1.0F, 0.4F, 1.0F},
            new float[]{1.0F, 1.0F, 0.2F, 1.0F},
            new float[]{1.0F, 1.0F, 0.0F, 1.0F},
            new float[]{1.0F, 0.8F, 1.0F, 1.0F},
            new float[]{1.0F, 0.8F, 0.8F, 1.0F},
            new float[]{1.0F, 0.8F, 0.6F, 1.0F},
            new float[]{1.0F, 0.8F, 0.4F, 1.0F},
            new float[]{1.0F, 0.8F, 0.2F, 1.0F},
            new float[]{1.0F, 0.8F, 0.0F, 1.0F},
            new float[]{1.0F, 0.6F, 1.0F, 1.0F},
            new float[]{1.0F, 0.6F, 0.8F, 1.0F},
            new float[]{1.0F, 0.6F, 0.6F, 1.0F},
            new float[]{1.0F, 0.6F, 0.4F, 1.0F},
            new float[]{1.0F, 0.6F, 0.2F, 1.0F},
            new float[]{1.0F, 0.6F, 0.0F, 1.0F},
            new float[]{1.0F, 0.4F, 1.0F, 1.0F},
            new float[]{1.0F, 0.4F, 0.8F, 1.0F},
            new float[]{1.0F, 0.4F, 0.6F, 1.0F},
            new float[]{1.0F, 0.4F, 0.4F, 1.0F},
            new float[]{1.0F, 0.4F, 0.2F, 1.0F},
            new float[]{1.0F, 0.4F, 0.0F, 1.0F},
            new float[]{1.0F, 0.2F, 1.0F, 1.0F},
            new float[]{1.0F, 0.2F, 0.8F, 1.0F},
            new float[]{1.0F, 0.2F, 0.6F, 1.0F},
            new float[]{1.0F, 0.2F, 0.4F, 1.0F},
            new float[]{1.0F, 0.2F, 0.2F, 1.0F},
            new float[]{1.0F, 0.2F, 0.0F, 1.0F},
            new float[]{1.0F, 0.0F, 1.0F, 1.0F},
            new float[]{1.0F, 0.0F, 0.8F, 1.0F},
            new float[]{1.0F, 0.0F, 0.6F, 1.0F},
            new float[]{1.0F, 0.0F, 0.4F, 1.0F},
            new float[]{1.0F, 0.0F, 0.2F, 1.0F},
            new float[]{1.0F, 0.0F, 0.0F, 1.0F},
            new float[]{0.8F, 1.0F, 1.0F, 1.0F},
            new float[]{0.8F, 1.0F, 0.8F, 1.0F},
            new float[]{0.8F, 1.0F, 0.6F, 1.0F},
            new float[]{0.8F, 1.0F, 0.4F, 1.0F},
            new float[]{0.8F, 1.0F, 0.2F, 1.0F},
            new float[]{0.8F, 1.0F, 0.0F, 1.0F},
            new float[]{0.8F, 0.8F, 1.0F, 1.0F},
            new float[]{0.8F, 0.8F, 0.8F, 1.0F},
            new float[]{0.8F, 0.8F, 0.6F, 1.0F},
            new float[]{0.8F, 0.8F, 0.4F, 1.0F},
            new float[]{0.8F, 0.8F, 0.2F, 1.0F},
            new float[]{0.8F, 0.8F, 0.0F, 1.0F},
            new float[]{0.8F, 0.6F, 1.0F, 1.0F},
            new float[]{0.8F, 0.6F, 0.8F, 1.0F},
            new float[]{0.8F, 0.6F, 0.6F, 1.0F},
            new float[]{0.8F, 0.6F, 0.4F, 1.0F},
            new float[]{0.8F, 0.6F, 0.2F, 1.0F},
            new float[]{0.8F, 0.6F, 0.0F, 1.0F},
            new float[]{0.8F, 0.4F, 1.0F, 1.0F},
            new float[]{0.8F, 0.4F, 0.8F, 1.0F},
            new float[]{0.8F, 0.4F, 0.6F, 1.0F},
            new float[]{0.8F, 0.4F, 0.4F, 1.0F},
            new float[]{0.8F, 0.4F, 0.2F, 1.0F},
            new float[]{0.8F, 0.4F, 0.0F, 1.0F},
            new float[]{0.8F, 0.2F, 1.0F, 1.0F},
            new float[]{0.8F, 0.2F, 0.8F, 1.0F},
            new float[]{0.8F, 0.2F, 0.6F, 1.0F},
            new float[]{0.8F, 0.2F, 0.4F, 1.0F},
            new float[]{0.8F, 0.2F, 0.2F, 1.0F},
            new float[]{0.8F, 0.2F, 0.0F, 1.0F},
            new float[]{0.8F, 0.0F, 1.0F, 1.0F},
            new float[]{0.8F, 0.0F, 0.8F, 1.0F},
            new float[]{0.8F, 0.0F, 0.6F, 1.0F},
            new float[]{0.8F, 0.0F, 0.4F, 1.0F},
            new float[]{0.8F, 0.0F, 0.2F, 1.0F},
            new float[]{0.8F, 0.0F, 0.0F, 1.0F},
            new float[]{0.6F, 1.0F, 1.0F, 1.0F},
            new float[]{0.6F, 1.0F, 0.8F, 1.0F},
            new float[]{0.6F, 1.0F, 0.6F, 1.0F},
            new float[]{0.6F, 1.0F, 0.4F, 1.0F},
            new float[]{0.6F, 1.0F, 0.2F, 1.0F},
            new float[]{0.6F, 1.0F, 0.0F, 1.0F},
            new float[]{0.6F, 0.8F, 1.0F, 1.0F},
            new float[]{0.6F, 0.8F, 0.8F, 1.0F},
            new float[]{0.6F, 0.8F, 0.6F, 1.0F},
            new float[]{0.6F, 0.8F, 0.4F, 1.0F},
            new float[]{0.6F, 0.8F, 0.2F, 1.0F},
            new float[]{0.6F, 0.8F, 0.0F, 1.0F},
            new float[]{0.6F, 0.6F, 1.0F, 1.0F},
            new float[]{0.6F, 0.6F, 0.8F, 1.0F},
            new float[]{0.6F, 0.6F, 0.6F, 1.0F},
            new float[]{0.6F, 0.6F, 0.4F, 1.0F},
            new float[]{0.6F, 0.6F, 0.2F, 1.0F},
            new float[]{0.6F, 0.6F, 0.0F, 1.0F},
            new float[]{0.6F, 0.4F, 1.0F, 1.0F},
            new float[]{0.6F, 0.4F, 0.8F, 1.0F},
            new float[]{0.6F, 0.4F, 0.6F, 1.0F},
            new float[]{0.6F, 0.4F, 0.4F, 1.0F},
            new float[]{0.6F, 0.4F, 0.2F, 1.0F},
            new float[]{0.6F, 0.4F, 0.0F, 1.0F},
            new float[]{0.6F, 0.2F, 1.0F, 1.0F},
            new float[]{0.6F, 0.2F, 0.8F, 1.0F},
            new float[]{0.6F, 0.2F, 0.6F, 1.0F},
            new float[]{0.6F, 0.2F, 0.4F, 1.0F},
            new float[]{0.6F, 0.2F, 0.2F, 1.0F},
            new float[]{0.6F, 0.2F, 0.0F, 1.0F},
            new float[]{0.6F, 0.0F, 1.0F, 1.0F},
            new float[]{0.6F, 0.0F, 0.8F, 1.0F},
            new float[]{0.6F, 0.0F, 0.6F, 1.0F},
            new float[]{0.6F, 0.0F, 0.4F, 1.0F},
            new float[]{0.6F, 0.0F, 0.2F, 1.0F},
            new float[]{0.6F, 0.0F, 0.0F, 1.0F},
            new float[]{0.4F, 1.0F, 1.0F, 1.0F},
            new float[]{0.4F, 1.0F, 0.8F, 1.0F},
            new float[]{0.4F, 1.0F, 0.6F, 1.0F},
            new float[]{0.4F, 1.0F, 0.4F, 1.0F},
            new float[]{0.4F, 1.0F, 0.2F, 1.0F},
            new float[]{0.4F, 1.0F, 0.0F, 1.0F},
            new float[]{0.4F, 0.8F, 1.0F, 1.0F},
            new float[]{0.4F, 0.8F, 0.8F, 1.0F},
            new float[]{0.4F, 0.8F, 0.6F, 1.0F},
            new float[]{0.4F, 0.8F, 0.4F, 1.0F},
            new float[]{0.4F, 0.8F, 0.2F, 1.0F},
            new float[]{0.4F, 0.8F, 0.0F, 1.0F},
            new float[]{0.4F, 0.6F, 1.0F, 1.0F},
            new float[]{0.4F, 0.6F, 0.8F, 1.0F},
            new float[]{0.4F, 0.6F, 0.6F, 1.0F},
            new float[]{0.4F, 0.6F, 0.4F, 1.0F},
            new float[]{0.4F, 0.6F, 0.2F, 1.0F},
            new float[]{0.4F, 0.6F, 0.0F, 1.0F},
            new float[]{0.4F, 0.4F, 1.0F, 1.0F},
            new float[]{0.4F, 0.4F, 0.8F, 1.0F},
            new float[]{0.4F, 0.4F, 0.6F, 1.0F},
            new float[]{0.4F, 0.4F, 0.4F, 1.0F},
            new float[]{0.4F, 0.4F, 0.2F, 1.0F},
            new float[]{0.4F, 0.4F, 0.0F, 1.0F},
            new float[]{0.4F, 0.2F, 1.0F, 1.0F},
            new float[]{0.4F, 0.2F, 0.8F, 1.0F},
            new float[]{0.4F, 0.2F, 0.6F, 1.0F},
            new float[]{0.4F, 0.2F, 0.4F, 1.0F},
            new float[]{0.4F, 0.2F, 0.2F, 1.0F},
            new float[]{0.4F, 0.2F, 0.0F, 1.0F},
            new float[]{0.4F, 0.0F, 1.0F, 1.0F},
            new float[]{0.4F, 0.0F, 0.8F, 1.0F},
            new float[]{0.4F, 0.0F, 0.6F, 1.0F},
            new float[]{0.4F, 0.0F, 0.4F, 1.0F},
            new float[]{0.4F, 0.0F, 0.2F, 1.0F},
            new float[]{0.4F, 0.0F, 0.0F, 1.0F},
            new float[]{0.2F, 1.0F, 1.0F, 1.0F},
            new float[]{0.2F, 1.0F, 0.8F, 1.0F},
            new float[]{0.2F, 1.0F, 0.6F, 1.0F},
            new float[]{0.2F, 1.0F, 0.4F, 1.0F},
            new float[]{0.2F, 1.0F, 0.2F, 1.0F},
            new float[]{0.2F, 1.0F, 0.0F, 1.0F},
            new float[]{0.2F, 0.8F, 1.0F, 1.0F},
            new float[]{0.2F, 0.8F, 0.8F, 1.0F},
            new float[]{0.2F, 0.8F, 0.6F, 1.0F},
            new float[]{0.2F, 0.8F, 0.4F, 1.0F},
            new float[]{0.2F, 0.8F, 0.2F, 1.0F},
            new float[]{0.2F, 0.8F, 0.0F, 1.0F},
            new float[]{0.2F, 0.6F, 1.0F, 1.0F},
            new float[]{0.2F, 0.6F, 0.8F, 1.0F},
            new float[]{0.2F, 0.6F, 0.6F, 1.0F},
            new float[]{0.2F, 0.6F, 0.4F, 1.0F},
            new float[]{0.2F, 0.6F, 0.2F, 1.0F},
            new float[]{0.2F, 0.6F, 0.0F, 1.0F},
            new float[]{0.2F, 0.4F, 1.0F, 1.0F},
            new float[]{0.2F, 0.4F, 0.8F, 1.0F},
            new float[]{0.2F, 0.4F, 0.6F, 1.0F},
            new float[]{0.2F, 0.4F, 0.4F, 1.0F},
            new float[]{0.2F, 0.4F, 0.2F, 1.0F},
            new float[]{0.2F, 0.4F, 0.0F, 1.0F},
            new float[]{0.2F, 0.2F, 1.0F, 1.0F},
            new float[]{0.2F, 0.2F, 0.8F, 1.0F},
            new float[]{0.2F, 0.2F, 0.6F, 1.0F},
            new float[]{0.2F, 0.2F, 0.4F, 1.0F},
            new float[]{0.2F, 0.2F, 0.2F, 1.0F},
            new float[]{0.2F, 0.2F, 0.0F, 1.0F},
            new float[]{0.2F, 0.0F, 1.0F, 1.0F},
            new float[]{0.2F, 0.0F, 0.8F, 1.0F},
            new float[]{0.2F, 0.0F, 0.6F, 1.0F},
            new float[]{0.2F, 0.0F, 0.4F, 1.0F},
            new float[]{0.2F, 0.0F, 0.2F, 1.0F},
            new float[]{0.2F, 0.0F, 0.0F, 1.0F},
            new float[]{0.0F, 1.0F, 1.0F, 1.0F},
            new float[]{0.0F, 1.0F, 0.8F, 1.0F},
            new float[]{0.0F, 1.0F, 0.6F, 1.0F},
            new float[]{0.0F, 1.0F, 0.4F, 1.0F},
            new float[]{0.0F, 1.0F, 0.2F, 1.0F},
            new float[]{0.0F, 1.0F, 0.0F, 1.0F},
            new float[]{0.0F, 0.8F, 1.0F, 1.0F},
            new float[]{0.0F, 0.8F, 0.8F, 1.0F},
            new float[]{0.0F, 0.8F, 0.6F, 1.0F},
            new float[]{0.0F, 0.8F, 0.4F, 1.0F},
            new float[]{0.0F, 0.8F, 0.2F, 1.0F},
            new float[]{0.0F, 0.8F, 0.0F, 1.0F},
            new float[]{0.0F, 0.6F, 1.0F, 1.0F},
            new float[]{0.0F, 0.6F, 0.8F, 1.0F},
            new float[]{0.0F, 0.6F, 0.6F, 1.0F},
            new float[]{0.0F, 0.6F, 0.4F, 1.0F},
            new float[]{0.0F, 0.6F, 0.2F, 1.0F},
            new float[]{0.0F, 0.6F, 0.0F, 1.0F},
            new float[]{0.0F, 0.4F, 1.0F, 1.0F},
            new float[]{0.0F, 0.4F, 0.8F, 1.0F},
            new float[]{0.0F, 0.4F, 0.6F, 1.0F},
            new float[]{0.0F, 0.4F, 0.4F, 1.0F},
            new float[]{0.0F, 0.4F, 0.2F, 1.0F},
            new float[]{0.0F, 0.4F, 0.0F, 1.0F},
            new float[]{0.0F, 0.2F, 1.0F, 1.0F},
            new float[]{0.0F, 0.2F, 0.8F, 1.0F},
            new float[]{0.0F, 0.2F, 0.6F, 1.0F},
            new float[]{0.0F, 0.2F, 0.4F, 1.0F},
            new float[]{0.0F, 0.2F, 0.2F, 1.0F},
            new float[]{0.0F, 0.2F, 0.0F, 1.0F},
            new float[]{0.0F, 0.0F, 1.0F, 1.0F},
            new float[]{0.0F, 0.0F, 0.8F, 1.0F},
            new float[]{0.0F, 0.0F, 0.6F, 1.0F},
            new float[]{0.0F, 0.0F, 0.4F, 1.0F},
            new float[]{0.0F, 0.0F, 0.2F, 1.0F},
            new float[]{0.9333333333333333F, 0.0F, 0.0F, 1.0F},
            new float[]{0.8666666666666667F, 0.0F, 0.0F, 1.0F},
            new float[]{0.7333333333333333F, 0.0F, 0.0F, 1.0F},
            new float[]{0.6666666666666666F, 0.0F, 0.0F, 1.0F},
            new float[]{0.5333333333333333F, 0.0F, 0.0F, 1.0F},
            new float[]{0.4666666666666667F, 0.0F, 0.0F, 1.0F},
            new float[]{0.3333333333333333F, 0.0F, 0.0F, 1.0F},
            new float[]{0.26666666666666666F, 0.0F, 0.0F, 1.0F},
            new float[]{0.13333333333333333F, 0.0F, 0.0F, 1.0F},
            new float[]{0.06666666666666667F, 0.0F, 0.0F, 1.0F},
            new float[]{0.0F, 0.9333333333333333F, 0.0F, 1.0F},
            new float[]{0.0F, 0.8666666666666667F, 0.0F, 1.0F},
            new float[]{0.0F, 0.7333333333333333F, 0.0F, 1.0F},
            new float[]{0.0F, 0.6666666666666666F, 0.0F, 1.0F},
            new float[]{0.0F, 0.5333333333333333F, 0.0F, 1.0F},
            new float[]{0.0F, 0.4666666666666667F, 0.0F, 1.0F},
            new float[]{0.0F, 0.3333333333333333F, 0.0F, 1.0F},
            new float[]{0.0F, 0.26666666666666666F, 0.0F, 1.0F},
            new float[]{0.0F, 0.13333333333333333F, 0.0F, 1.0F},
            new float[]{0.0F, 0.06666666666666667F, 0.0F, 1.0F},
            new float[]{0.0F, 0.0F, 0.9333333333333333F, 1.0F},
            new float[]{0.0F, 0.0F, 0.8666666666666667F, 1.0F},
            new float[]{0.0F, 0.0F, 0.7333333333333333F, 1.0F},
            new float[]{0.0F, 0.0F, 0.6666666666666666F, 1.0F},
            new float[]{0.0F, 0.0F, 0.5333333333333333F, 1.0F},
            new float[]{0.0F, 0.0F, 0.4666666666666667F, 1.0F},
            new float[]{0.0F, 0.0F, 0.3333333333333333F, 1.0F},
            new float[]{0.0F, 0.0F, 0.26666666666666666F, 1.0F},
            new float[]{0.0F, 0.0F, 0.13333333333333333F, 1.0F},
            new float[]{0.0F, 0.0F, 0.06666666666666667F, 1.0F},
            new float[]{0.9333333333333333F, 0.9333333333333333F, 0.9333333333333333F, 1.0F},
            new float[]{0.8666666666666667F, 0.8666666666666667F, 0.8666666666666667F, 1.0F},
            new float[]{0.7333333333333333F, 0.7333333333333333F, 0.7333333333333333F, 1.0F},
            new float[]{0.6666666666666666F, 0.6666666666666666F, 0.6666666666666666F, 1.0F},
            new float[]{0.5333333333333333F, 0.5333333333333333F, 0.5333333333333333F, 1.0F},
            new float[]{0.4666666666666667F, 0.4666666666666667F, 0.4666666666666667F, 1.0F},
            new float[]{0.3333333333333333F, 0.3333333333333333F, 0.3333333333333333F, 1.0F},
            new float[]{0.26666666666666666F, 0.26666666666666666F, 0.26666666666666666F, 1.0F},
            new float[]{0.13333333333333333F, 0.13333333333333333F, 0.13333333333333333F, 1.0F},
            new float[]{0.06666666666666667F, 0.06666666666666667F, 0.06666666666666667F, 1.0F},
            new float[]{0.0F, 0.0F, 0.0F, 0.0F},
        };

        public static byte[][] byteColors = new byte[256][];
        static PaletteDraw()
        {
            for(int i = 0; i < colors.Length; i++)
            {
                byteColors[i] = new byte[4];
                for(int n = 0; n < 4; n++)
                {
                    byteColors[i][n] = (byte)Math.Round(colors[i][n] * 255);
                }
            }
        }

        public static char SEP = Path.DirectorySeparatorChar;

        public const int
        Cube = 0,
        BrightTop = 1,
        DimTop = 2,
        BrightDim = 3,
        BrightDimTop = 4,
        BrightBottom = 5,
        DimBottom = 6,
        BrightDimBottom = 7,
        BrightBack = 8,
        DimBack = 9,
        BrightTopBack = 10,
        DimTopBack = 11,
        BrightBottomBack = 12,
        DimBottomBack = 13,
        BackBack = 14,
        BackBackTop = 15,
        BackBackBottom = 16,
        RearBrightTop = 17,
        RearDimTop = 18,
        RearBrightBottom = 19,
        RearDimBottom = 20,

        BrightDimTopThick = 21,
        BrightDimBottomThick = 22,
        BrightTopBackThick = 23,
        BrightBottomBackThick = 24,
        DimTopBackThick = 25,
        DimBottomBackThick = 26,
        BackBackTopThick = 27,
        BackBackBottomThick = 28;

        public static Dictionary<Slope, int> slopes = new Dictionary<Slope, int> { { Slope.Cube, Cube },
            { Slope.BrightTop, BrightTop }, { Slope.DimTop, DimTop }, { Slope.BrightDim, BrightDim }, { Slope.BrightDimTop, BrightDimTop }, { Slope.BrightBottom, BrightBottom }, { Slope.DimBottom, DimBottom },
            { Slope.BrightDimBottom, BrightDimBottom }, { Slope.BrightBack, BrightBack }, { Slope.DimBack, DimBack },
            { Slope.BrightTopBack, BrightTopBack }, { Slope.DimTopBack, DimTopBack }, { Slope.BrightBottomBack, BrightBottomBack }, { Slope.DimBottomBack, DimBottomBack }, { Slope.BackBack, BackBack },
            { Slope.BackBackTop, BackBackTop }, { Slope.BackBackBottom, BackBackBottom },
            { Slope.RearBrightTop, RearBrightTop }, { Slope.RearDimTop, RearDimTop }, { Slope.RearBrightBottom, RearBrightBottom }, { Slope.RearDimBottom, RearDimBottom },
            { Slope.BrightDimTopThick, BrightDimTopThick }, { Slope.BrightDimBottomThick, BrightDimBottomThick },
            { Slope.BrightTopBackThick, BrightTopBackThick }, { Slope.BrightBottomBackThick, BrightBottomBackThick },
            { Slope.DimTopBackThick, DimTopBackThick }, { Slope.DimBottomBackThick, DimBottomBackThick },
            { Slope.BackBackTopThick, BackBackTopThick }, { Slope.BackBackBottomThick, BackBackBottomThick } };

        public static int sizex = 0, sizey = 0, sizez = 0;

        public static Bitmap cube, ortho, white;

        /// <summary>
        /// Load a MagicaVoxel .vox format file into a MagicaVoxelData[] that we use for voxel chunks.
        /// </summary>
        /// <param name="stream">An open BinaryReader stream that is the .vox file.</param>
        /// <returns>The voxel chunk data for the MagicaVoxel .vox file.</returns>
        public static MagicaVoxelData[] FromMagica(BinaryReader stream)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below

            MagicaVoxelData[] voxelData = null;

            string magic = new string(stream.ReadChars(4));
            int version = stream.ReadInt32();

            // a MagicaVoxel .vox file starts with a 'magic' 4 character 'VOX ' identifier
            if (magic == "VOX ")
            {
                bool subsample = false;

                while (stream.BaseStream.Position < stream.BaseStream.Length)
                {
                    // each chunk has an ID, size and child chunks
                    char[] chunkId = stream.ReadChars(4);
                    int chunkSize = stream.ReadInt32();
                    int childChunks = stream.ReadInt32();
                    string chunkName = new string(chunkId);

                    // there are only 2 chunks we only care about, and they are SIZE and XYZI
                    if (chunkName == "SIZE")
                    {
                        sizex = stream.ReadInt32();
                        sizey = stream.ReadInt32();
                        sizez = stream.ReadInt32();
                        //                        Console.WriteLine("x is " + sizex + ", y is " + sizey + ", z is " + sizez);
                        if (sizex > 32 || sizey > 32) subsample = true;

                        stream.ReadBytes(chunkSize - 4 * 3);
                    }
                    else if (chunkName == "XYZI")
                    {
                        // XYZI contains n voxels
                        int numVoxels = stream.ReadInt32();
                        int div = (subsample ? 2 : 1);

                        // each voxel has x, y, z and color index values
                        voxelData = new MagicaVoxelData[numVoxels];
                        for (int i = 0; i < voxelData.Length; i++)
                            voxelData[i] = new MagicaVoxelData(stream, subsample);
                    }
                    else if (chunkName == "RGBA")
                    {
                        colors = new float[256][];

                        for(int i = 0; i < 256; i++)
                        {
                            byte r = stream.ReadByte();
                            byte g = stream.ReadByte();
                            byte b = stream.ReadByte();
                            byte a = stream.ReadByte();
                            byteColors[i] = new byte[] { r, g, b, a};
                            colors[i] = new float[] { r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f };
                        }
                    }
                    else stream.ReadBytes(chunkSize);   // read any excess bytes
                }

                if (voxelData.Length == 0) return voxelData; // failed to read any valid voxel data
                /*
                // now push the voxel data into our voxel chunk structure
                for (int i = 0; i < voxelData.Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (32x128x32)
//                    if (voxelData[i].x > 31 || voxelData[i].y > 31 || voxelData[i].z > 127) continue;
                    
                    // use the voxColors array by default, or overrideColor if it is available
//                    int voxel = (voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128);
                    //data[voxel] = (colors == null ? voxColors[voxelData[i].color - 1] : colors[voxelData[i].color - 1]);
                }*/
            }

            return voxelData;
        }
        static public int colorcount = 254;

        public static byte[][] rendered, renderedOrtho, rendered45;
        public static byte[][][] renderedFace, renderedFaceSmall;


        public static double Clamp(double x)
        {
            return Clamp(x, 0.0, 1.0);
        }

        public static double MercifulClamp(double x)
        {
            return Clamp(x, 0.01, 1.0);
        }

        public static double Clamp(double x, double min, double max)
        {
            return Math.Min(Math.Max(min, x), max);
        }

        public static int Clamp(int x)
        {
            return Clamp(x, 0, 255);
        }

        public static int MercifulClamp(int x)
        {
            return Clamp(x, 1, 255);
        }

        public static int Clamp(int x, int min, int max)
        {
            return Math.Min(Math.Max(min, x), max);
        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = MercifulClamp(Convert.ToInt32(value));
            int p = MercifulClamp(Convert.ToInt32(value * (1 - saturation)));
            int q = MercifulClamp(Convert.ToInt32(value * (1 - f * saturation)));
            int t = MercifulClamp(Convert.ToInt32(value * (1 - (1 - f) * saturation)));

            if(hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if(hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if(hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if(hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if(hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }


        private static byte[][] storeColorCubes()
        {
            colorcount = colors.Length;
            byte[,] cubes = new byte[colorcount, 80];

            Image image = cube;
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 5;
            float[][] colorMatrixElements = {
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);

            for(int current_color = 0; current_color < colorcount; current_color++)
            {
                Bitmap b =
                new Bitmap(width, height, PixelFormat.Format32bppArgb);

                Graphics g = Graphics.FromImage((Image)b);

                if(colors[current_color][3] == 0F)
                {
                    colorMatrix = new ColorMatrix(new float[][]{
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 1F}});
                }
                else
                {
                    colorMatrix = new ColorMatrix(new float[][]{
   new float[] {0.22F+colors[current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(image,
                   new Rectangle(0, 0,
                       width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                for(int i = 0; i < width; i++)
                {
                    for(int j = 0; j < height; j++)
                    {
                        Color c = b.GetPixel(i, j);
                        double h = 0.0, s = 1.0, v = 1.0;
                        ColorToHSV(c, out h, out s, out v);
                        double s_alter = (Math.Pow(s + 0.1, 2.2 - 2.2 * s)),
                            v_alter = Math.Pow(v, 2.0 - 2.0 * v);
                        v_alter *= Math.Pow(v_alter, 0.48);
                        c = ColorFromHSV(h, s_alter, v_alter);
                        cubes[current_color, i * 4 + j * 4 * width + 0] = c.B;
                        cubes[current_color, i * 4 + j * 4 * width + 1] = c.G;
                        cubes[current_color, i * 4 + j * 4 * width + 2] = c.R;
                        cubes[current_color, i * 4 + j * 4 * width + 3] = c.A;
                    }
                }
            }
            byte[][] cubes2 = new byte[colorcount][];
            for(int c = 0; c < colorcount; c++)
            {
                cubes2[c] = new byte[width * height * 4];
                for(int j = 0; j < width * height * 4; j++)
                {
                    cubes2[c][j] = cubes[c, j];
                }
            }

            return cubes2;
        }

        private static byte[][] storeColorCubesOrtho()
        {
            colorcount = colors.Length;
            byte[,] cubes = new byte[colorcount, 60];

            Image image = ortho;
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 3;
            int height = 5;
            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);

            for (int current_color = 0; current_color < colorcount; current_color++)
            {
                Bitmap b =
                new Bitmap(width, height, PixelFormat.Format32bppArgb);

                Graphics g = Graphics.FromImage((Image)b);

                if (colors[current_color][3] == 0F)
                {
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 1F}});
                }
                else
                {
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(image,
                   new Rectangle(0, 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        Color c = b.GetPixel(i, j);
                        double h = 0.0, s = 1.0, v = 1.0;
                        ColorToHSV(c, out h, out s, out v);
                        double s_alter = (Math.Pow(s + 0.1, 2.2 - 2.2 * s)),
                            v_alter = Math.Pow(v, 2.0 - 2.0 * v);
                        v_alter *= Math.Pow(v_alter, 0.48);
                        c = ColorFromHSV(h, s_alter, v_alter);

                        cubes[current_color, i * 4 + j * 4 * width + 0] = c.B;
                        cubes[current_color, i * 4 + j * 4 * width + 1] = c.G;
                        cubes[current_color, i * 4 + j * 4 * width + 2] = c.R;
                        cubes[current_color, i * 4 + j * 4 * width + 3] = c.A;
                    }
                }
            }

            byte[][] cubes2 = new byte[colorcount][];
            for (int c = 0; c < colorcount; c++)
            {
                cubes2[c] = new byte[width * height * 4];
                for (int j = 0; j < width * height * 4; j++)
                {
                    cubes2[c][j] = cubes[c, j];
                }
            }

            return cubes2;
        }

        private static byte[][] storeColorCubes45()
        {
            colorcount = colors.Length;
            byte[,] cubes = new byte[colorcount, 24];

            Image image = cube;
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 2;
            int height = 3;
            float[][] colorMatrixElements = {
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);

            for(int current_color = 0; current_color < colorcount; current_color++)
            {
                Bitmap b =
                new Bitmap(4, 5, PixelFormat.Format32bppArgb);

                Graphics g = Graphics.FromImage((Image)b);

                if(colors[current_color][3] == 0F)
                {
                    colorMatrix = new ColorMatrix(new float[][]{
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 1F}});
                }
                else
                {
                    colorMatrix = new ColorMatrix(new float[][]{
   new float[] {0.22F+colors[current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(image,
                   new Rectangle(0, 0,
                       4, 5),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   4,       // width of source rectangle
                   5,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                for(int i = 0; i < width; i++)
                {
                    for(int j = 0; j < height; j++)
                    {
                        Color c = b.GetPixel(i+1, j * 2);
                        double h = 0.0, s = 1.0, v = 1.0;
                        ColorToHSV(c, out h, out s, out v);
                        double s_alter = (Math.Pow(s + 0.1, 2.2 - 2.2 * s)),
                            v_alter = Math.Pow(v, 2.0 - 2.0 * v);
                        v_alter *= Math.Pow(v_alter, 0.48);
                        c = ColorFromHSV(h, s_alter, v_alter);
                        cubes[current_color, i * 4 + j * 4 * width + 0] = c.B;
                        cubes[current_color, i * 4 + j * 4 * width + 1] = c.G;
                        cubes[current_color, i * 4 + j * 4 * width + 2] = c.R;
                        cubes[current_color, i * 4 + j * 4 * width + 3] = c.A;
                    }
                }
            }
            byte[][] cubes2 = new byte[colorcount][];
            for(int c = 0; c < colorcount; c++)
            {
                cubes2[c] = new byte[width * height * 4];
                for(int j = 0; j < width * height * 4; j++)
                {
                    cubes2[c][j] = cubes[c, j];
                }
            }

            return cubes2;
        }

        public static void storeColorCubesFaces()
        {
            colorcount = colors.Length;
            // 29 is the number of Slope enum types.
            renderedFace = new byte[colorcount][][];
            for(int c = 0; c < colorcount; c++)
            {
                renderedFace[c] = new byte[29][];
                for(int sp = 0; sp < 29; sp++)
                {
                    renderedFace[c][sp] = new byte[80];
                }
            }

            Image image = white;
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 5;
            float[][] colorMatrixElements = {
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            for(int current_color = 0; current_color < colorcount; current_color++)
            {
                Bitmap b = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                Graphics g = Graphics.FromImage(b);

                if(colors[current_color][3] == 0F)
                {
                    colorMatrix = new ColorMatrix(new float[][]{
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 1F}});
                }
                else
                {
                    colorMatrix = new ColorMatrix(new float[][]{
   new float[] {0.22F+colors[current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(image,
                   new Rectangle(0, 0,
                       width, height),  // destination rectangle 
                                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                for(int i = 0; i < width; i++)
                {
                    for(int j = 0; j < height; j++)
                    {
                        Color c = b.GetPixel(i, j);
                        double h = 0.0, s = 1.0, v = 1.0;
                        ColorToHSV(c, out h, out s, out v);

                        for(int slp = 0; slp < 29; slp++)
                        {
                            Color c2 = Color.Transparent;
                            double s_alter = (Math.Pow(s + 0.04, 2.08 - 2.08 * s)),
                                        v_alter = Math.Pow(v, 2.0 - 2.0 * v);
                            v_alter = MercifulClamp(v_alter * (0.25 + Math.Pow(v_alter, 0.5)) * 0.76);
                            if(j == height - 1)
                            {
                                c2 = ColorFromHSV(h, Clamp((s + s * s * s * Math.Pow(s, 0.3)) * 1.55, 0.0112, 1.0), Clamp(v_alter * 0.65, 0.01, 1.0));
                            }
                            else
                            {
                                switch(slp)
                                {
                                    case Cube:
                                        {
                                            if(j == 0)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 0.85, 0.0112, 1.0), Clamp(v_alter * 1.05, 0.09, 1.0));
                                            }
                                            else if(i < width / 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 0.95, 0.0112, 1.0), Clamp(v_alter * 0.95, 0.06, 1.0));
                                            }
                                            else if(i >= width / 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.05, 0.0112, 1.0), Clamp(v_alter * 0.85, 0.03, 1.0));
                                            }
                                        }
                                        break;
                                    case BrightTop:
                                        {
                                            if(i + j / 2 >= 4)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.1, 0.0112, 1.0), Clamp(v_alter * 0.85, 0.03, 1.0));
                                            }
                                            else if(i + (j + 1) / 2 >= 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 0.9, 0.0112, 1.0), Clamp(v_alter * 1.1, 0.10, 1.0));
                                            }
                                        }
                                        break;
                                    case DimTop:
                                        {
                                            if(i < j / 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.0, 0.0112, 1.0), Clamp(v_alter * 0.95, 0.06, 1.0));
                                            }
                                            else if(i - 1 <= (j + 1) / 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.1, 0.0112, 1.0), Clamp(v_alter * 0.9, 0.05, 1.0));
                                            }
                                        }
                                        break;
                                    case BrightDim:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 1.15, 0.0112, 1.0), Clamp(v_alter * 0.9, 0.05, 1.0));
                                        }
                                        break;
                                    case BrightDimTop:
                                    case BrightDimTopThick:
                                        {
                                            if(((i > 0 && i < 3) || j >= 3) && j > 0)
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.0, 0.0112, 1.0), Clamp(v_alter * 0.9, 0.08, 1.0));
                                        }
                                        break;
                                    case BrightBottom:
                                        {
                                            if(i > (j + 1) / 2 + 1)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.1, 0.0112, 1.0), Clamp(v_alter * 0.85, 0.03, 1.0));
                                            }
                                            else if(i + 1 > (j + 1) / 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.2, 0.0112, 1.0), Clamp(v_alter * 0.6, 0.02, 1.0));
                                            }
                                        }
                                        break;
                                    case DimBottom:
                                        {
                                            if(i + (j + 1) / 2 < 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.05, 0.0112, 1.0), Clamp(v_alter * 0.95, 0.06, 1.0));
                                            }
                                            else if(i + (j + 1) / 2 < 4)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.2, 0.0112, 1.0), Clamp(v_alter * 0.5, 0.01, 1.0));
                                            }
                                        }
                                        break;
                                    case BrightDimBottom:
                                    case BrightDimBottomThick:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 1.15, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.015, 1.0));
                                        }
                                        break;

                                    case BrightBack:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 0.95, 0.0112, 1.0), Clamp(v_alter * 1.0, 0.09, 1.0));
                                        }
                                        break;
                                    case DimBack:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 1.0, 0.0112, 1.0), Clamp(v_alter * 1.0, 0.09, 1.0));
                                        }
                                        break;
                                    case BrightTopBack:
                                        {
                                            if(i + (j + 3) / 4 >= 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 0.95, 0.0112, 1.0), Clamp(v_alter * 1.05, 0.09, 1.0));
                                            }
                                        }
                                        break;
                                    case DimTopBack:
                                        {
                                            if(i - (j + 3) / 4 <= 1)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.05, 0.0112, 1.0), Clamp(v_alter * 0.9, 0.09, 1.0));
                                            }
                                        }
                                        break;
                                    case BrightBottomBack:
                                        {
                                            if(i >= j)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.2, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.01, 1.0));
                                            }
                                        }
                                        break;
                                    case DimBottomBack:
                                        {
                                            if(i + j <= 3)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.25, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.01, 1.0));
                                            }
                                        }
                                        break;
                                    case BrightTopBackThick:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 0.95, 0.0112, 1.0), Clamp(v_alter * 1.05, 0.09, 1.0));
                                        }
                                        break;
                                    case DimTopBackThick:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 1.05, 0.0112, 1.0), Clamp(v_alter * 0.9, 0.09, 1.0));
                                        }
                                        break;
                                    case BrightBottomBackThick:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 1.2, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.01, 1.0));
                                        }
                                        break;
                                    case DimBottomBackThick:
                                        {
                                            if(i + j <= 3)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.2, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.01, 1.0));
                                            }
                                        }
                                        break;
                                    case RearBrightTop:
                                        {
                                            if(i + (j + 3) / 4 >= 3 && j > 0)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 0.85, 0.0112, 1.0), Clamp(v_alter * 1.05, 0.09, 1.0));
                                            }
                                        }
                                        break;
                                    case RearDimTop:
                                        {
                                            if(i - (j + 3) / 4 <= 0 && j > 0)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.0, 0.0112, 1.0), Clamp(v_alter * 0.9, 0.09, 1.0));
                                            }
                                        }
                                        break;
                                    case RearBrightBottom:
                                        {
                                            if(i > j)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.05, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.01, 1.0));
                                            }
                                        }
                                        break;
                                    case RearDimBottom:
                                        {
                                            if(i + j <= 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.2, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.01, 1.0));
                                            }
                                        }
                                        break;
                                    case BackBackTop:
                                    case BackBack:
                                        {
                                            if(j > 0)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 0.85, 0.0112, 1.0), Clamp(v_alter * 1.0, 0.09, 1.0));
                                            }
                                        }
                                        break;
                                    case BackBackTopThick:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 0.85, 0.0112, 1.0), Clamp(v_alter * 1.0, 0.09, 1.0));
                                        }
                                        break;
                                        /*
                                    case BackBackBottom:
                                    case BackBackBottomThick:
                                    default:
                                        {

                                        }
                                        break;
                                        */
                                }
                            }

                            if(c2.A != 0)
                            {
                                renderedFace[current_color][slp][i * 4 + j * width * 4 + 0] = Math.Max((byte)1, c2.B);
                                renderedFace[current_color][slp][i * 4 + j * width * 4 + 1] = Math.Max((byte)1, c2.G);
                                renderedFace[current_color][slp][i * 4 + j * width * 4 + 2] = Math.Max((byte)1, c2.R);
                                renderedFace[current_color][slp][i * 4 + j * width * 4 + 3] = c2.A;
                            }
                            else
                            {
                                renderedFace[current_color][slp][i * 4 + j * 4 * width + 0] = 0;
                                renderedFace[current_color][slp][i * 4 + j * 4 * width + 1] = 0;
                                renderedFace[current_color][slp][i * 4 + j * 4 * width + 2] = 0;
                                renderedFace[current_color][slp][i * 4 + j * 4 * width + 3] = 0;
                            }
                        }
                    }
                }
            }
        }
        private static void storeColorCubesFacesSmall()
        {
            colorcount = colors.Length;
            // 29 is the number of Slope enum types.
            renderedFaceSmall = new byte[colorcount][][];
            for(int c = 0; c < colorcount; c++)
            {
                renderedFaceSmall[c] = new byte[29][];
                for(int sp = 0; sp < 29; sp++)
                {
                    renderedFaceSmall[c][sp] = new byte[96];
                }
            }


            Image image = white;
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 6;
            float[][] colorMatrixElements = {
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            for(int current_color = 0; current_color < colorcount; current_color++)
            {
                Bitmap b = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                Graphics g = Graphics.FromImage(b);

                if(colors[current_color][3] == 0F)
                {
                    colorMatrix = new ColorMatrix(new float[][]{
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 1F}});
                }
                else
                {
                    colorMatrix = new ColorMatrix(new float[][]{
   new float[] {0.22F+colors[current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(image,
                   new Rectangle(0, 0,
                       width, height),  // destination rectangle 
                                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);

                for(int i = 0; i < width; i++)
                {
                    for(int j = 0; j < height; j++)
                    {
                        Color c = b.GetPixel(i, j);
                        double h = 0.0, s = 1.0, v = 1.0;
                        ColorToHSV(c, out h, out s, out v);

                        for(int slp = 0; slp < 29; slp++)
                        {
                            Color c2 = Color.Transparent;
                            double s_alter = (Math.Pow(s + 0.04, 2.08 - 2.08 * s)),
                                        v_alter = Math.Pow(v, 2.0 - 2.0 * v);
                            v_alter = MercifulClamp(v_alter * (0.25 + Math.Pow(v_alter, 0.5)) * 0.76);
                            if(j == height - 1)
                            {
                                c2 = ColorFromHSV(h, Clamp((s + s * s * s * Math.Pow(s, 0.3)) * 1.55, 0.0112, 1.0), Clamp(v_alter * 0.65, 0.01, 1.0));
                            }
                            else
                            {
                                switch(slp)
                                {
                                    case Cube:
                                        {
                                            if(j == 0)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 0.85, 0.0112, 1.0), Clamp(v_alter * 1.05, 0.09, 1.0));
                                            }
                                            else if(i < width / 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 0.95, 0.0112, 1.0), Clamp(v_alter * 0.95, 0.06, 1.0));
                                            }
                                            else if(i >= width / 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.05, 0.0112, 1.0), Clamp(v_alter * 0.85, 0.03, 1.0));
                                            }
                                        }
                                        break;
                                    case BrightTop:
                                        {
                                            if(i + j / 2 >= 4)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.1, 0.0112, 1.0), Clamp(v_alter * 0.85, 0.03, 1.0));
                                            }
                                            else if(i + (j + 1) / 2 >= 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 0.9, 0.0112, 1.0), Clamp(v_alter * 1.1, 0.10, 1.0));
                                            }
                                        }
                                        break;
                                    case DimTop:
                                        {
                                            if(i < j / 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.0, 0.0112, 1.0), Clamp(v_alter * 0.95, 0.06, 1.0));
                                            }
                                            else if(i - 1 <= (j + 1) / 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.1, 0.0112, 1.0), Clamp(v_alter * 0.9, 0.05, 1.0));
                                            }
                                        }
                                        break;
                                    case BrightDim:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 1.15, 0.0112, 1.0), Clamp(v_alter * 0.9, 0.05, 1.0));
                                        }
                                        break;
                                    case BrightDimTop:
                                    case BrightDimTopThick:
                                        {
                                            if(((i > 0 && i < 3) || j >= 3) && j > 0)
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.0, 0.0112, 1.0), Clamp(v_alter * 0.9, 0.08, 1.0));
                                        }
                                        break;
                                    case BrightBottom:
                                        {
                                            if(i > (j + 1) / 2 + 1)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.1, 0.0112, 1.0), Clamp(v_alter * 0.85, 0.03, 1.0));
                                            }
                                            else if(i + 1 > (j + 1) / 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.2, 0.0112, 1.0), Clamp(v_alter * 0.6, 0.02, 1.0));
                                            }
                                        }
                                        break;
                                    case DimBottom:
                                        {
                                            if(i + (j + 1) / 2 < 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.05, 0.0112, 1.0), Clamp(v_alter * 0.95, 0.06, 1.0));
                                            }
                                            else if(i + (j + 1) / 2 < 4)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.2, 0.0112, 1.0), Clamp(v_alter * 0.5, 0.01, 1.0));
                                            }
                                        }
                                        break;
                                    case BrightDimBottom:
                                    case BrightDimBottomThick:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 1.15, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.015, 1.0));
                                        }
                                        break;

                                    case BrightBack:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 0.95, 0.0112, 1.0), Clamp(v_alter * 1.0, 0.09, 1.0));
                                        }
                                        break;
                                    case DimBack:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 1.0, 0.0112, 1.0), Clamp(v_alter * 1.0, 0.09, 1.0));
                                        }
                                        break;
                                    case BrightTopBack:
                                        {
                                            if(i + (j + 3) / 4 >= 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 0.95, 0.0112, 1.0), Clamp(v_alter * 1.05, 0.09, 1.0));
                                            }
                                        }
                                        break;
                                    case DimTopBack:
                                        {
                                            if(i - (j + 3) / 4 <= 1)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.05, 0.0112, 1.0), Clamp(v_alter * 0.9, 0.09, 1.0));
                                            }
                                        }
                                        break;
                                    case BrightBottomBack:
                                        {
                                            if(i >= j)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.2, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.01, 1.0));
                                            }
                                        }
                                        break;
                                    case DimBottomBack:
                                        {
                                            if(i + j <= 3)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.25, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.01, 1.0));
                                            }
                                        }
                                        break;
                                    case BrightTopBackThick:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 0.95, 0.0112, 1.0), Clamp(v_alter * 1.05, 0.09, 1.0));
                                        }
                                        break;
                                    case DimTopBackThick:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 1.05, 0.0112, 1.0), Clamp(v_alter * 0.9, 0.09, 1.0));
                                        }
                                        break;
                                    case BrightBottomBackThick:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 1.2, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.01, 1.0));
                                        }
                                        break;
                                    case DimBottomBackThick:
                                        {
                                            if(i + j <= 3)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.2, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.01, 1.0));
                                            }
                                        }
                                        break;
                                    case RearBrightTop:
                                        {
                                            if(i + (j + 3) / 4 >= 3 && j > 0)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 0.85, 0.0112, 1.0), Clamp(v_alter * 1.05, 0.09, 1.0));
                                            }
                                        }
                                        break;
                                    case RearDimTop:
                                        {
                                            if(i - (j + 3) / 4 <= 0 && j > 0)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.0, 0.0112, 1.0), Clamp(v_alter * 0.9, 0.09, 1.0));
                                            }
                                        }
                                        break;
                                    case RearBrightBottom:
                                        {
                                            if(i > j)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.05, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.01, 1.0));
                                            }
                                        }
                                        break;
                                    case RearDimBottom:
                                        {
                                            if(i + j <= 2)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 1.2, 0.0112, 1.0), Clamp(v_alter * 0.55, 0.01, 1.0));
                                            }
                                        }
                                        break;
                                    case BackBackTop:
                                    case BackBack:
                                        {
                                            if(j > 0)
                                            {
                                                c2 = ColorFromHSV(h, Clamp(s_alter * 0.85, 0.0112, 1.0), Clamp(v_alter * 1.0, 0.09, 1.0));
                                            }
                                        }
                                        break;
                                    case BackBackTopThick:
                                        {
                                            c2 = ColorFromHSV(h, Clamp(s_alter * 0.85, 0.0112, 1.0), Clamp(v_alter * 1.0, 0.09, 1.0));
                                        }
                                        break;
                                        /*
                                    case BackBackBottom:
                                    case BackBackBottomThick:
                                    default:
                                        {

                                        }
                                        break;
                                        */
                                }
                            }
                            if(c2.A != 0)
                            {
                                renderedFaceSmall[current_color][slp][i * 4 + j * width * 4 + 0] = Math.Max((byte)1, c2.B);
                                renderedFaceSmall[current_color][slp][i * 4 + j * width * 4 + 1] = Math.Max((byte)1, c2.G);
                                renderedFaceSmall[current_color][slp][i * 4 + j * width * 4 + 2] = Math.Max((byte)1, c2.R);
                                renderedFaceSmall[current_color][slp][i * 4 + j * width * 4 + 3] = 255;
                            }
                            else
                            {
                                renderedFaceSmall[current_color][slp][i * 4 + j * 4 * width + 0] = 0;
                                renderedFaceSmall[current_color][slp][i * 4 + j * 4 * width + 1] = 0;
                                renderedFaceSmall[current_color][slp][i * 4 + j * 4 * width + 2] = 0;
                                renderedFaceSmall[current_color][slp][i * 4 + j * 4 * width + 3] = 0;
                            }
                        }
                    }
                }
            }
           
            //return renderedFaceSmall;
        }



        /// <summary>
        /// Render voxel chunks in a MagicaVoxelData[] to a Bitmap with X pointing in any direction.
        /// </summary>
        /// <param name="voxels">The result of calling FromMagica.</param>
        /// <param name="xSize">The bounding X size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="ySize">The bounding Y size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="zSize">The bounding Z size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="dir">The Direction enum that specifies which way the X-axis should point.</param>
        /// <returns>A Bitmap view of the voxels in isometric pixel view.</returns>
        public static Bitmap render(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize, Direction dir)
        {
            int bWidth = (xSize + ySize) * 2;
            int bHeight = (xSize + ySize) + zSize * 3;
            Bitmap b = new Bitmap(bWidth, bHeight);
            Graphics g = Graphics.FromImage((Image)b);
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;

            float[][] colorMatrixElements = { 
   new float[] {1F,  0,  0,  0, 0},
   new float[] {0,  1F,  0,  0, 0},
   new float[] {0,   0, 1F,  0, 0},
   new float[] {0,   0,  0, 1F, 0},
   new float[] {0,   0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            switch (dir)
            {
                case Direction.SE:
                    vls = voxels;
                    break;
                case Direction.SW:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY) + (ySize / 2));
                        vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;

                case Direction.NW:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case Direction.NE:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].y = (byte)(tempX + (xSize / 2));
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128))
            {
                int currentColor = vx.color - 1;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[currentColor][0],  0,  0,  0, 0},
   new float[] {0,  colors[currentColor][1],  0,  0, 0},
   new float[] {0,  0,  colors[currentColor][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   cube,
                    //(3 * zSize - 2)
                   new Rectangle((vx.x + vx.y) * 2, (bHeight - xSize - 2) - vx.y + vx.x - 3 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }
        /*
        /// <summary>
        /// Render outline chunks in a MagicaVoxelData[] to a Bitmap with X pointing in any direction.
        /// </summary>
        /// <param name="voxels">The result of calling FromMagica.</param>
        /// <param name="xSize">The bounding X size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="ySize">The bounding Y size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="zSize">The bounding Z size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="dir">The Direction enum that specifies which way the X-axis should point.</param>
        /// <returns>A Bitmap view of the voxels in isometric pixel view.</returns>
        public static Bitmap renderOutline(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize, Direction dir)
        {
            int bWidth = (xSize + ySize) * 2 + 4;
            int bHeight = (xSize + ySize) + zSize * 3 + 4;
            Bitmap b = new Bitmap(bWidth, bHeight);
            Graphics g = Graphics.FromImage((Image)b);
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            float[][] colorMatrixElements = { 
   new float[] {1.2F,  0,  0,  0, 0},
   new float[] {0,  1.2F,  0,  0, 0},
   new float[] {0,   0, 1.2F,  0, 0},
   new float[] {0,   0,    0, 1F, 0},
   new float[] {0,   0,    0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            switch (dir)
            {
                case Direction.SE:
                    vls = voxels;
                    break;
                case Direction.SW:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY) + (ySize / 2));
                        vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;

                case Direction.NW:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case Direction.NE:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].y = (byte)(tempX + (xSize / 2));
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128))
            {
                g.DrawImage(
                   outline,
                    //(3 * zSize - 2)
                   new Rectangle((vx.x + vx.y) * 2, (bHeight - xSize - 2) - vx.y + vx.x - 3 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }
        */
        private static int voxelToPixel(int innerX, int innerY, int x, int y, int z, int current_color, int stride, byte xSize, byte ySize, byte zSize)
        {
            return 4 * ((x + y) * 2 + 4)
                + innerX +
                stride * (((xSize + ySize) + zSize * 3) - (Math.Max(xSize, ySize)) - y + x - z * 3 + innerY); //(xSize + ySize) * 2
        }
        private static int voxelToPixelSmall(int innerX, int innerY, int x, int y, int z, int current_color, int stride, byte xSize, byte ySize, byte zSize)
        {
            return 4 * ((x + y) * 2 + 4)
                + innerX +
                stride * (((xSize + ySize) + zSize * 4) - (Math.Max(xSize, ySize)) - y + x - z * 4 + innerY); //(xSize + ySize) * 2
        }

        private static int voxelToPixelOrtho(int innerX, int innerY, int x, int y, int z, int current_color, int stride, byte xSize, byte ySize, byte zSize)
        {
            /*
            4 * (vx.y * 3 + 6 + ((current_color == 136) ? jitter - 1 : 0))
                             + i +
                           bmpData.Stride * (308 - 60 - 8 + vx.x - vx.z * 3 - ((xcolors[current_color + faction][3] == flat_alpha) ? -3 : jitter) + j)
             */
            return 4 * (y * 3 + 4)
                 + innerX +
                stride * ((zSize * 3 - 1) + x - z * 3 + innerY);
        }

        private static int voxelToPixelOrtho45(int innerX, int innerY, int x, int y, int z, int current_color, int stride, byte xSize, byte ySize, byte zSize)
        {
            return 4 * (y * 3 + 2)
                 + innerX +
                stride * (zSize * 2 + x * 3 - z * 2 + innerY);
        }

        private static int voxelToPixelGeneric(int innerX, int x, int y, int z, int stride, int zdim, int multiplier)
        {
            return (4 * y + 4 + innerX +
                stride * (zdim * multiplier + (x >> 1) - z));
        }

        private static int voxelToPixel45(int innerX, int innerY, int x, int y, int z, int current_color, int stride, byte xSize, byte ySize, byte zSize)
        {
            return 8 * (x + y + 2)
                + innerX +
                stride * (((xSize + ySize + zSize) - Math.Max(xSize, ySize)) * 2 + 2 * (x - z - y) + innerY); //(xSize + ySize) * 2
        }

        private static byte Shade(byte[] sprite, int innerX, int aboveback, int above, int abovefront)
        {
            //            switch((((7 * innerX) * (3 * innerY) + x + y + z) ^ ((11 * innerX) * (5 * innerY) + x + y + z) ^ (7 - innerX - innerY)) % 16)
            if(above > 0)
                return sprite[12 + innerX];
            else if(aboveback > 0 || abovefront > 0)
                return sprite[12 + innerX];
            else
                return sprite[innerX];
        }

        /// <summary>
        /// Render outline chunks in a MagicaVoxelData[] to a Bitmap with X pointing in any direction.
        /// </summary>
        /// <param name="voxels">The result of calling FromMagica.</param>
        /// <param name="xSize">The bounding X size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="ySize">The bounding Y size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="zSize">The bounding Z size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="dir">The Direction enum that specifies which way the X-axis should point.</param>
        /// <returns>A Bitmap view of the voxels in isometric pixel view.</returns>
        private static Bitmap renderSmartFaces(FaceVoxel[,,] faces, byte xSize, byte ySize, byte zSize, Outlining o, bool shrink)
        {
            byte tsx = (byte)sizex, tsy = (byte)sizey;
            byte hSize = Math.Max(ySize, xSize);

            xSize = hSize;
            ySize = hSize;

            int bWidth = (xSize + ySize) * 2 + 8;
            int bHeight = (xSize + ySize) + zSize * 3 + 8;
            Bitmap bmp = new Bitmap(bWidth, bHeight, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);

            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            for(int fz = zSize - 1; fz >= 0; fz--)
            {
                for(int fx = xSize - 1; fx >= 0; fx--)
                {
                    for(int fy = 0; fy < ySize; fy++)
                    {
                        if(faces[fx, fy, fz] == null) continue;
                        MagicaVoxelData vx = faces[fx, fy, fz].vox;
                        if(vx.color == 0) continue;
                        Slope slope = faces[fx, fy, fz].slope;
                        int current_color = vx.color - 1;
                        int p = 0;

                        if(renderedFace[current_color][0][3] == 0F)
                            continue;
                        else
                        {
                            int sp = slopes[slope];


                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixel(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, xSize, ySize, zSize);

                                    if(argbValues[p] == 0)
                                    {

                                        if(renderedFace[current_color][sp][((i / 4) * 4 + 3) + j * 16] != 0)
                                        {
                                            argbValues[p] = renderedFace[current_color][sp][i + j * 16];

                                            zbuffer[p] = vx.z + vx.x - vx.y;
                                        }

                                        if(outlineValues[p] == 0)
                                            outlineValues[p] = renderedFace[current_color][0][i + 64];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            switch(o)
            {
                case Outlining.Full:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {
                                if(i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if(i + 4 >= 0 && i + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if(i - 4 >= 0 && i - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; } else if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; } else if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; } else if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; } else if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if(i + 8 >= 0 && i + 8 < argbValues.Length && argbValues[i + 8] == 0) { outlineValues[i + 8] = 255; } else if(i + 8 >= 0 && i + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if(i - 8 >= 0 && i - 8 < argbValues.Length && argbValues[i - 8] == 0) { outlineValues[i - 8] = 255; } else if(i - 8 >= 0 && i - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && argbValues[i + bmpData.Stride * 2] == 0) { outlineValues[i + bmpData.Stride * 2] = 255; } else if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && argbValues[i - bmpData.Stride * 2] == 0) { outlineValues[i - bmpData.Stride * 2] = 255; } else if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && argbValues[i + bmpData.Stride + 8] == 0) { outlineValues[i + bmpData.Stride + 8] = 255; } else if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && argbValues[i - bmpData.Stride + 8] == 0) { outlineValues[i - bmpData.Stride + 8] = 255; } else if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && argbValues[i + bmpData.Stride - 8] == 0) { outlineValues[i + bmpData.Stride - 8] = 255; } else if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && argbValues[i - bmpData.Stride - 8] == 0) { outlineValues[i - bmpData.Stride - 8] = 255; } else if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 8] == 0) { outlineValues[i + bmpData.Stride * 2 + 8] = 255; } else if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 4] == 0) { outlineValues[i + bmpData.Stride * 2 + 4] = 255; } else if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 4] == 0) { outlineValues[i + bmpData.Stride * 2 - 4] = 255; } else if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 8] == 0) { outlineValues[i + bmpData.Stride * 2 - 8] = 255; } else if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 8] == 0) { outlineValues[i - bmpData.Stride * 2 + 8] = 255; } else if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 4] == 0) { outlineValues[i - bmpData.Stride * 2 + 4] = 255; } else if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 4] == 0) { outlineValues[i - bmpData.Stride * 2 - 4] = 255; } else if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 8] == 0) { outlineValues[i - bmpData.Stride * 2 - 8] = 255; } else if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
                case Outlining.Light:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(i + 4 >= 0 && i + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(i - 4 >= 0 && i - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if(i + 8 >= 0 && i + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if(i - 8 >= 0 && i - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
                case Outlining.Partial:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0) { } else if(i + 4 >= 0 && i + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0) { } else if(i - 4 >= 0 && i - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0) { } else if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0) { } else if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0) { } else if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0) { } else if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0) { } else if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0) { } else if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if(i + 8 >= 0 && i + 8 < argbValues.Length && argbValues[i + 8] == 0) { } else if(i + 8 >= 0 && i + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if(i - 8 >= 0 && i - 8 < argbValues.Length && argbValues[i - 8] == 0) { } else if(i - 8 >= 0 && i - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && argbValues[i + bmpData.Stride * 2] == 0) { } else if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && argbValues[i - bmpData.Stride * 2] == 0) { } else if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && argbValues[i + bmpData.Stride + 8] == 0) { } else if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && argbValues[i - bmpData.Stride + 8] == 0) { } else if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && argbValues[i + bmpData.Stride - 8] == 0) { } else if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && argbValues[i - bmpData.Stride - 8] == 0) { } else if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 8] == 0) { } else if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 4] == 0) { } else if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 4] == 0) { } else if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 8] == 0) { } else if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 8] == 0) { } else if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 4] == 0) { } else if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 4] == 0) { } else if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 8] == 0) { } else if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
            }

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 0)
                    argbValues[i] = 255;
                if(outlineValues[i] == 255) argbValues[i] = 255;
            }

            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            if(!shrink)
            {
                return bmp;
            }
            else
            {
                Graphics g = Graphics.FromImage(bmp);
                Bitmap b2 = new Bitmap(bWidth / 2, bHeight / 2, PixelFormat.Format32bppArgb);
                Graphics g2 = Graphics.FromImage(b2);
                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g2.DrawImage(bmp.Clone(new Rectangle(0, 0, bWidth, bHeight), bmp.PixelFormat), 0, 0, bWidth / 2, bHeight / 2);
                g2.Dispose();
                return b2;
            }
        }

        /// <summary>
        /// Render outline chunks in a MagicaVoxelData[] to a Bitmap with a diffeent perspective, with X pointing in any direction.
        /// </summary>
        /// <param name="voxels">The result of calling FromMagica.</param>
        /// <param name="xSize">The bounding X size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="ySize">The bounding Y size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="zSize">The bounding Z size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="dir">The Direction enum that specifies which way the X-axis should point.</param>
        /// <returns>A Bitmap view of the voxels in isometric pixel view.</returns>
        private static Bitmap renderSmartFacesSmall(FaceVoxel[,,] faces, byte xSize, byte ySize, byte zSize, Outlining o, bool shrink)
        {
            byte tsx = (byte)sizex, tsy = (byte)sizey;
            byte hSize = Math.Max(ySize, xSize);

            xSize = hSize;
            ySize = hSize;

            int bWidth = (xSize + ySize) * 2 + 8;
            int bHeight = (xSize + ySize) + zSize * 4 + 8;
            Bitmap bmp = new Bitmap(bWidth, bHeight, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);

            byte[] editValues = new byte[numBytes];

            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            for(int fz = zSize - 1; fz >= 0; fz--)
            {
                for(int fx = xSize - 1; fx >= 0; fx--)
                {
                    for(int fy = 0; fy < ySize; fy++)
                    {
                        if(faces[fx, fy, fz] == null) continue;
                        MagicaVoxelData vx = faces[fx, fy, fz].vox;
                        if(vx.color == 0) continue;
                        Slope slope = faces[fx, fy, fz].slope;
                        int current_color = vx.color - 1;
                        int p = 0;

                        if(renderedFaceSmall[current_color][0][3] == 0F)
                            continue;
                        else
                        {
                            int sp = slopes[slope];

                            for(int j = 0; j < 5; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixelSmall(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, xSize, ySize, zSize);

                                    if(argbValues[p] == 0)
                                    {

                                        if(renderedFaceSmall[current_color][sp][((i / 4) * 4 + 3) + j * 16] != 0)
                                        {
                                            argbValues[p] = renderedFaceSmall[current_color][sp][i + j * 16];

                                            zbuffer[p] = vx.z * 3 + (vx.x - vx.y) * 2;
                                        }

                                        if(outlineValues[p] == 0)
                                            outlineValues[p] = renderedFaceSmall[current_color][0][i + 80];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            switch(o)
            {
                case Outlining.Full:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {
                                bool shade = false;

                                if(i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if(i + 4 >= 0 && i + 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i + 4]) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if(i - 4 >= 0 && i - 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i - 4]) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && zbuffer[i] - 12 > zbuffer[i + bmpData.Stride]) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && zbuffer[i] - 12 > zbuffer[i - bmpData.Stride]) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; } else if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i + bmpData.Stride + 4]) { editValues[i + bmpData.Stride + 4] = 255; editValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; } else if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i - bmpData.Stride - 4]) { editValues[i - bmpData.Stride - 4] = 255; editValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; } else if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i + bmpData.Stride - 4]) { editValues[i + bmpData.Stride - 4] = 255; editValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; } else if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i - bmpData.Stride + 4]) { editValues[i - bmpData.Stride + 4] = 255; editValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; shade = true; }

                                if(i + 8 >= 0 && i + 8 < argbValues.Length && argbValues[i + 8] == 0) { outlineValues[i + 8] = 255; } else if(i + 8 >= 0 && i + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + 8]) { editValues[i + 8] = 255; editValues[i + 8 - 1] = outlineValues[i - 1]; editValues[i + 8 - 2] = outlineValues[i - 2]; editValues[i + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - 8 >= 0 && i - 8 < argbValues.Length && argbValues[i - 8] == 0) { outlineValues[i - 8] = 255; } else if(i - 8 >= 0 && i - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - 8]) { editValues[i - 8] = 255; editValues[i - 8 - 1] = outlineValues[i - 1]; editValues[i - 8 - 2] = outlineValues[i - 2]; editValues[i - 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && argbValues[i + bmpData.Stride * 2] == 0) { outlineValues[i + bmpData.Stride * 2] = 255; } else if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2]) { editValues[i + bmpData.Stride * 2] = 255; editValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && argbValues[i - bmpData.Stride * 2] == 0) { outlineValues[i - bmpData.Stride * 2] = 255; } else if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2]) { editValues[i - bmpData.Stride * 2] = 255; editValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && argbValues[i + bmpData.Stride + 8] == 0) { outlineValues[i + bmpData.Stride + 8] = 255; } else if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride + 8]) { editValues[i + bmpData.Stride + 8] = 255; editValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && argbValues[i - bmpData.Stride + 8] == 0) { outlineValues[i - bmpData.Stride + 8] = 255; } else if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride + 8]) { editValues[i - bmpData.Stride + 8] = 255; editValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && argbValues[i + bmpData.Stride - 8] == 0) { outlineValues[i + bmpData.Stride - 8] = 255; } else if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride - 8]) { editValues[i + bmpData.Stride - 8] = 255; editValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && argbValues[i - bmpData.Stride - 8] == 0) { outlineValues[i - bmpData.Stride - 8] = 255; } else if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride - 8]) { editValues[i - bmpData.Stride - 8] = 255; editValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 8] == 0) { outlineValues[i + bmpData.Stride * 2 + 8] = 255; } else if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2 + 8]) { editValues[i + bmpData.Stride * 2 + 8] = 255; editValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 4] == 0) { outlineValues[i + bmpData.Stride * 2 + 4] = 255; } else if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2 + 4]) { editValues[i + bmpData.Stride * 2 + 4] = 255; editValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 4] == 0) { outlineValues[i + bmpData.Stride * 2 - 4] = 255; } else if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2 - 4]) { editValues[i + bmpData.Stride * 2 - 4] = 255; editValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 8] == 0) { outlineValues[i + bmpData.Stride * 2 - 8] = 255; } else if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2 - 8]) { editValues[i + bmpData.Stride * 2 - 8] = 255; editValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 8] == 0) { outlineValues[i - bmpData.Stride * 2 + 8] = 255; } else if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2 + 8]) { editValues[i - bmpData.Stride * 2 + 8] = 255; editValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 4] == 0) { outlineValues[i - bmpData.Stride * 2 + 4] = 255; } else if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2 + 4]) { editValues[i - bmpData.Stride * 2 + 4] = 255; editValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 4] == 0) { outlineValues[i - bmpData.Stride * 2 - 4] = 255; } else if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2 - 4]) { editValues[i - bmpData.Stride * 2 - 4] = 255; editValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 8] == 0) { outlineValues[i - bmpData.Stride * 2 - 8] = 255; } else if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2 - 8]) { editValues[i - bmpData.Stride * 2 - 8] = 255; editValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; shade = true; }

                                if(shade) editValues[i] = 255;
                            }
                        }
                    }
                    break;
                case Outlining.Light:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {
                                bool shade = false;
                                if(i + 4 >= 0 && i + 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i + 4]) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - 4 >= 0 && i - 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i - 4]) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && zbuffer[i] - 12 > zbuffer[i + bmpData.Stride]) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && zbuffer[i] - 12 > zbuffer[i - bmpData.Stride]) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i + bmpData.Stride + 4]) { editValues[i + bmpData.Stride + 4] = 255; editValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i - bmpData.Stride - 4]) { editValues[i - bmpData.Stride - 4] = 255; editValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i + bmpData.Stride - 4]) { editValues[i + bmpData.Stride - 4] = 255; editValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i - bmpData.Stride + 4]) { editValues[i - bmpData.Stride + 4] = 255; editValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; shade = true; }

                                if(i + 8 >= 0 && i + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + 8]) { editValues[i + 8] = 255; editValues[i + 8 - 1] = outlineValues[i - 1]; editValues[i + 8 - 2] = outlineValues[i - 2]; editValues[i + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - 8 >= 0 && i - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - 8]) { editValues[i - 8] = 255; editValues[i - 8 - 1] = outlineValues[i - 1]; editValues[i - 8 - 2] = outlineValues[i - 2]; editValues[i - 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2]) { editValues[i + bmpData.Stride * 2] = 255; editValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2]) { editValues[i - bmpData.Stride * 2] = 255; editValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride + 8]) { editValues[i + bmpData.Stride + 8] = 255; editValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride + 8]) { editValues[i - bmpData.Stride + 8] = 255; editValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride - 8]) { editValues[i + bmpData.Stride - 8] = 255; editValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride - 8]) { editValues[i - bmpData.Stride - 8] = 255; editValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2 + 4]) { editValues[i + bmpData.Stride * 2 + 4] = 255; editValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2 - 4]) { editValues[i + bmpData.Stride * 2 - 4] = 255; editValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2 + 4]) { editValues[i - bmpData.Stride * 2 + 4] = 255; editValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2 - 4]) { editValues[i - bmpData.Stride * 2 - 4] = 255; editValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; shade = true; }

                                if(shade)
                                {
                                    editValues[i] = 255; editValues[i - 1] = outlineValues[i - 1]; editValues[i - 2] = outlineValues[i - 2]; editValues[i - 3] = outlineValues[i - 3];
                                }
                            }
                        }
                    }
                    break;
                case Outlining.Partial:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {
                                bool shade = false;

                                if(i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0) { } else if(i + 4 >= 0 && i + 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i + 4]) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0) { } else if(i - 4 >= 0 && i - 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i - 4]) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0) { } else if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && zbuffer[i] - 12 > zbuffer[i + bmpData.Stride]) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0) { } else if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && zbuffer[i] - 12 > zbuffer[i - bmpData.Stride]) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0) { } else if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i + bmpData.Stride + 4]) { editValues[i + bmpData.Stride + 4] = 255; editValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0) { } else if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i - bmpData.Stride - 4]) { editValues[i - bmpData.Stride - 4] = 255; editValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0) { } else if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i + bmpData.Stride - 4]) { editValues[i + bmpData.Stride - 4] = 255; editValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0) { } else if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && zbuffer[i] - 12 > zbuffer[i - bmpData.Stride + 4]) { editValues[i - bmpData.Stride + 4] = 255; editValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; shade = true; }

                                if(i + 8 >= 0 && i + 8 < argbValues.Length && argbValues[i + 8] == 0) { } else if(i + 8 >= 0 && i + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + 8]) { editValues[i + 8] = 255; editValues[i + 8 - 1] = outlineValues[i - 1]; editValues[i + 8 - 2] = outlineValues[i - 2]; editValues[i + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - 8 >= 0 && i - 8 < argbValues.Length && argbValues[i - 8] == 0) { } else if(i - 8 >= 0 && i - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - 8]) { editValues[i - 8] = 255; editValues[i - 8 - 1] = outlineValues[i - 1]; editValues[i - 8 - 2] = outlineValues[i - 2]; editValues[i - 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && argbValues[i + bmpData.Stride * 2] == 0) { } else if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2]) { editValues[i + bmpData.Stride * 2] = 255; editValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && argbValues[i - bmpData.Stride * 2] == 0) { } else if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2]) { editValues[i - bmpData.Stride * 2] = 255; editValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && argbValues[i + bmpData.Stride + 8] == 0) { } else if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride + 8]) { editValues[i + bmpData.Stride + 8] = 255; editValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && argbValues[i - bmpData.Stride + 8] == 0) { } else if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride + 8]) { editValues[i - bmpData.Stride + 8] = 255; editValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && argbValues[i + bmpData.Stride - 8] == 0) { } else if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride - 8]) { editValues[i + bmpData.Stride - 8] = 255; editValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && argbValues[i - bmpData.Stride - 8] == 0) { } else if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride - 8]) { editValues[i - bmpData.Stride - 8] = 255; editValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 8] == 0) { } else if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2 + 8]) { editValues[i + bmpData.Stride * 2 + 8] = 255; editValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 4] == 0) { } else if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2 + 4]) { editValues[i + bmpData.Stride * 2 + 4] = 255; editValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 4] == 0) { } else if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2 - 4]) { editValues[i + bmpData.Stride * 2 - 4] = 255; editValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 8] == 0) { } else if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i + bmpData.Stride * 2 - 8]) { editValues[i + bmpData.Stride * 2 - 8] = 255; editValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 8] == 0) { } else if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2 + 8]) { editValues[i - bmpData.Stride * 2 + 8] = 255; editValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 4] == 0) { } else if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2 + 4]) { editValues[i - bmpData.Stride * 2 + 4] = 255; editValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 4] == 0) { } else if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2 - 4]) { editValues[i - bmpData.Stride * 2 - 4] = 255; editValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; shade = true; }
                                if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 8] == 0) { } else if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && zbuffer[i] - 20 > zbuffer[i - bmpData.Stride * 2 - 8]) { editValues[i - bmpData.Stride * 2 - 8] = 255; editValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; shade = true; }

                                if(shade)
                                {
                                    editValues[i] = 255; editValues[i - 1] = outlineValues[i - 1]; editValues[i - 2] = outlineValues[i - 2]; editValues[i - 3] = outlineValues[i - 3];
                                }

                            }
                        }
                    }
                    break;
            }
            int runningX, runningY;
            byte currentEdit, edit1, edit2, edit3;

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 0)
                    argbValues[i] = 255;
                if(outlineValues[i] == 255) argbValues[i] = 255;
                if((currentEdit = editValues[i]) > 0)
                {
                    argbValues[i] = currentEdit;
                    edit1 = argbValues[i - 1] = editValues[i - 1];
                    edit2 = argbValues[i - 2] = editValues[i - 2];
                    edit3 = argbValues[i - 3] = editValues[i - 3];
                    runningX = i % bWidth;
                    runningY = i / bWidth;
                    if(runningX < 1 || runningX >= bWidth - 1 || runningY < 1 || runningY >= bHeight - 1)
                        continue;
                    if(editValues[i - 4 - bmpData.Stride] > 0)
                    {
                        if(argbValues[i - 4] == 0)
                        {
                            argbValues[i - 4] = currentEdit;
                            argbValues[i - 5] = edit1;
                            argbValues[i - 6] = edit2;
                            argbValues[i - 7] = edit3;
                        }
                        if(argbValues[i - bmpData.Stride] == 0)
                        {
                            argbValues[i - bmpData.Stride] = currentEdit;
                            argbValues[i - bmpData.Stride - 1] = edit1;
                            argbValues[i - bmpData.Stride - 2] = edit2;
                            argbValues[i - bmpData.Stride - 3] = edit3;
                        }
                    }
                    if(editValues[i + 4 - bmpData.Stride] > 0)
                    {
                        if(argbValues[i + 4] == 0)
                        {
                            argbValues[i + 4] = currentEdit;
                            argbValues[i + 3] = edit1;
                            argbValues[i + 2] = edit2;
                            argbValues[i + 1] = edit3;
                        }
                        if(argbValues[i - bmpData.Stride] == 0)
                        {
                            argbValues[i - bmpData.Stride] = currentEdit;
                            argbValues[i - bmpData.Stride - 1] = edit1;
                            argbValues[i - bmpData.Stride - 2] = edit2;
                            argbValues[i - bmpData.Stride - 3] = edit3;
                        }
                    }
                    if(editValues[i - 4 + bmpData.Stride] > 0)
                    {
                        if(argbValues[i - 4] == 0)
                        {
                            argbValues[i - 4] = currentEdit;
                            argbValues[i - 5] = edit1;
                            argbValues[i - 6] = edit2;
                            argbValues[i - 7] = edit3;
                        }
                        if(argbValues[i + bmpData.Stride] == 0)
                        {
                            argbValues[i + bmpData.Stride] = currentEdit;
                            argbValues[i + bmpData.Stride - 1] = edit1;
                            argbValues[i + bmpData.Stride - 2] = edit2;
                            argbValues[i + bmpData.Stride - 3] = edit3;
                        }
                    }
                    if(editValues[i + 4 + bmpData.Stride] > 0)
                    {
                        if(argbValues[i + 4] == 0)
                        {
                            argbValues[i + 4] = currentEdit;
                            argbValues[i + 3] = edit1;
                            argbValues[i + 2] = edit2;
                            argbValues[i + 1] = edit3;
                        }
                        if(argbValues[i + bmpData.Stride] == 0)
                        {
                            argbValues[i + bmpData.Stride] = currentEdit;
                            argbValues[i + bmpData.Stride - 1] = edit1;
                            argbValues[i + bmpData.Stride - 2] = edit2;
                            argbValues[i + bmpData.Stride - 3] = edit3;
                        }
                    }
                }

            }

            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            if(!shrink)
            {
                return bmp;
            }
            else
            {
                Graphics g = Graphics.FromImage(bmp);
                Bitmap b2 = new Bitmap(bWidth / 4, bHeight / 4, PixelFormat.Format32bppArgb);
                Graphics g2 = Graphics.FromImage(b2);
                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g2.DrawImage(bmp.Clone(new Rectangle(0, 0, bWidth, bHeight), bmp.PixelFormat), 0, 0, bWidth / 4, bHeight / 4);
                g2.Dispose();
                return b2;
            }
        }

        private static Bitmap RenderOrthoMultiSize(byte[,,] colors, int xDim, int yDim, int zDim, Outlining o, int multiplier)
        {
            int rows = (xDim / 2 + zDim) * multiplier + 2, cols = yDim * multiplier + 2;

            Bitmap bmp = new Bitmap(cols, rows, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            byte[] outlineValues = new byte[numBytes];

            int xSize = xDim * multiplier, ySize = yDim * multiplier, zSize = zDim * multiplier;
            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);
            int[] xbuffer = new int[numBytes];
            xbuffer.Fill<int>(-999);

            for(int fz = zSize - 1; fz >= 0; fz--)
            {
                for(int fx = xSize - 1; fx >= 0; fx--)
                {
                    for(int fy = 0; fy < ySize; fy++)
                    {
                        if(colors[fx, fy, fz] == 0) continue;
                        int current_color = colors[fx, fy, fz] - 1;
                        for(int i = 0; i < 4; i++)
                        {
                            int p = voxelToPixelGeneric(i, fx, fy, fz, bmpData.Stride, zDim, multiplier);
                            if(argbValues[p] == 0)
                            {
                                if(renderedOrtho[current_color][12] != 0)
                                {
                                    if(fz == zSize - 1 || fx == xSize - 1 || fx == 0)
                                        argbValues[p] = Shade(renderedOrtho[current_color], i, 0, 0, 0);
                                    else
                                        argbValues[p] = Shade(renderedOrtho[current_color], i, colors[fx - 1, fy, fz + 1], colors[fx, fy, fz + 1], colors[fx + 1, fy, fz + 1]);

                                    zbuffer[p] = fz;
                                    xbuffer[p] = fx;

                                    if(outlineValues[p] == 0)
                                        outlineValues[p] = renderedOrtho[current_color][i + 48];
                                }
                            }

                        }
                    }
                }
            }

            int[] xmods = new int[] { -1, 0, 1, -1, 0, 1, -1, 0, 1 }, ymods = new int[] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
            
            switch(o)
            {
                case Outlining.Full:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(argbValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if((zbuffer[i] - zbuffer[i + 4]) > 1 || (xbuffer[i] - xbuffer[i + 4]) > 3) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if((zbuffer[i] - zbuffer[i - 4]) > 1 || (xbuffer[i] - xbuffer[i - 4]) > 3) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride]) > 3) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride]) <= 0)) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) > 3) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) <= 0)) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) > 3) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) <= 0)) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                /*
                                if(argbValues[i + 8] == 0) { outlineValues[i + 8] = 255; } else if((zbuffer[i] - zbuffer[i + 8]) > 1 || (xbuffer[i] - xbuffer[i + 8]) > 3) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - 8] == 0) { outlineValues[i - 8] = 255; } else if((zbuffer[i] - zbuffer[i - 8]) > 1 || (xbuffer[i] - xbuffer[i - 8]) > 3) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2] == 0) { outlineValues[i + bmpData.Stride * 2] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) > 3) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2] == 0) { outlineValues[i - bmpData.Stride * 2] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) <= 0)) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride + 8] == 0) { outlineValues[i + bmpData.Stride + 8] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) > 3) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride + 8] == 0) { outlineValues[i - bmpData.Stride + 8] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) <= 0)) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride - 8] == 0) { outlineValues[i + bmpData.Stride - 8] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) > 3) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride - 8] == 0) { outlineValues[i - bmpData.Stride - 8] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) <= 0)) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 + 8] == 0) { outlineValues[i + bmpData.Stride * 2 + 8] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) > 5) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 + 4] == 0) { outlineValues[i + bmpData.Stride * 2 + 4] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) > 5) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 - 4] == 0) { outlineValues[i + bmpData.Stride * 2 - 4] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) > 5) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 - 8] == 0) { outlineValues[i + bmpData.Stride * 2 - 8] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) > 5) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 + 8] == 0) { outlineValues[i - bmpData.Stride * 2 + 8] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 + 4] == 0) { outlineValues[i - bmpData.Stride * 2 + 4] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 - 4] == 0) { outlineValues[i - bmpData.Stride * 2 - 4] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 - 8] == 0) { outlineValues[i - bmpData.Stride * 2 - 8] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                */
                            }
                        }
                    }
                    break;
                case Outlining.Light:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if((zbuffer[i] - zbuffer[i + 4]) > 1 || (xbuffer[i] - xbuffer[i + 4]) > 3) {
                                    argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3];
                                }
                                if((zbuffer[i] - zbuffer[i - 4]) > 1 || (xbuffer[i] - xbuffer[i - 4]) > 3) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride]) > 3) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride]) <= 0)) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) > 3) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) <= 0)) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) > 3) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) <= 0)) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                /*
                                if((zbuffer[i] - zbuffer[i + 8]) > 1 || (xbuffer[i] - xbuffer[i + 8]) > 3) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - 8]) > 1 || (xbuffer[i] - xbuffer[i - 8]) > 3) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) > 4) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) <= 0)) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) > 3) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) <= 0)) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) > 3) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) <= 0)) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) > 5) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) > 5) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) > 5) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) > 5) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                */
                            }
                        }
                    }
                    break;
                case Outlining.Partial:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(argbValues[i + 4] == 0) { } else if((zbuffer[i] - zbuffer[i + 4]) > 1 || (xbuffer[i] - xbuffer[i + 4]) > 3) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - 4] == 0) { } else if((zbuffer[i] - zbuffer[i - 4]) > 1 || (xbuffer[i] - xbuffer[i - 4]) > 3) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride]) > 3) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride]) <= 0)) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride + 4] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) > 3) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride - 4] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) <= 0)) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride - 4] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) > 3) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride + 4] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) <= 0)) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                /*
                                if(argbValues[i + 8] == 0) { } else if((zbuffer[i] - zbuffer[i + 8]) > 1 || (xbuffer[i] - xbuffer[i + 8]) > 3) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - 8] == 0) { } else if((zbuffer[i] - zbuffer[i - 8]) > 1 || (xbuffer[i] - xbuffer[i - 8]) > 3) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) > 3) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) <= 0)) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride + 8] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) > 3) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride + 8] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) <= 0)) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride - 8] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) > 3) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride - 8] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) <= 0)) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 + 8] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) > 5) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 + 4] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) > 5) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 - 4] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) > 5) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 - 8] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) > 5) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 + 8] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 + 4] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 - 4] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 - 8] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                */
                            }
                        }
                    }
                    break;
            }

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 0) // && argbValues[i] <= 255 * flat_alpha
                    argbValues[i] = 255;
                if(outlineValues[i] == 255) argbValues[i] = 255;
            }

            Marshal.Copy(argbValues, 0, ptr, numBytes);
            
            bmp.UnlockBits(bmpData);
            return bmp;

        }




        private static Bitmap renderSmart(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize, Direction dir, Outlining o, bool shrink)
        {

            byte tsx = (byte)sizex, tsy = (byte)sizey;
            byte hSize = Math.Max(ySize, xSize);

            xSize = hSize;
            ySize = hSize;

            int bWidth = (xSize + ySize) * 2 + 8;
            int bHeight = (xSize + ySize) + zSize * 3 + 8;
            Bitmap bmp = new Bitmap(bWidth, bHeight, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            byte[] bareValues = new byte[numBytes];
            bareValues.Fill<byte>(0);
            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];

            switch(dir)
            {
                case Direction.SE:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            vls[i].x = (byte)(voxels[i].x + (hSize - tsx) / 2);
                            vls[i].y = (byte)(voxels[i].y + (hSize - tsy) / 2);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case Direction.SW:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempY) + (ySize / 2));
                            vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case Direction.NW:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                            vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case Direction.NE:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                            vls[i].y = (byte)(tempX + (xSize / 2));
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
            }
            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            foreach(MagicaVoxelData vx in vls.OrderByDescending(v => v.x * ySize * 2 - v.y + v.z * ySize * xSize * 4)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int p = 0;
                int mod_color = vx.color - 1;
                /*
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[currentColor][0],  0,  0,  0, 0},
   new float[] {0,  colors[currentColor][1],  0,  0, 0},
   new float[] {0,  0,  colors[currentColor][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);*/



                for(int j = 0; j < 4; j++)
                {
                    for(int i = 0; i < 16; i++)
                    {
                        p = voxelToPixel(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, xSize, ySize, zSize);

                        if(argbValues[p] == 0)
                        {
                            zbuffer[p] = vx.z + vx.x - vx.y;
                            argbValues[p] = rendered[mod_color][i + j * 16];
                            if(outlineValues[p] == 0)
                                outlineValues[p] = rendered[mod_color][i + 64]; //(argbValues[p] * 1.2 + 2 < 255) ? (byte)(argbValues[p] * 1.2 + 2) : (byte)255;

                        }
                    }
                }

            }
            switch(o)
            {
                case Outlining.Full:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if(i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if(i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; } else if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; } else if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; } else if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; } else if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if(i + 8 >= 0 && i + 8 < argbValues.Length && argbValues[i + 8] == 0) { outlineValues[i + 8] = 255; } else if(i + 8 >= 0 && i + 8 < argbValues.Length && barePositions[i + 8] == false && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if(i - 8 >= 0 && i - 8 < argbValues.Length && argbValues[i - 8] == 0) { outlineValues[i - 8] = 255; } else if(i - 8 >= 0 && i - 8 < argbValues.Length && barePositions[i - 8] == false && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && argbValues[i + bmpData.Stride * 2] == 0) { outlineValues[i + bmpData.Stride * 2] = 255; } else if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && argbValues[i - bmpData.Stride * 2] == 0) { outlineValues[i - bmpData.Stride * 2] = 255; } else if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && argbValues[i + bmpData.Stride + 8] == 0) { outlineValues[i + bmpData.Stride + 8] = 255; } else if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && argbValues[i - bmpData.Stride + 8] == 0) { outlineValues[i - bmpData.Stride + 8] = 255; } else if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && argbValues[i + bmpData.Stride - 8] == 0) { outlineValues[i + bmpData.Stride - 8] = 255; } else if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && argbValues[i - bmpData.Stride - 8] == 0) { outlineValues[i - bmpData.Stride - 8] = 255; } else if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 8] == 0) { outlineValues[i + bmpData.Stride * 2 + 8] = 255; } else if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 4] == 0) { outlineValues[i + bmpData.Stride * 2 + 4] = 255; } else if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 4] == 0) { outlineValues[i + bmpData.Stride * 2 - 4] = 255; } else if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 8] == 0) { outlineValues[i + bmpData.Stride * 2 - 8] = 255; } else if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 8] == 0) { outlineValues[i - bmpData.Stride * 2 + 8] = 255; } else if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 4] == 0) { outlineValues[i - bmpData.Stride * 2 + 4] = 255; } else if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 4] == 0) { outlineValues[i - bmpData.Stride * 2 - 4] = 255; } else if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 8] == 0) { outlineValues[i - bmpData.Stride * 2 - 8] = 255; } else if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
                case Outlining.Light:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if(i + 8 >= 0 && i + 8 < argbValues.Length && barePositions[i + 8] == false && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if(i - 8 >= 0 && i - 8 < argbValues.Length && barePositions[i - 8] == false && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
                case Outlining.Partial:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0) { } else if(i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0) { } else if(i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0) { } else if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0) { } else if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0) { } else if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0) { } else if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0) { } else if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0) { } else if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if(i + 8 >= 0 && i + 8 < argbValues.Length && argbValues[i + 8] == 0) { } else if(i + 8 >= 0 && i + 8 < argbValues.Length && barePositions[i + 8] == false && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if(i - 8 >= 0 && i - 8 < argbValues.Length && argbValues[i - 8] == 0) { } else if(i - 8 >= 0 && i - 8 < argbValues.Length && barePositions[i - 8] == false && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && argbValues[i + bmpData.Stride * 2] == 0) { } else if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && argbValues[i - bmpData.Stride * 2] == 0) { } else if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && argbValues[i + bmpData.Stride + 8] == 0) { } else if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && argbValues[i - bmpData.Stride + 8] == 0) { } else if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && argbValues[i + bmpData.Stride - 8] == 0) { } else if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && argbValues[i - bmpData.Stride - 8] == 0) { } else if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 8] == 0) { } else if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 4] == 0) { } else if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 4] == 0) { } else if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 8] == 0) { } else if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 8] == 0) { } else if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 4] == 0) { } else if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 4] == 0) { } else if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 8] == 0) { } else if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
            }

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 0) // && argbValues[i] <= 255 * flat_alpha
                    argbValues[i] = 255;
                if(outlineValues[i] == 255) argbValues[i] = 255;
            }

            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            if(!shrink)
            {
                return bmp;
            }
            else
            {
                Graphics g = Graphics.FromImage(bmp);
                Bitmap b2 = new Bitmap(bWidth / 2, bHeight / 2, PixelFormat.Format32bppArgb);
                Graphics g2 = Graphics.FromImage(b2);
                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g2.DrawImage(bmp.Clone(new Rectangle(0, 0, bWidth, bHeight), bmp.PixelFormat), 0, 0, bWidth / 2, bHeight / 2);
                g2.Dispose();
                return b2;
            }
        }

        private static Bitmap renderSmartOrtho(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize, OrthoDirection dir, Outlining o, bool shrink)
        {
            byte tsx = (byte)sizex, tsy = (byte)sizey;

            byte hSize = Math.Max(ySize, xSize);

            xSize = hSize;
            ySize = hSize;

            int bWidth = ySize * 3 + 8;
            int bHeight = xSize + zSize * 3 + 8;
            Bitmap bmp = new Bitmap(bWidth, bHeight, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];

            switch(dir)
            {
                case OrthoDirection.S:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            vls[i].x = (byte)(voxels[i].x + (hSize - tsx) / 2);
                            vls[i].y = (byte)(voxels[i].y + (hSize - tsy) / 2);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case OrthoDirection.W:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempY) + (ySize / 2));
                            vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case OrthoDirection.N:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                            vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case OrthoDirection.E:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                            vls[i].y = (byte)(tempX + (xSize / 2));
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
            }

            int[] xbuffer = new int[numBytes];
            xbuffer.Fill<int>(-999);
            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            foreach(MagicaVoxelData vx in vls.OrderByDescending(v => v.x * ySize * 4 + v.y + v.z * ySize * xSize * 4)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {

                int p = 0;
                int mod_color = vx.color - 1;

                for(int j = 0; j < 4; j++)
                {
                    for(int i = 0; i < 12; i++)
                    {
                        p = voxelToPixelOrtho(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, xSize, ySize, zSize);
                        if(argbValues[p] == 0)
                        {
                            argbValues[p] = renderedOrtho[mod_color][i + j * 12];
                            zbuffer[p] = vx.z;
                            xbuffer[p] = vx.x;
                            if(outlineValues[p] == 0)
                                outlineValues[p] = renderedOrtho[mod_color][i + 48];
                        }
                    }
                }

            }

            switch(o)
            {
                case Outlining.Full:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(argbValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if((zbuffer[i] - zbuffer[i + 4]) > 1 || (xbuffer[i] - xbuffer[i + 4]) > 3) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if((zbuffer[i] - zbuffer[i - 4]) > 1 || (xbuffer[i] - xbuffer[i - 4]) > 3) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride]) > 3) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride]) <= 0)) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) > 3) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) <= 0)) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) > 3) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) <= 0)) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if(argbValues[i + 8] == 0) { outlineValues[i + 8] = 255; } else if((zbuffer[i] - zbuffer[i + 8]) > 1 || (xbuffer[i] - xbuffer[i + 8]) > 3) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - 8] == 0) { outlineValues[i - 8] = 255; } else if((zbuffer[i] - zbuffer[i - 8]) > 1 || (xbuffer[i] - xbuffer[i - 8]) > 3) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2] == 0) { outlineValues[i + bmpData.Stride * 2] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) > 3) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2] == 0) { outlineValues[i - bmpData.Stride * 2] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) <= 0)) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride + 8] == 0) { outlineValues[i + bmpData.Stride + 8] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) > 3) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride + 8] == 0) { outlineValues[i - bmpData.Stride + 8] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) <= 0)) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride - 8] == 0) { outlineValues[i + bmpData.Stride - 8] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) > 3) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride - 8] == 0) { outlineValues[i - bmpData.Stride - 8] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) <= 0)) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 + 8] == 0) { outlineValues[i + bmpData.Stride * 2 + 8] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) > 5) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 + 4] == 0) { outlineValues[i + bmpData.Stride * 2 + 4] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) > 5) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 - 4] == 0) { outlineValues[i + bmpData.Stride * 2 - 4] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) > 5) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 - 8] == 0) { outlineValues[i + bmpData.Stride * 2 - 8] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) > 5) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 + 8] == 0) { outlineValues[i - bmpData.Stride * 2 + 8] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 + 4] == 0) { outlineValues[i - bmpData.Stride * 2 + 4] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 - 4] == 0) { outlineValues[i - bmpData.Stride * 2 - 4] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 - 8] == 0) { outlineValues[i - bmpData.Stride * 2 - 8] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
                case Outlining.Light:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if((zbuffer[i] - zbuffer[i + 4]) > 1 || (xbuffer[i] - xbuffer[i + 4]) > 3) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - 4]) > 1 || (xbuffer[i] - xbuffer[i - 4]) > 3) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride]) > 3) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride]) <= 0)) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) > 3) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) <= 0)) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) > 3) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) <= 0)) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if((zbuffer[i] - zbuffer[i + 8]) > 1 || (xbuffer[i] - xbuffer[i + 8]) > 3) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - 8]) > 1 || (xbuffer[i] - xbuffer[i - 8]) > 3) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) > 4) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) <= 0)) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) > 3) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) <= 0)) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) > 3) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) <= 0)) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) > 5) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) > 5) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) > 5) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) > 5) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
                case Outlining.Partial:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(argbValues[i + 4] == 0) { } else if((zbuffer[i] - zbuffer[i + 4]) > 1 || (xbuffer[i] - xbuffer[i + 4]) > 3) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - 4] == 0) { } else if((zbuffer[i] - zbuffer[i - 4]) > 1 || (xbuffer[i] - xbuffer[i - 4]) > 3) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride]) > 3) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride]) <= 0)) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride + 4] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) > 3) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride - 4] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) <= 0)) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride - 4] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) > 3) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride + 4] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) <= 0)) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if(argbValues[i + 8] == 0) { } else if((zbuffer[i] - zbuffer[i + 8]) > 1 || (xbuffer[i] - xbuffer[i + 8]) > 3) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - 8] == 0) { } else if((zbuffer[i] - zbuffer[i - 8]) > 1 || (xbuffer[i] - xbuffer[i - 8]) > 3) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) > 3) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) <= 0)) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride + 8] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) > 3) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride + 8] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) <= 0)) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride - 8] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) > 3) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride - 8] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) <= 0)) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 + 8] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) > 5) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 + 4] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) > 5) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 - 4] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) > 5) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride * 2 - 8] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) > 5) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 + 8] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 + 4] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 - 4] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride * 2 - 8] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
            }

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 0) // && argbValues[i] <= 255 * flat_alpha
                    argbValues[i] = 255;
                if(outlineValues[i] == 255) argbValues[i] = 255;
            }

            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
            if(!shrink)
            {
                return bmp;
            }
            else
            {
                Graphics g = Graphics.FromImage(bmp);
                Bitmap b2 = new Bitmap(bWidth / 2, bHeight / 2, PixelFormat.Format32bppArgb);
                Graphics g2 = Graphics.FromImage(b2);
                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g2.DrawImage(bmp.Clone(new Rectangle(0, 0, bWidth, bHeight), bmp.PixelFormat), 0, 0, bWidth / 2, bHeight / 2);
                g2.Dispose();
                return b2;
            }
        }


        private static Bitmap renderSmart45(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize, Direction dir, Outlining o, bool shrink)
        {

            byte tsx = (byte)sizex, tsy = (byte)sizey;
            byte hSize = Math.Max(ySize, xSize);

            xSize = hSize;
            ySize = hSize;

            int bWidth = hSize * 4 + 4;
            int bHeight = hSize * 4 + zSize * 2 + 4;
            Bitmap bmp = new Bitmap(bWidth, bHeight, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            byte[] bareValues = new byte[numBytes];
            bareValues.Fill<byte>(0);
            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];

            switch(dir)
            {
                case Direction.SE:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            vls[i].x = (byte)(voxels[i].x + (hSize - tsx >> 1));
                            vls[i].y = (byte)(voxels[i].y + (hSize - tsy >> 1));
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case Direction.SW:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + (hSize - tsx >> 1) - (xSize >> 1));
                            byte tempY = (byte)(voxels[i].y + (hSize - tsy >> 1) - (ySize >> 1));
                            vls[i].x = (byte)((tempY) + (ySize >> 1));
                            vls[i].y = (byte)((-1 - tempX) + (xSize >> 1));
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case Direction.NW:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + (hSize - tsx >> 1) - (xSize >> 1));
                            byte tempY = (byte)(voxels[i].y + (hSize - tsy >> 1) - (ySize >> 1));
                            vls[i].x = (byte)((-1 - tempX) + (xSize >> 1));
                            vls[i].y = (byte)((-1 - tempY) + (ySize >> 1));
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case Direction.NE:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + (hSize - tsx >> 1) - (xSize >> 1));
                            byte tempY = (byte)(voxels[i].y + (hSize - tsy >> 1) - (ySize >> 1));
                            vls[i].x = (byte)((-1 - tempY) + (ySize >> 1));
                            vls[i].y = (byte)(tempX + (xSize >> 1));
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
            }
            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            foreach(MagicaVoxelData vx in vls.OrderByDescending(v => v.x * xSize * 2 - v.y + v.z * xSize * ySize * 4)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int p = 0;
                int mod_color = vx.color - 1;
                /*
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[currentColor][0],  0,  0,  0, 0},
   new float[] {0,  colors[currentColor][1],  0,  0, 0},
   new float[] {0,  0,  colors[currentColor][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);*/



                for(int j = 0; j < 4; j++)
                {
                    for(int i = 0; i < 16; i++)
                    {
                        p = voxelToPixel45(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, xSize, ySize, zSize);

                        if(argbValues[p] == 0)
                        {
                            zbuffer[p] = vx.z + (vx.x - vx.y) * 2;
                            argbValues[p] = rendered45[mod_color][((i & 3)|4*(i>>3)) + (j>>1) * 8];
                            if(outlineValues[p] == 0)
                                outlineValues[p] = rendered45[mod_color][(i & 3) + 16]; //(argbValues[p] * 1.2 + 2 < 255) ? (byte)(argbValues[p] * 1.2 + 2) : (byte)255;

                        }
                    }
                }

            }
            switch(o)
            {
                case Outlining.Full:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if(i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 7 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if(i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 7 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; } else if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; } else if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; } else if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; } else if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(shrink)
                                {
                                    if(i + 8 >= 0 && i + 8 < argbValues.Length && argbValues[i + 8] == 0) { outlineValues[i + 8] = 255; } else if(i + 8 >= 0 && i + 8 < argbValues.Length && barePositions[i + 8] == false && zbuffer[i] - 7 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                    if(i - 8 >= 0 && i - 8 < argbValues.Length && argbValues[i - 8] == 0) { outlineValues[i - 8] = 255; } else if(i - 8 >= 0 && i - 8 < argbValues.Length && barePositions[i - 8] == false && zbuffer[i] - 7 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && argbValues[i + bmpData.Stride * 2] == 0) { outlineValues[i + bmpData.Stride * 2] = 255; } else if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && argbValues[i - bmpData.Stride * 2] == 0) { outlineValues[i - bmpData.Stride * 2] = 255; } else if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && argbValues[i + bmpData.Stride + 8] == 0) { outlineValues[i + bmpData.Stride + 8] = 255; } else if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && argbValues[i - bmpData.Stride + 8] == 0) { outlineValues[i - bmpData.Stride + 8] = 255; } else if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && argbValues[i + bmpData.Stride - 8] == 0) { outlineValues[i + bmpData.Stride - 8] = 255; } else if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && argbValues[i - bmpData.Stride - 8] == 0) { outlineValues[i - bmpData.Stride - 8] = 255; } else if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 8] == 0) { outlineValues[i + bmpData.Stride * 2 + 8] = 255; } else if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 4] == 0) { outlineValues[i + bmpData.Stride * 2 + 4] = 255; } else if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 4] == 0) { outlineValues[i + bmpData.Stride * 2 - 4] = 255; } else if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 8] == 0) { outlineValues[i + bmpData.Stride * 2 - 8] = 255; } else if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 8] == 0) { outlineValues[i - bmpData.Stride * 2 + 8] = 255; } else if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 4] == 0) { outlineValues[i - bmpData.Stride * 2 + 4] = 255; } else if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 4] == 0) { outlineValues[i - bmpData.Stride * 2 - 4] = 255; } else if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 8] == 0) { outlineValues[i - bmpData.Stride * 2 - 8] = 255; } else if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                }
                            }
                        }
                    }
                    break;
                case Outlining.Light:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 7 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 7 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(shrink)
                                {
                                    if(i + 8 >= 0 && i + 8 < argbValues.Length && barePositions[i + 8] == false && zbuffer[i] - 7 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                    if(i - 8 >= 0 && i - 8 < argbValues.Length && barePositions[i - 8] == false && zbuffer[i] - 7 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                }
                            }
                        }
                    }
                    break;
                case Outlining.Partial:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0) { } else if(i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 7 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0) { } else if(i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 7 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0) { } else if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0) { } else if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0) { } else if(i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0) { } else if(i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0) { } else if(i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0) { } else if(i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(shrink)
                                {
                                    if(i + 8 >= 0 && i + 8 < argbValues.Length && argbValues[i + 8] == 0) { } else if(i + 8 >= 0 && i + 8 < argbValues.Length && barePositions[i + 8] == false && zbuffer[i] - 7 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                    if(i - 8 >= 0 && i - 8 < argbValues.Length && argbValues[i - 8] == 0) { } else if(i - 8 >= 0 && i - 8 < argbValues.Length && barePositions[i - 8] == false && zbuffer[i] - 7 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && argbValues[i + bmpData.Stride * 2] == 0) { } else if(i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && argbValues[i - bmpData.Stride * 2] == 0) { } else if(i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && argbValues[i + bmpData.Stride + 8] == 0) { } else if(i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && argbValues[i - bmpData.Stride + 8] == 0) { } else if(i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && argbValues[i + bmpData.Stride - 8] == 0) { } else if(i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && argbValues[i - bmpData.Stride - 8] == 0) { } else if(i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 8] == 0) { } else if(i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 4] == 0) { } else if(i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 4] == 0) { } else if(i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                    if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 8] == 0) { } else if(i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 8] == 0) { } else if(i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 4] == 0) { } else if(i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 4] == 0) { } else if(i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                    if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 8] == 0) { } else if(i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                }
                            }
                        }
                    }
                    break;
            }

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 0) // && argbValues[i] <= 255 * flat_alpha
                    argbValues[i] = 255;
                if(outlineValues[i] == 255) argbValues[i] = 255;
            }

            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            if(!shrink)
            {
                return bmp;
            }
            else
            {
                Graphics g = Graphics.FromImage(bmp);
                Bitmap b2 = new Bitmap(bWidth / 2, bHeight / 2, PixelFormat.Format32bppArgb);
                Graphics g2 = Graphics.FromImage(b2);
                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g2.DrawImage(bmp.Clone(new Rectangle(0, 0, bWidth, bHeight), bmp.PixelFormat), 0, 0, bWidth / 2, bHeight / 2);
                g2.Dispose();
                return b2;
            }
        }

        private static Bitmap renderSmartOrtho45(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize, OrthoDirection dir, Outlining o, bool shrink)
        {
            byte tsx = (byte)sizex, tsy = (byte)sizey;

            byte hSize = Math.Max(ySize, xSize);

            xSize = hSize;
            ySize = hSize;

            int bWidth = hSize * 3 + 4;
            int bHeight = hSize * 3 + zSize * 2 + 4;
            Bitmap bmp = new Bitmap(bWidth, bHeight, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];

            switch(dir)
            {
                case OrthoDirection.S:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            vls[i].x = (byte)(voxels[i].x + (hSize - tsx) / 2);
                            vls[i].y = (byte)(voxels[i].y + (hSize - tsy) / 2);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case OrthoDirection.W:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempY) + (ySize / 2));
                            vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case OrthoDirection.N:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                            vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case OrthoDirection.E:
                    {
                        for(int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                            vls[i].y = (byte)(tempX + (xSize / 2));
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
            }

            int[] xbuffer = new int[numBytes];
            xbuffer.Fill<int>(-999);
            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            foreach(MagicaVoxelData vx in vls.OrderByDescending(v => v.x * ySize * 4 + v.y + v.z * ySize * xSize * 4)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {

                int p = 0;
                int mod_color = vx.color - 1;

                for(int j = 0; j < 5; j++)
                {
                    for(int i = 0; i < 12; i++)
                    {
                        p = voxelToPixelOrtho45(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, xSize, ySize, zSize);
                        if(argbValues[p] == 0)
                        {
                            argbValues[p] = renderedOrtho[mod_color][i + (j/3) * 12];
                            zbuffer[p] = vx.z;
                            xbuffer[p] = vx.x;
                            if(outlineValues[p] == 0)
                                outlineValues[p] = renderedOrtho[mod_color][i + 48];
                        }
                    }
                }
            }
            switch(o)
            {
                case Outlining.Full:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(argbValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if((zbuffer[i] - zbuffer[i + 4]) > 2 || (xbuffer[i] - xbuffer[i + 4]) > 3) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if((zbuffer[i] - zbuffer[i - 4]) > 2 || (xbuffer[i] - xbuffer[i - 4]) > 3) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride]) > 3) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride]) <= 0)) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) > 3) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) <= 0)) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) > 3) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) <= 0)) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(shrink)
                                {
                                    if(argbValues[i + 8] == 0) { outlineValues[i + 8] = 255; } else if((zbuffer[i] - zbuffer[i + 8]) > 2 || (xbuffer[i] - xbuffer[i + 8]) > 3) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - 8] == 0) { outlineValues[i - 8] = 255; } else if((zbuffer[i] - zbuffer[i - 8]) > 2 || (xbuffer[i] - xbuffer[i - 8]) > 3) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride * 2] == 0) { outlineValues[i + bmpData.Stride * 2] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) > 3) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride * 2] == 0) { outlineValues[i - bmpData.Stride * 2] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) <= 0)) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride + 8] == 0) { outlineValues[i + bmpData.Stride + 8] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) > 3) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride + 8] == 0) { outlineValues[i - bmpData.Stride + 8] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) <= 0)) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride - 8] == 0) { outlineValues[i + bmpData.Stride - 8] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) > 3) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride - 8] == 0) { outlineValues[i - bmpData.Stride - 8] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) <= 0)) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride * 2 + 8] == 0) { outlineValues[i + bmpData.Stride * 2 + 8] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) > 5) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride * 2 + 4] == 0) { outlineValues[i + bmpData.Stride * 2 + 4] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) > 5) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride * 2 - 4] == 0) { outlineValues[i + bmpData.Stride * 2 - 4] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) > 5) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride * 2 - 8] == 0) { outlineValues[i + bmpData.Stride * 2 - 8] = 255; } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) > 5) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride * 2 + 8] == 0) { outlineValues[i - bmpData.Stride * 2 + 8] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride * 2 + 4] == 0) { outlineValues[i - bmpData.Stride * 2 + 4] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride * 2 - 4] == 0) { outlineValues[i - bmpData.Stride * 2 - 4] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride * 2 - 8] == 0) { outlineValues[i - bmpData.Stride * 2 - 8] = 255; } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                }
                            }
                        }
                    }
                    break;
                case Outlining.Light:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if((zbuffer[i] - zbuffer[i + 4]) > 2 || (xbuffer[i] - xbuffer[i + 4]) > 3) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - 4]) > 2 || (xbuffer[i] - xbuffer[i - 4]) > 3) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride]) > 2 || ((xbuffer[i] - xbuffer[i + bmpData.Stride]) > 3 && (zbuffer[i] - zbuffer[i + bmpData.Stride]) <= 0)) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride]) > 3 && (zbuffer[i] - zbuffer[i - bmpData.Stride]) <= 0)) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) > 2 || ((xbuffer[i] - xbuffer[i + bmpData.Stride - 4]) > 3 && (zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) <= 0)) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 4]) > 3 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) <= 0)) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) > 2 || ((xbuffer[i] - xbuffer[i + bmpData.Stride + 4]) > 3 && (zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) <= 0)) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if((zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 4]) > 3 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) <= 0)) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(shrink)
                                {
                                    if((zbuffer[i] - zbuffer[i + 8]) > 2 || (xbuffer[i] - xbuffer[i + 8]) > 3) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i - 8]) > 2 || (xbuffer[i] - xbuffer[i - 8]) > 3) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) > 2 || ((xbuffer[i] - xbuffer[i + bmpData.Stride * 2]) > 3 && (zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) <= 0)) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2]) > 3 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) <= 0)) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) > 2 || ((xbuffer[i] - xbuffer[i + bmpData.Stride + 8]) > 3 && (zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) <= 0)) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 8]) > 3 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) <= 0)) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) > 2 || ((xbuffer[i] - xbuffer[i + bmpData.Stride - 8]) > 3 && (zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) <= 0)) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 8]) > 3 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) <= 0)) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }

                                    if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) > 2 || ((xbuffer[i] - xbuffer[i + bmpData.Stride * 2 + 8]) > 3 && (zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) > 2 || ((xbuffer[i] - xbuffer[i + bmpData.Stride * 2 + 4]) > 3 && (zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) > 2 || ((xbuffer[i] - xbuffer[i + bmpData.Stride * 2 - 4]) > 3 && (zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) > 2 || ((xbuffer[i] - xbuffer[i + bmpData.Stride * 2 - 8]) > 3 && (zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }

                                    if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 8]) > 3 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 4]) > 3 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 4]) > 3 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                    if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 8]) > 3 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                }
                            }
                        }
                    }
                    break;
                case Outlining.Partial:
                    {
                        for(int i = 3; i < numBytes; i += 4)
                        {
                            if(argbValues[i] > 0)
                            {

                                if(argbValues[i + 4] == 0) { } else if((zbuffer[i] - zbuffer[i + 4]) > 2 || (xbuffer[i] - xbuffer[i + 4]) > 3) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - 4] == 0) { } else if((zbuffer[i] - zbuffer[i - 4]) > 2 || (xbuffer[i] - xbuffer[i - 4]) > 3) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride]) > 3) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride]) <= 0)) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride + 4] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) > 3) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride - 4] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) <= 0)) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i + bmpData.Stride - 4] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) > 3) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if(argbValues[i - bmpData.Stride + 4] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) <= 0)) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if(shrink)
                                {
                                    if(argbValues[i + 8] == 0) { } else if((zbuffer[i] - zbuffer[i + 8]) > 2 || (xbuffer[i] - xbuffer[i + 8]) > 3) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - 8] == 0) { } else if((zbuffer[i] - zbuffer[i - 8]) > 2 || (xbuffer[i] - xbuffer[i - 8]) > 3) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride * 2] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) > 3) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride * 2] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) <= 0)) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride + 8] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) > 3) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride + 8] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) <= 0)) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride - 8] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) > 3) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride - 8] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) > 2 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) <= 0)) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride * 2 + 8] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) > 5) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride * 2 + 4] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) > 5) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride * 2 - 4] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) > 5) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i + bmpData.Stride * 2 - 8] == 0) { } else if((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) > 5) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride * 2 + 8] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride * 2 + 4] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride * 2 - 4] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                    if(argbValues[i - bmpData.Stride * 2 - 8] == 0) { } else if((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                }
                            }
                        }
                    }
                    break;
            }

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 0) // && argbValues[i] <= 255 * flat_alpha
                    argbValues[i] = 255;
                if(outlineValues[i] == 255) argbValues[i] = 255;
            }

            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
            if(!shrink)
            {
                return bmp;
            }
            else
            {
                Graphics g = Graphics.FromImage(bmp);
                Bitmap b2 = new Bitmap(bWidth / 2, bHeight / 2, PixelFormat.Format32bppArgb);
                Graphics g2 = Graphics.FromImage(b2);
                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g2.DrawImage(bmp.Clone(new Rectangle(0, 0, bWidth, bHeight), bmp.PixelFormat), 0, 0, bWidth / 2, bHeight / 2);
                g2.Dispose();
                return b2;
            }
            
        }


        /*
        public static Bitmap processSingleOutlined(MagicaVoxelData[] parsed, byte xSize, byte ySize, byte zSize, Direction dir)
        {
            Graphics g;
            Bitmap b, o;
            b = render(parsed, xSize, ySize, zSize, dir);
            o = renderOutline(parsed, xSize, ySize, zSize, dir);
            g = Graphics.FromImage(o);
            g.DrawImage(b, 2,6);
            return o;
        }
        private static void processUnitOutlined(MagicaVoxelData[] parsed, string u, byte xSize, byte ySize, byte zSize)
        {
            u = u.Substring(0, u.Length - 4);
            System.IO.Directory.CreateDirectory(u);

            processSingleOutlined(parsed, xSize, ySize, zSize, Direction.SE).Save(u + SEP + u + "_outline_SE" + ".png", ImageFormat.Png); //se
            processSingleOutlined(parsed, xSize, ySize, zSize, Direction.SW).Save(u + SEP + u + "_outline_SW" + ".png", ImageFormat.Png); //sw
            processSingleOutlined(parsed, xSize, ySize, zSize, Direction.NW).Save(u + SEP + u + "_outline_NW" + ".png", ImageFormat.Png); //nw
            processSingleOutlined(parsed, xSize, ySize, zSize, Direction.NE).Save(u + SEP + u + "_outline_NE" + ".png", ImageFormat.Png); //ne

        }
        private static void processUnit(MagicaVoxelData[] parsed, string u, byte xSize, byte ySize, byte zSize)
        {
            u = u.Substring(0, u.Length - 4);
            System.IO.Directory.CreateDirectory(u);

            render(parsed, xSize, ySize, zSize, Direction.SE).Save(u + SEP + u + "_SE" + ".png", ImageFormat.Png); //se
            render(parsed, xSize, ySize, zSize, Direction.SW).Save(u + SEP + u + "_SW" + ".png", ImageFormat.Png); //sw
            render(parsed, xSize, ySize, zSize, Direction.NW).Save(u + SEP + u + "_NW" + ".png", ImageFormat.Png); //nw
            render(parsed, xSize, ySize, zSize, Direction.NE).Save(u + SEP + u + "_NE" + ".png", ImageFormat.Png); //ne

        }
        */
        public static void processUnitSmart(MagicaVoxelData[] parsed, string u, byte xSize, byte ySize, byte zSize, Outlining o, int multiplier)
        {
            if(xSize <= sizex) xSize = (byte)(sizex);
            if(ySize <= sizey) ySize = (byte)(sizey);
            if(zSize <= sizez) zSize = (byte)(sizez);
            if(xSize % 2 == 1) xSize++;
            if(ySize % 2 == 1) ySize++;
            if(zSize % 2 == 1) zSize++;

            u = u.Substring(0, u.Length - 4);
            DirectoryInfo di = Directory.CreateDirectory(u);
            u = di.Name;

            renderSmart(parsed, xSize, ySize, zSize, Direction.SE, o, true).Save(di.FullName + SEP + u + "_SE" + ".png", ImageFormat.Png); //se
            renderSmart(parsed, xSize, ySize, zSize, Direction.SW, o, true).Save(di.FullName + SEP + u + "_SW" + ".png", ImageFormat.Png); //sw
            renderSmart(parsed, xSize, ySize, zSize, Direction.NW, o, true).Save(di.FullName + SEP + u + "_NW" + ".png", ImageFormat.Png); //nw
            renderSmart(parsed, xSize, ySize, zSize, Direction.NE, o, true).Save(di.FullName + SEP + u + "_NE" + ".png", ImageFormat.Png); //ne

            renderSmart45(parsed, xSize, ySize, zSize, Direction.SE, o, true).Save(di.FullName + SEP + u + "_Above_SE" + ".png", ImageFormat.Png); //se
            renderSmart45(parsed, xSize, ySize, zSize, Direction.SW, o, true).Save(di.FullName + SEP + u + "_Above_SW" + ".png", ImageFormat.Png); //sw
            renderSmart45(parsed, xSize, ySize, zSize, Direction.NW, o, true).Save(di.FullName + SEP + u + "_Above_NW" + ".png", ImageFormat.Png); //nw
            renderSmart45(parsed, xSize, ySize, zSize, Direction.NE, o, true).Save(di.FullName + SEP + u + "_Above_NE" + ".png", ImageFormat.Png); //ne

            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.S, o, true).Save(di.FullName + SEP + u + "_S" + ".png", ImageFormat.Png); //s
            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.W, o, true).Save(di.FullName + SEP + u + "_W" + ".png", ImageFormat.Png); //w
            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.N, o, true).Save(di.FullName + SEP + u + "_N" + ".png", ImageFormat.Png); //n
            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.E, o, true).Save(di.FullName + SEP + u + "_E" + ".png", ImageFormat.Png); //e

            renderSmartOrtho45(parsed, xSize, ySize, zSize, OrthoDirection.S, o, true).Save(di.FullName + SEP + u + "_Above_S" + ".png", ImageFormat.Png); //s
            renderSmartOrtho45(parsed, xSize, ySize, zSize, OrthoDirection.W, o, true).Save(di.FullName + SEP + u + "_Above_W" + ".png", ImageFormat.Png); //w
            renderSmartOrtho45(parsed, xSize, ySize, zSize, OrthoDirection.N, o, true).Save(di.FullName + SEP + u + "_Above_N" + ".png", ImageFormat.Png); //n
            renderSmartOrtho45(parsed, xSize, ySize, zSize, OrthoDirection.E, o, true).Save(di.FullName + SEP + u + "_Above_E" + ".png", ImageFormat.Png); //e

            byte[,,] colors = TransformLogic.VoxListToArray(parsed, xSize, ySize, zSize);
            FaceVoxel[,,] faces0 = FaceLogic.GetFaces(colors),
                faces1 = FaceLogic.GetFaces(TransformLogic.RotateYaw(colors, 90)),
                faces2 = FaceLogic.GetFaces(TransformLogic.RotateYaw(colors, 180)),
                faces3 = FaceLogic.GetFaces(TransformLogic.RotateYaw(colors, 270));
            renderSmartFaces(faces0, xSize, ySize, zSize, o, true).Save(di.FullName + SEP + u + "_Slope_SE" + ".png", ImageFormat.Png); //se
            renderSmartFaces(faces1, ySize, xSize, zSize, o, true).Save(di.FullName + SEP + u + "_Slope_SW" + ".png", ImageFormat.Png); //sw
            renderSmartFaces(faces2, xSize, ySize, zSize, o, true).Save(di.FullName + SEP + u + "_Slope_NW" + ".png", ImageFormat.Png); //nw
            renderSmartFaces(faces3, ySize, xSize, zSize, o, true).Save(di.FullName + SEP + u + "_Slope_NE" + ".png", ImageFormat.Png); //ne

            renderSmartFacesSmall(faces0, xSize, ySize, zSize, o, true).Save(di.FullName + SEP + u + "_Small_Slope_SE" + ".png", ImageFormat.Png); //se
            renderSmartFacesSmall(faces1, ySize, xSize, zSize, o, true).Save(di.FullName + SEP + u + "_Small_Slope_SW" + ".png", ImageFormat.Png); //sw
            renderSmartFacesSmall(faces2, xSize, ySize, zSize, o, true).Save(di.FullName + SEP + u + "_Small_Slope_NW" + ".png", ImageFormat.Png); //nw
            renderSmartFacesSmall(faces3, ySize, xSize, zSize, o, true).Save(di.FullName + SEP + u + "_Small_Slope_NE" + ".png", ImageFormat.Png); //ne


            for(int s = 1; s <= multiplier; s++)
            {
                byte[,,] colors2;
                if(s > 1) colors2 = TransformLogic.RunCA(TransformLogic.ScalePartial(colors, s), s);
                else colors2 = colors.Replicate();
                RenderOrthoMultiSize(TransformLogic.SealGaps(colors2), xSize, ySize, zSize, o, s).Save(di.FullName + SEP + u + "_Size"+ s + "_S" + ".png", ImageFormat.Png); //s
                RenderOrthoMultiSize(TransformLogic.SealGaps(TransformLogic.RotateYaw(colors2, 90)), ySize, xSize, zSize, o, s).Save(di.FullName + SEP + u + "_Size" + s + "_W" + ".png", ImageFormat.Png); //w
                RenderOrthoMultiSize(TransformLogic.SealGaps(TransformLogic.RotateYaw(colors2, 180)), xSize, ySize, zSize, o, s).Save(di.FullName + SEP + u + "_Size" + s + "_N" + ".png", ImageFormat.Png); //n
                RenderOrthoMultiSize(TransformLogic.SealGaps(TransformLogic.RotateYaw(colors2, 270)), ySize, xSize, zSize, o, s).Save(di.FullName + SEP + u + "_Size" + s + "_E" + ".png", ImageFormat.Png); //e
            }
            xSize *= 2;
            ySize *= 2;
            zSize *= 2;
            sizex *= 2;
            sizey *= 2;
            sizez *= 2;
            parsed = TransformLogic.VoxArrayToList(FaceLogic.FaceArrayToByteArray(FaceLogic.DoubleSize(faces0))).ToArray();
            renderSmart(parsed, xSize, ySize, zSize, Direction.SE, o, true).Save(di.FullName + SEP + u + "_Big_SE" + ".png", ImageFormat.Png); //se
            renderSmart(parsed, xSize, ySize, zSize, Direction.SW, o, true).Save(di.FullName + SEP + u + "_Big_SW" + ".png", ImageFormat.Png); //sw
            renderSmart(parsed, xSize, ySize, zSize, Direction.NW, o, true).Save(di.FullName + SEP + u + "_Big_NW" + ".png", ImageFormat.Png); //nw
            renderSmart(parsed, xSize, ySize, zSize, Direction.NE, o, true).Save(di.FullName + SEP + u + "_Big_NE" + ".png", ImageFormat.Png); //ne

            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.S, o, true).Save(di.FullName + SEP + u + "_Big_S" + ".png", ImageFormat.Png); //s
            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.W, o, true).Save(di.FullName + SEP + u + "_Big_W" + ".png", ImageFormat.Png); //w
            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.N, o, true).Save(di.FullName + SEP + u + "_Big_N" + ".png", ImageFormat.Png); //n
            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.E, o, true).Save(di.FullName + SEP + u + "_Big_E" + ".png", ImageFormat.Png); //e

        }
        static void Main(string[] args)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream imageStream = assembly.GetManifestResourceStream("IsoVoxel.cube_soft.png");
            cube = new Bitmap(imageStream);
            imageStream = assembly.GetManifestResourceStream("IsoVoxel.cube_ortho.png");
            ortho = new Bitmap(imageStream);
            imageStream = assembly.GetManifestResourceStream("IsoVoxel.white.png");
            white = new Bitmap(imageStream);
            string voxfile = "Zombie.vox";
            if (args.Length >= 1)
            {
                voxfile = args[0];
            }
            else
            {
                Console.WriteLine("Args: 'file x y z m o'. file is a MagicaVoxel .vox file, x y z are sizes,");
                Console.WriteLine("m is a multiplier to draw ortho renders up to that size (integer, at least 1),");
                Console.WriteLine("o must be one of these words, changing how outlines are drawn (default light):");
                Console.WriteLine("  outline=full    Draw a black outer outline and shaded inner outlines.");
                Console.WriteLine("  outline=light   Draw a shaded outer outline and shaded inner outlines.");
                Console.WriteLine("  outline=partial Draw no outer outline and shaded inner outlines.");
                Console.WriteLine("  outline=none    Draw no outlines.");
                Console.WriteLine("x y z m o are all optional, but o must be the last if present.");
                Console.WriteLine("Defaults: runs on Zombie.vox with x y z set by the model, m is 1, o is light.");
                Console.WriteLine("Given no arguments, running on Zombie.vox ...");
            }
            byte x = 0, y = 0, z = 0;
            int m = 3;
            Outlining o = Outlining.Light;
            int al = args.Length;
            if (al >= 2 && args.Last().StartsWith("outline", StringComparison.OrdinalIgnoreCase))
            {
                o = GetOutlining(args.Last().ToLowerInvariant().Split('=').Last());
                --al;
            }
            try
            {
                if (al >= 2)
                {
                    x = byte.Parse(args[1]);
                }
                if (al >= 3)
                {
                    y = byte.Parse(args[2]);
                }
                if(al >= 4)
                {
                    z = byte.Parse(args[3]);
                }
                if(al >= 5)
                {
                    m = int.Parse(args[4]);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Args: 'file x y z m o'. file is a MagicaVoxel .vox file, x y z are sizes,");
                Console.WriteLine("m is a multiplier to draw ortho renders up to that size (integer, at least 1),");
                Console.WriteLine("o must be one of these words, changing how outlines are drawn (default light):");
                Console.WriteLine("  outline=full    Draw a black outer outline and shaded inner outlines.");
                Console.WriteLine("  outline=light   Draw a shaded outer outline and shaded inner outlines.");
                Console.WriteLine("  outline=partial Draw no outer outline and shaded inner outlines.");
                Console.WriteLine("  outline=none    Draw no outlines.");
                Console.WriteLine("x y z m o are all optional, but o must be the last if present.");
                Console.WriteLine("Defaults: runs on Zombie.vox with x y z set by the model, m is 1, o is light.");

            }
            BinaryReader bin = new BinaryReader(File.Open(voxfile, FileMode.Open));
            MagicaVoxelData[] mvd = FromMagica(bin);
            rendered = storeColorCubes();
            rendered45 = storeColorCubes45();
            renderedOrtho = storeColorCubesOrtho();
            storeColorCubesFaces();
            storeColorCubesFacesSmall();
            processUnitSmart(mvd, voxfile, x, y, z, o, m);
            bin.Close();
        }

        private static Outlining GetOutlining(string s)
        {
            switch(s)
            {
                case "full": return Outlining.Full;
                case "light": return Outlining.Light;
                case "partial": return Outlining.Partial;
                case "none": return Outlining.None;
            }
            return Outlining.Full;
        }
    }
}
