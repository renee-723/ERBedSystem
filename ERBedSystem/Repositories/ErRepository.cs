using ERBedSystem.Data;
using ERBedSystem.Models;

namespace ERBedSystem.Repositories
{
    public class ErRepository
    {
        private readonly ErDbContext _context;

        public ErRepository(ErDbContext context)
        {
            _context = context;
        }

        public List<Bed> GetAllBeds()
        {
            return _context.Beds.ToList();
        }

        public Patient GetWaitingPatient(string patientId)   //正在等待配床的病人
        {
            return _context.Patients.FirstOrDefault(p => p.Id == patientId && p.Status == "Waiting");
        }

        public Bed GetAvailableIcuBed() //取得可用icu床
        {
            return _context.Beds.FirstOrDefault(b => b.Zone == "ICU" && b.Status == BedStatus.Available);
        }

        //public Bed GetAvailableWardOrPedsBed()  //取得可用病床或是兒科床
        //{
        //    return _context.Beds.FirstOrDefault(b => (b.Zone == "Ward" || b.Zone == "Peds") && b.Status == "Available");
        //}
       
        public Bed GetAvailableWardBed()  // 找出一張可用的「成人留觀床」
        {
            return _context.Beds.FirstOrDefault(b => b.Zone == "Ward" && b.Status == BedStatus.Available);
        }

        public Bed GetAvailablePedsBed() // 撈取一張可用的「兒科留觀床」
        {
            return _context.Beds.FirstOrDefault(b => b.Zone == "Peds" && b.Status == BedStatus.Available);
        }

        // 根據病人 ID，把「已經躺床（Bedded）」的病人撈出來辦出院
        public Patient GetBeddedPatient(string patientId)
        {
            return _context.Patients.FirstOrDefault(p => p.Id == patientId && p.Status == "Bedded");
        }
        // 根據病人 ID，撈出他「正在進行中、尚未蓋上結束時間」的就醫紀錄
        public Encounter GetActiveEncounter(string patientId)
        {
            return _context.Encounters.FirstOrDefault(e => e.PatientId == patientId && e.EndTime == null);
        }

        // 根據床號，精準撈出被追蹤的病床物件
        public Bed GetBedById(string bedId)
        {
            return _context.Beds.FirstOrDefault(b => b.Id == bedId);
        }

        ////更新床位資訊
        //public void UpdateBed(Bed bed)
        //{
        //    _context.Beds.Update(bed);
        //}
            
        //用病歷號搜尋病人
        public Patient GetPatientById(string patientId)
        {
            return _context.Patients.FirstOrDefault(p => p.Id == patientId);
        }
        public void AddEncounter(Encounter encounter)  //就醫紀錄(一次獨立的醫療服務事件)
        {
            _context.Encounters.Add(encounter);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
