using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DropItemSystem.UnityStuff
{
    static class DiceRoll
    {
        private static Random rGenerator = new Random();

        /// <summary>
        /// Uses a random generator
        /// </summary>
        /// <returns>A double between 0 and 1</returns>
        public static double RandOneZero()
        {
            return rGenerator.NextDouble();
        }
        public static int RandInt()
        {
            return rGenerator.Next();
        }
        public static int RandInt(int Max)
        {
            return rGenerator.Next(Max);
        }

        public static int CustomRoll(int Low, int High)
        {
            return rGenerator.Next(Low, High);
        }
        public static int Roll_V6()
        {
            return rGenerator.Next(1, 6);
        }
        public static int Roll_V100()
        {
            return rGenerator.Next(1, 100);
        }

        public static double[] PointInCir(double radius)
        {
            double area = 2 * Math.PI * rGenerator.NextDouble();
            double rands = rGenerator.NextDouble() + rGenerator.NextDouble();
            double s = 0.0;

            double[] toReturn = new double[2];

            if (rands > 1)
                s = 2 - rands;
            else
                s = rands;

            toReturn[0] = radius * s * Math.Cos(area);
            toReturn[1] = radius * s * Math.Sin(area);

            return toReturn;
        }

        /// <summary>
        /// Enter a number between 0 and 1, 
        /// divides Random Num by 100 and compares it to the parameter "percentage"
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns>True or False</returns>
        public static bool Chance(double percentage)
        {            
            if (rGenerator.NextDouble() < percentage)
                return true;
            else
                return false;
        }

        public static float[] PointInCir(float rad)
        {
            float[] toReturn = new float[2];
            double temp1, temp2, temp3;

            temp1 = rGenerator.NextDouble() + rGenerator.NextDouble();
            temp2 = 0.0f;

            temp3 = 2 * Math.PI * rGenerator.NextDouble();

            if (temp1 > 1)
                temp2 = 2 - temp1;
            else
                temp2 = temp1;

            toReturn[0] = (float)(rad * temp2 * Math.Cos(temp3));
            toReturn[1] = (float)(rad * temp2 * Math.Sin(temp3));

            return toReturn;
        }
    }
}
