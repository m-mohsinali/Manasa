using Award.Core.Entities;
using Award.Core.Interfaces;
using Award.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;



namespace Award.Infrastructure.Services

{

    public class EmployeeService : IEmployeeService

    {

        private readonly AwardDbContext _dbContext;

        private readonly IConfiguration _configuration;



        public EmployeeService(AwardDbContext dbContext, IConfiguration configuration)

        {

            _dbContext = dbContext;

            _configuration = configuration;

        }





        private string getPhotoUrl(ManasaEmployee manasaEmployee)

        {

            try

            {

                return string.IsNullOrEmpty(manasaEmployee.EmployeePhotoURL)

                    ? string.Empty

                    : $"{manasaEmployee.EmployeePhotoURL.Replace("/**Server**/", _configuration["ServerName"])}";

            }

            catch (Exception ex)

            {

                //_logger.LogCritical($"{nameof(getPhotoUrl)} throws some exception");

                //_logger.LogError(ex, ex.InnerException?.Message);

            }

            return manasaEmployee.EmployeePhotoURL.Replace("/**Server**/", _configuration["ServerName"]);

        }



        public async Task<Employee> CreateEmployee(ManasaEmployee manasaEmployee)

        {

            var sector = await getSectorAsync(manasaEmployee).ConfigureAwait(false);

            sector = addSector(manasaEmployee, sector);



            var department = await getOrAddDepartmentAsync(manasaEmployee).ConfigureAwait(false);





            var branch = await getorAddBranchAsync(manasaEmployee).ConfigureAwait(false);

            var unit = await getOrAddUnitAsync(manasaEmployee).ConfigureAwait(false);

            var section = await getOrAddSectionAsync(manasaEmployee).ConfigureAwait(false);



            return new Employee

            {

                UserDomain = manasaEmployee.UserDomain,

                Grp = manasaEmployee.GRP.HasValue ? manasaEmployee.GRP.Value.ToString() : string.Empty,



                RankEn = manasaEmployee.RankEN,

                RankAr = manasaEmployee.RankAR,



                NameAr = manasaEmployee.NameAR,

                NameEn = manasaEmployee.NameEN,



                LastUpdateAt = manasaEmployee.LastUpdaeDate,



                JobAr = manasaEmployee.JobTitleAR,

                JobEn = manasaEmployee.JobTitleEN,



                Sector = sector,

                Department = department,

                Branch = branch,

                Unit = unit,

                Section = section,



                ClassAr = manasaEmployee.ClassAR,

                ClassEn = manasaEmployee.ClassEN,



                SexAr = manasaEmployee.SexAR,

                SexEn = manasaEmployee.SexEN,

                EmployeePhotoUrl = getPhotoUrl(manasaEmployee),

            };

        }



        private Sector addSector(ManasaEmployee manasaEmployee, Sector sector)

        {

            if (sector != null)

            {

                return sector;

            }



            //if (string.IsNullOrEmpty(manasaEmployee.SectorNameAR) &&

            //         string.IsNullOrEmpty(manasaEmployee.SectorNameEN))



            //{

            //    return null;

            //}



            sector = new Sector { NameAr = string.IsNullOrEmpty(manasaEmployee.SectorNameAR) ? "No Sector Found" : manasaEmployee.SectorNameAR, NameEn = string.IsNullOrEmpty(manasaEmployee.SectorNameEN) ? "No Sector Found" : manasaEmployee.SectorNameEN, CreateDate = DateTime.Now };

            _dbContext.Add(sector);

            return sector;

        }



        private async Task<Section> getOrAddSectionAsync(ManasaEmployee manasaEmployee)

        {

            //if (string.IsNullOrEmpty(manasaEmployee.SectionNameAR) &&

            //    string.IsNullOrEmpty(manasaEmployee.SectionNameEN))

            //{

            //    return null;

            //}



            var section = await _dbContext.Sections.FirstOrDefaultAsync(s =>

                string.Equals(s.NameAr, manasaEmployee.SectionNameAR) ||

                string.Equals(s.NameEn, manasaEmployee.SectionNameEN));

            if (section != null)

            {

                return section;

            }

            section = new Section { NameEn = string.IsNullOrEmpty(manasaEmployee.SectionNameEN) ? "No Section Found" : manasaEmployee.SectionNameEN, NameAr = string.IsNullOrEmpty(manasaEmployee.SectionNameEN) ? "No Section Found" : manasaEmployee.SectionNameAR, CreateDate = DateTime.Now };

            _dbContext.Add(section);

            return section;

        }



