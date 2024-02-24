using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Data
{
    internal class Connect
    {
        private static String cliComConnectionString = GetConnectString();

        internal static String ConnectionString
        {
            get => cliComConnectionString;
        }

        private static String GetConnectString()
        {

            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = "(local)";
            cs.InitialCatalog = "Project";
            cs.UserID = "sa";
            cs.Password = "sysadm";

            return cs.ConnectionString;
        }

    }//end class connect

    internal class DataTables
    {
        private static SqlDataAdapter adapterPrograms = InitAdapterPrograms();
        private static SqlDataAdapter adapterStudents = InitAdapterStudents();
        private static SqlDataAdapter adapterCourses = InitAdapterCourses();
        private static SqlDataAdapter adapterEnrollments = InitAdapterEnrollments();
        private static SqlDataAdapter adapterDisplayEnrollments = InitAdapterDisplayEnrollments();

        private static DataSet ds = InitDataSet();

        private static SqlDataAdapter InitAdapterStudents()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Students ORDER BY StId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            //For the ON UPDATE CASCADE relative to ProgId
            builder.ConflictOption = ConflictOption.OverwriteChanges;

            r.UpdateCommand = builder.GetUpdateCommand();

            return r;

        }

        private static SqlDataAdapter InitAdapterEnrollments()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Enrollments ORDER BY StId, CId",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            //For the ON UPDATE CASCADE relative to ProgId
            builder.ConflictOption = ConflictOption.OverwriteChanges;

            r.UpdateCommand = builder.GetUpdateCommand();

            return r;

        }



        private static SqlDataAdapter InitAdapterCourses()
        {

            SqlDataAdapter r = new SqlDataAdapter(
                               "SELECT * FROM Courses ORDER BY CId ",
                                              Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            //For the ON UPDATE CASCADE relative to ProgId
            builder.ConflictOption = ConflictOption.OverwriteChanges;

            r.UpdateCommand = builder.GetUpdateCommand();

            return r;

        }

        private static SqlDataAdapter InitAdapterPrograms()
        {

            SqlDataAdapter r = new SqlDataAdapter(
                 "SELECT * FROM Programs ORDER BY ProgId ",
                 Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            //For the ON UPDATE CASCADE relative to ProgId
            builder.ConflictOption = ConflictOption.OverwriteChanges;

            r.UpdateCommand = builder.GetUpdateCommand();

            return r;

        }

        private static SqlDataAdapter InitAdapterDisplayEnrollments()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT s.StId, s.StName, c.CId, c.CName, e.FinalGrade, p.ProgId, p.ProgName " +
                "FROM Students s, Courses c, Enrollments e, Programs p " +
                "WHERE s.StId = e.StId AND c.CId = e.CId AND p.ProgId = s.ProgId AND p.ProgId = c.ProgId " +
                "ORDER BY StId, CId ",
                Connect.ConnectionString);


            return r;

        }

        private static DataSet InitDataSet()
        {
            DataSet ds = new DataSet();
            loadPrograms(ds);
            loadStudents(ds);
            loadCourses(ds);
            loadEnrollments(ds);
            loadDisplayEnrollments(ds);
            return ds;
        }

        private static void loadStudents(DataSet ds)
        {
            adapterStudents.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            adapterStudents.Fill(ds, "Students");
        }

        private static void loadCourses(DataSet ds)
        {
            adapterCourses.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            adapterCourses.Fill(ds, "Courses");
        }

        private static void loadPrograms(DataSet ds)
        {
            adapterPrograms.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            adapterPrograms.Fill(ds, "Programs");

        }

        private static void loadEnrollments(DataSet ds)
        {
            adapterEnrollments.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            adapterEnrollments.Fill(ds, "Enrollments");

            //ADD THE CONSTRAINTS OF ALL FOREGIN KEYS

            //FK StId in table Enrollments
            ForeignKeyConstraint FKStId = new ForeignKeyConstraint("FKStId",
                 new DataColumn[] { ds.Tables["Students"].Columns["StId"] },
                 new DataColumn[] { ds.Tables["Enrollments"].Columns["StId"] });

            FKStId.DeleteRule = Rule.Cascade;
            FKStId.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollments"].Constraints.Add(FKStId);

            //FK CId in table Enrollments
            ForeignKeyConstraint FKCId = new ForeignKeyConstraint("FKCId",
                 new DataColumn[] { ds.Tables["Courses"].Columns["CId"] },
                 new DataColumn[] { ds.Tables["Enrollments"].Columns["CId"] });

            FKCId.DeleteRule = Rule.None;
            FKCId.UpdateRule = Rule.None;
            ds.Tables["Enrollments"].Constraints.Add(FKCId);


            //FK ProgId in table Courses
            ForeignKeyConstraint FKProgId = new ForeignKeyConstraint("FKProgId",
                 new DataColumn[] { ds.Tables["Programs"].Columns["ProgId"] },
                 new DataColumn[] { ds.Tables["Courses"].Columns["ProgId"] });

            FKProgId.DeleteRule = Rule.Cascade;
            FKProgId.UpdateRule = Rule.Cascade;
            ds.Tables["Courses"].Constraints.Add(FKProgId);

            //FK ProgId in table Students
            ForeignKeyConstraint FKProgId2 = new ForeignKeyConstraint("FKProgId2",
                 new DataColumn[] { ds.Tables["Programs"].Columns["ProgId"] },
                 new DataColumn[] { ds.Tables["Students"].Columns["ProgId"] });

            FKProgId2.DeleteRule = Rule.None;
            FKProgId2.UpdateRule = Rule.Cascade;
            ds.Tables["Students"].Constraints.Add(FKProgId2);
        }

        private static void loadDisplayEnrollments(DataSet ds)
        {
            adapterDisplayEnrollments.Fill(ds, "DisplayEnrollments");

            ForeignKeyConstraint myFK01 = new ForeignKeyConstraint("myFK01",
                new DataColumn[] {
                    ds.Tables["Enrollments"].Columns["StId"],
                    ds.Tables["Enrollments"].Columns["CId"]
                },
                new DataColumn[]
                {
                    ds.Tables["DisplayEnrollments"].Columns["StId"],
                    ds.Tables["DisplayEnrollments"].Columns["CId"]
                });

            myFK01.DeleteRule = Rule.Cascade;
            myFK01.UpdateRule = Rule.Cascade;
            ds.Tables["DisplayEnrollments"].Constraints.Add(myFK01);

        }

        internal static SqlDataAdapter getAdapterStudents()
        {
            return adapterStudents;
        }

        internal static SqlDataAdapter getAdapterCourses()
        {
            return adapterCourses;
        }

        internal static SqlDataAdapter getAdapterPrograms()
        {
            return adapterPrograms;
        }

        internal static SqlDataAdapter getAdapterEnrollments()
        {
            return adapterEnrollments;
        }

        internal static SqlDataAdapter getAdapterDisplayEnrollments()
        {
            return adapterDisplayEnrollments;
        }

        internal static DataSet getDataSet()
        {
            return ds;
        }


    }//end class DataTables

    //classes for the Students, Courses, Programs and Enrollments tables
    internal class Students
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterStudents();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetStudents()
        {
            return ds.Tables["Students"];
        }

        internal static int UpdateStudents()
        {
            if (!ds.Tables["Students"].HasErrors)
                return adapter.Update(ds.Tables["Students"]);
            else
                return -1;
        }

    }//end of class Students

    internal class Courses
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterCourses();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetCourses()
        {
            return ds.Tables["Courses"];
        }

        internal static int UpdateCourses()
        {
            if (!ds.Tables["Courses"].HasErrors)
                return adapter.Update(ds.Tables["Courses"]);
            else
                return -1;
        }

    }//end of class Courses

    internal class Programs
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterPrograms();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetPrograms()
        {
            return ds.Tables["Programs"];
        }

        internal static int UpdatePrograms()
        {
            if (!ds.Tables["Programs"].HasErrors)
                return adapter.Update(ds.Tables["Programs"]);
            else
                return -1;
        }

    }//end of class Programs

    internal class Enrollments
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterEnrollments();
        private static DataSet ds = DataTables.getDataSet();

        private static DataTable displayEnrollments = null;

        internal static DataTable GetDisplayEnrollments()
        {
            displayEnrollments = ds.Tables["DisplayEnrollments"];
            return displayEnrollments;
        }

        internal static int InsertData(string[] a)
        {
            var test = (
                from enrollment in ds.Tables["Enrollments"].AsEnumerable()
                where enrollment.Field<string>("StId") == a[0]
                where enrollment.Field<string>("CId") == a[1]
                select enrollment);

            if (test.Count() > 0)
            {
                Project.Form1.DALMessage("This enrollment already exists");
                return -1;
            }
            try
            {
                DataRow line = ds.Tables["Enrollments"].NewRow();
                line.SetField("StId", a[0]);
                line.SetField("CId", a[1]);

                //create a query to get the program id from the course id
                var queryStdProgram = (
                      from std in ds.Tables["Students"].AsEnumerable()
                      where std.Field<string>("StId") == a[0]
                      select std.Field<string>("ProgId"));
                //verify if the ProgId from the CId 
                var queryCrsProgram = (
                      from crs in ds.Tables["Courses"].AsEnumerable()
                      where crs.Field<string>("CId") == a[1]
                      select crs.Field<string>("ProgId"));

                if (queryStdProgram.First() != queryCrsProgram.First())
                {
                    Project.Form1.DALMessage("Cannot enroll the student in courses from this program.");
                }
                else
                {
                    ds.Tables["Enrollments"].Rows.Add(line);

                    adapter.Update(ds.Tables["Enrollments"]);
                    adapter.Update(ds.Tables["DisplayEnrollments"]);

                }

                if (displayEnrollments != null)
                {
                    var query = (
                          from std in ds.Tables["Students"].AsEnumerable()
                          from crs in ds.Tables["Courses"].AsEnumerable()
                          from pro in ds.Tables["Programs"].AsEnumerable()
                              //  from pro in ds.Tables["Programs"].AsEnumerable()
                          where std.Field<string>("StId") == a[0]
                          where crs.Field<string>("CId") == a[1]
                          where pro.Field<string>("ProgId") == std.Field<string>("ProgId")
                          select new
                          {
                              StId = std.Field<string>("StId"),
                              StName = std.Field<string>("StName"),
                              CId = crs.Field<string>("CId"),
                              CName = crs.Field<string>("CName"),
                              grade = line.Field<Nullable<int>>("FinalGrade"),
                              ProgId = std.Field<string>("ProgId"),
                              ProgName = pro.Field<string>("ProgName")
                              //   ProgId = std.Field<string>("ProgId"),
                              //   ProgName = pro.Field<string>("ProgName")
                          });

                    var r = query.Single();
                    displayEnrollments.Rows.Add(r.StId, r.StName, r.CId, r.CName, r.grade, r.ProgId, r.ProgName);
                   

                    GetDisplayEnrollments();
                   
                }
                Project.Form1.DALMessage("Student added successfully!");
                return 0;

            }
            catch (Exception)
            {
                Project.Form1.DALMessage("Add / Modify rejected");
                return -1;
            }

        }

        internal static int DeleteData(List<string[]> lId)
        {
            try
            {
                var lines = ds.Tables["Enrollments"].AsEnumerable().Where(s => lId.Any(x => (x[0] == s.Field<string>("StId") && x[1] == s.Field<string>("CId"))));

                foreach (var line in lines)
                {
                    line.Delete();
                }

                adapter.Update(ds.Tables["Enrollments"]);
                return 0;
            }
            catch (Exception)
            {
                Project.Form1.DALMessage("Delete rejected");
                return -1;
            }
        }

        internal static int UpdateFinalGrade(string[] a, Nullable<int> grade)
        {
            try
            {
                  var line = ds.Tables["Enrollments"].AsEnumerable().Where(s => (s.Field<string>("StId") == a[0] && s.Field<string>("CId") == a[1])).Single();

                  line.SetField("FinalGrade", grade);
                
                  adapter.Update(ds.Tables["Enrollments"]);

                if (displayEnrollments != null)
                {
                    var r = displayEnrollments.AsEnumerable().Where(s => (s.Field<string>("StId") == a[0] && s.Field<string>("CId") == a[1])).Single();

                    r.SetField("FinalGrade", grade);
                   
                    adapter.Update(ds.Tables["Enrollments"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                Project.Form1.DALMessage($"Update rejected. Exception: {ex}");
                return -1;
            }

        }




    }//end of class Enrollments
}
