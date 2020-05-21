using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Pandemic_AI_Framework
{
    public class MyUtilities
    {

        public static float MapValueFloat(float value, float min1, float max1, float min2, float max2)
        {
            return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
        }

        public static float MapValueFloatConstrained(float value, float min1, float max1, float min2, float max2)
        {

            if (min1 < max1)
            {
                if (value < min1)
                {
                    return min2;
                }
                else if (value > max1)
                {
                    return max2;
                }
                else
                {
                    return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
                }
            }
            else
            {
                if (value > min1)
                {
                    return min2;
                }
                else if (value < max1)
                {
                    return max2;
                }
                else
                {
                    return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
                }
            }

        }

        public static double MapValueDouble(double value, double min1, double max1, double min2, double max2)
        {
            return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
        }


        public static int[] UniqueRandomIntegerArray(int minimumPossibleIntegerValue, int maximumPossibleIntegerValue, int numberOfUsedValues)
        {
            int[] uniqueRandomIntegerArray = new int[numberOfUsedValues];

            //initialize the array, by setting all of its values to zero
            for (int i = 0; i < numberOfUsedValues; i++)
            {
                uniqueRandomIntegerArray[i] = 0;
            }

            int numberOfPossibleNumbers = maximumPossibleIntegerValue - minimumPossibleIntegerValue;
            int[] possibleIntegerValuesContainer = new int[numberOfPossibleNumbers];
            int[] possibleIntegerValuesTransferContainer = new int[numberOfPossibleNumbers];

            // fill container
            for (int i = 0; i < numberOfPossibleNumbers; i++)
            {
                possibleIntegerValuesContainer[i] = minimumPossibleIntegerValue + i;
            }

            for (int i = 0; i < numberOfUsedValues; i++)
            {
                Random rnd = new Random();
                int randomPickIndex = rnd.Next(0, possibleIntegerValuesContainer.Length);
                int randomPickNumber = possibleIntegerValuesContainer[randomPickIndex];
                uniqueRandomIntegerArray[i] = randomPickNumber;
                possibleIntegerValuesTransferContainer = new int[possibleIntegerValuesContainer.Length - 1];
                int cnt = 0;
                for (int j = 0; j < (possibleIntegerValuesContainer.Length); j++)
                {
                    if (j != randomPickIndex)
                    {
                        possibleIntegerValuesTransferContainer[cnt] = possibleIntegerValuesContainer[j];
                        cnt++;
                    }
                }
                possibleIntegerValuesContainer = possibleIntegerValuesTransferContainer;
            }
            return uniqueRandomIntegerArray;
        }
    }
}