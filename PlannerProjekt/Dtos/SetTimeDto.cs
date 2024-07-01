namespace PlannerProjekt.Dtos
{
    public class SetTimeDto
    {
        public int Id { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public string Type { get; set; }
    }

}
