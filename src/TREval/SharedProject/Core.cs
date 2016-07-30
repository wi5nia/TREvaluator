using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SharedProject
{
    public class Core
    {
        private static async Task<Emotion[]> GetEmotion(Stream stream)
        {
            string emotionKey = "396d77fff930414889863cbadbdf6399";

            EmotionServiceClient emotionClient = new EmotionServiceClient(emotionKey);

            var emotionResults = await emotionClient.RecognizeAsync(stream);

            if (emotionResults == null || emotionResults.Count() == 0)
            {
                throw new Exception("Can't detect face");
            }

            return emotionResults;
        }

        //Average happiness calculation in case of multiple people
        public static async Task<float> GetAvgEmotionScore(Stream stream)
        {
            Emotion[] emotionResults = await GetEmotion(stream);

            float score = 0;
            foreach (var emotionResult in emotionResults)
            {
                score = score + emotionResult.Scores.Happiness;
            }

            return score / emotionResults.Count();
        }

        public static string GetEmotionMessage(float score)
        {
            score = score * 100;
            double result = Math.Round(score, 2);

            if (score >= 70)
                return result + " % :-)";
            //else if (score >= 45 || score <= 69)
            //    return result + "% :-|";
            else
                return result + "% :-(";
        }
    }
}