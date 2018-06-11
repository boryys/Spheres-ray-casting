using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spheres_ray_casting
{
    public class Sphere
    {
        public Vector3 ps { get; set; }
        public float r { get; set; }
        public Material material { get; set; }

        public Sphere(Vector3 _ps, float _r, Material m)
        {
            ps = _ps;
            r = _r;
            material = m;
        }
    }

    public class Material
    {
        public MaterialCoeff ka { get; set; }
        public MaterialCoeff kd { get; set; }
        public MaterialCoeff ks { get; set; }
        public float m { get; set; }

        public Material(MaterialCoeff _ka, MaterialCoeff _kd, MaterialCoeff _ks, float _m)
        {
            ka = _ka;
            kd = _kd;
            ks = _ks;
            m = _m;
        }
    }

    public class MaterialCoeff
    {
        public float r { get; set; }
        public float g { get; set; }
        public float b { get; set; }

        public MaterialCoeff(float _r, float _g, float _b)
        {
            r = _r;
            g = _g;
            b = _b;
        }
    }
}
