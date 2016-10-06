using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIPReportCat.EmailService
{
    public class EmailMessage
    {
        
        public string GetErrorMessage(string []errorCodes, string refNumbers)
        {

            string msg = "Errors:\n\n";
            
            if (errorCodes[1] == "1")
            {
                return msg += "WorkCode cannot be empty when status is C, see refNumbers: \n" + refNumbers;
            }
            if (errorCodes[2] == "2")
            {
                return msg += "Both ReplacementESN AND MftOTC fields cannot be populate on the same record, see refNumbers: \n" + refNumbers;
            }
            if (errorCodes[3] == "3")
            {
                return msg += "Complaint codes cannot be empty when status is C, see refNumbers: \n" + refNumbers;
            }
            if (errorCodes[4] == "4")
            {
                return msg += "ReplacementMSN AND MftMSN cannot be empty when status is C, see refNumbers: \n" + refNumbers;
            }
            if (errorCodes[5] == "5")
            {
                return msg += "ProductCode cannot be empty, see refNumbers: \n" + refNumbers;
            }
            if (errorCodes[6] == "6")
            {
                return msg += "FaultCode cannot be empty when status is C, see refNumbers: \n" + refNumbers;
            }
            if (errorCodes[0] == "0")
            {
                msg += null;
            }
            return msg;
        }
    }
}
