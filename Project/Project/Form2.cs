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
    public partial class Form2 : Form
    {
        internal enum Modes
        {
            Add,
            Modify,
            Delete
        }

        internal static Form2 current;

        private Modes mode = Modes.Add;

        private string[] enrollmentInitial;

        public Form2()
        {
            current = this;
            InitializeComponent();
        }

        internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        {
            //FORM USED TO ADD AND MODIFY
            mode = m;
            Text = "" + mode;

            comboBox1.DisplayMember = "StId";
            comboBox1.ValueMember = "StId";
            comboBox1.DataSource = Data.Students.GetStudents();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;

            comboBox2.DisplayMember = "CId";
            comboBox2.ValueMember = "CId";
            comboBox2.DataSource = Data.Courses.GetCourses();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedIndex = 0;

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;

            if ((mode == Modes.Modify) && (c != null)){

                comboBox1.SelectedValue = c[0].Cells["StId"].Value;
                comboBox2.SelectedValue = c[0].Cells["CId"].Value;

                enrollmentInitial = new string[] { (string)c[0].Cells["StId"].Value, (string)c[0].Cells["CId"].Value };
            }

            if (mode == Modes.Modify)
            {
                comboBox1.Enabled = false; //if not work change to readonly
                textBox1.ReadOnly = true;  //if not work change Enable = false
                textBox2.ReadOnly = true;
            }

            ShowDialog();

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                var a = from r in Data.Students.GetStudents().AsEnumerable()
                        where r.Field<string>("StId") == (string)comboBox1.SelectedValue
                        select new { Name = r.Field<string>("StName") };

                    textBox1.Text = a.Single().Name;

            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                var a = from r in Data.Courses.GetCourses().AsEnumerable()
                        where r.Field<string>("CId") == (string)comboBox2.SelectedValue
                        select new { Name = r.Field<string>("CName") };

                    textBox2.Text = a.Single().Name;

                
            }
               
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int r = -1;
            if (mode == Modes.Add)
            {
                r = Data.Enrollments.InsertData(new string[] {(string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });
            }
            if (mode == Modes.Modify)
            {
                List<string[]> lId = new List<string[]>();
                lId.Add(enrollmentInitial);
                r = Data.Enrollments.InsertData(new string[] { (string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });

                if (r == 0)
                {
                    r = Data.Enrollments.DeleteData(lId);
                }
            }
            if (r == 0) { Close(); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
