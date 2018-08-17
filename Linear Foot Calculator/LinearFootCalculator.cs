using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/* update needs: 
 * - addButton_Click method needs to be seperated into its own method
 * - later add functionality to edit an existing box
 * - add spot quote calculator
 * - add volume quote calculator
 * - add freight class calculator
 */
/***************************************************************************************************************
 * *************************************************************************************************************
 * *******************************Linear Foot Calculator Section************************************************
 * */

namespace Linear_Foot_Calculator
{
   public partial class TimeCriticalHelper : Form
   {

                public TimeCriticalHelper()
                {
                    InitializeComponent();
                }

     

        private void textBox_EnterSelectAll(object sender, EventArgs e)
        {
            TextBox clearBox = (TextBox)sender;
            clearBox.SelectAll();
        }


        /***************************
         * Volume Quote Helper Calculations
         * *************************/

        /*private void linearToCubicEntry(object sender, EventArgs e)
        {
            int input;
            TextBox inputBox = (TextBox)sender;
            if (int.TryParse(inputBox.Text, out input)){
                if(inputBox == linInchBox)
                {
                    input /=12;
                }
                cubeOutputLabel.Text = PalletCalc.linearToCubicCalc(input).ToString();
            }
            else
            {
                cubeOutputLabel.Text = "";
            }

        }*/

        /***********************
         * Volume Notes Creation
         * *********************/


        private void generateNotesButton_Click(object sender, EventArgs e)
        {
            string notes = createNotes();
            Clipboard.SetText(notes);
            copyNotes.Text = notes;
        }

        //checks which rate notes are needed and creates the string to load
        //into the note box
        private string createNotes()
        {
            bool[] ratesNeeded = new bool[] { checkGMS1.Checked, checkGMS2.Checked,
                                              checkGMS3.Checked, checkGMSX.Checked };
            string notes= "Rates by Calculator:\r\n";
            decimal accessorial = 0;
            decimal customPercent;

            if (getClipboardTotal() != -1)
            {
                notes += Clipboard.GetText()+"\r\n";
            }
            if (decimal.TryParse(accessorialBox.Text, out accessorial)) { }

            for (int i = 0; i < 3; i++) {
                if (ratesNeeded[i])
                {
                    notes += "GMS" + (i + 1).ToString() + ": " + 
                        createNoteString((decimal)(.45 + (i * .2)), accessorial) + "\r\n";
                }
            }

            if (checkGMSX.Checked)
            {
                if (decimal.TryParse(gmsxBox.Text, out customPercent))
                {
                    customPercent /= 100;
                    notes += "GMSX: " + createNoteString(customPercent, accessorial);
                }
                else
                {
                    MessageBox.Show("Invalid percent,no notes created");
                }
            }

            return notes;
            
        }

        //creates a note string for a single rate and returns the string
        private string createNoteString(decimal percent,decimal accessorial)
        {
            decimal total;
            decimal upcharge;
            decimal rate;
            string noteString = "";
            string percentAsString = ((int)(percent * 100)).ToString() + "%";
            if (getClipboardTotal() != -1)
            {
                total = getClipboardTotal();
            }
            else
            {
                decimal.TryParse(laneReviewTotalBox.Text, out total);
            }
            
                upcharge = total * percent;
                rate = (decimal)(total + upcharge+ accessorial);
                noteString += total.ToString("C") + " total + " + upcharge.ToString("C") + "(" + percentAsString + ")";
                if(accessorial > 0)
                {
                    noteString += "  " + ((decimal)accessorial).ToString("C") + "acc ";
                }
                noteString += "= " + rate.ToString("C");
            
            return noteString;
        }

        private void customCheckBoxClicked(object sender, EventArgs e)
        {
            gmsxBox.Visible = checkGMSX.Checked;
            gmsxPercentLabel.Visible = checkGMSX.Checked;
        }

        private void quoteNotesClearButton_Click(object sender, EventArgs e)
        {
            copyNotes.Text = "";
            Clipboard.Clear();
        }

        private void copyToClipboardButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(copyNotes.Text);
        }

        //tokenizes the string, checks if the second to last token is "Total" and returns the total
        //if the string is empty or the second to last is not "Total" returns -1 to signify not found
        private decimal getClipboardTotal()
        {
            string[] tokenClipBoard = Clipboard.GetText().Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            
            decimal volTotal = 0;
            if (tokenClipBoard.Length>=2 && tokenClipBoard[tokenClipBoard.Length - 2].Equals("Total"))
            {
                volTotal = decimal.Parse(tokenClipBoard[tokenClipBoard.Length - 1]);
            }
            else
            {
                return -1;
            }
            return volTotal;
        }

        /*******************************
         * Spot Quote Notes Creation
         * *****************************/

