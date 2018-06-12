using System;
using System.Drawing;
using System.Windows.Forms;

namespace Spheres_ray_casting
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Graphics grp;
        Vector3[] M;
        Vector3 cPos;
        Vector3 cTarget;
        Vector3 cUp;

        public Form1()
        {
            InitializeComponent();

            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            grp = Graphics.FromImage(bmp);

            cPos = new Vector3(0, 0, 0, 1);
            cTarget = new Vector3(0, 0, -1, 1);
            cUp = new Vector3(0, 1, 0, 1);

            M = CameraConstruction(cPos, cTarget, cUp);
            RayCasting();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.Image = bmp;
        }

        public Vector3 Cross(Vector3 A, Vector3 B)
        {
            Vector3 C = new Vector3();
            C.x = A.y * B.z - A.z * B.y;
            C.y = A.z * B.x - A.x * B.z;
            C.z = A.x * B.y - A.y * B.x;
            return C;
        }

        public float Dot(Vector3 A, Vector3 B)
        {
            return A.x * B.x + A.y * B.y + A.z * B.z;
        }

        public float VecLenght(Vector3 A)
        {
            return (float)Math.Sqrt(Dot(A, A));
        }

        public Vector3 Div(Vector3 A, float b)
        {
            Vector3 C = new Vector3();
            C.x = A.x / b;
            C.y = A.y / b;
            C.z = A.z / b;
            C.w = A.w / b;
            return C;
        }

        public Vector3 Multip(Vector3 A, float b)
        {
            Vector3 C = new Vector3();
            C.x = A.x * b;
            C.y = A.y * b;
            C.z = A.z * b;
            C.w = A.w * b;
            return C;
        }

        public Vector3 Minus(Vector3 A, Vector3 B)
        {
            Vector3 C = new Vector3();
            C.x = A.x - B.x;
            C.y = A.y - B.y;
            C.z = A.z - B.z;
            C.w = A.w - B.w;
            return C;
        }

        public double check(double t1, double t2)
        {
            if ((t1.Equals(double.NaN) && t2.Equals(double.NaN)) || (t1 < 0 && t2 < 0)) return double.NaN;
            else
            {
                if (t1.Equals(double.NaN) || t1 < 0) return t2;
                else
                {
                    if (t2.Equals(double.NaN) || t2 < 0) return t1;
                    else
                    {
                        if (t1 < t2) return t1;
                        else return t2;
                    }
                }
            }
        }

        static double quadForm(float a, float b, float c, bool pos)
        {
            var preRoot = b * b - 4 * a * c;
            if (preRoot < 0)
            {
                return double.NaN;
            }
            else
            {
                var sgn = pos ? 1.0 : -1.0;
                return (sgn * Math.Sqrt(preRoot) - b) / (2.0 * a);
            }
        }

        public Vector3 MatrixVec(Vector3[] M, Vector3 vec)
        {
            Vector3 res = new Vector3();

            res.x = M[0].x * vec.x + M[0].y * vec.y + M[0].z * vec.z + M[0].w * vec.w;
            res.y = M[1].x * vec.x + M[1].y * vec.y + M[1].z * vec.z + M[1].w * vec.w;
            res.z = M[2].x * vec.x + M[2].y * vec.y + M[2].z * vec.z + M[2].w * vec.w;
            res.w = M[3].x * vec.x + M[3].y * vec.y + M[3].z * vec.z + M[3].w * vec.w;

            return res;
        }

        public Vector3[] CameraConstruction(Vector3 cPos, Vector3 cTarget, Vector3 cUp)
        {
            Vector3 cZ = Div(Minus(cPos, cTarget), VecLenght(Minus(cPos, cTarget)));
            Vector3 cX = Div(Cross(cUp, cZ), VecLenght(Cross(cUp, cZ)));
            Vector3 cY = Div(Cross(cZ, cX), VecLenght(Cross(cZ, cX)));

            Vector3[] Matrix = {
                         new Vector3(cX.x, cX.y, cX.z, Dot(cX, cPos)),
                         new Vector3(cY.x, cY.y, cY.z, Dot(cY, cPos)),
                         new Vector3(cZ.x, cZ.y, cZ.z, Dot(cZ, cPos)),
                         new Vector3(0, 0, 0, 1)
                         };
                   

            return Matrix;
        }

        public Vector3[] RayConstruction(Sphere sphere, Vector3 p, Vector3 v)
        {
            float b = 2 * Dot(v, Minus(p, sphere.ps));
            float c = (VecLenght(Minus(p, sphere.ps)) * VecLenght(Minus(p, sphere.ps))) - (sphere.r * sphere.r);

            double t1 = quadForm(1, b, c, true);
            double t2 = quadForm(1, b, c, false);
            double t = check(t1, t2);

            if (t.Equals(double.NaN) == true) return null;

            Vector3 pt = new Vector3();
            pt.x = p.x + (float)t * v.x;
            pt.y = p.y + (float)t * v.y;
            pt.z = p.z + (float)t * v.z;
            pt.w = p.w + (float)t * v.w;

            Vector3 n = Div(Minus(pt, sphere.ps), VecLenght(Minus(pt, sphere.ps)));

            Vector3[] npt = {n, pt}; 

            return npt;
        }

        public Color PointColor(Sphere s, DirectionalLight light, Vector3 pc, Vector3 n, Vector3 pt)
        {
            Vector3 v = Div(Minus(pc, pt), VecLenght(Minus(pc, pt)));
            Vector3 l = Multip(light.d, -1);
            Vector3 r = Minus(Multip(n, 2 * Dot(n, l)), l);
            Color I = light.Ip;

            float Ir = I.R * s.material.ka.r + s.material.kd.r * I.R * Math.Max(0, Dot(n, l)) + s.material.ks.r * I.R * (float)Math.Pow(Math.Max(0, Dot(v, l)), s.material.m);
            float Ig = I.G * s.material.ka.g + s.material.kd.g * I.G * Math.Max(0, Dot(n, l)) + s.material.ks.g * I.G * (float)Math.Pow(Math.Max(0, Dot(v, l)), s.material.m);
            float Ib = I.B * s.material.ka.b + s.material.kd.b * I.B * Math.Max(0, Dot(n, l)) + s.material.ks.b * I.B * (float)Math.Pow(Math.Max(0, Dot(v, l)), s.material.m);

            if (Ir > 255) Ir = 255;
            else
            {
                if (Ir < 0) Ir = 0;
            }

            if (Ig > 255) Ig = 255;
            else
            {
                if (Ig < 0) Ig = 0;
            }

            if (Ib > 255) Ib = 255;
            else
            {
                if (Ib < 0) Ig = 0;
            }

            Color c = Color.FromArgb((int)Ir, (int)Ig, (int)Ib);
            return c;
        }

        public void RayCasting()
        {
            Sphere sphere = new Sphere(new Vector3(0, 0, -100, 1), 10, new Material(new MaterialCoeff(0.5f, 0.5f, 0.5f), new MaterialCoeff(0.5f, 0.5f, 0.5f), new MaterialCoeff(0.5f, 0.5f, 0.5f), 50));

            Vector3 l = new Vector3(0, -100, -1, 0);
            DirectionalLight light = new DirectionalLight(Div(l, VecLenght(l)), Color.White, 10);

            float Cx = (bmp.Width - 1) / 2;
            float Cy = (bmp.Height - 1) / 2;

            Vector3 Pprim = new Vector3(0,0,0,1);
            Vector3 p = MatrixVec(M, Pprim);

            float angle = 90;
            float radians = angle * (float)(Math.PI / 180);
            float d = (bmp.Width / 2 * (1f / (float)Math.Tan(radians / 2)));

            for (float y = 0; y < bmp.Height; y++)
            {
                for (float x = 0; x < bmp.Width; x++)
                {
                    Vector3 q = new Vector3(x - Cx, -y + Cy, -d, 1);
                    Vector3 Vprim = Div(Minus(q, Pprim), VecLenght(Minus(q, Pprim)));
                    Vector3 v = MatrixVec(M, Vprim);

                    Vector3[] npt = RayConstruction(sphere, p, v);

                    if (npt == null) continue;

                    Color c = PointColor(sphere, light, cPos, npt[0], npt[1]);
                    bmp.SetPixel((int)(x), (int)(y), c);
                }
            }
        }
    }
}
