using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SoftRenderer.Math;
using SoftRenderer.RenderData;
using SoftRenderer.Test;
using System.Timers;

namespace SoftRenderer {
    public partial class SoftRendererApp : Form
    {
        private Bitmap _texture;  //纹理
        private Bitmap _frameBuff;//用一张bitmap来做帧缓冲
        private Graphics _frameG;
        private float[,] _zBuff;//z缓冲，用来做深度测试
        private Mesh _mesh;
        private Light _light;
        private Camera _camera;
        private SoftRenderer.RenderData.Color _ambientColor;//全局环境光颜色 

        //
        private RenderMode _currentMode;//渲染模式
        private LightMode _lightMode;//光照模式
        private TextureFilterMode _textureFilterMode;//纹理采样模式
        //
        private uint _showTrisCount;//测试数据，记录当前显示的三角形数

        public SoftRendererApp()
        {
            InitializeComponent();
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile("../../Texture/cc.jpg");
                _texture = new Bitmap(img, 256, 256);
            }
            catch (Exception)
            {
                _texture = new Bitmap(256, 256);
                initTexture();
            }

            _frameBuff = new Bitmap(this.MaximumSize.Width, this.MaximumSize.Height);
            _frameG = Graphics.FromImage(_frameBuff);
            _zBuff = new float[this.MaximumSize.Height, this.MaximumSize.Width];
            _ambientColor = new RenderData.Color(1f, 1f, 1f);


            _mesh = new Mesh(CubeTestData.pointList, CubeTestData.indexs, CubeTestData.uvs, CubeTestData.vertColors, CubeTestData.norlmas, CubeTestData.mat);

            //定义光照
            _light = new Light(new Vector3D(50, 0, 0), new RenderData.Color(1, 1, 1));
            //定义相机
            _camera = new Camera(new Vector3D(0, 0, 0, 1), new Vector3D(0, 0, 1, 1), new Vector3D(0, 1, 0, 0), (float)System.Math.PI / 4, this.MaximumSize.Width / (float)this.MaximumSize.Height, 1f, 500f);


            System.Timers.Timer mainTimer = new System.Timers.Timer(1000 / 60f);

            mainTimer.Elapsed += new ElapsedEventHandler(Tick);
            mainTimer.AutoReset = true;
            mainTimer.Enabled = true;
            mainTimer.Start();
        }


