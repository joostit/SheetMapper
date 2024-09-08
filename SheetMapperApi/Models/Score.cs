namespace SheetMapperApi.Models
{
    public class Score
    {

        public int Positive { get; set; }

        public int Negative { get; set; }

        /// <summary>
        /// Gets the review ratio as a value between -1 and 0
        /// </summary>
        public double Ratio
        {
            get => (Positive - Negative) / (double)(Positive + Negative);
        }

    }
}
