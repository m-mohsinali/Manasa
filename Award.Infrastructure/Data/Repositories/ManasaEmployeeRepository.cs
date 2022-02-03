using Award.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Linq;
using Award.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Award.Infrastructure.Data.Repositories

{

    public class ManasaEmployeeRepository : IManasaEmployeeRepository
    {
        private string _connectionString => _configuration.GetConnectionString("IntegrationDbContextConnection");
        private string _connectionStringAward => _configuration.GetConnectionString("AwardDbContextConnection");

        //private string serverName = "http://qastg.dnrd.gov.ae";

        private readonly IConfiguration _configuration;
        private readonly AwardDbContext _context;

        public ManasaEmployeeRepository(IConfiguration configuration, AwardDbContext context)

        {
            this._context = context;

            _configuration = configuration;

        }
        /// <summary>
        /// Function for bulk update from View to our dataBase
        /// </summary>
        /// <returns></returns>
        public List<ManasaEmployee> UpdateBulkData (string logfilepath)
        {
            List<ManasaEmployee> employees = new List<ManasaEmployee>();
            try
            {
                System.IO.File.AppendAllText(logfilepath, DateTime.Now + ": Connecting Inegration Database" + Environment.NewLine);                
                using (var conn = new SqlConnection(_connectionString))
                {
                    string sql = "SELECT * FROM vwManasaEmployees";
                    employees = conn.Query<ManasaEmployee>(sql).ToList();
                    System.IO.File.AppendAllText(logfilepath, DateTime.Now + ": Loading Data from View" + Environment.NewLine);
                    foreach (var employee in employees)
                    {
                        System.IO.File.AppendAllText(logfilepath, DateTime.Now + ": Inserting Data for :" + employee.UserDomain + Environment.NewLine);
                        InsertUsersFromManasaView(employee, "");
                    }
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logfilepath, DateTime.Now + ": Scheduler Exception :" + ex.Message + Environment.NewLine);
                throw;
            }
            
            return employees;
        }

        public ManasaEmployee GetByUsername(string username, string email = "")

        {

            ManasaEmployee employee = null;
            using (var conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT TOP 1 * FROM vwManasaEmployees WHERE UserDomain = @username";
                var employeesList = conn.Query<ManasaEmployee>(sql, new { username });
                employee = employeesList != null && employeesList.Count() > 0 ? employeesList.First() : null;
            }

            if (_configuration["DataInsertRequired"] != "false")

            {

                if (employee != null)

                    InsertUsersFromManasaView(employee, email);

            }





            return employee;

        }




        /// <summary>
        /// Update by Mohsin on 03/02/2022
        /// </summary>       
        public void InsertUsersFromManasaView(ManasaEmployee data, string Email)
        {
            try
            {
                data.EmployeePhotoURL = data.EmployeePhotoURL.Replace("/**Server**", _configuration["ServerName"]);
                using (var conn = new SqlConnection(_connectionStringAward))
                using (var cmd = new SqlCommand("Award.InsertTestDataNew", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();
                    cmd.Parameters.Add(new SqlParameter("@Email", Email));
                    cmd.Parameters.Add(new SqlParameter("@NameAr", data.NameAR));
                    cmd.Parameters.Add(new SqlParameter("@NameEn", data.NameEN));
                    cmd.Parameters.Add(new SqlParameter("@SexEn", data.SexEN));
                    cmd.Parameters.Add(new SqlParameter("@SexAr", data.SexAR));
                    cmd.Parameters.Add(new SqlParameter("@Grp", data.GRP == null ? 0 : data.GRP));
                    cmd.Parameters.Add(new SqlParameter("@UserDomain", data.UserDomain));
                    cmd.Parameters.Add(new SqlParameter("@RankAr", data.RankAR));
                    cmd.Parameters.Add(new SqlParameter("@RankEn", data.RankEN));
                    cmd.Parameters.Add(new SqlParameter("@JobAr", data.JobTitleAR == null ? "No Job Found" : data.JobTitleAR));
                    cmd.Parameters.Add(new SqlParameter("@JobEn", data.JobTitleEN == null ? "No Job Found" : data.JobTitleEN));
                    cmd.Parameters.Add(new SqlParameter("@ClassAr", data.ClassAR));
                    cmd.Parameters.Add(new SqlParameter("@ClassEn", data.ClassEN));
                    cmd.Parameters.Add(new SqlParameter("@EmployeePhotoUrl", data.EmployeePhotoURL));
                    cmd.Parameters.Add(new SqlParameter("@SectorNameEN", data.SectorNameEN == null || data.SectorNameEN == "" ? "No Sector Name Found" : data.SectorNameEN));
                    cmd.Parameters.Add(new SqlParameter("@SectorNameAr", data.SectorNameAR == null || data.SectorNameAR == "" ? "No Sector Name Found" : data.SectorNameAR));
                    cmd.Parameters.Add(new SqlParameter("@BranchNameEN", data.BranchNameEN == null || data.BranchNameEN == "" ? "No Branch Name Name Found" : data.BranchNameEN));
                    cmd.Parameters.Add(new SqlParameter("@BranchNameAr", data.BranchNameAR == null || data.BranchNameAR == "" ? "No Branch Name Found" : data.BranchNameAR));
                    cmd.Parameters.Add(new SqlParameter("@DepartmentNameEN", data.DeptNameEN == null || data.DeptNameEN == "" ? "No Dept Name Found" : data.DeptNameEN));
                    cmd.Parameters.Add(new SqlParameter("@DepartmentNameAr", data.DeptNameAR == null || data.DeptNameAR == "" ? "No Dept Name Found" : data.DeptNameAR));
                    cmd.Parameters.Add(new SqlParameter("@SectionNameEN", data.SectionNameEN == null || data.SectionNameEN == "" ? "No Section Name Found" : data.SectionNameEN));
                    cmd.Parameters.Add(new SqlParameter("@SectionNameAr", data.SectionNameAR == null || data.SectionNameAR == "" ? "No Section Name Found" : data.SectionNameAR));
                    cmd.Parameters.Add(new SqlParameter("@UnitsNameEN", data.UnitNameEN == null || data.UnitNameEN == "" ? "No Unit Name Found" : data.UnitNameEN));
                    cmd.Parameters.Add(new SqlParameter("@UnitsNameAr", data.UnitNameAR == null || data.UnitNameAR == "" ? "No Unit Name Found" : data.UnitNameAR));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public class UserDetailWithEmployee

        {

            public string NameAr { get; set; }

            public string NameEn { get; set; }

            public string SexEn { get; set; }

            public string SexAr { get; set; }

            public string Grp { get; set; }

            public string UserDomain { get; set; }

            public string RankAr { get; set; }

            public string RankEn { get; set; }

            public string JobAr { get; set; }

            public string JobEn { get; set; }

            public string ClassAr { get; set; }

            public string ClassEn { get; set; }

            public string EmployeePhotoUrl { get; set; }

            public string Sectorid { get; set; }

            public string Branchid { get; set; }

            public string DepartmentId { get; set; }

            public string SectionId { get; set; }

            public string UnitId { get; set; }



        }



        //for testing only

        public Awards GetAward(string username)

        {

            Awards award = null;



            using (var conn = new SqlConnection(_connectionString))

            {

                string sql = "SELECT TOP 1 * FROM Awards";

                var awardsList = conn.Query<Awards>(sql);

                award = awardsList != null && awardsList.Count() > 0 ? awardsList.First() : null;

            }



            return award;

        }



        public List<ReportGenericData> GetReportGenericData()
        {
            try
            {

                using (var conn = new SqlConnection(_connectionString))
                {
                    string sql = "EXEC [Sp_GetGenericDataForReports]";
                    var reportdata = conn.Query<ReportGenericData>(sql);
                    return reportdata.ToList();

                }
            }
            catch (Exception ex)

            {

                throw ex;
            }



        }


    }

}