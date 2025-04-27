using System;

namespace Shared.Dtos.Map.Requests;

public class SearchPostRequest
{
    public double x { get; set; }
    public double y { get; set; }
    public double range { get; set; }

    public SearchPostRequest FromGetSearchPostInputDto(DataTransferObject.InputDto.GetSearchPostInputDto dto)
    {
        if (dto == null) return null;

        x = dto.x;
        y = dto.y;
        range = dto.range;

        return this;
    }
}