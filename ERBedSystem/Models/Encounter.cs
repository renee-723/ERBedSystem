using System.Data;

namespace ERBedSystem.Models
{
    public class Encounter
    {
        //健保卡號
        public string Id {  get; set; }

        //紀錄病人病歷號
        public string PatientId {  get; set; }

        //幾床
        public string? BedId {  get; set; }

        //推入此床時間
        public DateTime StartTime {  get; set; }

        //離院時間或是轉院轉床轉科別
        public DateTime? EndTime {  get; set; }
    }
}