        private async Task<Unit> getOrAddUnitAsync(ManasaEmployee manasaEmployee)

        {

            //if (string.IsNullOrEmpty(manasaEmployee.UnitNameEN) && string.IsNullOrEmpty(manasaEmployee.UnitNameAR))

            //{

            //    return null;

            //}



            var unit = await _dbContext.Units.FirstOrDefaultAsync(u =>

                string.Equals(u.NameAr, manasaEmployee.UnitNameAR) ||

                string.Equals(u.NameEn, manasaEmployee.UnitNameEN));

            if (unit != null)

            {

                return unit;

            }

            unit = new Unit { NameEn = string.IsNullOrEmpty(manasaEmployee.UnitNameEN) ? "No Unit found" : manasaEmployee.UnitNameEN, NameAr = string.IsNullOrEmpty(manasaEmployee.UnitNameAR) ? "No unit found" : manasaEmployee.UnitNameAR, CreateDate = DateTime.Now };

            _dbContext.Add(unit);

            return unit;



        }



        private async Task<Branch> getorAddBranchAsync(ManasaEmployee manasaEmployee)

        {

            //if (string.IsNullOrEmpty(manasaEmployee.BranchNameAR) && string.IsNullOrEmpty(manasaEmployee.BranchNameEN))

            //{

            //    return null;

            //}



            var branch = await _dbContext.Branches.FirstOrDefaultAsync(d =>

                string.Equals(d.NameEn, manasaEmployee.BranchNameEN) ||

                string.Equals(d.NameAr, manasaEmployee.BranchNameAR));



            if (branch != null)

            {

                return branch;

            }

            branch = new Branch { NameEn = string.IsNullOrEmpty(manasaEmployee.BranchNameEN) ? "No Branch Found" : manasaEmployee.BranchNameEN, NameAr = string.IsNullOrEmpty(manasaEmployee.BranchNameAR) ? "No Branch Found" : manasaEmployee.BranchNameAR, CreateDate = DateTime.Now };

            _dbContext.Add(branch);

            return branch;

        }



        private async Task<Department> getOrAddDepartmentAsync(ManasaEmployee manasaEmployee)

        {

            if (string.IsNullOrEmpty(manasaEmployee.DeptNameEN) && string.IsNullOrEmpty(manasaEmployee.DeptNameAR))

            {

                return null;

            }

            var department = await _dbContext.Departments.FirstOrDefaultAsync(d =>

                string.Equals(d.NameEn, manasaEmployee.DeptNameEN) ||

                string.Equals(d.NameAr, manasaEmployee.DeptNameAR)).ConfigureAwait(false);



            if (department != null)

            {

                return department;

            }

            department = new Department { NameEn = string.IsNullOrEmpty(manasaEmployee.DeptNameEN) ? "No Department found" : manasaEmployee.DeptNameEN, NameAr = string.IsNullOrEmpty(manasaEmployee.DeptNameAR) ? "No Department found" : manasaEmployee.DeptNameAR, CreateDate = DateTime.Now };

            _dbContext.Add(department);

            return department;

        }



        private async Task<Sector> getSectorAsync(ManasaEmployee manasaEmployee)

        {

            if (string.IsNullOrEmpty(manasaEmployee.SectorNameAR) && string.IsNullOrEmpty(manasaEmployee.SectorNameEN))

            {

                return null;

            }



            var sector = await _dbContext.Sectors.FirstOrDefaultAsync(s =>

                string.Equals(s.NameAr, manasaEmployee.SectorNameAR) ||

                string.Equals(s.NameEn, manasaEmployee.SectorNameEN)).ConfigureAwait(false);

            return sector;

        }

    }

}