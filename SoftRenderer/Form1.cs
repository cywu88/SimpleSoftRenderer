using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftRenderer {
    public partial class SoftRendererApp : Form
    {
        private Bitmap _texture;  //纹理
        private Bitmap _frameBuff;//用一张bitmap来做帧缓冲
        private Graphics _frameG;


        public SoftRendererApp() {
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
    }
 
}
