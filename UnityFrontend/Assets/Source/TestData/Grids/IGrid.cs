using System.Collections.Generic;
using Foam;

namespace Grids
{
    public interface IGrid
    {
        List<Face> Faces { get; }
    }
}
