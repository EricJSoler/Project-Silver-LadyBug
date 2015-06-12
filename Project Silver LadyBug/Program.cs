using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.IO;
//Stream reader stream writer

namespace Project_Silver_LadyBug
{

    /// <summary>
    /// The flow of this program will be recieve a degree, quarter of enrollment and
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {

            
           // StreamReader inputReader = new StreamReader(@"C:\Users\ejsoler\Desktop\Project-Silver-LadyBug\Project Silver LadyBug\LocationManager.config");
            String line;
            //while((line = inputReader.ReadLine()) != null)
            //{
             //   String[] stuff = line.Split(' ');
            //}

            try
            {
                List<String> quarter = new List<String>();
                quarter.Add("Fall");
                quarter.Add("Winter");
                quarter.Add("Spring");
                SQLHANDLER.start();
                Output final = new Output();
                //Console.WriteLine("Enter File Path To XML input");
                //string filePath = Console.ReadLine();
                PreReq pre = new PreReq();
                //PreReq pre = new PreReq(filePath);
                List<List<Match>> all = new List<List<Match>>();
                List<Course> qual;
                string x;
                int i = 0;
                int year = 0;
                do
                {
                    if (i % 3 == 0)
                        year++;
                    TimeFilter time = new TimeFilter();
                    qual = pre.getQualifiedCourses();

                    List<Match> recomended = time.buildMyScheduleFor(qual, quarter[i % 3], 3);
                    all.Add(recomended);
                    pre.updateCompleted(recomended);
                    final.addToOutput(quarter[i % 3], recomended, year);
                    i++;
                    if (i > 15)
                        break;
                } while (!(pre.amIDone()));
                final.finalizeOutput();
                Console.WriteLine("sdf");

            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                System.Environment.Exit(1);
            }
        }
        
    }
}

//TODO:: Update the time filter to let any on-line class be recommended