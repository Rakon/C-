using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        Double result = 0;
        String WhatOperation = "";
        bool isOperationPreformed = false;
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button_click(object sender, EventArgs e)
        {
            if ((txtbresult.Text == "0" || isOperationPreformed))
                txtbresult.Clear();
            isOperationPreformed = false;
            Button button = (Button)sender;
            if(button.Text == ".")
            {
                if (!txtbresult.Text.Contains("."))
                    txtbresult.Text = txtbresult.Text + button.Text;
            }else
            txtbresult.Text = txtbresult.Text + button.Text;
        }

        private void operator_click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (result != 0)
            {
                btnEqual.PerformClick();
                WhatOperation = button.Text;
                lblSubResult.Text = result + " " + WhatOperation;
                isOperationPreformed = true;
            }
            else
            {
                WhatOperation = button.Text;
                result = Double.Parse(txtbresult.Text);
                lblSubResult.Text = result + " " + WhatOperation;
                isOperationPreformed = true;
            }
        }


        private void btnClearEntry_Click(object sender, EventArgs e)
        {
            txtbresult.Text = "0";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtbresult.Text = "0";
            result = 0;
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            switch(WhatOperation)
            {
                case "+":
                    txtbresult.Text = (result + Double.Parse(txtbresult.Text)).ToString();
                    break;
                case "-":
                    txtbresult.Text = (result - Double.Parse(txtbresult.Text)).ToString();
                    break;
                case "*":
                    txtbresult.Text = (result * Double.Parse(txtbresult.Text)).ToString();
                    break;
                case "/":
                    txtbresult.Text = (result / Double.Parse(txtbresult.Text)).ToString();
                    break;
                default:
                    break;
                
            }
            result = double.Parse(txtbresult.Text);
            lblSubResult.Text = "";
        }
        /*
private void btn0_Click(object sender, EventArgs e)
{
txtbresult.Text = txtbresult.Text + "0";
}

private void btn1_Click(object sender, EventArgs e)
{
txtbresult.Text = txtbresult.Text + "1";
}

private void btn2_Click(object sender, EventArgs e)
{
txtbresult.Text = txtbresult.Text + "2";
}
private void btn3_Click(object sender, EventArgs e)
{
txtbresult.Text = txtbresult.Text + "3";
}
private void btn4_Click(object sender, EventArgs e)
{
txtbresult.Text = txtbresult.Text + "4";
}
private void btn5_Click(object sender, EventArgs e)
{
txtbresult.Text = txtbresult.Text + "5";
}
private void btn6_Click(object sender, EventArgs e)
{
txtbresult.Text = txtbresult.Text + "6";
}
private void btn7_Click(object sender, EventArgs e)
{
txtbresult.Text = txtbresult.Text + "7";
}
private void btn8_Click(object sender, EventArgs e)
{
txtbresult.Text = txtbresult.Text + "8";
}
private void btn9_Click(object sender, EventArgs e)
{
txtbresult.Text = txtbresult.Text + "9";
}

private void button_click(object sender, EventArgs e)
{

}
*/
    }
}
