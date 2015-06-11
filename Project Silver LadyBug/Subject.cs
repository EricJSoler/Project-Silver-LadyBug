using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.IO;

namespace Project_Silver_LadyBug
{
    /// <summary>
    /// The subject class allows for courses from the same department to be organized better
    /// and each subject contains a list of all the courses that are required for the degree
    /// in that department
    /// </summary>
    public class Subject
    {
        public Subject()
        {
            reqCourses = new Dictionary<string, Course>();
        }
        public Subject(string dept)
        {
            reqCourses = new Dictionary<string, Course>();
            department = dept;
           // addCourseRequirementsForDepartment();
        }

        public void addCourse(string dep, string num)
        {
            try
            {
                reqCourses.Add(dep + num, new Course(dep, num));
            }
            catch(ArgumentException e)
            { }
        }

        public void addCourseRequirementsForDepartment()
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "dbo.read_courseID_by_program_department";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@DepartmentID", department));
            cmd.Parameters.Add(new SqlParameter("@program", PreReq.program));
            cmd.Connection = SQLHANDLER.myConnection2;

            reader = cmd.ExecuteReader();

            string readValue;
            while (reader.Read())
            {
                readValue = reader.GetValue(0).ToString();
                try
                {
                    reqCourses.Add(department + readValue, new Course(department, readValue));
                }
                catch (ArgumentException e)
                {
                    //do nothing
                }
            }
            reader.Close();
        }

        public Dictionary<String, Course> reqCourses;
        public string department;
    }
}
