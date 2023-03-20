﻿using System.Collections.Generic;
using Objects.Geometry;
using Speckle.Core.Kits;
using Speckle.Core.Models;

namespace Objects.BuiltElements.AdvanceSteel
{
  public class AsteelGrating : AsteelObject
  {
    [DetachProperty]
    public List<Mesh> displayValue { get; set; }

    //[SchemaInfo("AsteelGrating", "Creates a Advance Steel grating.", "Advance Steel", "Structure")]
    public AsteelGrating()
    {

    }
  }
}
