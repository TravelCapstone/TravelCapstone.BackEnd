namespace TravelCapstone.BackEnd.Common.DTO.ProcessDTO;

public class Prediction
{
    public string Description { get; set; }
    public CompoundInfo Compound { get; set; }
}

public class CompoundInfo
{
    public string District { get; set; }
    public string Commune { get; set; }
    public string Province { get; set; }
}

public class RootObject
{
    public List<Prediction> Predictions { get; set; }
}