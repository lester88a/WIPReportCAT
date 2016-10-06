using System.Data.SqlClient;
using SQL = System.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using WIPReportCat.Models;

namespace WIPReportCat.Controllers
{
    public class DataController
    {
        // This method is used to convert datatable to json string
        public ObservableCollection<RepairModel> GetAllRepairData()
        {
            //Connection connection = new Connection();
            string connectionString = Connection.Instance.GetConnectionString();
            string query = Connection.Instance.GetQueryAll();

            SQL.DataTable dt = new SQL.DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;

                    foreach (SQL.DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (SQL.DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    //conver json string to json obejct
                    var items = JsonConvert.DeserializeObject<ObservableCollection<RepairModel>>(JsonConvert.SerializeObject(rows));
                    System.Console.WriteLine("DataController items.Count: " + items.Count);
                    return items;
                }
            }
        }

        //method for verify the table exists
        private static bool IsTabelExists()
        {
            //Connection con = new Connection();
            string connectionString = Connection.Instance.GetConnectionString();

            bool exists;

            #region verify the exists of tblRepair
            try
            {
                int count;
                using (SqlConnection thisConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmdCount = new SqlCommand("select case when exists((select * from reportdb.information_schema.tables where table_name = 'tblReportWIP')) then 1 else 0 end", thisConnection))
                    {
                        thisConnection.Open();
                        cmdCount.CommandTimeout = 0;
                        count = (int)cmdCount.ExecuteScalar();
                    }
                }

                if (count == 1)
                {
                    exists = true;
                }
                else
                {
                    exists = false;
                }
            }
            catch
            {
                exists = false;
            }
            #endregion

            return exists;
        }
    }
}
