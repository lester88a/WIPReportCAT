using System.Linq;
using SQL = System.Data;
using System.Collections.ObjectModel;
using FastMember;
using WIPReportCat.Models;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace WIPReportCat.Controllers
{
    public class CatController
    {
        //instance variables
        private ObservableCollection<CatModel> CatDataModel;
        private SQL.DataTable CatDataTable;
        private string ErrorMessage = null;


        //Get DataIn DataTable
        public SQL.DataTable GetCatDataTable(ObservableCollection<RepairModel> AllRepairData, ref string errorMessage)
        {
            //initialize objects
            CatDataModel = new ObservableCollection<CatModel>();
            CatDataTable = new SQL.DataTable();

            //get all data
            var allData = AllRepairData;

            //date time: today's date at 7:00 AM
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0);

            //LINQ query to get catData data from all data
            var catData = from i in allData.AsEnumerable()
                          orderby i.Aging descending, i.RefNumber ascending
                          select i;
            
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00") + " +1:00";

            //assign data to connection
            foreach (var item in catData)
            {
                
                //boolean variables are used for IMEI Number Out
                #region
                bool isIMEINumberOutWorkCode1 = false;
                bool isIMEINumberOutWorkCode2 = false;
                bool isIMEINumberOutWorkCode3 = false;
                if ((new[] { "51", "52" }.Any(c => item.WorkCode1.Contains(c))) && item.Status == "C") { isIMEINumberOutWorkCode1 = true; }
                if ((new[] { "51", "52" }.Any(c => item.WorkCode2.Contains(c))) && item.Status == "C") { isIMEINumberOutWorkCode2 = true; }
                if ((new[] { "51", "52" }.Any(c => item.WorkCode3.Contains(c))) && item.Status == "C") { isIMEINumberOutWorkCode3 = true; }
                #endregion

                //boolean variables are used for Mandatory
                #region
                bool isMandatoryWorkCode1 = false;
                bool isMandatoryWorkCode2 = false;
                bool isMandatoryWorkCode3 = false;
                if ((new[] { "10", "20", "21" }.Any(c => item.WorkCode1.Contains(c))) && item.FaultCode == "53") { isMandatoryWorkCode1 = true; }
                if ((new[] { "10", "20", "21" }.Any(c => item.WorkCode2.Contains(c))) && item.FaultCode == "53") { isMandatoryWorkCode2 = true; }
                if ((new[] { "10", "20", "21" }.Any(c => item.WorkCode3.Contains(c))) && item.FaultCode == "53") { isMandatoryWorkCode3 = true; }
                #endregion

                //boolean variables are used for Quotation End Date
                #region
                string quotationEndDate;
                if (!String.IsNullOrEmpty(item.DateApproved) && String.IsNullOrEmpty(item.DateReject)) { quotationEndDate = item.DateApproved + " +1:00"; }
                else if (String.IsNullOrEmpty(item.DateApproved) && !String.IsNullOrEmpty(item.DateReject)) { quotationEndDate = item.DateReject + " +1:00"; }
                else { quotationEndDate = "NULL"; }
                #endregion

                //RepairStatusCode
                #region
                string repairStatusCode = "ERROR";
                if (item.Status == "C") { repairStatusCode = "16"; }
                else if (new[] { "A", "B", "E", "J" }.Any(c => item.Status.Contains(c))) { repairStatusCode = "50"; }
                else if (item.Status == "I") { repairStatusCode = "10"; }
                else if (item.Status == "R") { repairStatusCode = "15"; }
                #endregion

                //PartNumber
                #region
                string partNumber = null;
                if (item.Status == "C" && item.PartNumber == "===")
                {
                    partNumber = "NULL";
                }
                else if (item.Status == "C" && String.IsNullOrEmpty(item.PartNumber))
                {
                    partNumber = "NULL";
                }
                else
                {
                    partNumber = item.PartNumber;
                }
                #endregion

                //CatDataModel.Add
                #region
                CatDataModel.Add(new CatModel
                {
                    AccidentDate = "NULL",
                    ActionCode = (item.Status == "C" && item.DateComplete >= today) ? item.WorkCode1 + ":" + item.WorkCode2 + ":" + item.WorkCode3 : "NULL",
                    ActionReasonCode = (item.Status == "C") ? "10" : "NULL",
                    AWBIn = "NULL",
                    AWBOut = "NULL",
                    ClaimID = item.RefNumber,
                    CourierIn = "NULL",
                    CourierOut = "NULL",
                    CustomerComplaintCoddPrimary = item.Complaint1 + ":" + item.Complaint2 + ":" + item.Complaint3,
                    DateIn = item.DateIn.ToString() + " +1:00",
                    DateOut = "NULL",
                    DeliveryDate = "NULL",
                    DeliveryNumber = "NULL",
                    FaultCode = (item.Status == "C") ? item.FaultCode : "NULL",
                    FieldBulletinNumber = "NULL",
                    IMEI2In = "NULL",
                    IMEI2Out = "NULL",
                    IMEINumberIn = item.ESN,
                    IMEINumberOut = (isIMEINumberOutWorkCode1 || isIMEINumberOutWorkCode2 || isIMEINumberOutWorkCode3) ? item.FutureTelOTC + item.ManufacturerOTC : "NULL",
                    InboundShipmentType = "NULL",
                    ItemCodeOut = "NULL",
                    JobCreationDate = item.DateIn.ToString() + " +1:00",
                    Mandatory = (isMandatoryWorkCode1 || isMandatoryWorkCode2 || isMandatoryWorkCode3) ? "1" : "NULL",
                    ManufactureDate = "NULL",
                    MaterialNumber = item.Status == "C" ? partNumber : "NULL",
                    OEM = "BULLITT",
                    OutboundShipmentType = "NULL",
                    Payer = (item.Status == "C") ? ((item.Warranty == true) ? "10" : "30") : "NULL",
                    PickupArrangedDate = "NULL",
                    PickupDate = "NULL",
                    POPDate = "NULL",
                    POPSupplier = "NULL",
                    ProblemFoundCode = (item.Status == "C") ? item.Problem1 + ":" + item.Problem2 + ":" + item.Problem3 : "NULL",
                    ProductCodeIn = item.ProductCode,
                    ProductCodeOut = "NULL",
                    ProductType = "NULL",
                    ProductVersionIn = "NULL",
                    ProductVersionOut = "NULL",
                    Project = "NULL",
                    ProviderCarrier = "NULL",
                    QuantityReplaced = (item.Status == "C") ? item.Qty.ToString() : "NULL",
                    QuotationEndDate = quotationEndDate,
                    QuotationStartDate = (!String.IsNullOrEmpty(item.DateEstimation)) ? item.DateEstimation + " +1:00" : "NULL",
                    ReferenceDesignatorNumber = "NULL",
                    RepairServicePartnerID = "1247001",
                    RepairStatusCode = repairStatusCode,
                    RepairTimeStamp = "NULL",
                    ReportDate = currentTime,
                    ReturnDate = "NULL",
                    SecondStatus = "NULL",
                    SerialNumberIn = (!String.IsNullOrEmpty(item.MSN)) ? item.MSN : "NULL",
                    SerialNumberOut= (isIMEINumberOutWorkCode1 || isIMEINumberOutWorkCode2 || isIMEINumberOutWorkCode3) ? item.FuturetelMSN + item.MftrMSN : "NULL",
                    ShopID = "NULL",
                    SoftwareIn = "NULL",
                    SoftwareOut = "NULL",
                    SpecialProjectNumber = "NULL",
                    TechnicianID = "NULL",
                    TransactionCode = (item.Status == "C") ? item.Mot_Tran : "NULL",
                    WarrantyFlag = "NULL"
                });
                #endregion
                
                //Error handle after generate report
                #region

                if (item.Status=="C" && String.IsNullOrEmpty(item.WorkCode1))
                {
                    ErrorMessage += "\n\nFound ErrorCodes[1]\nWorkCode cannot be empty when status is C, see RefNumber: " + item.RefNumber +"\n";
                }
                if (!String.IsNullOrEmpty(item.FutureTelOTC)&& !String.IsNullOrEmpty(item.FutureTelOTC))
                {
                    ErrorMessage += "\n\nFound ErrorCodes[2]\nBoth ReplacementESN AND MftOTC fields cannot be populate on the same record, see RefNumber: " + item.RefNumber + "\n";
                }
                if (item.Status == "C" && (String.IsNullOrEmpty(item.Complaint1)&& String.IsNullOrEmpty(item.Complaint2)&& String.IsNullOrEmpty(item.Complaint3)))
                {
                    ErrorMessage += "\n\nFound ErrorCodes[3]\nComplaint codes cannot be empty when status is C, see RefNumber: " + item.RefNumber + "\n";
                }
                if (!String.IsNullOrEmpty(item.FuturetelMSN) && !String.IsNullOrEmpty(item.MftrMSN))
                {
                    ErrorMessage += "\n\nFound ErrorCode[4]\nReplacementMSN AND MftMSN cannot be empty when status is C, see RefNumber: " + item.RefNumber + "\n";
                }
                if (String.IsNullOrEmpty(item.ProductCode))
                {
                    ErrorMessage += "\n\nFound ErrorCode[5]\nProductCode cannot be empty, see RefNumber: " + item.RefNumber + "\n";
                }
                if (item.Status == "C" && String.IsNullOrEmpty(item.FaultCode))
                {
                    ErrorMessage += "\n\nFound ErrorCode[7]\nFaultCode cannot be empty when status is C, see RefNumber: " + item.RefNumber + "\n";
                }
                //assign error message
                if (String.IsNullOrEmpty(this.ErrorMessage))
                {
                    errorMessage = null;
                }
                else
                {
                    string errorFileName = Connection.Instance.GetFileName();
                    string msg = "\nError has been found on these records!\nPlease check the original file:\n" + errorFileName;
                    msg += "\n\nNOTE: Avoid sending the error file to client!";
                    errorMessage = msg + ErrorMessage;
                }
                

                #endregion

                
            }

            //convert ObservableCollection to DataTable using FastMember reference
            using (var reader = ObjectReader.Create(CatDataModel))
            {
                CatDataTable.Load(reader);
            }

            //order the datatable's columns and set column names
            #region order the datatable's columns and set column names
            //CatDataTable.Columns["RefNumber"].SetOrdinal(0);
            CatDataTable.Columns["AccidentDate"].ColumnName = "Accident Date";
            CatDataTable.Columns["ActionCode"].ColumnName = "Action Code";
            CatDataTable.Columns["ActionReasonCode"].ColumnName = "Action Reason Code";
            CatDataTable.Columns["AWBIn"].ColumnName = "AWB In";
            CatDataTable.Columns["AWBOut"].ColumnName = "AWB Out";
            CatDataTable.Columns["ClaimID"].ColumnName = "Claim ID";
            CatDataTable.Columns["CourierIn"].ColumnName = "Courier In";
            CatDataTable.Columns["CourierOut"].ColumnName = "Courier Out";
            CatDataTable.Columns["CustomerComplaintCoddPrimary"].ColumnName = "Customer Complaint Code - Primary";
            CatDataTable.Columns["DateIn"].ColumnName = "Date In";
            CatDataTable.Columns["DateOut"].ColumnName = "Date Out";
            CatDataTable.Columns["DeliveryDate"].ColumnName = "Delivery Date";
            CatDataTable.Columns["DeliveryNumber"].ColumnName = "Delivery Number";
            CatDataTable.Columns["FaultCode"].ColumnName = "Fault Code";
            CatDataTable.Columns["FieldBulletinNumber"].ColumnName = "Field Bulletin Number";
            CatDataTable.Columns["IMEI2In"].ColumnName = "IMEI 2 In";
            CatDataTable.Columns["IMEI2Out"].ColumnName = "IMEI 2 Out";
            CatDataTable.Columns["IMEINumberIn"].ColumnName = "IMEI Number In";
            CatDataTable.Columns["IMEINumberOut"].ColumnName = "IMEI Number Out";
            CatDataTable.Columns["InboundShipmentType"].ColumnName = "Inbound Shipment Type";
            CatDataTable.Columns["ItemCodeOut"].ColumnName = "Item code out";
            CatDataTable.Columns["JobCreationDate"].ColumnName = "Job Creation Date";
            //CatDataTable.Columns["Mandatory"].ColumnName = "Mandatory";
            CatDataTable.Columns["ManufactureDate"].ColumnName = "Manufacture date";
            CatDataTable.Columns["MaterialNumber"].ColumnName = "Material Number";
            //CatDataTable.Columns["OEM"].ColumnName = "OEM";
            CatDataTable.Columns["OutboundShipmentType"].ColumnName = "Outbound Shipment Type";
            //CatDataTable.Columns["Payer"].ColumnName = "Payer";
            CatDataTable.Columns["PickupArrangedDate"].ColumnName = "Pickup Arranged Date";
            CatDataTable.Columns["PickupDate"].ColumnName = "Pickup Date";
            CatDataTable.Columns["POPDate"].ColumnName = "POP date";
            CatDataTable.Columns["POPSupplier"].ColumnName = "POP supplier";
            CatDataTable.Columns["ProblemFoundCode"].ColumnName = "Problem Found Code";
            CatDataTable.Columns["ProductCodeIn"].ColumnName = "Product Code In";
            CatDataTable.Columns["ProductCodeOut"].ColumnName = "Product Code Out";
            CatDataTable.Columns["ProductType"].ColumnName = "Product Type";
            CatDataTable.Columns["ProductVersionIn"].ColumnName = "Product version in";
            CatDataTable.Columns["ProductVersionOut"].ColumnName = "Product version out";
            CatDataTable.Columns["Project"].ColumnName = "Project";
            CatDataTable.Columns["ProviderCarrier"].ColumnName = @"Provider/Carrier";
            CatDataTable.Columns["QuantityReplaced"].ColumnName = "Quantity Replaced";
            CatDataTable.Columns["QuotationEndDate"].ColumnName = "Quotation End Date";
            CatDataTable.Columns["QuotationStartDate"].ColumnName = "Quotation Start Date";
            CatDataTable.Columns["ReferenceDesignatorNumber"].ColumnName = "Reference designator number";
            CatDataTable.Columns["RepairServicePartnerID"].ColumnName = "Repair Service Partner ID";
            CatDataTable.Columns["RepairStatusCode"].ColumnName = "Repair Status Code";
            CatDataTable.Columns["RepairTimeStamp"].ColumnName = "Repair Time Stamp";
            CatDataTable.Columns["ReportDate"].ColumnName = "Report date";
            CatDataTable.Columns["ReturnDate"].ColumnName = "Return Date";
            CatDataTable.Columns["SecondStatus"].ColumnName = "Second Status";
            CatDataTable.Columns["SerialNumberIn"].ColumnName = "Serial Number In";
            CatDataTable.Columns["SerialNumberOut"].ColumnName = "Serial Number Out";
            CatDataTable.Columns["ShopID"].ColumnName = "Shop ID";
            CatDataTable.Columns["SoftwareIn"].ColumnName = "Software In";
            CatDataTable.Columns["SoftwareOut"].ColumnName = "Software Out";
            CatDataTable.Columns["SpecialProjectNumber"].ColumnName = "Special project number";
            CatDataTable.Columns["TechnicianID"].ColumnName = "Technician ID";
            CatDataTable.Columns["TransactionCode"].ColumnName = "Transaction Code";
            CatDataTable.Columns["WarrantyFlag"].ColumnName = "Warranty Flag";
            #endregion
            

            return CatDataTable;
        }


    }
}
