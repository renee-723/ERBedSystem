namespace ERBedSystem.Models
{
    public class AuditLog
    {
        public int Id {  get; set; }
        public string Operation {  get; set; } //做了什麼
        public string PatientId {  get; set; }
        public string Message {  get; set; }  //描述
        public DateTime Timestamp { get; set; } = DateTime.Now; //操作時間
    }
}
