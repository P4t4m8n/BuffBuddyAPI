namespace BuffBuddyAPI;

public class UserSetDTO : IID
{
    public required string Id { get; set; }
    public int Reps { get; set; }
    public int Weight { get; set; } // in kg
    public int RestTime { get; set; } // in seconds
    public bool IsCompleted { get; set; } = false; // Indicates if the set was completed
    public bool IsMuscleFailure { get; set; } = false; // Indicates if the set was done to muscle failure
    public bool IsWarmup { get; set; } = false; // Indicates if the set was a warmup set
    public bool IsJointPain { get; set; } = false; // Indicates if the user experienced joint pain during the set


}
