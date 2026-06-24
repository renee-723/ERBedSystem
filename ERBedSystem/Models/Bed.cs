namespace ERBedSystem.Models
{
    public class Bed
    {
       //唯一辨識碼
        public string Id {  get; set; }

        //區域(ICU、留觀區、兒科急診)
        public string Zone { get; set; }

        //狀態機核心(空床、使用中、消毒中)
        public BedStatus Status {  get; set; }

        //床位備註(需靠近護理站，有血壓機，有幫浦)
        public string Description {  get; set; }
    }
}