        const int MILES_PER_HOUR = 45;
        private void spotQuoteNoteButton_Click(object sender, EventArgs e)
        {
            string termNum = dedicatedTextBox.Text;
            string quoteNum = quoteNumTextBox.Text;
            decimal ltlCost;
            int transitToTerm;
            int dedicatedMileage;
            decimal truckCost = getTruckCost();
            decimal accessorialCost = getAccesorialCost();

            Decimal.TryParse(ltlCostTextBox.Text, out ltlCost);
            Int32.TryParse(transitTimeTextBox.Text, out transitToTerm);
            Int32.TryParse(mileageTextBox.Text, out dedicatedMileage);

            string spotQuoteNotes = createSpotQuoteNotes(termNum, quoteNum, ltlCost, transitToTerm,
                                                        dedicatedMileage, truckCost,accessorialCost);
            copyNotes.Text = spotQuoteNotes;
            Clipboard.SetText(spotQuoteNotes);
            
        }

        private void checkIsNum(object sender, EventArgs e)
        {
            numsOnly(sender);   
        }

        private void checkIsNum(object sender, KeyEventArgs e)
        {
            numsOnly(sender);
        }

        private void numsOnly(object sender)
        {
            TextBox sentFrom = (TextBox)sender;
            decimal dummy = 0;
            if (!Decimal.TryParse(sentFrom.Text, out dummy))
            {
                if (sentFrom.Text.Length == 0)
                {
                    sentFrom.Text = "";
                }
                else
                {
                    sentFrom.Text = sentFrom.Text.Remove(sentFrom.Text.Length - 1);
                }
            }
        }

        private decimal getAccesorialCost()
        {
            Tuple<CheckBox, decimal>[] dedAccess =
           {
                new Tuple<CheckBox,decimal>(liftGateCheck,275M),
                new Tuple<CheckBox,decimal>(insideDelCheck,225M),
                new Tuple<CheckBox,decimal>(newYorkDelCheck,500M),
                new Tuple<CheckBox,decimal>(hazCheck,500M),
                new Tuple<CheckBox,decimal>(upgradeCheck,75M),
           };
            decimal total = 0M;
            for(int i=0; i<5;i++)
            {
                if (dedAccess[i].Item1.Checked)
                    total+= dedAccess[i].Item2;
            }

            decimal pickupAccess;
            Decimal.TryParse(pickAccessText.Text, out pickupAccess);
            total += pickupAccess;
            return total;
        }

        private decimal getTruckCost()
        {
            decimal truckCost = 0;
            switch (truckSizeComboBox.Text)
            {
                case "Cargo Van":
                    truckCost = 3.75M;
                    break;
                case "12ft Truck":
                    truckCost = 5.00M;
                    break;
                case "24ft Truck":
                    truckCost = 5.50M;
                    break;
                case "53ft Truck":
                    truckCost = 6.50M;
                    break;
            }
            return truckCost;

        }

        private decimal checkTruckMin(decimal truckCost,decimal dedicatedCost)
        {
            decimal retValue = dedicatedCost;
            if(truckCost==3.75M && dedicatedCost< 650.00M){
                retValue = 650.00M;
            }
            if (truckCost == 5.00M && dedicatedCost < 795.00M)
            {
                retValue = 795.00M;
            }
            if (truckCost ==5.50M && dedicatedCost < 895.00M)
            {
                retValue = 895.00M;
            }
            if (truckCost == 6.50M && dedicatedCost < 1495.00M)
            {
                retValue = 1495.00M;
            }

            return retValue;

        }

        private string createSpotQuoteNotes(string termNum, string quoteNum, decimal ltlCost, int transitToTerm,
                                                        int dedicatedMileage, decimal truckCost,decimal accessorialCost)
        {
            //calculate transit hours based on mileage/45 + 2 hours for every 1000 miles, starting at mile 1
            int transitHours = (int)(dedicatedMileage / MILES_PER_HOUR + .3)+2+((dedicatedMileage/1000)*2);
            decimal dedicatedCost = checkTruckMin(truckCost,(dedicatedMileage * truckCost));
            decimal total = ltlCost + dedicatedCost + accessorialCost;

            string notes = "";
            string ltlAsString = ltlCost.ToString("C");
            string truckAsString = truckCost.ToString("C");
            string dedicatedAsString = dedicatedCost.ToString("C");
            string accessorialAsString = accessorialCost.ToString("C");
            string totalAsString = total.ToString("C");

            notes += "Spot quote ded from " + termNum + "\r\n";
            notes += "Orig to " + termNum + " " + transitToTerm + " day trans ltl=";
            notes += ltlAsString + " Q#" + quoteNum + "\r\n";
            notes += termNum + " to dest " + dedicatedMileage + "mi/" + transitHours + "hr trans @" + truckAsString;
            notes += "/mi = " + dedicatedAsString + "\r\n";
            notes += ltlAsString + "LTL + " + dedicatedAsString + "DED ";
            if(accessorialCost>0)
                notes+= " + " + accessorialAsString + "ACCESS \r\n";
            notes += totalAsString + "\r\nBOA and weather\r\n";
            

            return notes;
        }

