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
    /// Output class recieves courses that were recomended form the time filter and puts them
    /// into a structure containing the information needed to output them into xml form
    /// </summary>
    public class Output
    {
        public Output()
        {
            schedules = new List<TermOutput>();
        }


        public void addToOutput(string quarter, List<Match> recieved, int year)
        {
            TermOutput thisTerm = new TermOutput(quarter,recieved,year);
            schedules.Add(thisTerm);
        }

        //Serialize stuff
        public void finalizeOutput()
        {
            string fileName = "ClassOrder.xml";
            using (var stream = new FileStream(fileName, FileMode.Create)) {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var XML = new XmlSerializer(typeof(List<TermOutput>));
                XML.Serialize(stream, schedules, ns);
            }
        }

        private List<TermOutput> schedules;

        //public Degree degree;
        //public Schedule schedule;
        //public TransferSchool transfer;
        //public Student student;
    }

    public class TermOutput
    {
        public TermOutput()
        { }
        public TermOutput(string term, List<Match> recieved, int y)
        {
            schedule = recieved;
            termID = term;
            year = y;
        }


        public string termID;
        public List<Match> schedule;
        public int year;

    }

    //Schedule includes a list of Courses
    //public class Schedule
    //{
    //    const int MEMBERS = 5;
    //    private List<outputCourse> schedule = new List<outputCourse> { };
        

    //    //Main input function. Accepts array of Class Specs, in order: Year, Quarter, Department, Course ID, Section.
    //    public void setschedule(List<string> given)
    //    {
    //        int index = 0;
    //        int lastcourse = -1;

    //        foreach(string element in given)
    //        {
    //            int member = index % MEMBERS;
    //            int course = index / MEMBERS;//Updates to next course every MEMBERSth iteration

    //            //if (course != lastcourse)
    //            //    schedule.Add(new outputCourse());

    //            switch(member)
    //            {
    //                case 0:
    //                    schedule[course].setyear(element);
    //                    break;
    //                case 1:
    //                    schedule[course].setquarter(element);
    //                    break;
    //                case 2:
    //                    schedule[course].setdepartment(element);
    //                    break;
    //                case 3:
    //                    schedule[course].setcourseID(element);
    //                    break;
    //                case 4:
    //                    schedule[course].setsection(element);
    //                    break;
    //            }

    //            lastcourse = course;
    //            index++;
    //        }
    //    }
    //}

    ////public class Quarter
    ////{
    ////    public string qtr;
    ////    public string year;
    ////    public List<Courses> courses = new List<Courses> { };
    ////}

    ////Courses is a list of courses for a single Quarter
    ////public class Courses
    ////{
    ////    public List<outputCourse> list = new List<outputCourse> { };
    ////}

    ////outputCourse outputCourse represents single courses
    //public class outputCourse
    //{
    //    public void setyear(string yr) { year = yr; }
    //    public void setquarter(string qtr) { quarter = qtr; }
    //    public void setdepartment(string dept) { department = dept; }
    //    public void setcourseID(string id) { courseID = id; }
    //    public void setsection(string sect) { section = sect; }

    //    private string year;
    //    private string quarter;
    //    private string department;
    //    private string courseID;
    //    private string section;//char?
    //}

    //public class Degree
    //{
    //    private string degree;

    //    public void setdegree(string deg) { degree = deg; }
    //}

    //public class TransferSchool
    //{
    //    private string school;

    //    public void setschool(string sch) { school = sch; }
    //}

    //public class Student
    //{
    //    private string name;
    //    private string studentID;

    //    public void setstudentID(string id) { studentID = id; }
    //    public void setname(string n) { name = n; }
    //}
}