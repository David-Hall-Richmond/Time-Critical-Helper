using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linear_Foot_Calculator
{
    class PalletCalc
    {
        public static int calcLinFeet(PalletGroup current,bool calcWeight2Feet)
        {

            int howManyWide = 96 / current.Width;
            int heightMultiple = 96 / current.Height;
            int linFeetByWeight = ((int)Math.Ceiling(current.Weight / 1000.00));
            int linearFeet;
            if (current.Stack)
            {
                howManyWide *= heightMultiple;
            }

            int howManyLong = current.NumPiece / howManyWide;
            int leftOver = current.NumPiece % howManyWide;
            int linearInches = (howManyLong * current.Length);
            if (leftOver > 0)
            {
                if (leftOver * current.Length <= 96 && current.Length > current.Width)
                {
                    linearInches += current.Width;
                }
                else
                {
                    linearInches += current.Length;
                }
            }

            linearFeet = (int)Math.Ceiling((double)linearInches / 12);
            if (linearFeet < linFeetByWeight && calcWeight2Feet)
            {
                return linFeetByWeight;
            }

            return linearFeet;
        }

        public static double getPCF(PalletGroup current)
        {
            double cubicFeet = ((current.Length * current.Width * current.Height) * current.NumPiece) / 1728;
            return (current.Weight / cubicFeet);
        }

    }
}
