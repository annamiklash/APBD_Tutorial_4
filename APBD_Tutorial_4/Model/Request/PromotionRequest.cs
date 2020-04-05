namespace APBD_Tutorial_4.Model
{
    public class PromotionRequest
    {
        public string Studies { get; set; }
        public int Semester { get; set; }

        public PromotionRequest(string studiesName, int semester)
        {
            Studies = studiesName;
            Semester = semester;
        }

        public PromotionRequest()
        {

        }
    }
}