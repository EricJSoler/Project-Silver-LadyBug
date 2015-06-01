using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Silver_LadyBug
{
    
   
   /// <summary>
    ///  Section contains information about when a course is offered
   /// </summary>

    public class Section
    {
        public Section()
        {
            
        }
        public Section(string name)
        {
            sectionID = name;
         
        }
        public Section(Section passed)
        {
            sectionID = passed.sectionID;
        }
        public string sectionID;

    }
}
