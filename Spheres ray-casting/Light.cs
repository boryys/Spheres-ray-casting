using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spheres_ray_casting
{
    public class PointLight
    {
        public Vector3 p { get; set; }
        public Color Ip { get; set; }
        public int r { get; set; }

        public PointLight(Vector3 _p, Color _Ip, int _r)
        {
            p = _p;
            Ip = _Ip;
            r = _r;
        }
    }

    public class DirectionalLight
    {
        public Vector3 d { get; set; }
        public Color Ip { get; set; }
        public int r { get; set; }

        public DirectionalLight(Vector3 _d, Color _Ip, int _r)
        {
            d = _d;
            Ip = _Ip;
            r = _r;
        }
    }

    public class SpotLight
    {
        public Vector3 p { get; set; }
        public Vector3 d { get; set; }
        public Color Ip { get; set; }
        public int r { get; set; }

        public SpotLight(Vector3 _p, Vector3 _d, Color _Ip, int _r)
        {
            p = _p;
            d = _d;
            Ip = _Ip;
            r = _r;
        }
    }
}
