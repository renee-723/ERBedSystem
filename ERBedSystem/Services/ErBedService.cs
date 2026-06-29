using ERBedSystem.Models;
using ERBedSystem.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
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
        //用病歷號找病人
        public Patient GetPatient(string patientId)
        {
            return _repo.GetPatientById(patientId);
        }

        //新增病人
        public bool CreatePatient(Patient patient,out string message)
        {
            message = " ";
            if(_repo.GetPatientById(patient.Id) != null)
            {
                message = "該病歷號已存在，無法重複建立";
                return false;
            }
            patient.Status = "Waiting";
            _repo.AddPatient(patient);
            message = "病人建立成功";
            return true;
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
                if(patient.Age < 18)
                {
                    targetBed= _repo.GetAvailablePedsBed();

                    if(targetBed == null)
                    {
                        targetBed=_repo.GetAvailableWardBed();
                        if(targetBed != null)
                        {
                            message = "【臨床調度】因兒科床位全滿，啟動替代機制引導至一般留觀床。";
                        }
                    }
                }
                else
                {
                    targetBed = _repo.GetAvailableWardBed();
                }
                
            }

            if (targetBed == null)
            {
                //message = $"目前急診無空床!查無符合檢傷級數{patient.TriageLevel}的病床";
                isSuccess = false;
                message = $"【滿床警告】目前急診大爆滿！查無適合病人 (年齡:{patient.Age}歲/檢傷:{patient.TriageLevel}級) 的可用空床，已安排於候診區等待。";
                return null;
            }

            targetBed.Status = BedStatus.Occupied;
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
            var targetBed = _repo.GetBedById(encounter.BedId);
            if (targetBed != null)
            {
                targetBed.Status = BedStatus.Cleaning;
            }
            else
            {
                message= $"【警告】病人已辦理出院，但在資料庫中找不到對應的 [{encounter.BedId}] 號病床，請手動確認。";
            }
        

            patient.Status = "Discharged"; //病人狀態改為已出院
            encounter.EndTime = DateTime.Now; //改上出院時間
            _repo.SaveChanges(); //存檔
            
            isSuccess=true;
            message= $"【出院成功】病人 [{patient.Name}] 已辦理出院！目前 [{encounter.BedId}] 號病床已切換為【清潔中】狀態，請通知清消人員。";

            return encounter;
        }

        //完成清消並讓床位恢復可用狀態
        public bool CompleteBedCleaning(string bedId , out string message)
        {
            message = " ";
            var bed = _repo.GetBedById(bedId);
            if(bed == null)
            {
                message = $"找不到床號為{bedId}的床位";
                return false;
            }
            if(bed.Status != BedStatus.Cleaning)
            {
                message = $"病床[{bedId}]目前狀態為{bed.Status}，不需執行清消";
                return false;
            }
            bed.Status = BedStatus.Available;
            _repo.SaveChanges();

            message = $"[清消完成]病床[{bedId}]已完成清潔消毒，恢復為可使用空床";
            return true;
        }
    }
}
