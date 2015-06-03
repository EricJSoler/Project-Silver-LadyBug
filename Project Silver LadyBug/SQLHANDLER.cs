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
    /// SQLHandler handles the connection with the database it currently has two connections one 
    /// for the "Pretty Database" and one for the "Sandbox database" that we are playing in 
    /// trying to make pre-req filter work
    /// </summary>
    public static class SQLHANDLER
    {
       
        public static void start()
        {
            myConnection = new SqlConnection("User ID = Algo;" + "Password = Alg0rithm; server = algo.database.windows.net;" + "database =Advising_20150515;"
    + "Connection Timeout = 30;");
            try
            {
                myConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                System.Environment.Exit(1);
            }
            myConnection2 = new SqlConnection("User ID = Algo;" + "Password = Alg0rithm; server = algo.database.windows.net;" + "database =Test_0506;"
   + "Connection Timeout = 30;"+"MultipleActiveResultSets=true;");
            try
            {
                myConnection2.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                System.Environment.Exit(1);
            }

           
        }


        /// <summary>
        /// myConnection is the connection to the main database(The Pretty one that Steve is building)
        /// </summary>
        public static SqlConnection myConnection;
        /// <summary>
        /// myConnection2 is the connection to the "sandbox" database that we are throwing 
        /// non-finalized information into as we need it to build the rest of this processing system
        /// </summary>
        public static SqlConnection myConnection2;
    }
}
