namespace DAL.Models
{
    public class ScheduledCaseFile
    {
        public int Id { get; set; }

        public int CaseFileId { get; set; }

        public CaseFile CaseFile { get; set; }
    }
}
