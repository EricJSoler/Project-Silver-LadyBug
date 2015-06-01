using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Silver_LadyBug
{
    /// <summary>
    /// Match simply holds the department id and number id for a course this is used by the
    /// time filter to select an appropriate schedule with no conflicts will be passed to 
    /// "Output cleaner upper" to get cleaned up into something pretty we want to output
    /// </summary>
    public class Match
    {
        public Match()
        {
            sectionOptions = new List<Section>();
        }

        public Match(string dep, string num)
        {
            departmentID = dep;
            numberID = num;
            sectionOptions = new List<Section>();
        }
        public Match(Match a)
        {
            sectionOptions = new List<Section>();
            departmentID = a.departmentID;
            numberID = a.numberID;
            importance = a.importance;

            
        }

     public string departmentID;
     public string numberID;
     public List<Section> sectionOptions;
     public int importance;
       
    }
}
