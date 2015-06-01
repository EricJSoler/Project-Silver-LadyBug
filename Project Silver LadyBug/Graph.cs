using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Silver_LadyBug
{
    /// <summary>
    /// Graph class will hold all the required courses in order to complete a degree in 
    /// a dictionary. Relationships between courses will be represented by an adjacency
    /// matrix.
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Graph()
        {
            allCourses = new Dictionary<String, GraphNode>();
            courseGrid = new List<List<List<bool>>>();
            coursesPlacedInto = new Dictionary<String, int>();
            frontLoaded = new Dictionary<String, List<PrereqRow>>();
            gridDepth = 0;
        }

        public void frontLoad(string dep, string num)
        {
            List<PrereqRow> pres = getCoursePrereq(dep, num);
            try {
                frontLoaded.Add(dep + num, pres);
            }
            catch (ArgumentException e) { /*already have it do nothing*/}
        }


        public void fixDepths()
        {
            for (int i = 0; i < courseGrid.Count; i++) {
                int rowsDepth = 0;
                for (int j = 0; j < courseGrid[i].Count; j++) {
                    if (courseGrid[i][j].Count > rowsDepth)
                        rowsDepth = courseGrid[i][j].Count;
                }
                for (int j = 0; j < courseGrid[i].Count; j++) {
                    while (courseGrid[i][j].Count < rowsDepth)
                        courseGrid[i][j].Add(false);
                }
            }
        }

        /// <summary>
        /// Inserts a Course Into the graph. This calls the functions for building the 
        /// graph since it didnt make since to have to pass the parent index from main 
        /// so it calls the addCourseToGrid() which returns which index information for 
        /// this course will be stored
        /// </summary>
        /// <param name="dep"></param>
        /// <param name="num"></param>
        public void insertCourse(string dep, string num)
        {
            try {
                List<PrereqRow> row;
                if (frontLoaded.TryGetValue(dep + num, out row)) { /* do nothing*/}
                else
                    row = this.getCoursePrereq(dep, num);
                GraphNode temp = new GraphNode(dep, num);
                allCourses.Add(dep + num, temp);
                int courseRow = addCourseToGrid();
                temp.row = courseRow;
                int placementnumber;

                if ((coursesPlacedInto.TryGetValue(dep, out placementnumber)) && (placementnumber.ToString() == num)) { /*Do nothing on purpose*/}
                else
                    this.build(row, courseRow);
            }
            catch (ArgumentException e) {
                //Already have it do nothing brah
            }
        }

        /// <summary>
        /// Checks if the depth is the correct size for all lists
        /// </summary>
        /// <param name="parentCourse">The parent course index to set to true 
        /// (or the row course)</param>
        /// <param name="prereqCourse">The prereq index of the parent course to set 
        /// to true (or column of prereq)</param>
        /// <param name="depth">The current depth to check against</param>
        public void checkDepth(int parentCourse, int prereqCourse, int depth)
        {
            // make each column the same depth
            for (int i = 0; i < courseGrid.Count; i++)
                while (courseGrid[parentCourse][i].Count <= depth)
                    courseGrid[parentCourse][i].Add(false);

            courseGrid[parentCourse][prereqCourse][depth] = true;
            if (gridDepth < depth)
                gridDepth = depth;
        }

        /// <summary>
        /// Adds a single course to the adjacency matrix and returns the integer value 
        /// of what row information about this course will be stored in
        /// </summary>
        private int addCourseToGrid()
        {
            courseGrid.Add(new List<List<bool>>());
            for (int i = 0; i < courseGrid.Count; i++) {
                courseGrid[i].Add(new List<bool>());
                if (courseGrid.Count > 1 && i > 0)
                    courseGrid[courseGrid.Count - 1].Add(new List<bool>());
            }
            return (courseGrid.Count) - 1;
        }

        /// <summary>
        /// Removes the last course added to the courseGrid.
        /// </summary>
        private void removeCourseFromGird(int parentIndex)
        {
            courseGrid.RemoveAt(parentIndex);
            for (int i = 0; i < courseGrid.Count; i++) {
                courseGrid[i].RemoveAt(parentIndex);
            }
        }


        private void findMyGroup(PrereqRow toBePlaced, LocalGraphNode root)
        {
            bool reccomended = false;
            if (root.children.Count == 0) {
                LocalGraphNode placement = new LocalGraphNode();
                placement.group = toBePlaced.groupID;
                placement.departmentID = toBePlaced.prereqDepartmentID;
                placement.numberID = toBePlaced.prereqNumberID;
                placement.allCourseIndex = toBePlaced.gridLocation;
                root.children.Add(placement);
                reccomended = true;
                return;
            }
            foreach (LocalGraphNode element in root.children) {

                if (toBePlaced.groupID == element.group && toBePlaced.type == "AND") {
                    LocalGraphNode placement = new LocalGraphNode();
                    placement.group = toBePlaced.groupID;
                    placement.departmentID = toBePlaced.prereqDepartmentID;
                    placement.numberID = toBePlaced.prereqNumberID;
                    placement.allCourseIndex = toBePlaced.gridLocation;
                    element.children.Add(placement);
                    reccomended = true;
                    return;
                }
                else if (toBePlaced.groupID == element.group && toBePlaced.type == "OR") {
                    LocalGraphNode placement = new LocalGraphNode();
                    placement.group = toBePlaced.groupID;
                    placement.departmentID = toBePlaced.prereqDepartmentID;
                    placement.numberID = toBePlaced.prereqNumberID;
                    placement.allCourseIndex = toBePlaced.gridLocation;
                    root.children.Add(placement);
                    reccomended = true;
                    return;
                }
                else {
                    int similiarites = 0;
                    for (int i = 0, k = 0; i < element.group.Count() && k < toBePlaced.groupID.Count(); i++, k++) {
                        if (element.group[i] == toBePlaced.groupID[k])
                            similiarites++;
                    }
                    if (similiarites == element.group.Count()) {
                        findMyGroup(toBePlaced, element);
                        reccomended = true;
                        return;
                    }
                    //Check for off by one that means they are the  same level
                }

            }
            //If none of these conditions were true it must insert here

            if (!reccomended) {
                LocalGraphNode placement2 = new LocalGraphNode();
                placement2.group = toBePlaced.groupID;
                placement2.departmentID = toBePlaced.prereqDepartmentID;
                placement2.numberID = toBePlaced.prereqNumberID;
                placement2.allCourseIndex = toBePlaced.gridLocation;
                root.children.Add(placement2);
            }
        }

        /// <summary>
        /// Builds the graph with the given list of required courses
        /// </summary>
        /// <param name="prereqRows">List of required courses</param>
        /// <param name="parentIndex">0 for first (for recursion)</param>
        private void build(List<PrereqRow> prereqRows, int parentIndex)
        {
            //
            List<PrereqRow> coursesAddedThisTimeAround = new List<PrereqRow>();

            //This is where the local graph will stem from
            LocalGraphNode parent = new LocalGraphNode();
            parent.group = "1";

            //Sorting the prereqRows
            for (int i = 0; i < prereqRows.Count; i++) {
                int smallIndex = i;
                for (int j = i + 1; j < prereqRows.Count; j++) {
                    if (prereqRows[j].groupIDCount < prereqRows[i].groupIDCount) {
                        smallIndex = j;
                    }
                }
                PrereqRow temp = prereqRows[i];
                prereqRows[i] = prereqRows[smallIndex];
                prereqRows[smallIndex] = temp;
            }
            //Building the local graph
            foreach (PrereqRow row in prereqRows) {
                if (row.prereqDepartmentID != "MASTER") {
                    bool temporaryLazyDeleteHelper = false;
                    if (row.prereqDepartmentID == "PLACEMENT") {
                        if ((coursesPlacedInto.ContainsKey(row.departmentID) &&
                            coursesPlacedInto[row.departmentID] != Convert.ToInt32(row.numberID))) {
                            GraphNode temp;
                            allCourses.TryGetValue(row.departmentID + row.numberID, out temp);
                            temp.completed = true;
                            break;
                        }

                        continue;
                    }
                    // If we placed above the course, skip it.
                    if (coursesPlacedInto.ContainsKey(row.prereqDepartmentID) &&
                        (coursesPlacedInto[row.prereqDepartmentID] > Convert.ToInt32(row.prereqNumberID))) {
                        continue;
                    }
                    try {
                        GraphNode temp = new GraphNode(row.prereqDepartmentID, row.prereqNumberID);
                        allCourses.Add(row.prereqDepartmentID + row.prereqNumberID, temp);
                        int tempsRow = addCourseToGrid();
                        temp.row = tempsRow;
                        temp.completed = temporaryLazyDeleteHelper;
                        row.gridLocation = temp.row;
                        coursesAddedThisTimeAround.Add(row);
                    }
                    catch (ArgumentException e) {
                        GraphNode found;
                        if (allCourses.TryGetValue(row.prereqDepartmentID + row.prereqNumberID, out found)) {
                            row.gridLocation = found.row;
                        }
                    }
                    findMyGroup(row, parent);

                }
            }

            //////////
            ///Do the traversal right here and insert it into the row for the parent index.
            List<List<int>> paths = parent.getPaths();
            for (int i = 0; i < paths.Count; i++) {
                for (int j = 0; j < paths[i].Count; j++) {
                    checkDepth(parentIndex, paths[i][j], i);
                }
            }
            //////////

            List<PrereqRow> morePrereqs;
            foreach (PrereqRow row in coursesAddedThisTimeAround) {
                if (frontLoaded.TryGetValue(row.prereqDepartmentID + row.prereqNumberID, out morePrereqs)) { /* do nothing*/}
                else {
                    morePrereqs = this.getCoursePrereq(row.prereqDepartmentID, row.prereqNumberID);
                    try {
                        frontLoaded.Add(row.prereqDepartmentID + row.prereqNumberID, morePrereqs);
                    }
                    catch (ArgumentException e) { }
                }
                if (!(coursesPlacedInto.ContainsKey(row.prereqDepartmentID) &&
                    (coursesPlacedInto[row.prereqDepartmentID] == Convert.ToInt32(row.prereqNumberID)))) {
                    build(morePrereqs, row.gridLocation);
                }
            }
        }


        /// <summary>
        /// Gets the list of courses required to take for the desired course
        /// </summary>
        /// <param name="departmentID">The courses department ID that you want the prereqs for</param>
        /// <param name="numberID">The courses number ID that you want the prereqs for</param>
        /// <returns></returns>
        public List<PrereqRow> getCoursePrereq(String departmentID, String numberID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "dbo.Read_PreReqDep_PreReqN_Ext_GroupID_By_DepID_NumID";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@DepID", departmentID));
            cmd.Parameters.Add(new SqlParameter("@NumID", numberID));
            cmd.Connection = SQLHANDLER.myConnection2;

            reader = cmd.ExecuteReader();

            List<PrereqRow> prereqs = new List<PrereqRow>();
            int i = 0;
            while (reader.Read()) {
                prereqs.Add(new PrereqRow());
                prereqs[i].departmentID = departmentID;
                prereqs[i].numberID = numberID;
                prereqs[i].prereqDepartmentID = reader.GetValue(0).ToString();
                prereqs[i].prereqNumberID = reader.GetValue(1).ToString();
                prereqs[i].type = reader.GetValue(2).ToString();
                String recieved = reader.GetValue(3).ToString();
                String[] stuff = recieved.Split('.');

                for (int j = 0; j < stuff.Count(); j++)
                    prereqs[i].groupID += stuff[j];
                prereqs[i].groupIDCount = prereqs[i].groupID.Count();
                i++;
            }
            reader.Close();

            return prereqs;
        }
        /// <summary>
        /// fidQualifiedCourses() will traverse through the adjacency matrix and search
        /// for the courses a student is currently qualified for. It will then store 
        /// this in a List<Course> qualified</Course> and return this value to be sent 
        /// to the time filter 
        /// </summary>
        /// <returns></returns>
        public List<Course> findQualifiedCourses()
        {
            List<Course> qualified = new List<Course>();
            List<GraphNode> alreadyAddedThisLoop = new List<GraphNode>();
            //check each row i
            for (int i = 0; i < courseGrid.Count; i++) {
                int count = 0;
                // check each depth of row i (depth is constant through the row)
                for (int j = 0; j < courseGrid[i][0].Count; j++) {
                    count = 0;
                    // check each column at depth j
                    for (int k = 0; k < courseGrid[i].Count; k++) {
                        // check if the course is a prereq for this course
                        if (courseGrid[i][k][j] == true) {
                            count++;
                            break;
                        }
                    }

                    if (count == 0) {
                        foreach (GraphNode element in allCourses.Values) {
                            if (element.row == i)
                                if (!element.completed && !(alreadyAddedThisLoop.Contains(element))) {
                                    alreadyAddedThisLoop.Add(element);
                                    qualified.Add(new Course(element.m_departmentID, element.m_numberID));
                                }
                        }
                    }
                }

                if (count == 0) {
                    foreach (GraphNode element in allCourses.Values) {
                        if (element.row == i)
                            if (!element.completed && !(alreadyAddedThisLoop.Contains(element))) {
                                alreadyAddedThisLoop.Add(element);
                                qualified.Add(new Course(element.m_departmentID, element.m_numberID));
                            }
                    }
                }
            }

            return qualified;
        }

        /// <summary>
        /// update Completed will intake a List<Course> recommended </Course> that have
        /// been completed and will update the graph to mark these courses as completed
        /// </summary>
        /// <param name="recommended"></param>
        public void updateCompleted(List<Course> recommended)
        {
            foreach (Course element in recommended) {
                GraphNode temp;
                String key = element.departmentID + element.numberID;
                allCourses.TryGetValue(key, out temp);
                int col = temp.row;
                temp.completed = true;
                updateColumn(col);
            }
        }

        private void updateColumn(int column)
        {
            for (int i = 0; i < courseGrid.Count; i++) {
                for (int j = 0; j < courseGrid[i][column].Count; j++) {
                    courseGrid[i][column][j] = false;
                }
            }
        }


        public int occurenceCount(String dep, String num)
        {
            int count = 0;
            GraphNode temp;
            allCourses.TryGetValue(dep + num, out temp);
            int location = temp.row;
            for (int i = 0; i < courseGrid.Count; i++) {
                foreach (bool element in courseGrid[i][location]) {
                    if (element == true)
                        count++;
                }
            }
            return count;
        }

        /// <summary>
        /// allCourses is a dictionary of GraphNode using integers as keys. The integer
        /// key for each entry in the dictionary will represent what row in the 
        /// adjacency matrix information about the course can be found
        /// </summary>
        public Dictionary<String, GraphNode> allCourses;

        /// <summary>
        /// courseGrid is a 3 dimensional List matrix. Each the rows of the matrix 
        /// will be used to store information for a course from the allCourses 
        /// dictionary. Columns Will be used to represent Pre-Requisites for the course
        /// in a given row. Width will be used to represent the different possible 
        /// combinations of taking different courses
        /// i.e. [Course][Pre-Reqs][Combinations] 
        /// </summary>
        public List<List<List<bool>>> courseGrid;


        /// <summary>
        /// The depth of the entire grid (different paths)
        /// </summary>
        public int gridDepth;

        /// <summary>
        /// Keeps track of all the courses we've placed into 
        /// (Default placement in the constructor)
        /// </summary>
        public Dictionary<String, int> coursesPlacedInto;

        public Dictionary<String, List<PrereqRow>> frontLoaded;
    }

    /// <summary>
    /// The rows that the db tables return
    /// </summary>
    public class PrereqRow
    {

        /// <summary>
        /// Represents the department of the MASTER course
        /// </summary>
        public String departmentID;
        /// <summary>
        /// Represents the course number of the MASTER course
        /// </summary>
        public String numberID;
        /// <summary>
        /// Represents the department of the prereq course
        /// </summary>
        public String prereqDepartmentID;
        /// <summary>
        /// Represents the course number of the prereq course
        /// </summary>
        public String prereqNumberID;
        /// <summary>
        /// The type of parent the prereq has (AND or OR)
        /// </summary>
        public String type;
        /// <summary>
        /// The group ID its parent has ex: 1.1.3
        /// </summary>
        public String groupID;

        public int groupIDCount;

        public int gridLocation;
    }
}