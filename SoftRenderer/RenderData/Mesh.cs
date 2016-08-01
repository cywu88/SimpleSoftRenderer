using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftRenderer.Math;

namespace SoftRenderer.RenderData
{
    /// <summary>
    /// 顾名思义 网格...
    /// </summary>
    public class Mesh
    {
        private Vertex[] _verts;

        // 顶点数组
        public Vertex[] vertices
        {
            get { return _verts; }
        }

        private Material _mat;

        public Material material
        {
            get { return _mat; }
        }

        public Mesh(Vector3D[] pointList, int[] indexs, Point2D[] Uvs, Vector3D[] vertColors, Vector3D[] normals, Material mat)
        {
            _verts = new Vertex[indexs.Length];

            //生成顶点列表
            for (int i = 0; i < indexs.Length; i++)
            {
                int pointIndex = indexs[i];
                Vector3D point = pointList[pointIndex];
                _verts[i] = new Vertex(point, normals[i], Uvs[i].x, Uvs[i].y, vertColors[i].x, vertColors[i].y, vertColors[i].z);
            }
            _mat = mat;
        }
    }
}
