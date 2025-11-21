using LW_4_3_5_Daryev_PI231.Enumerations;

namespace LW_4_3_5_Daryev_PI231.DTOs
{
    public class UpdateAssetDTO
    {
        public string Name { get; set; }
        public DeviceType Type { get; set; }
        public DateTime HourlyRate { get; set; }
        public Status Status { get; set; }
    }
}
