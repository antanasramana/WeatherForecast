namespace Domain;

public record Weather
{
    public int Id { get; set; }
    // Added timestamp of weather so that persistence of data makes sense
    public DateTimeOffset TimeStamp { get; set; }
    public int Temperature { get; set; }
    public int Precipitation { get; set; }
    public double WindSpeed { get; set; }
    public required string Summary { get; set; }
    public required string City { get; set; }
}