﻿using System.Collections.Generic;
using Objects.Geometry;
using Speckle.Core.Kits;
using Speckle.Core.Models;

namespace Objects.BuiltElements.AdvanceSteel
{
  public abstract class AsteelBolt : AsteelObject
  {
    [DetachProperty]
    public List<Mesh> displayValue { get; set; }

    public AsteelBolt()
    {

    }
  }

  public class AsteelCircularBolt : AsteelBolt
  {
    //[SchemaInfo("AsteelCircularBolt", "Creates a Advance Steel circular bolt.", "Advance Steel", "Structure")]
    public AsteelCircularBolt()
    {

    }
  }

  public class AsteelRectangularBolt : AsteelBolt
  {
    //[SchemaInfo("AsteelRectangularBolt", "Creates a Advance Steel rectangular bolt.", "Advance Steel", "Structure")]
    public AsteelRectangularBolt()
    {

    }
  }
}
