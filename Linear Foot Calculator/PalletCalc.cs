using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linear_Foot_Calculator
{
    class PalletCalc
    {   
        //returns the linear feet for the given pallet group. calcWeight2Feet option
        //allows to return a pure linear feet if false, or return the greater of calc
        //by dim or calc by weight if true
        public static int calcLinFeet(PalletGroup current,bool calcWeight2Feet)
        {

            int howManyWide = (current.Width < 96) ? 96 / current.Width : 1;
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

        public static string calcClass(PalletGroup current)
        {
            double poundsPer = PalletCalc.getPCF(current);
            if (poundsPer >= 50)
            {
                return 50.ToString();
            }
            else if (poundsPer < 50 && poundsPer >= 35)
            {
                return 55.ToString();
            }
            else if (poundsPer < 35 && poundsPer >= 30)
            {
                return 60.ToString();
            }
            else if (poundsPer < 30 && poundsPer >= 22.5)
            {
                return 65.ToString();
            }
            else if (poundsPer < 22.5 && poundsPer >= 15)
            {
                return 70.ToString();
            }
            else if (poundsPer < 15 && poundsPer >= 13.5)
            {
                return 77.5.ToString();
            }
            else if (poundsPer < 13.5 && poundsPer >= 12)
            {
                return 85.ToString();
            }
            else if (poundsPer < 12 && poundsPer >= 10.5)
            {
                return 92.5.ToString();
            }
            else if (poundsPer < 10.5 && poundsPer >= 9)
            {
                return 100.ToString();
            }
            else if (poundsPer < 9 && poundsPer >= 8)
            {
                return 110.ToString();
            }
            else if (poundsPer < 8 && poundsPer >= 7)
            {
                return 125.ToString();
            }
            else if (poundsPer < 7 && poundsPer >= 6)
            {
                return 150.ToString();
            }
            else if (poundsPer < 6 && poundsPer >= 5)
            {
                return 175.ToString();
            }
            else if (poundsPer < 5 && poundsPer >= 4)
            {
                return 200.ToString();
            }
            else if (poundsPer < 4 && poundsPer >= 3)
            {
                return 250.ToString();
            }
            else if (poundsPer < 3 && poundsPer >= 2)
            {
                return 300.ToString();
            }
            else if (poundsPer < 2 && poundsPer >= 1)
            {
                return 400.ToString();
            }
            else
            {
                return 500.ToString();
            }

        }

        public static int linearToCubicCalc(int linFeet)
        {
            return linFeet * 64;
        }

    }
}
