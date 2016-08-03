using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using SoftRenderer.Math;

namespace SoftRenderer.RenderData
{
    /// <summary>
    /// 顶点信息
    /// </summary>
    public struct Vertex
    {
        // 顶点位置
        public Vector3D point;

        // 纹理坐标
        public float u;
        public float v;

        // 顶点色
        public Color vcolor;

        //法线
        public Vector3D normal;

        // 光照颜色
        public Color lightingColor;

        /// 1/z，用于顶点信息的透视校正
        public float onePerZ;

        //public Vertex()
        //{
        //    this.point = new Vector3D();
        //    this.vcolor = new Color();
        //    this.normal = new Vector3D();
        //    this.lightingColor  = new Color();
        //}


        public Vertex(Vector3D point, Vector3D normal, float u, float v, float r, float g, float b)
        {
            this.point = point;
            this.normal = normal;
            this.point.w = 1;
            vcolor = new Color();
            vcolor.r = r;
            vcolor.g = g;
            vcolor.b = b;
            onePerZ = 1;
            this.u = u;
            this.v = v;
            lightingColor = new Color();
            lightingColor.r = 1;
            lightingColor.g = 1;
            lightingColor.b = 1;
        }

        public Vertex(Vertex v)
        {
            point = v.point;
            normal = v.normal;
            this.vcolor = v.vcolor;
            onePerZ = 1;
            this.u = v.u;
            this.v = v.v;
            this.lightingColor = v.lightingColor;
        }
    }
}
