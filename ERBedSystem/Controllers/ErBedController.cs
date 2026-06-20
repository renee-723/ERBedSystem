using ERBedSystem.Data;
using ERBedSystem.Models;
using ERBedSystem.Repositories;
using ERBedSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERBedSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErBedController : ControllerBase
    {
        //宣告私有的資料庫大管家欄位
        private readonly ErBedService _bedService;
        // 利用建構子注入（Constructor Injection），讓大樓在開機時把大管家送進來
        public ErBedController(ErBedService bedService)
        {
            _bedService = bedService;
        }

        //// 臨時記憶體急診室：模擬資料庫的靜態資料夾
        //private static List<Bed> _beds = new List<Bed>
        //{
        //    new Bed { Id = "ICU-01", Zone = "ICU", Status = "Available", Description = "靠近護理站，有生理監視器" },
        //    new Bed { Id = "ICU-02", Zone = "ICU", Status = "Occupied", Description = "葉克膜專用床" },
        //    new Bed { Id = "ER-A01", Zone = "Ward", Status = "Available", Description = "留觀 A 區靠窗" },
        //    new Bed { Id = "ER-A02", Zone = "Ward", Status = "Cleaning", Description = "留觀 A 區，剛出院消毒中" },
        //    new Bed { Id = "PEDS-01", Zone = "Peds", Status = "Available", Description = "兒科急診獨立隔離床" }
        //};

        //private static List<Patient> _patients = new List<Patient>
        //{
        //    new Patient { Id = "P101", Name = "張小明", TriageLevel = 3, Status = "Waiting", ChiefComplaint = "車禍右腳擦傷" },
        //    new Patient { Id = "P102", Name = "王大同", TriageLevel = 1, Status = "Waiting", ChiefComplaint = "疑似心肌梗塞，OHCA" }
        //};

        //private static List<Encounter> _encounters = new List<Encounter>();


        // 窗口 1：查看所有病床現況 (GET api/erbed)
        [HttpGet]
        public IActionResult GetAllBeds()
        {
            var beds = _bedService.GetAllBeds();
            return Ok(beds); // 200 OK 吐出整張床位表
        }

        [HttpGet("auto-assign")]
        public IActionResult AutoAssignBed([FromQuery]string patientId)
        {
            //用LinQ抓出是哪位病人要派床
            var encounter = _bedService.AssignBed(patientId, out string message, out bool isSuccess);
            if (!isSuccess)
            {
                if (message.Contains("找不到"))
                {
                    return NotFound(message);
                }
                return BadRequest(message);
            }

            ////檢傷級數分類
            //if (patient.TriageLevel <= 2)
            //{
            //    //檢傷1-2級優先派往icu重症區床位
            //    targetBed = _bedService.GetAvailableIcuBed();
            //}
            //else
            //{
            //    //檢傷3-5級派往普通留觀區或兒科床
            //    targetBed = _repo.GetAvailableWardOrPedsBed();
            //}

            ////如果沒床就塞回等待序列
            //if (targetBed == null)
            //{
            //    return BadRequest($"目前急診無空床!查無符合檢傷級數{patient.TriageLevel}級可用病床!");
            //}

            //targetBed.Status = "Occupied";  //病床狀態改為"使用中"
            //patient.Status = "Bedded"; //病人狀態改為"已躺床"

            ////建立就醫事件紀錄
            //var newEncounter = new Encounter
            //{
            //    Id = "E"+ DateTime.Now.ToString("yyyyMMddHHmmss"),
            //    PatientId = patientId,
            //    BedId = targetBed.Id,
            //    StartTime = DateTime.Now,
            //    EndTime = null
            //};
            //_repo.AddEncounter(newEncounter);
            //_repo.SaveChanges();

            return Ok(new
            {
                Message = message,
                EncounterRecord =encounter
            });
        }
        //辦理病人出院並釋出床位(POST api/erbed/discharge)
        [HttpGet("discharge")]
        public IActionResult DischargePatient([FromQuery] string patientId)
        {
            var encounter = _bedService.DischargePatient(patientId, out string message, out bool isSuccess);
            if (!isSuccess)
            {
                return BadRequest(message);
            }

            //辦理成功，回傳200 OK
            return Ok(new
            {
                Message = message, //回傳訊息
                EncounterRecord = encounter
            });
        }
    }
}

