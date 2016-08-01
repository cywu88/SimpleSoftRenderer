using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftRenderer.Math;

namespace SoftRenderer.RenderData
{
    public class Camera
    {
        public Vector3D pos;
        public Vector3D lookAt;
        public Vector3D up;

        // 观察角 弧度
        public float fov;

        // 长宽比
        public float aspect;

        // 近裁平面
        public float zn;

        // 远裁平面
        public float zf;

        public Camera(Vector3D pos,Vector3D lookAt,Vector3D up,float fov,float aspect,float zn,float zf)
        {
            this.pos = pos;
            this.lookAt = lookAt;
            this.up = up;
            this.fov = fov;
            this.aspect = aspect;
            this.zn = zn;
            this.zf = zf;
        }
    }
}
