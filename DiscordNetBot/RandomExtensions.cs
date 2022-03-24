using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordNetBot
{
    public static class RandomExtensions
    {
        public static T Shuffle<T>(this IList<T> items)
        {
            int n = items.Count;
            Random random = new Random();
            while (n > 1)
            {
                int k = (random.Next(0, n) % n);
                n--;
                T value = items[k];
                items[k] = items[n];
                items[n] = value;
            }

            return items[random.Next(items.Count)];
        }

        public static T Random<T>(this IList<T> items)
        {
            var random = new Random();

            return items[random.Next(items.Count)];
        }
    }
}
