#nullable enable
using System.Collections.Generic;

public sealed class FeatureCollection
{
    public List<Feature>? Features { get; set; }
}

public sealed class Feature
{
    public Properties? Properties { get; set; }
}

public sealed class Properties
{
    public string? Place { get; set; }
    public double? Mag { get; set; }
}
