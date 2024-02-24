using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Business
{
    internal class Students
    {
        internal static int UpdateStudents()
        {
            
            DataTable dt = Data.Students.GetStudents()
                .GetChanges(DataRowState.Added | DataRowState.Modified);

            if (dt != null)
            {
                //Regex to control the StId first letter S followed by 9 digits
                Regex studentIdRegex = new Regex(@"^S\d{9}$");
                foreach (DataRow row in dt.Rows)
                {
                    if (!studentIdRegex.IsMatch(row["StID"].ToString()))
                    {
                        Project.Form1.BLLMessage("Student ID must start with S followed by 9 digits");
                        Data.Students.GetStudents().RejectChanges();
                        return -1;   
                    }
                }

                return Data.Students.UpdateStudents();

            }
            else
            {
                return 0;
            }
        }

    }

    internal class Enrollments
    {

        internal static int UpdateEnrollments()
        {
            DataTable dt = Data.Enrollments.GetDisplayEnrollments()
               .GetChanges(DataRowState.Added | DataRowState.Modified);

            return 0;
        }

        internal static int UpdateFinalGrade(string[]a, string grade)
        {
            Nullable<int> finalGrade;
            int temp;

            if (grade == "")
            {
                finalGrade = null;
                
            }
            else if(int.TryParse(grade, out temp) && (0<=temp && temp<=100) )
            {
                finalGrade = temp;
                
            }
            else
            {
                Project.Form1.BLLMessage("Grade must be a number between 0 and 100");   
                return -1;
            }
            return Data.Enrollments.UpdateFinalGrade(a, finalGrade);
        }
       

    }

    internal class Courses
    {
        internal static int UpdateCourses()
        {
            DataTable dt = Data.Courses.GetCourses()
               .GetChanges(DataRowState.Added | DataRowState.Modified);
            // regex to control the course
            if (dt != null)
            {
                //Regex to control the CId first letter C followed by 6 digits
                Regex coursesIdRegex = new Regex(@"^C\d{6}$");
                foreach (DataRow row in dt.Rows)
                {
                    if (!coursesIdRegex.IsMatch(row["CID"].ToString()))
                    {
                        Project.Form1.BLLMessage("Course ID must start with C followed by 6 digits");
                        Data.Courses.GetCourses().RejectChanges();
                        return -1;
                    }
                }

                return Data.Courses.UpdateCourses();

            }
            else
            {
                return 0;
            }
        }


    }   

    internal class Programs
    {
        internal static int UpdatePrograms()
        {
            DataTable dt = Data.Programs.GetPrograms()
               .GetChanges(DataRowState.Added | DataRowState.Modified);

            //create the Regex to control the program
            if (dt != null)
            {
                //Regex to control the ProgId first letter P followed by 4 digits
                Regex programIdRegex = new Regex(@"^P\d{4}$");
                foreach (DataRow row in dt.Rows)
                {
                    if (!programIdRegex.IsMatch(row["ProgID"].ToString()))
                    {
                        Project.Form1.BLLMessage("Program ID must start with P followed by 4 digits");
                        Data.Programs.GetPrograms().RejectChanges();
                        return -1;
                    }
                }

                return Data.Programs.UpdatePrograms();

            }
            else
            {
                return 0;
            }
        }


    }
}
