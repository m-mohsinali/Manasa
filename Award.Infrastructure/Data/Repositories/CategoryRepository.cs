using Award.Core.Common;
using Award.Core.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Award.Infrastructure.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private string _connectionString => _configuration.GetConnectionString("IntegrationDbContextConnection");
        private readonly IConfiguration _configuration;
        public CategoryRepository(IConfiguration configuration)
        { 
            _configuration = configuration;
        }
        public DataTable GetCategoryCount()
        {

            DataTable dt = new DataTable ();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Query.GetCategoryCountAwardWise))
                {
                    cmd.Connection = conn;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            return dt;
        }


        public DataTable GetSectorsCount()
        {

            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Query.GetSectorCount))
                {
                    cmd.Connection = conn;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            return dt;
        }
        public DataTable GetSubmissionFromQSM()
        {

            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Query.GetSubmissionFromQSM))
                {
                    cmd.Connection = conn;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            return dt;
        }
        public DataTable GetSubmissionFromJury()
        {

            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Query.GetSubmissionFromJury))
                {
                    cmd.Connection = conn;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            return dt;
        }
        public DataTable GetAuditManagerSubmission()
        {

            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Query.GetAuditManagerSubmission))
                {
                    cmd.Connection = conn;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            return dt;
        }
        
    }
}
