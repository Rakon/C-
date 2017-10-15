﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Romijnsconverter
{
    public partial class Form1 : Form
    {
        int[] index = { 1, 4, 5, 9, 10, 40, 50, 90, 100, 400, 500, 900, 1000 };
        string[] rIndex = { "I", "IV", "V", "IX", "X", "XL", "L", "XC", "C", "CD", "D", "CM", "M" };
        int indexLimit = 12;
        string output = "";
        public Form1()
        {
            InitializeComponent();
            
        }
        private void lblresult_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
               
        private void btnC_Click(object sender, EventArgs e)
        {
            int num = 0;
            output = "";
            num = Convert.ToInt32(textBox1.Text);
            while (num > 0)
            {
                num = find(num);

            }
            lblresult.Text = output;
        }
        public int find(int Num)
        {
            int i = 0;
            
            while (index[i] <= Num)
            //System.IndexOutOfRangeException: 'De index ligt buiten de matrixgrenzen.'
            {
                i += 1;
            }
            
            if (i != 0)
            {
                indexLimit = i - 1;
            }
            else
            {
                indexLimit = 0;
            }
            output = output + rIndex[indexLimit];
            
            Num = Num - index[indexLimit];
            
            return Num;
            
        }
    }
}
