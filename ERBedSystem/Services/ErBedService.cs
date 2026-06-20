using ERBedSystem.Models;
using ERBedSystem.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.X509Certificates;

namespace ERBedSystem.Services
{
    public class ErBedService
    {
        private readonly ErRepository _repo;

        public ErBedService(ErRepository repo)
        {
            _repo = repo;
        }
        public List<Bed> GetAllBeds()
        {
            return _repo.GetAllBeds();
        }
        //自動派床演算法
        public Encounter AssignBed(string patientId , out string message , out bool isSuccess)
        {
            message = "";
            isSuccess = false;

            var patient = _repo.GetWaitingPatient(patientId);
            if (patient == null)
            {
                message = "找不到該位等待配床的病人或該病人已安置完成";
                return null;
            }

            Bed targetBed = null;

            
            //檢傷邏輯演算法
            if(patient.TriageLevel <= 2)
            {
                targetBed = _repo.GetAvailableIcuBed();
            }
            else
            {
                targetBed=_repo.GetAvailableWardOrPedsBed();
            }

            if (targetBed == null)
            {
                message = $"目前急診無空床!查無符合檢傷級數{patient.TriageLevel}的病床";
                return null;
            }

            targetBed.Status = "Occupied";
            patient.Status = "Bedded";

            var newEncounter = new Encounter
            {
                Id = "E" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                PatientId = patientId,
                BedId = targetBed.Id,
                StartTime = DateTime.Now,
                EndTime = null
            };

            _repo.AddEncounter(newEncounter);
            _repo.SaveChanges();

            isSuccess=true;
            message = $"【配床成功】危急程度{patient.TriageLevel}級病人{patient.Name}已成功指派至[{targetBed.Id}]床!";
            return newEncounter;
        }
        //辦理出院與釋出床位
        public Encounter DischargePatient(string patientId, out string message , out bool isSuccess)
        {
            message = "";
            isSuccess = false;

            var patient = _repo.GetBeddedPatient(patientId);
            if(patient == null)
            {
                message = "找不到該位正在急診的病人，無法辦理出院";
                return null;
            }
            //翻出目前的就醫紀錄
            var encounter=_repo.GetActiveEncounter(patientId);
            if (encounter == null)
            {
                message = "查無該病人進行中的就醫紀錄";
                return null;
            }

            //查完就醫紀錄後，把正在使用的床一起撈出來
            //var beds = _repo.GetAllBeds();
            //var targetBed = beds.FirstOrDefault(b => b.Id == encounter.BedId);
            var targetBed = _repo.GetBedById(encounter.Id);
            if (targetBed != null)
            {
                targetBed.Status = "Available";
            }

            patient.Status = "Discharged"; //病人狀態改為已出院
            encounter.EndTime = DateTime.Now; //改上出院時間
            _repo.SaveChanges(); //存檔
            
            isSuccess=true;
            message=$"【出院成功】病人 [{patient.Name}] 已辦理出院，順利釋放 [{encounter.BedId}] 號病床！";

            return encounter;
        }
    }
}
