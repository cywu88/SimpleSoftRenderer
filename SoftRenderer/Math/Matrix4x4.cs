using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer.Math
{
    /// <summary>
    /// 4*4矩阵，行优先存储
    /// </summary>
    public class Matrix4x4
    {
        private float[,] _m = new float[4, 4];

        public Matrix4x4()
        {

        }

        public Matrix4x4(float a1, float a2, float a3, float a4,
            float b1, float b2, float b3, float b4,
            float c1, float c2, float c3, float c4,
            float d1, float d2, float d3, float d4)
        {
            _m[0, 0] = a1; _m[0, 1] = a2; _m[0, 2] = a3; _m[0, 3] = a4;
            //
            _m[1, 0] = b1; _m[1, 1] = b2; _m[1, 2] = b3; _m[1, 3] = b4;
            //
            _m[2, 0] = c1; _m[2, 1] = c2; _m[2, 2] = c3; _m[2, 3] = c4;
            //
            _m[3, 0] = d1; _m[3, 1] = d2; _m[3, 2] = d3; _m[3, 3] = d4;
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            Matrix4x4 nm = new Matrix4x4();
            //nm.SetZero();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        nm._m[i, j] += lhs._m[i, k] * rhs._m[k, j];
                    }
                }
            }
            return nm;
        }

        public float this[int i, int j]
        {
            get { return _m[i, j]; }
            set { _m[i, j] = value; }
        }

        /// <summary>
        /// 单位化矩阵
        /// </summary>
        public void Identity()
        {
            _m[0, 0] = 1; _m[0, 1] = 0; _m[0, 2] = 0; _m[0, 3] = 0;
            //
            _m[1, 0] = 0; _m[1, 1] = 1; _m[1, 2] = 0; _m[1, 3] = 0;
            //
            _m[2, 0] = 0; _m[2, 1] = 0; _m[2, 2] = 1; _m[2, 3] = 0;
            //
            _m[3, 0] = 0; _m[3, 1] = 0; _m[3, 2] = 0; _m[3, 3] = 1;
        }

        public void SetZero()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _m[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// 矩阵的专置   T  交换m[i,j] 和m[j,i]
        /// </summary>
        /// <returns></returns>
        public Matrix4x4 Transpose()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = i; j < 4; j++)
                {

                    float temp = _m[i, j];
                    _m[i, j] = _m[j, i];
                    _m[j, i] = temp;
                }
            }
            return this;
        }


        public float Determinate()
        {
            return Determinate(_m, 4);
        }

        private float Determinate(float[,] m, int n)
        {
            var testValue = 0;
            return testValue;
        }

        /// <summary>
        /// 获取当前矩阵的伴随矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix4x4 GetAdjoint()
        {
            Matrix4x4 result = new Matrix4x4();
            return result.Transpose();
        }

        /// <summary>
        ///  求当前矩阵的逆矩阵  T的(-1)次方
        /// </summary>
        /// <returns></returns>
        public Matrix4x4 Inverse()
        {
            Matrix4x4 adj = GetAdjoint();//伴随矩阵
            return adj;
        }






    }
}