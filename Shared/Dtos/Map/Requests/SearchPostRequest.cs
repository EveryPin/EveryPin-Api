using System;

namespace Shared.Dtos.Map.Requests;

public class SearchPostRequest
{
    public double x { get; set; }
    public double y { get; set; }
    public double range { get; set; }
}