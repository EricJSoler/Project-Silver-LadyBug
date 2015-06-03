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
    /// This is the module currently being worked on and needs to build an object made up of 
    /// courses that a student is qualified for
    /// </summary>
    public class PreReq
    {

        public PreReq()
        {
            program = "AAS-Pre-Engineering-MCAIMS";
            subjectRequirements = new Dictionary<string, Subject>();
            courseGraph = new Graph();
            completed = new List<Match>();
            info = new InputInfo(@"C:\Users\ejsoler\Desktop\InputConfig.xml");
            Dictionary<String, int> coursesPlacedInto = new Dictionary<String, int>();//TODO: recieve this from taylors input thing
            courseGraph.coursesPlacedInto = coursesPlacedInto = info.placement;
            findDepartments();
        }
        public PreReq(string filePath)
        {
            program = "AAS-Pre-Engineering-MCAIMS";
            subjectRequirements = new Dictionary<string, Subject>();
            courseGraph = new Graph();
            completed = new List<Match>();

            info = new InputInfo(@filePath);
            Dictionary<String, int> coursesPlacedInto = new Dictionary<String, int>();//TODO: recieve this from taylors input thing
            courseGraph.coursesPlacedInto = coursesPlacedInto = info.placement;
            findDepartments();
        }

        public void updateCompleted(List<Match> completed)
        {
            List<Course> toBeUpdated = new List<Course>();
            foreach (Match element in completed)
            {
                Subject checkIfMadeProgress;
                if (subjectRequirements.TryGetValue(element.departmentID, out checkIfMadeProgress)) {
                    if (checkIfMadeProgress.reqCourses.ContainsKey(element.departmentID+element.numberID))
                        requiredCourseCount -= 1;
                }
                
                toBeUpdated.Add(new Course(element.departmentID, element.numberID));
                courseGraph.updateCompleted(toBeUpdated);
            }
        }

        private void findDepartments()
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "dbo.ReadRequiredCourses_FromGeneralDegree_By_ProgramID";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ProgramID", program));
            cmd.Connection = SQLHANDLER.myConnection2;

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string readValue = reader.GetValue(0).ToString();
                Subject temp;
                if(!(subjectRequirements.TryGetValue(readValue,out temp)))
                {
                    subjectRequirements.Add(readValue, new Subject(readValue));
                    requiredCourseCount += 1;
                    subjectRequirements[readValue].addCourse(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                }
                else
                {
                    requiredCourseCount += 1;
                    temp.addCourse(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                }
            }

            reader.Close();

            foreach (Subject element in subjectRequirements.Values) {
                foreach (Course ele in element.reqCourses.Values) {
                    courseGraph.frontLoad(ele.departmentID, ele.numberID);
                }
            }

            foreach (Subject element in subjectRequirements.Values) {
                foreach (Course ele in element.reqCourses.Values) {
                    courseGraph.insertCourse(ele.departmentID, ele.numberID);
                }
            }

            //courseGraph.insertCourse("MATH", "151");
            //courseGraph.insertCourse("MATH", "142");
            //courseGraph.insertCourse("CHEM", "161");

            courseGraph.fixDepths();

            //courseGraph.insertCourse("MATH", "144");
            //test values
            //courseGraph.insertCourse("MATH", "163");
            //courseGraph.insertCourse("PHYS", "243");
            //courseGraph.insertCourse("CS", "132");
            //courseGraph.insertCourse("CS", "131");
            Console.WriteLine("Graph has been built");
        }

        public List<Course> getQualifiedCourses()
        {
            List<Course> qual = new List<Course>();
            qual = courseGraph.findQualifiedCourses();
            foreach(Course element in qual)
            {
                element.importance = getImportanceRating(element);
            }
            return qual;
        }



        private int getImportanceRating(Course passed)
        {
            int importance = 0;
            Subject temp;
            subjectRequirements.TryGetValue(passed.departmentID, out temp);
            Course tempC;
            if (temp != null)
            {
                if (temp.reqCourses.TryGetValue(passed.departmentID + passed.numberID, out tempC))
                    importance += 10;
            }
            importance += courseGraph.occurenceCount(passed.departmentID, passed.numberID);
            switch (passed.departmentID)
            {
                case "MATH":
                    importance += 55;
                    break;
                case "PHYS":
                    importance += 20;
                    break;
            }
            return importance;
        }


        public bool amIDone()
        {
            if (requiredCourseCount == 0)
                return true;
            else
                return false;
        }


        public static string program;
        /// <summary>
        /// Dictionary<string,Subject> stores the  major related subjects inside each subject is
        /// a Course list containing the  Courses required for each department
        /// </summary>
        public Dictionary<string, Subject> subjectRequirements; 
        public Graph courseGraph;
        private List<Match> completed;
        private int requiredCourseCount;
        public static InputInfo info;
    }
}
