using System.Threading.Tasks;
using UnityEngine;

namespace tetris.Scripts.Extensions
{
    public static class TimeExtensions
    {
        public static int ToMiliseconds(this float seconds) => (int)(seconds * 1000);
        public static int ToMiliseconds(this int seconds) => (seconds * 1000);

        public static float RandomTime(int min, int max)
        {
            float randomTime = Random.Range(min, max);
            return randomTime;
        }
        
        public static float RandomTime(float min, float max)
        {
            float randomTime = Random.Range(min, max);
            return randomTime;
        }
    }
}