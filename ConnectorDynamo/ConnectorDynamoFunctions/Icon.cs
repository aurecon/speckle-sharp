using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace Speckle.ConnectorDynamo.Functions.AAA;

public static class ASpeckleBrick
{
  public static List<Point> Brick()
  {
    var points = new List<Point>();
    for (var x = 0; x < 10; x++)
    {
      for (var y = 0; y < 10; y++)
      {
        for (var z = 0; z < 10; z++)
        {
          points.Add(Point.ByCoordinates(x, y, z));
        }
      }
    }

    return points;
  }
}
