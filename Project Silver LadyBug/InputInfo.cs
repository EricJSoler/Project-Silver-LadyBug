using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Globalization;


//Function InputInfo constructor takes a string with the name of the file w/.xml

namespace Project_Silver_LadyBug
{
    public class InputInfo
    {
        public StudentInfo student;
        public TransferSchool school;
        public StartingQuarter quarter;
        public Dictionary<String, int> placement;
        public Dictionary<String, int> specialization;
        public InputInfo(string xmlFile)
        {
            student = new StudentInfo();
            school = new TransferSchool();
            quarter = new StartingQuarter();
            placement = new Dictionary<string, int>();
            specialization = new Dictionary<string, int>();
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(xmlFile);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                System.Environment.Exit(-1);
                return;
            }
            foreach (XmlNode node in xDoc.SelectNodes("//Input"))
            {

                foreach (XmlNode fileNode in node.ChildNodes)
                {
                    // add  to ownership dictionary for future reference
                    if (fileNode.Name == "Student")
                    {
                        student.name = fileNode.Attributes["name"].Value;
                        student.ID = fileNode.Attributes["ID"].Value;
                        student.Credits = Convert.ToInt32(fileNode.Attributes["Credits_per_Quarter"].Value);
                    }
                    if (fileNode.Name == "TransferSchool")
                    {
                        school.ID = fileNode.Attributes["ID"].Value;
                        school.ID = fileNode.Attributes["name"].Value;
                    }
                    if (fileNode.Name == "Placement")
                    {

                        foreach (XmlNode grandNode in fileNode.ChildNodes)
                        {
                            if (placement.Count == 0 || !placement.ContainsKey(grandNode.Attributes["Department"].Value.ToUpperInvariant()))
                            {
                                placement.Add(grandNode.Attributes["Department"].Value.ToUpperInvariant(), Convert.ToInt32(grandNode.Attributes["CourseID"].Value));
                            }
                        }
                    }

                    if (fileNode.Name == "Specialization")
                    {
                        foreach (XmlNode grandNode in fileNode.ChildNodes)
                        {
                            if (specialization.Count == 0 || !specialization.ContainsKey(grandNode.Attributes["Department"].Value.ToUpperInvariant()))
                            {
                                specialization.Add(grandNode.Attributes["Department"].Value.ToUpperInvariant(), Convert.ToInt32(grandNode.Attributes["CourseID"].Value));
                            }
                        }
                    }

                }

            }
        }
    }

    public class StudentInfo
    {
        public string name;
        public string ID;
        public int Credits;
    }
    public class TransferSchool
    {
        public string ID;
        public string name;
    }
    public class StartingQuarter
    {
        public string quarter;
        public int year;
    }

}
         
    