        public void initTexture()
        {
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    _texture.SetPixel(j, i, ((j + i) % 32 == 0) ? System.Drawing.Color.White : System.Drawing.Color.Green);
                }
            }
        }

        private void ClearBuff()
        {
            _frameG.Clear(System.Drawing.Color.Black);
            Array.Clear(_zBuff, 0, _zBuff.Length);
        }


        private float rot = 0;
        Graphics g = null;
        private void Tick(object sender, EventArgs e)
        {
            lock (_frameBuff)
            {
                ClearBuff();
                rot += 0.05f;


                if (g == null)
                {
                    g = this.CreateGraphics();
                }
                g.Clear(System.Drawing.Color.Black);
                g.DrawImage(_frameBuff, rot, 0);
            }
        }

        private void Draw(Matrix4x4 m, Matrix4x4 v, Matrix4x4 p)
        {
            _showTrisCount = 0;
            for (int i = 0; i + 2 < _mesh.vertices.Length; i += 3)
            {
                DrawTriangle(_mesh.vertices[i], _mesh.vertices[i + 1], _mesh.vertices[i + 2], m, v, p);
            }
            Console.WriteLine("显示的三角形数：" + _showTrisCount);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="mvp"></param>
        private void DrawTriangle(Vertex p1, Vertex p2, Vertex p3, Matrix4x4 m, Matrix4x4 v, Matrix4x4 p)
        {
            if (_lightMode == LightMode.On)
            {
                //进行顶点光照               
            }

            SetMVTransform(m, v, ref p1);
            SetMVTransform(m, v, ref p2);
            SetMVTransform(m, v, ref p3);

            //变换到齐次剪裁空间
            SetProjectionTransform(p, ref p1);
            SetProjectionTransform(p, ref p2);
            SetProjectionTransform(p, ref p3);

            //在相机空间进行背面消隐
            if (BackFaceCulling(p1, p2, p3) == false)
            {
                return;
            }

            //变换到齐次剪裁空间
            SetProjectionTransform(p, ref p1);
            SetProjectionTransform(p, ref p2);
            SetProjectionTransform(p, ref p3);


            //--------------------光栅化阶段---------------------------
            if (_currentMode == RenderMode.Wireframe)
            {//线框模式
                BresenhamDrawLine(p1, p2);
                BresenhamDrawLine(p2, p3);
                BresenhamDrawLine(p3, p1);
            }
            else
            {
                TriangleRasterization(p1, p2, p3);
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 进行mv矩阵变换，从本地模型空间到世界空间，再到相机空间
        /// </summary>
        private void SetMVTransform(Matrix4x4 m, Matrix4x4 v, ref Vertex vertex)
        {
            vertex.point = vertex.point * m * v;
        }

        /// <summary>
        /// 投影变换，从相机空间到其次剪裁空间
        /// </summary>
        /// <param name="p"></param>
        /// <param name="vertex"></param>
        private void SetProjectionTransform(Matrix4x4 p, ref Vertex vertex)
        {
            vertex.point = vertex.point * p;
            //得到齐次裁剪空间的点 v.point.w 中保存着原来的z(具体是z还是-z要看使用的投影矩阵,我们使用投影矩阵是让w中保存着z)


            // onePerZ 保存 1/z,方便之后对1/z关于x',y',插值得到1/z'
            vertex.onePerZ = 1 / vertex.point.w;

            vertex.u *= vertex.onePerZ;
            vertex.v *= vertex.onePerZ;

            //
            vertex.vcolor *= vertex.onePerZ;
            //
            vertex.lightingColor *= vertex.onePerZ;
        }

        /// <summary>
        /// 检查是否裁剪这个顶点,简单的cvv裁剪,在透视除法之前
        /// </summary>
        /// <returns>是否通关剪裁</returns>
        private bool Clip(Vertex v)
        {
            //cvv为 x-1,1  y-1,1  z0,1
            if (v.point.x >= -v.point.w && v.point.x <= v.point.w &&
                v.point.y >= -v.point.w && v.point.y <= v.point.w &&
                v.point.z >= 0f && v.point.z <= v.point.w)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 从齐次剪裁坐标系转到屏幕坐标
        /// </summary>
        private void TransformToScreen(ref Vertex v)
        {
            if (v.point.w != 0)
            {
                //先进行透视除法，转到cvv
                v.point.x *= 1 / v.point.w;
                v.point.y *= 1 / v.point.w;
                v.point.z *= 1 / v.point.w;
                v.point.w = 1;


                //这段代码怎么来的  //11111111111111111111111111111111111111111
                //cvv到屏幕坐标
                v.point.x = (v.point.x + 1) * 0.5f * this.MaximumSize.Width;
                v.point.y = (1 - v.point.y) * 0.5f * this.MaximumSize.Height;
            }

        }

        private bool BackFaceCulling(Vertex p1, Vertex p2, Vertex p3)
        {
            if (_currentMode == RenderMode.Wireframe)
            {//线框模式不进行背面消隐
                return true;
            }
            else
            {
                Vector3D v1 = p2.point - p1.point;
                Vector3D v2 = p3.point - p2.point;
                Vector3D normal = Vector3D.Cross(v1, v2);

                //2222222222222222222222222222222222222222222222222222
                //由于在视空间中，所以相机点就是（0,0,0）
                Vector3D viewDir = p1.point - new Vector3D(0, 0, 0);
                if (Vector3D.Dot(normal, viewDir) > 0)
                {
                    _showTrisCount++;
                    return true;
                }
                return false;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 光栅化三角形
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        private void TriangleRasterization(Vertex p1, Vertex p2, Vertex p3)
        {
            if(p1.point.y == p2.point.y)
            {
                if (p1.point.y < p3.point.y)
                {//平顶
                    DrawTriangleTop(p1, p2, p3);
                }
                else
                {//平底
                    DrawTriangleBottom(p3, p1, p2);
                }
            }
        }


        private void DrawTriangleTop(Vertex p1, Vertex p2, Vertex p3)
        {
            for (float y = p1.point.y;  y <= p3.point.y; y += 0.5f)
            {
                int yIndex = (int)(System.Math.Round(y, MidpointRounding.AwayFromZero));
               
            }
        }

        private void DrawTriangleBottom(Vertex p1, Vertex p2, Vertex p3)
        {

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 绘制直线，使用bresenham算法
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private void BresenhamDrawLine(Vertex p1, Vertex p2)
        {

        }


    }
}
