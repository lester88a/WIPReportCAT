using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIPReportCat.Configuration;

namespace WIPReportCat
{
    public class Connection
    {
        //Thread Safe Singleton without using locks and no lazy instantiation
        private static readonly Connection instance = new Connection();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Connection() { }
        private Connection() { }

        //connection string
        private static string ConnectionString { get; set; }
        //query for all data
        private static string QueryAll { get; set; }
        //file name
        private static string FileName { get; set; }

        public static Connection Instance
        {
            get
            {
                //Config config = new Config();
                string dataSource = Config.Instance.GetDataSource();
                string initialCatalog = Config.Instance.GetInitialCatalog();
                string filePath = Config.Instance.GetFileSavedPath();
                string manufacturer = Config.Instance.GetManufacturer();


                ConnectionString = "Data Source=" + dataSource + ";Initial Catalog=" + initialCatalog + ";Integrated Security=True";

                QueryAll = @"SELECT R.RefNumber,DATEDIFF(day, R.DateIn, convert(date, GETDATE())) as Aging,
							R.ESN,R.MSN,R.Status,Format(R.DateIn, N'yyyy-MM-dd HH:mm:ss') as DateIn,R.DateDockOut,R.DateComplete,
							Format(R.DateEstimation, N'yyyy-MM-dd HH:mm:ss') as DateEstimation,
                            Format(R.DateApproved, N'yyyy-MM-dd HH:mm:ss') as DateApproved,
                            Format(R.DateReject, N'yyyy-MM-dd HH:mm:ss') as DateReject,
                            Format(R.PurchaseDate, N'yyyy-MM-dd HH:mm:ss') as PurchaseDate,R.Mot_Tran,
							R.Warranty,R.Manufacturer,R.ManufacturerOTC,R.MftrMSN,R.FuturetelMSN,R.FutureTelOTC,
							R.WorkCode1,R.WorkCode2,R.WorkCode3,R.Problem1,R.Problem2,R.Problem3,
							R.Complaint1,R.Complaint2,R.Complaint3,M.ProductCode,
                            P.Qty,P.PartNumber,P.FaultCode
                            FROM " + initialCatalog + @".dbo.tblRepair R
							LEFT JOIN " + initialCatalog + @".dbo.tblRepairParts P
							ON R.RefNumber = P.RefNumber
							LEFT JOIN " + initialCatalog + @".dbo.tblModel M
							ON R.ModelNumber = M.ModelNumber
							where R.Manufacturer = '" + manufacturer + @"' 
							And R.DateDockOut is null
							And (R.Status = 'R' or R.Status != 'X')
                            And (R.DateComplete >= (Format(GetDate(), N'yyyy-MM-dd')+' 07:00:00') or R.DateComplete is null)
							And R.FuturetelLocation != 'CATL_CUST'
                            AND R.RefNumber in 
                            (select R.RefNumber
                            FROM EasyDB.dbo.tblRepair R
                            LEFT JOIN easydb.dbo.tblRepairParts P
                            ON R.RefNumber = P.RefNumber
                            LEFT JOIN EasyDB.dbo.tblModel M
                            ON R.ModelNumber = M.ModelNumber
                            group by R.RefNumber
                            Having COUNT(*)=1)
							order by R.RefNumber";

                //current location
                string currentLocation = AppDomain.CurrentDomain.BaseDirectory;
                string currentDate = DateTime.Now.ToString("yyyyMMdd-HHmm");
                //file name
                FileName = @"" + filePath + manufacturer + " WIP " + currentDate + ".xlsx";

                return instance;
            }
        }
        
        public string GetConnectionString()
        {
            return ConnectionString;
        }

        public string GetQueryAll()
        {
            return QueryAll;
        }
        
        public string GetFileName()
        {
            return FileName;
        }
    }
}
