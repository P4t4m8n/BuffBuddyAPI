namespace BuffBuddyAPI;

public class CoreSetDTO : IID
{
    public required string Id { get; set; }
    public int Reps { get; set; }
    public int Weight { get; set; } // in kg
    public int RestTime { get; set; } // in seconds
    public int Order { get; set; } // Order of the set in the exercise
    public int RepsInReserve { get; set; } = 0; // Indicates if the reps are in reserve

    public bool IsWarmup { get; set; } = false; // Indicates if the set was a warmup set


}
