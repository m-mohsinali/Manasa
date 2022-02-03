using Award.Core.Entities;
using System.Collections.Generic;

namespace Award.Core.Interfaces
{
    public interface IManasaEmployeeRepository
    {
        Awards GetAward(string username);
        ManasaEmployee GetByUsername(string username, string email="");
        List<ReportGenericData> GetReportGenericData();
        List<ManasaEmployee> UpdateBulkData(string logfilepath);
    }
}