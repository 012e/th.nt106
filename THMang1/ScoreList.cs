using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THMang1
{
    internal class ScoreList
    {
        public List<double> Scores {  get; private set; } = new List<double>();

        internal ScoreList(List<double> scores)
        {
            this.Scores = scores;
        }

        public double GetAverage()
        {
            double sum = 0;
            foreach (double score in Scores)
            {
                sum += score;
            }
            return sum / Scores.Count;
        }

        public double GetMax()
        {
            double max = Scores[0];
            foreach (double score in Scores)
            {
                if (score > max)
                {
                    max = score;
                }
            }
            return max;
        }

        public double GetMin()
        {
            double min = Scores[0];
            foreach (double score in Scores)
            {
                if (score < min)
                {
                    min = score;
                }
            }
            return min;
        }

        public int GetPassedCount()
        {
            int count = 0;
            foreach (double score in Scores)
            {
                if (score >= 5d)
                {
                    count++;
                }
            }
            return count;
        }

        public int GetFailedCount()
        {
            return Scores.Count - GetPassedCount();
        }

        private int GetCountOfScoreBelow(double score)
        {
            int count = 0;
            for (int i = 0; i < Scores.Count; i++)
            {
                if (Scores[i] < score)
                {
                    count++;
                }
            }
            return count;
        }

        public string GetClassification()
        {
            double averageScore = GetAverage();
            if (averageScore >= 8d && GetCountOfScoreBelow(6.5) == 0)
            {
                return "Giỏi";
            }
            else if (averageScore >= 6.5d && GetCountOfScoreBelow(5) == 0)
            {
                return "Khá";
            }
            else if (averageScore >= 5d && GetCountOfScoreBelow(3.5) == 0)
            {
                return "TB";
            }
            else if (averageScore >= 3.5d && GetCountOfScoreBelow(2) == 0)
            {
                return "Yếu";
            }
            return "Kém";
        }


        public static ScoreList FromString(string s)
        {
            if (s == "")
            {
                throw new Exception("Vui lòng nhập danh sách điểm.");
            }
            try
            {
                List<double> scores = new List<double>();
                string[] scoreStrings = s.Split(',');
                foreach (string scoreString in scoreStrings)
                {
                    scores.Add(Double.Parse(scoreString));
                }
                return new ScoreList(scores);
            }
            catch (Exception e)
            {
                throw new Exception("Chuỗi đầu vào không hợp lệ. Xin vui lòng nhập lai.");
            }
        }
    }
}
