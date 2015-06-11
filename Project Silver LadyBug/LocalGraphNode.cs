using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Silver_LadyBug
{
    /// <summary>
    /// Local graph node has a list of graph nodes this class is only used inside of the build
    /// function within Graph to find all the paths from pre reqs so it can then be transformed 
    /// into the format needed for the graph
    /// </summary>
    public class LocalGraphNode
    {
        public LocalGraphNode()
        {
            children = new List<LocalGraphNode>();
        }
        public List<int> groupID;
        public String group;
        public string departmentID;
        public string numberID;
        public List<LocalGraphNode> children;
        public int allCourseIndex;

        public List<List<int>> getPaths()
        {
            List<List<int>> paths = new List<List<int>>();
            paths.Add(new List<int>());
            getPaths(children, paths);
            paths.RemoveAt(paths.Count - 1);

            return paths;
        }

        private void getPaths(List<LocalGraphNode> nodes, List<List<int>> paths)
        {
            if (nodes.Count == 0) {
                paths.Add(new List<int>());
                for (int i = 0; i < paths[paths.Count - 2].Count; i++)
                    paths[paths.Count - 1].Add(paths[paths.Count - 2][i]);
            }
            else {
                foreach (LocalGraphNode node in nodes) {
                    paths[paths.Count - 1].Add(node.allCourseIndex);
                    getPaths(node.children, paths);
                    paths[paths.Count - 1].RemoveAt(paths[paths.Count - 1].Count - 1);
                }
            }
        }
    }
}
