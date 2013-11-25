using System.Collections.Generic;

namespace TwitterQuiz.Domain
{
    public class Round
    {
        public string Name { get; set; }
        public int Sequence { get; set; }
        public IList<Question> Questions { get; set; }

        public Round()
        {
            Questions = new List<Question>();
        }
    }
}
