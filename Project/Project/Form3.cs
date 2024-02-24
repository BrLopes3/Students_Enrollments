using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Form3 : Form
    {
        //no necessity of enumeration
        internal enum Modes
        {
            FinalGrade
        }

        internal static Form3 current;

        private Modes mode = Modes.FinalGrade;

        private string[] gradeInitial;

        public Form3()
        {
            current = this;
            InitializeComponent();
        }

        internal void Start(Modes FinalGrade, DataGridViewSelectedRowCollection c)
        {
            Text = "" + FinalGrade;
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = false;

            if (c != null)
            {
                textBox1.Text = "" + c[0].Cells["StId"].Value;
                textBox2.Text = "" + c[0].Cells["StName"].Value;
                textBox3.Text = "" + c[0].Cells["CId"].Value;
                textBox4.Text = "" + c[0].Cells["CName"].Value;
                textBox5.Text = "" + c[0].Cells["FinalGrade"].Value;
                gradeInitial = new string[] { (string)c[0].Cells["StId"].Value, (string)c[0].Cells["CId"].Value };
            }

            ShowDialog();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int r = -1;
            if (mode == Modes.FinalGrade)
            {
                
                if (textBox5.Text == "")
                {
                    r = Business.Enrollments.UpdateFinalGrade(gradeInitial, textBox5.Text);
                }
                else
                {
                    r = Business.Enrollments.UpdateFinalGrade(gradeInitial, textBox5.Text);
                }

                
            }

            if (r == 0)
            {
                Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
        }

       
    }
}
