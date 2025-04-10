namespace BuffBuddyAPI;

public class PaginationDTO
{
    public int Page { get; set; } = 1;
    private int recordsPerPage = 10;
    private int maxRecordsPerPage = 50;

    public int RecordsPerPage
    {
        get { return recordsPerPage;}
        set { recordsPerPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value; }
    } // 10 by default, max 50
}
