using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Silver_LadyBug
{
    
    /// <summary>
    /// Term serves to allow for information in courses to be organized more efficiently it 
    /// contains a list of ownedSections
    /// </summary>

    public class Term
    {
        public Term()
        {
            ownedSections = new List<Section>();
        }
        public Term(string name)
        {
            termID = name;
            ownedSections = new List<Section>();

        }

        /// <summary>
        /// Represents what term the List<Section>ownedSections </Section> are in 
        /// </summary>
        public string termID;
        /// <summary>
        /// A List<Section>ownedSections </Section> that contain information about what time
        /// block a course is offered in
        /// </summary>
        public List<Section> ownedSections;
    }
}
