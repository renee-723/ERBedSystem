namespace ERBedSystem.Models
{
    public class Patient
    {
        //病歷號
        public string Id {  get; set; }

        //姓名
        public string Name { get; set; }

        public int Age { get; set; }

        //檢傷級數
        public int TriageLevel {  get; set; }

        //病人狀態(剛被送到急診時，狀態一律是 Waiting；等我們執行派床成功，就要把他的狀態改成 Bedded)
        public string Status {  get; set; }

        //主訴/旁訴
        public string ChiefComplaint {  get; set; }
    }
}
