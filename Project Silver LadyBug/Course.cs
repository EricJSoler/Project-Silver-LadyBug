using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.IO;

namespace Project_Silver_LadyBug//We got a problem wiht naming a column external
{
  

    /// <summary>
    /// Course: Stores and pulls information about individual courses for the forseable future.
    /// Usability: Create a course with the constructor accepting a string refering to the numberID 
    /// of the course to pull information for and a string refering to the department ID of the 
    /// course to pull information for it owns a list of Term objects 
    /// which each term (i.e. Fall, Winter etc...) owns a list of sections that contain specific
    /// information about the courses different oferring times. Once this object is created you 
    /// can navigate through its list of Term objects "ownedTerms" and through "ownedTerms" list
    /// of Section objects "ownedSections" to compare the times that offered between different courses.
    /// </summary>
    public class Course 
    {
     public Course()
        { }
        

        public Course(string dID, string numID)
        {
            
            numberID = numID;
            departmentID = dID;
        }

       
        public string departmentID;
        public string numberID;
        public List<Term> ownedTerms;
        public int importance;

        public void readDataForCourseName()
        {
            ownedTerms = new List<Term>();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "Course.spOfferingRead_ByDepartmentIdAndNumberId";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@DepartmentID", departmentID));
            cmd.Parameters.Add(new SqlParameter("@NumberID", numberID));
            cmd.Connection = SQLHANDLER.myConnection;

            reader = cmd.ExecuteReader();


            int counter = 0;
            while (reader.Read())
            {
                int count = reader.FieldCount;

                for (int i = 0; i < count; i++)
                {

                    string colName;
                    colName = reader.GetName(i);
                    string readValue = reader.GetValue(i).ToString();
                    if (colName == "IntervalID")
                    {
                        bool stillEmpty = true;
                        foreach (Term element in ownedTerms)
                        {
                            if (readValue == element.termID)
                            {
                                element.ownedSections.Add(new Section(reader.GetValue(++i).ToString()));
                                stillEmpty = false;
                            }
                        }
                        if (stillEmpty)
                        {
                            ownedTerms.Add(new Term(reader.GetValue(i).ToString()));
                            ownedTerms[counter].ownedSections.Add(new Section(reader.GetValue(++i).ToString()));
                            counter++;
                        }
                    }

                }

            }

            reader.Close();
           
        }
    }
}
