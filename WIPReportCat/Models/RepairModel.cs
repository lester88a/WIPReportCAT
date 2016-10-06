using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIPReportCat.Models
{
    public class RepairModel
    {
        public int RefNumber { get; set; }
        public int Aging { get; set; }
        public string ESN { get; set; }
        public string MSN { get; set; }
        public string Status { get; set; }
        public string DateIn { get; set; }
        public DateTime? DateDockOut { get; set; }
        public DateTime? DateComplete { get; set; }
        public string DateEstimation { get; set; }
        public string DateApproved { get; set; }
        public string DateReject { get; set; }
        public string PurchaseDate { get; set; }
        public string Mot_Tran { get; set; }
        public bool Warranty { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerOTC { get; set; }
        public string MftrMSN { get; set; }
        public string FuturetelMSN { get; set; }
        public string FutureTelOTC { get; set; }
        public string WorkCode1 { get; set; }
        public string WorkCode2 { get; set; }
        public string WorkCode3 { get; set; }
        public string Complaint1 { get; set; }
        public string Complaint2 { get; set; }
        public string Complaint3 { get; set; }
        public string Problem1 { get; set; }
        public string Problem2 { get; set; }
        public string Problem3 { get; set; }
        public string ProductCode { get; set; } //require let join table of tblModel
        public short? Qty { get; set; }//require left join table of tblRepairParts
        public string PartNumber { get; set; }//require left join table of tblRepairParts
        public string FaultCode { get; set; }//require left join table of tblRepairParts
    }
}
