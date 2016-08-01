using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftRenderer.Math;

namespace SoftRenderer.RenderData
{
    public class Light
    {
        /// <summary>
        /// 灯光世界坐标
        /// </summary>
        public Vector3D worldPosition;

        /// <summary>
        /// 灯光颜色
        /// </summary>
        public Color lightColor;


        public Light(Vector3D worldPosition, Color lightColor)
        {
            this.worldPosition = worldPosition;
            this.lightColor = lightColor;
        }
    }
}
