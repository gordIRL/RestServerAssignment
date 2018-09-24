using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace simpleRESTServer
{
    public class UserEntryPersistence
    {

        private SqlConnection conn;
        
        public UserEntryPersistence()
        {
            string myConnectionString;
            myConnectionString = Properties.Settings.Default.connString;

            try
            {
                conn = new SqlConnection
                {
                    ConnectionString = myConnectionString
                };
                conn.Open();
            }
            catch(SqlException ex)
            {

            }
        }// end constructor


        public int SaveUserEntry(UserEntry userEntryToSave)
        {
            // define INSERT query with parameters
            string query = "INSERT INTO dbo.UserEntry (Category, Notes, StartDate, EndDate) " +
                           "VALUES (@Category, @Notes, @StartDate, @EndDate) ; " +
                           "SELECT SCOPE_IDENTITY() ";

            SqlCommand cmd = new SqlCommand(query, conn);

            // define parameters and their values
            //cmd.Parameters.Add("@Id", SqlDbType.Int).Value = 26;
            cmd.Parameters.Add("@Category", SqlDbType.VarChar, 50).Value = userEntryToSave.Category;
            cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 50).Value = userEntryToSave.Notes;

            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = userEntryToSave.StartDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = userEntryToSave.EndDate;

            // use ExecuteScaler to get autoincremented primary key back
            int insertedID = Convert.ToInt32(cmd.ExecuteScalar());

            return insertedID;
        }


        public UserEntry GetUserEntry(int id)
        {
            UserEntry userEntry = new UserEntry();
            SqlDataReader sqlDataReader = null;

            string query = "SELECT * FROM dbo.UserEntry WHERE Id = @Id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            sqlDataReader = cmd.ExecuteReader();
            if (sqlDataReader.Read())
            {
                userEntry.Id = sqlDataReader.GetInt32(0);
                userEntry.Category = sqlDataReader.GetString(1);
                userEntry.Notes = sqlDataReader.GetString(2);
                userEntry.StartDate = sqlDataReader.GetDateTime(3);
                userEntry.EndDate = sqlDataReader.GetDateTime(4);
                return userEntry;
            }
            else
            {
                return null;
            }
        }




        public List<UserEntry> GetUserEntries()
        {              
            List<UserEntry> userEntryList = new List<UserEntry>();

            SqlDataReader sqlDataReader = null;

            string query = "SELECT * FROM dbo.UserEntry ";   
            SqlCommand cmd = new SqlCommand(query, conn);

            sqlDataReader = cmd.ExecuteReader();              

            while (sqlDataReader.Read())
            {
                UserEntry userEntry = new UserEntry();
                userEntry.Id = sqlDataReader.GetInt32(0);
                userEntry.Category = sqlDataReader.GetString(1);
                userEntry.Notes = sqlDataReader.GetString(2);
                userEntry.StartDate = sqlDataReader.GetDateTime(3);
                userEntry.EndDate = sqlDataReader.GetDateTime(4);
                userEntryList.Add(userEntry);
            }
            return userEntryList;
        }




        public bool DeleteUserEntry(int id)
        {
            SqlDataReader sqlDataReader = null;

            string query = "SELECT * FROM dbo.UserEntry WHERE Id = @Id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            sqlDataReader = cmd.ExecuteReader();

            if (sqlDataReader.Read())
            {
                sqlDataReader.Close();

                query = "DELETE FROM dbo.UserEntry WHERE Id = @Id";

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                cmd.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }
        }




        public bool UpdateUserEntry(int id, UserEntry userEntryToUpdate)
        {
            SqlDataReader sqlDataReader = null;

            string query = "SELECT * FROM dbo.UserEntry WHERE Id = @Id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            sqlDataReader = cmd.ExecuteReader();
            if (sqlDataReader.Read())
            {
                sqlDataReader.Close();
                                                
                query = "UPDATE dbo.UserEntry SET Category= @Category, Notes= @Notes, " +
                    "StartDate= @StartDate, EndDate= @EndDate  WHERE Id = @Id";

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                cmd.Parameters.Add("@Category", SqlDbType.VarChar, 50).Value = userEntryToUpdate.Category;
                cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 50).Value = userEntryToUpdate.Notes;

                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = userEntryToUpdate.StartDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = userEntryToUpdate.EndDate;

                cmd.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}