using ERBedSystem.Data;
using ERBedSystem.Models;
using ERBedSystem.Repositories;
using ERBedSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace ERBedSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErBedController : ControllerBase
    {
        //宣告私有的資料庫大管家欄位
        private readonly ErBedService _bedService;
        private readonly ErRepository _repo;
        // 利用建構子注入（Constructor Injection），讓大樓在開機時把大管家送進來
        public ErBedController(ErBedService bedService, ErRepository repo)
        {
            _bedService = bedService;
            _repo = repo;
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

            return Ok(new
            {
                Message = message,
                EncounterRecord =encounter
            });
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

        }
        //新增病床數量
        [HttpPost("addBed")]
        public IActionResult CreateBed([FromBody] Bed bed)
        {
            if (bed == null || string.IsNullOrEmpty(bed.Id))
            {
                return BadRequest("床位資料不完整，必須包含床位編號(Id)");
            }
            //檢查是否有重複的床位
            if (_bedService.BedExists(bed.Id))
            {
                return BadRequest($"床位編號 [{bed.Id}] 已存在");
            }
            //執行新增
            _bedService.CreateBed(bed);
            return Ok(new { Message = "床位已新增成功", Bed = bed });
        }
        //計算總床數有多少
        [HttpGet("allBedsNumber")]
        public IActionResult GetAllBedNumber()
        {
            var stats = _bedService.GetAllBedNumber();
            return Ok(stats);
        }

        //新增病人
        [HttpPost("patient")]
        public IActionResult CreatPatient([FromBody]Patient patient)
        {
            if(patient == null || string.IsNullOrEmpty(patient.Id))
            {
                return BadRequest("病人資料不得為空且必須包含病歷號");
            }
            bool isSuccess = _bedService.CreatePatient(patient, out string message);
            if (!isSuccess)
            {
                return BadRequest(message);
            }
            return Ok(new { message, patient });
        }
        //查詢病人
        [HttpGet("patient/{patientId}")]
        public IActionResult GetPatient(string patientId)
        {
            var patient = _bedService.GetPatient(patientId);
            if (patient == null)
            {
                return NotFound($"查無此病歷號[{patientId}]的病人資料");
            }
            return Ok(patient);
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
                Message = message, 
                EncounterRecord = encounter
            });
        }
        //清床
        [HttpPut("complete-cleaning")]
        public IActionResult CompleteCleaning([FromQuery] string bedId)
        {
            bool isSuccess = _bedService.CompleteBedCleaning(bedId, out string message);
            if (!isSuccess)
            {
                return BadRequest(message);   //回傳400錯誤
            }
            return Ok(message);  //回傳200 ok
        }
        //取消病人出院
        [HttpPut("patient/{patientId}/undo-discharge")]
        public IActionResult UndoDischarge(string patientId)
        {
            bool isSuccess = _bedService.UndoDischarge(patientId, out string message);
            if(!isSuccess) return BadRequest(message);

            return Ok(message);
        }

    }
}

