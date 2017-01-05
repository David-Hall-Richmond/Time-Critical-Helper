using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linear_Foot_Calculator
{
    class PalletGroup
    {
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Stack { get; set; }
        public int NumPiece { get; set; }
        public bool Placed { get; set; }
        public int Weight { get; set; }

        public PalletGroup(int length,int width,int height,int numPieces = 0, int weight = 0,bool stack = false)
        {
            Length = length;
            Width = width;
            Height = height;
            Stack = stack;
            NumPiece = numPieces;
            Weight = weight;
            Placed = false;
        }

        public PalletGroup(int[] inputArray,bool stack = false)
        {
            Length = inputArray[1];
            Width = inputArray[2];
            Height = inputArray[3];
            Weight = inputArray[4];
            Stack = (stack && Height <= 48);
            NumPiece = inputArray[0];
            Placed = false;
        }

        

        public override string ToString()
        {
            string piece = NumPiece.ToString();
            string len = Length.ToString();
            string wid = Width.ToString();
            string ht = Height.ToString();
            string wt = Weight.ToString();
            string stk= "non-stack";
            if (Stack)
            {
                stk = "stackable";
            }
            return piece + "@" + len + "x" + wid + "x" + ht + "   " + wt + "lbs"+ "  " + stk;

        }

        public void SwapDims()
        {
            int temp = this.Length;
            this.Length = this.Width;
            this.Width = temp;
        }

        public bool checkDims()
        {
            return (this.Length <= (53 * 12) && this.Width <= 96
                && this.Height <= 96 && (this.Weight/this.NumPiece)< 4000);
        }

        public void checkSwap()
        {
            bool needsSwap = false;
            PalletGroup swappedPallet = new PalletGroup(this.Length, this.Width,this.Height,this.NumPiece);
            swappedPallet.SwapDims();
            if (this.Length <= 96)
            {
                if (this.NumPiece == 1 && this.Width < this.Length)
                {
                    needsSwap = true;
                }
                else if (this.Width > 48 && this.Length <= 48)
                {
                    needsSwap = true;
                }
                else if (this.Length <= 48 && this.Width < this.Length)
                {
                    needsSwap = true;
                }
                else if(PalletCalc.calcLinFeet(swappedPallet, false) < PalletCalc.calcLinFeet(this, false))
                {
                    needsSwap = true;
                }
            }
            if (needsSwap)
            {
                this.SwapDims();
            }
        }

        
    }
}
