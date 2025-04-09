namespace BuffBuddyAPI;

public interface IRepository
{
    List<Exercise> GetAllExercises();
    Task<Exercise?> GetById(string id);
    bool Exists(string name);
}
