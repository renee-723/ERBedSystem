using Microsoft.EntityFrameworkCore;
using ERBedSystem.Models;

namespace ERBedSystem.Data
{
    // 繼承 DbContext，代表它正式成為 EF Core 的資料庫大管家
    public class ErDbContext : DbContext
    {
        public ErDbContext(DbContextOptions<ErDbContext> options) : base(options)
        {
        }

        // 宣告資料庫裡要有的三張實體資料表（抽屜）
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Encounter> Encounters { get; set; }


        // ==========================================
        // 【全新加入】當資料庫建立時，順便播下初始資料的種子
        // ==========================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 1. 灌入初始病床資料
            modelBuilder.Entity<Bed>().HasData
              (
               new Bed { Id = "ICU-01", Zone = "ICU", Status = BedStatus.Available, Description = "靠近護理站，有生理監視器" },
               new Bed { Id = "ICU-02", Zone = "ICU", Status = BedStatus.Occupied, Description = "葉克膜專用床" },
               new Bed { Id = "ER-A01", Zone = "Ward", Status = BedStatus.Available, Description = "留觀 A 區靠窗" },
               new Bed { Id = "ER-A02", Zone = "Ward", Status = BedStatus.Cleaning, Description = "留觀 A 區，剛出院消毒中" },
               new Bed { Id = "PEDS-01", Zone = "Peds", Status = BedStatus.Available, Description = "兒科急診獨立隔離床" }
              );
            // 2. 灌入初始病人資料
            modelBuilder.Entity<Patient>().HasData
              (
                new Patient { Id = "P101", Name = "張小明", TriageLevel = 3, Status = "Waiting", ChiefComplaint = "車禍右腳擦傷" },
                new Patient { Id = "P102", Name = "王大同", TriageLevel = 1, Status = "Waiting", ChiefComplaint = "疑似心肌梗塞，OHCA" },
                new Patient { Id = "P103", Name = "李小奎", TriageLevel = 3, Status = "Waiting", ChiefComplaint = "AGE" }

              );
        }
    }
}