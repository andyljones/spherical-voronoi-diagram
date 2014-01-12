using Grids.IcosahedralGridGenerator;
namespace Initialization
{
    public class Options : 
        IIcosahedralGridOptions
    {
        public float Radius { get; set; }
        public float Resolution { get; set; }
    }
}
