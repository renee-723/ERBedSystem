namespace ERBedSystem.Models
{
    public class TransferRequest
    {
        public string PatientId { get; set; }
        public string OldBedId { get; set; }
        public string NewBedId { get; set; }
        public string Reason { get; set; }
    }
}