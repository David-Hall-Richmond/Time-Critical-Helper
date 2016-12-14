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
            piecesTextBox.Focus();
            
        }

        //adds a pallet to the list based on the current input, needs its own method
        private void addButton_Click(object sender, EventArgs e)
        {
            int[] numBoxArray = new int[5];
            bool hasZero = false;
            TextBox[] inputBoxList = new TextBox[5]{ piecesTextBox, lengthTextBox,widthTextBox,heightTextBox,weightTextBox };
           
                for(int i = 0; i < inputBoxList.Length; i++)
                {
                    if (int.TryParse(inputBoxList[i].Text, out numBoxArray[i])) { }
                    else
                    {
                        numBoxArray[i] = 0;
                    hasZero = true;
                    }
                }

            if (hasZero)
            {
                MessageBox.Show("No Field Can Be 0");
            }
            
            else
            {
                PalletGroup newPallet = new PalletGroup(numBoxArray, stackCheckBox.Checked);
                if (newPallet.checkDims())
                {
                    newPallet.checkSwap();
                    if(newPallet.Stack && newPallet.Height > 48)
                    {
                        newPallet.Stack = false;
                        MessageBox.Show("Stackable freight cannot exceed 48\"");
                    }
                        boxListBox.Items.Add(newPallet);
                        clearEntryFields();
                        calcAllButton.Enabled = true;
                        boxListBox.Focus();
                        boxListBox.SelectedIndex=(boxListBox.Items.Count - 1);
                }
                else
                {
                    MessageBox.Show("If length exceeds 336in or weight per pallet is over 4000lbs use Exclusive Use rate. If height or width exceed 96in use flatbed");
                }
            }
           
        }


        //calculates the linear feet of the group selected
        private void calcButton_Click(object sender, EventArgs e)
        {
            PalletGroup calcOnePallet;
            int linearFeet;
            if (boxListBox.SelectedIndex >= 0)
            {
                calcOnePallet = getPalletAtIndex(boxListBox.SelectedIndex);
                linearFeet = PalletCalc.calcLinFeet(calcOnePallet,true);
                resultTextBox.Text = linearFeet.ToString();
                cubicFeetResult.Text = (linearFeet * 64).ToString();
            }
            else if (boxListBox.Items.Count == 0)
            {
                MessageBox.Show("Please add an item to the box to calculate");
            }
            else
            {
                MessageBox.Show("Please select a pallet group from the list to calculate");
            }
        }
        

        //returns a palletgroup object at the given index
        private PalletGroup getPalletAtIndex(int index)
        {
            PalletGroup currentPallet;

            currentPallet = (PalletGroup)boxListBox.Items[index];

            return currentPallet;
            
        }

        //returns the linear feet that the calc group takes up rounded up to the nearest integer
       

        private void deletePalletGroup()
        {
            boxListBox.Items.Remove(boxListBox.Items[boxListBox.SelectedIndex]);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            deletePalletGroup();
        }

        //calculates the linear feet of all groups in the listbox
        private int calcAllLinFeet()
        {
            int linFeetTotal = 0;
            int totalWeight = 0;
            for(int i = 0;i < boxListBox.Items.Count; i++)
            {
                PalletGroup j = getPalletAtIndex(i);
                linFeetTotal += PalletCalc.calcLinFeet(j,false);
                totalWeight += j.Weight;
            }
            if(linFeetTotal < totalWeight/1000)
            {
                return totalWeight / 1000;
            }
            return linFeetTotal;
        }

        private void calcAllButton_Click(object sender, EventArgs e)
        {
            int linearFeet = calcAllLinFeet();
            int cubicFeet = linearFeet * 64;

            resultTextBox.Text = linearFeet.ToString();
            cubicFeetResult.Text = cubicFeet.ToString();
        }

        private void boxListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                deletePalletGroup();
            }
        }

        private void clearEntryFields()
        {
            piecesTextBox.Text = "";
            lengthTextBox.Text = "";
            widthTextBox.Text = "";
            heightTextBox.Text = "";
            weightTextBox.Text = "";
            stackCheckBox.Checked = false;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            boxListBox.Items.Clear();
            resultTextBox.Text = "";
            clearEntryFields();
            piecesTextBox.Focus();
            cubicFeetResult.Text = "";
            calcButton.Enabled = false;
            calcAllButton.Enabled = false;
        }

        private void textBox_EnterSelectAll(object sender, EventArgs e)
        {
            TextBox clearBox = (TextBox)sender;
            clearBox.SelectAll();
        }

        private void palletGroupSelected(object sender, EventArgs e)
        {
            if (boxListBox.SelectedIndex >= 0)
            {
                calcButton.Enabled = true;
                calcFreighClassButton.Enabled = true;
            }
            else
            {
                calcButton.Enabled = false;
                calcFreighClassButton.Enabled = false;
            }
            if( boxListBox.Items.Count == 0)
            {
                calcAllButton.Enabled = false;
                calcFreighClassButton.Enabled = false;
            }
        }
        private string calcClass(PalletGroup current)
        {
            double poundsPer = PalletCalc.getPCF(current);
            if(poundsPer >= 50)
            {
                return 50.ToString();
            }
            else if(poundsPer <50 && poundsPer >= 35)
            {
                return 55.ToString();
            }
            else if(poundsPer <35 && poundsPer >= 30)
            {
                return 60.ToString();
            }
            else if(poundsPer < 30 && poundsPer >=22.5)
            {
                return 65.ToString();
            }
            else if(poundsPer < 22.5 && poundsPer >= 15)
            {
                return 70.ToString();
            }
            else if(poundsPer < 15 && poundsPer >= 13.5)
            {
                return 77.5.ToString();
            }
            else if(poundsPer < 13.5 && poundsPer >= 12)
            {
                return 85.ToString();
            }
            else if(poundsPer < 12 && poundsPer >= 10.5)
            {
                return 92.5.ToString();
            }
            else if(poundsPer < 10.5 && poundsPer >= 9)
            {
                return 100.ToString();
            }
            else if(poundsPer < 9 && poundsPer >= 8)
            {
                return 110.ToString();
            }
            else if(poundsPer < 8 && poundsPer >= 7)
            {
                return 125.ToString();
            }
            else if(poundsPer < 7 && poundsPer >= 6)
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

        private void calcFreighClassButton_Click(object sender, EventArgs e)
        {
            PalletGroup current = getPalletAtIndex(boxListBox.SelectedIndex);
            calcDensityLabel.Text = calcClass(current);

        }

        private void linearToCubicEntry(object sender, EventArgs e)
        {
            int input;
            TextBox inputBox = (TextBox)sender;
            if (int.TryParse(inputBox.Text, out input)){
                if(inputBox == linInchBox)
                {
                    input /=12;
                }
                cubeOutputLabel.Text = linearToCubicCalc(input).ToString();
            }
            else
            {
                cubeOutputLabel.Text = "";
            }

        }
        private int linearToCubicCalc(int linFeet)
        {
            return linFeet * 64;
        }

        private void generateNotesButton_Click(object sender, EventArgs e)
        {
            string volumeNotes = createNotes();
            copyNotes.Text = volumeNotes;
            
            
            Clipboard.SetText(Clipboard.GetText() +"\r\n" + volumeNotes);
        }
        private string createNotes()
        {
            bool[] ratesNeeded = new bool[] { checkGMS1.Checked, checkGMS2.Checked,
                                              checkGMS3.Checked, checkGMSX.Checked };
            string notes= "Rates by Calculator:\r\n";
            decimal accessorial = 0;
            decimal customPercent;

            if (decimal.TryParse(accessorialBox.Text, out accessorial)) { }

            for (int i = 0; i < 3; i++) {
                if (ratesNeeded[i])
                {
                    notes += "GMS" + (i + 1).ToString() + ": " + 
                        createNoteString((decimal)(.45 + (i * .2)), accessorial) + "\r\n";
                }
            }
            
            if(decimal.TryParse(gmsxBox.Text,out customPercent) && checkGMSX.Checked)
            {
                customPercent /= 100;
                notes += "GMSX: " + createNoteString(customPercent, accessorial);
            }

            return notes;
        }

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
                rate = (decimal)(total + upcharge);
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
    }
}
