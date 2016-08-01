﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftRenderer.RenderData;

namespace SoftRenderer.Math
{
    public class MathUtil
    {

        /// <summary>
        /// 获取平移矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Matrix4x4 GetTranslate(float x, float y, float z)
        {
            return new Matrix4x4(1, 0, 0, 0,
                                   0, 1, 0, 0,
                                   0, 0, 1, 0,
                                   x, y, z, 1);
        }

        /// <summary>
        /// 获取缩放矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Matrix4x4 GetScale(float x, float y, float z)
        {
            return new Matrix4x4(x, 0, 0, 0,
                                  0, y, 0, 0,
                                  0, 0, z, 0,
                                  0, 0, 0, 1);
        }

        public static Matrix4x4 GetRotateY(float r)
        {
            Matrix4x4 rm = new Matrix4x4();
            rm.Identity();
            rm[0, 0] = (float)(System.Math.Cos(r));

            rm[0, 2] = (float)(-System.Math.Sin(r));
            //

            rm[2, 0] = (float)(System.Math.Sin(r));
            rm[2, 2] = (float)(System.Math.Cos(r));
            return rm;
        }

        public static Matrix4x4 GetRotateX(float r)
        {
            Matrix4x4 rm = new Matrix4x4();
            rm.Identity();
            rm[1, 1] = (float)(System.Math.Cos(r));
            rm[1, 2] = (float)(System.Math.Sin(r));
            //

            rm[2, 1] = (float)(-System.Math.Sin(r));
            rm[2, 2] = (float)(System.Math.Cos(r));
            return rm;
        }

        public static Matrix4x4 GetRotateZ(float r)
        {
            Matrix4x4 rm = new Matrix4x4();
            rm.Identity();
            rm[0, 0] = (float)(System.Math.Cos(r));
            rm[0, 1] = (float)(System.Math.Sin(r));
            //
            rm[1, 0] = (float)(-System.Math.Sin(r));
            rm[1, 1] = (float)(System.Math.Cos(r));
            return rm;
        }

        /// <summary>
        /// 线性插值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Lerp(float a, float b, float t)
        {
            if (t <= 0)
            {
                return a;
            }
            else if (t >= 1)
            {
                return b;
            }
            else
            {
                return b * t + (1 - t) * a;
            }
        }


        /// <summary>
        /// 获取视矩阵
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="lookAt"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public static Matrix4x4 GetView(Vector3D pos, Vector3D lookAt, Vector3D up)
        {
            //视线方向
            Vector3D dir = lookAt - pos;
            Vector3D right = Vector3D.Cross(up, dir);
            right.Normalize();
            //平移部分
            Matrix4x4 t = new Matrix4x4(1, 0, 0, 0,
                                           0, 1, 0, 0,
                                           0, 0, 1, 0,
                                           -pos.x, -pos.y, -pos.z, 1);
            //旋转部分
            Matrix4x4 r = new Matrix4x4(right.x, up.x, dir.x, 0,
                                           right.y, up.y, dir.y, 0,
                                           right.z, up.z, dir.z, 0,
                                           0, 0, 0, 1);
            return t * r;
        }


        /// <summary>
        /// 插值函数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Color Lerp(Color a, Color b, float t)
        {
            if (t <= 0)
            {
                return a;
            }
            else if (t >= 1)
            {
                return b;
            }
            else
            {
                return t * b + (1 - t) * a;
            }
        }

        /// <summary>
        /// 屏幕空间插值生成新顶点，此时已近经过透视除法，z信息已经没有作用
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static void ScreenSpaceLerpVertex(ref Vertex v, Vertex v1, Vertex v2, float t)
        {
            v.onePerZ = MathUtil.Lerp(v1.onePerZ, v2.onePerZ, t);
            //
            v.u = MathUtil.Lerp(v1.u, v2.u, t);
            v.v = MathUtil.Lerp(v1.v, v2.v, t);
            //
            v.vcolor = MathUtil.Lerp(v1.vcolor, v2.vcolor, t);
            //
            v.lightingColor = MathUtil.Lerp(v1.lightingColor, v2.lightingColor, t);
        }


        public static int Range(int v, int min, int max)
        {
            if (v <= min)
            {
                return min;
            }
            if (v >= max)
            {
                return max;
            }
            return v;
        }

        public static float Range(float v, float min, float max)
        {
            if (v <= min)
            {
                return min;
            }
            if (v >= max)
            {
                return max;
            }
            return v;
        }
    }
}