        private void spotQuoteResetButton_Click(object sender, EventArgs e)
        {
            dedicatedTextBox.Text = "";
            quoteNumTextBox.Text = "";
            ltlCostTextBox.Text = "";
            transitTimeTextBox.Text = "";
            mileageTextBox.Text = "";
            truckSizeComboBox.SelectedItem = "12ft Truck";
        }

        private void actualAmtGroup_Enter(object sender, EventArgs e)
        {
            
        }

        private void updateSpecialPricingLabels(){
            decimal twelveKGross;
            decimal CWT;
            decimal twelveKDiscount;

            Int32 actualWeight;
            decimal fuelPercent;
            decimal gmsCharge;
            decimal totalAccessorials;

            decimal actualGross;
            decimal actualDiscountPercentage;
            decimal percentForDisplay;
            decimal actualDiscountAmount;
            decimal fuelTotal;
            decimal netTotal;
            decimal totalCharge;


            Decimal.TryParse(tenKGrossBox.Text, out twelveKGross);
            Decimal.TryParse(CWTBox.Text, out CWT);
            Decimal.TryParse(tenKDiscBox.Text, out twelveKDiscount);

            Int32.TryParse(actualWgtBox.Text, out actualWeight);
            Decimal.TryParse(fuelBox.Text, out fuelPercent);
            fuelPercent /= 100;
            //Decimal.TryParse(gmsChargeBox.Text, out gmsCharge);
            Decimal.TryParse(accessBox.Text, out totalAccessorials);

            actualGross = (actualWeight / 100) * CWT;
            actualDiscountPercentage = twelveKDiscount / twelveKGross;
            percentForDisplay = actualDiscountPercentage * 100;
            actualDiscountAmount = actualGross * actualDiscountPercentage;
            netTotal = actualGross - actualDiscountAmount;
            fuelTotal = netTotal * fuelPercent;
            netTotal += fuelTotal + totalAccessorials;
 

            string actGrossString = "$" + actualGross.ToString("F");
            string actDiscAmountString = "$" + actualDiscountAmount.ToString("F");
            string fuelTotalString = "$" + fuelTotal.ToString("F"); ;
            string netTotalString = "$" + netTotal.ToString("F");
            string twelveKGrossString = "$" + twelveKGross.ToString("F");
            string twelveKDiscountString = "$" + twelveKDiscount.ToString("F");
            string CWTString = "$" + CWTBox.Text;
            string fscString = fuelBox.Text + "%";
            string percDisplayString = percentForDisplay + "%";
            
            actGrossResultLabel.Text = actGrossString;
            actDiscPercLabel.Text = percDisplayString;
            actDiscAmtLabel.Text = actDiscAmountString;
            fuelSurchResultLabel.Text = fuelTotalString;
            netChargeLabel.Text = netTotalString;
            //totalChargeResultLabel.Text = totalChargeString; 
            string noteString = createSpecPricingNotes(actGrossString, actDiscAmountString, fscString, fuelTotalString,
                netTotalString, twelveKGrossString, twelveKDiscountString, CWTString,totalAccessorials);
            copyNotes.Text = noteString;
            Clipboard.SetText(noteString);
        }

        private string createSpecPricingNotes(string actGross,string actDisc, string fuelPerc,string fuelTotal,string actNet
          ,string twelveKGross,string twelveKDisc,string CWT,decimal totalAccess)
        {
            string newNote = "";
            newNote += "11,999LB GROSS: "+ twelveKGross +" DISC: " + twelveKDisc + "\r\n";
            newNote += "11,999 CWT: "+ CWT+ " FSC %: " + fuelPerc + "\r\n";
            newNote += actualWgtBox.Text + "LB GROSS: " + actGross + " DISC: " + actDisc + "\r\n";
            if (totalAccess > 0)
            {
                string totalAccessorialsString = "$" + totalAccess.ToString("F");
                newNote += "Total accessorials: " + totalAccessorialsString + "\r\n";
            }
            newNote += actualWgtBox.Text +"LB NET:" + actNet;
            

            return newNote;
        }
        private void calculateTotalButton_Click(object sender, EventArgs e)
        {
                updateSpecialPricingLabels();
        }

        private void clearFieldsButton_Click(object sender, EventArgs e)
        {
            tenKGrossBox.Text = "";
            CWTBox.Text = "";
            tenKDiscBox.Text = "";

            actualWgtBox.Text = "";
            fuelBox.Text = "";

            
            accessBox.Text = "";
            actGrossResultLabel.Text = "";
            actDiscPercLabel.Text = "";
            actDiscAmtLabel.Text = "";
            fuelSurchResultLabel.Text = "";
            netChargeLabel.Text = "";
        }
    }
}
