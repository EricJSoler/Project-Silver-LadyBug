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
    /// The flow of this program will be recieve a degree, quarter of enrollment and
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {

            List<String> quarter = new List<String>();
            quarter.Add("Fall");
            quarter.Add("Winter");
            quarter.Add("Spring");
            SQLHANDLER.start();
            PreReq pre = new PreReq();
            //Graph graph = new Graph();
            List<List<Match>> all = new List<List<Match>>();
            List<Course> qual;   
            string x;
            int i = 0;
            do {
                TimeFilter time = new TimeFilter();
                qual = pre.getQualifiedCourses();
                //string message = "QualifiedFor: in qtr " + i.ToString();
                //Console.WriteLine(message);
                //foreach(Course element in qual)
                //{

                //    Console.Write(element.departmentID);
                //    Console.Write(" ");
                //    Console.WriteLine(element.numberID);
                //}
                List<Match> recomended = time.buildMyScheduleFor(qual, quarter[i%3], 3);
                all.Add(recomended);
                pre.updateCompleted(recomended);

                string message1 = "Recommended: " + i.ToString();
                Console.WriteLine(message1);
                foreach(Match element in recomended)
                {

                    Console.Write(element.departmentID);
                    Console.Write(" ");
                    Console.WriteLine(element.numberID);
                }
                if(i==7)
                Console.WriteLine("Press 'q' to quit");
               // x = Console.ReadLine();
                i++;
            } while (i<10);
            
            Console.WriteLine("sdf");

        }

        
    }
}

//TODO:: Update the time filter to let any on-line class be recommended