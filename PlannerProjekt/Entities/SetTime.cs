using System.ComponentModel.DataAnnotations;

namespace PlannerProjekt.Entities
{
    public class SetTime
    {
        [Key]
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime TimeFrom { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime TimeTo { get; set; }
        public string Type { get; set; } = null!; // WorkTime/FreeTime/Default

    }
}
