using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.IO;

namespace Project_Silver_LadyBug
{
    /// <summary>
    /// The Time Filter Will receive a List<Course> qualifiedFor</Course> from the Pre-req filter 
    /// this will consist of all the courses a student is currently qualified for 
    /// The time filter will then filter which courses can be scheduled, output these courses to 
    /// a container then send a List<Match>recommended</Match> 
    /// representing courses that were successfully scheduled and send that back to the Pre-Req filter
    /// for the Pre-Req filter to update its graph and to output
    /// </summary>


    public class TimeFilter
    {

        public TimeFilter()
        {
            matches = new List<Match>();
        }

        public List<Match> buildMyScheduleFor(List<Course> qualifications, string term, int numberOfCourses)
        {

            for (int i = 0, k = 0; i < numberOfCourses && k < qualifications.Count; i++, k++)
            {
                qualifications[k].readDataForCourseName();
                if (!addMatches(qualifications[k], term))
                    i--;
            }
                return this.matches;
        }
        
        private bool addMatches(Course recievedCourse, string termID)
        {
            List<Match> updated = new List<Match>();
            
            //Create a new match to add to the matches then start removing overlapping sections
            Match addedMatch = new Match(recievedCourse.departmentID, recievedCourse.numberID);
            addedMatch.importance = recievedCourse.importance;
            //Copy over section information for the specific term to the newly addedMatch
            foreach (Term term in recievedCourse.ownedTerms)
            {
                if (term.termID == termID)
                {
                    foreach (Section sect in term.ownedSections)
                    {
                        addedMatch.sectionOptions.Add(new Section(sect));
                    }
                }
            }
            //If there isn't anything in matches yet we know the course we can try to add 
            //the new match without comparing anything
            //Check to make sure there are some section options in the addedMatch
            if (matches.Count == 0 && addedMatch.sectionOptions.Count > 0)
            {
                matches.Add(addedMatch);
                return true;
            }//If there aren't any sections in the addedMatch we can return false
            if (addedMatch.sectionOptions.Count == 0)
            {
                return false;
            }
           
            matches.Add(addedMatch);
            //
            bool matchHasPriority = false;
            bool courseHasPriority = false;
            for (int i = 0; i < matches.Count; i++) //Match element in matches)
            {
                if(matches[i].departmentID == addedMatch.departmentID && matches[i].numberID == addedMatch.numberID)
                {
                    break;
                }
                for (int j = 0; j < matches[i].sectionOptions.Count;j++)
                {
                    for (int k = 0; k < addedMatch.sectionOptions.Count ;k++)
                    {   //If a course only has one option give it priority
                        int sizeOfSectionForMatch = matches[i].sectionOptions.Count;
                        int sizeOfComparedCourse = addedMatch.sectionOptions.Count;
                        if (sizeOfSectionForMatch == 1)
                            matchHasPriority = true;
                        if (sizeOfComparedCourse == 1)
                            courseHasPriority = true;
                        //These Classes overlap and a decision need to be made on which one to drop
                        if (addedMatch.sectionOptions[k].sectionID == matches[i].sectionOptions[j].sectionID && courseHasPriority && matchHasPriority)
                        {
                            if (matches[i].importance < addedMatch.importance)
                            {
                                matches[i].sectionOptions.RemoveAt(j);
                            }
                            else
                            {
                                addedMatch.sectionOptions.RemoveAt(k);
                            }
                        }
                        else if (addedMatch.sectionOptions[k].sectionID == matches[i].sectionOptions[j].sectionID && matchHasPriority)
                        {
                            addedMatch.sectionOptions.RemoveAt(k);
                        }
                        else if (addedMatch.sectionOptions[k].sectionID == matches[i].sectionOptions[j].sectionID && courseHasPriority)
                        {
                            //Maybe put in an if that checks for a range of difference in importance
                            matches[i].sectionOptions.RemoveAt(j);
                        }
                        else if (addedMatch.sectionOptions[k].sectionID == matches[i].sectionOptions[j].sectionID)
                        {
                            if (matches[i].importance < addedMatch.importance)
                            {
                                matches[i].sectionOptions.RemoveAt(j);
                            }
                            else
                            {
                                addedMatch.sectionOptions.RemoveAt(k);
                            }
                        }

                    }
                }
            }

            //Clean up the matches
            for (int i = 0; i < matches.Count; i++ )
            {
                if (matches[i].sectionOptions.Count == 0)
                    matches.RemoveAt(i);
            }
                if (addedMatch.sectionOptions.Count > 0)
                {

                    return true;
                }
                else
                    return false;
        }

        /// <summary>
        /// This is a list of combinations of courses that can be taken in the given quarter
        /// It serves as a broken down version of the Course object to allow for easy comparisons
        /// between courses that have already been scheduled
        /// </summary>
        private List<Match> matches; 
    }




}
