using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Form1 : Form
    {
        internal enum Grids
        {
            Std,
            Enr,
            Crs,
            Prog
        }

        internal static Form1 current;

        private Grids grid;


        private bool OKToChange = true;

        public Form1()
        {
            current = this;
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e){ 


            new Form2();
            Form2.current.Visible = false;
            
            dataGridView1.Dock = DockStyle.Fill;
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange)
            {
                //STUDENTS - USING BIDING SOURCE
                grid = Grids.Std;
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource1.DataSource = Data.Students.GetStudents();
                bindingSource1.Sort = "StId";
                dataGridView1.DataSource = bindingSource1;

                dataGridView1.Columns["StId"].HeaderText = "Student ID";
                dataGridView1.Columns["StId"].DisplayIndex = 0;
                dataGridView1.Columns["StName"].DisplayIndex = 1;
                dataGridView1.Columns["ProgId"].DisplayIndex = 2;

            }


        }

        private void enrollmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange && (grid != Grids.Enr))
            {
                //COURSES - USING BIDING SOURCE 2
                grid = Grids.Enr;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource2.DataSource = Data.Enrollments.GetDisplayEnrollments();
                bindingSource2.Sort = "StId, CId";
                dataGridView1.DataSource = bindingSource2;

                dataGridView1.Columns["StId"].HeaderText = "Student ID";
                dataGridView1.Columns["FinalGrade"].HeaderText = "Final Grade";
                dataGridView1.Columns["StId"].DisplayIndex = 0;
                dataGridView1.Columns["StName"].DisplayIndex = 1;
                dataGridView1.Columns["CId"].DisplayIndex = 2;
                dataGridView1.Columns["CName"].DisplayIndex = 3;
                dataGridView1.Columns["FinalGrade"].DisplayIndex = 4;
                dataGridView1.Columns["ProgId"].DisplayIndex = 5;
                dataGridView1.Columns["ProgName"].DisplayIndex = 6;

            }

        }
        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //COURSES - USING BIDING SOURCE 3
            grid = Grids.Crs;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource3.DataSource = Data.Courses.GetCourses();
            bindingSource3.Sort = "CId";
            dataGridView1.DataSource = bindingSource3;

            dataGridView1.Columns["CId"].HeaderText = "Course ID";
            dataGridView1.Columns["CId"].DisplayIndex = 0;
            dataGridView1.Columns["CName"].DisplayIndex = 1;
            dataGridView1.Columns["ProgId"].DisplayIndex = 2;

        }

        private void programsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //PROGRAMS - USING BIDING SOURCE 4
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource4.DataSource = Data.Programs.GetPrograms();
            bindingSource4.Sort = "ProgId";
            dataGridView1.DataSource = bindingSource4;

            dataGridView1.Columns["ProgId"].HeaderText = "Programs ID";
            dataGridView1.Columns["ProgId"].DisplayIndex = 0;
            dataGridView1.Columns["ProgName"].DisplayIndex = 1;

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            if (Business.Students.UpdateStudents() == -1)
            {
                
                bindingSource1.ResetBindings(false);
            }

        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {
            if (Business.Enrollments.UpdateEnrollments() == -1)
            {

               bindingSource1.ResetBindings(false);
            }

        }


        private void bindingSource3_CurrentChanged(object sender, EventArgs e)
        {
            if (Business.Courses.UpdateCourses() == -1)
            {

                bindingSource3.ResetBindings(false);
            }

        }

        private void bindingSource4_CurrentChanged(object sender, EventArgs e)
        {
            if (Business.Programs.UpdatePrograms() == -1)
            {

                bindingSource4.ResetBindings(false);
            }


        }

        private void menuStrip1_Click(object sender, EventArgs e)
        {
            // =========================================================================
            // If the insertion / updated is ended by just changing to the other table 
            // (clicking on the menu strip) without clicking on datagrid, we need 
            // this code to ensure the database is updated. 
            // =========================================================================
            OKToChange = true;

            BindingSource temp = (BindingSource)dataGridView1.DataSource;

            Validate();

            if (temp == bindingSource1)
            {
                if (Business.Students.UpdateStudents() == -1)
                {
                    OKToChange = false;
                }
            }
            else if (temp == bindingSource2)
            {
                if (Business.Enrollments.UpdateEnrollments() == -1)
                {
                    OKToChange = false;
                }
            }
            else if (temp == bindingSource3)
            {
                if (Business.Courses.UpdateCourses() == -1)
                {
                    OKToChange = false;
                }
            }
            else if (temp == bindingSource4)
            {
                if (Business.Programs.UpdatePrograms() == -1)
                {
                    OKToChange = false;
                }
            }
        }

        internal static void BLLMessage(string s)
        {
            MessageBox.Show("Business Layer: "+s);
        }

        internal static void DALMessage(string s)
        {
            MessageBox.Show("Data Layer: " + s);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2.current.Start(Form2.Modes.Add, null);

        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("Please select a row to modify");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Please select only one row to modify");
            }
            else
            {
                if (string.IsNullOrEmpty("" + c[0].Cells["FinalGrade"].Value))
                {
                    Form2.current.Start(Form2.Modes.Modify, c);
                }
                else
                {
                    MessageBox.Show("Cannot modify. To update this row, Final Grade must be removed first");
                }
                
            }

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("Please select a row to delete");
            }
            else
            {

                if (string.IsNullOrEmpty("" + c[0].Cells["FinalGrade"].Value) == true)
                {
                    List<string[]> lId = new List<string[]>();
                    foreach (DataGridViewRow r in c)
                    {
                        lId.Add(new string[] { "" + r.Cells["StId"].Value, "" + r.Cells["CId"].Value });
                    }
                    Data.Enrollments.DeleteData(lId);

                }
                else
                {
                    MessageBox.Show("Cannot delete. To delete this row, Final Grade must be removed first");
                }
                    
                
            }
            
        }

        private void manageFinalGradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("Select a row to manage final grade");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected to manage final grade");
            }
            else
            {
                Form3 current = new Form3();
                current.Start(Form3.Modes.FinalGrade, c); //Create the form3
            }

        }

      
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Impossible to insert / update / delete");
            e.Cancel = false;
            OKToChange = false;
        }


        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
           //NOT USED 
        }

    }
}
