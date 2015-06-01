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
    /// Graph Node: Will be the nodes used in the Graph class list of allCourses. 
    /// Relationships between GraphNodes will be represented by the adjacency matrix adj in 
    /// the Graph class
    /// </summary>
    public class GraphNode
    {
        /** Constructor
         *  Initializes the graph node with the department ID and the number ID 
         *  \param departmentID The department ID of the course.
         *  \param numberID The number ID of the course.
         */
        public GraphNode(String departmentID, String numberID)
        {
            m_departmentID = departmentID;
            m_numberID = numberID;
            completed = false;
        }

        /// The department ID of the course
        public String m_departmentID;
        /// The number ID of the course
        public String m_numberID;
        public bool completed;
        public int row;
    }
}
