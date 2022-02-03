using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Award.Core.Entities;
using Award.Core.Interfaces;
using Award.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Award.Core.Common.Constants;

namespace Award.Web.Controllers
{
    [Authorize(Roles = "Administrator , SuperAdmin")]

    public class SectorController : Controller
    {
        private readonly IAsyncRepository<Sector> _sectorRepository;
        private readonly IAsyncRepository<QualitySectorManager> _qualitySectorManagerRepository;
        private readonly IUserService _userService;

        public SectorController(IAsyncRepository<Sector> sectorRepository, IAsyncRepository<QualitySectorManager> qualitySectorManagerRepository, IUserService userService)
        {
            _sectorRepository = sectorRepository;
            _qualitySectorManagerRepository = qualitySectorManagerRepository;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var sectorList = await _sectorRepository.ListAllAsync();

            return View(sectorList);
        }

        public async Task<IActionResult> AssingQualitySectorManager(long sectorId)
        {
            var sector = await _sectorRepository.GetByIdAsync(sectorId);

            if (sector == null)
            {
                return NotFound();
            }

            var userWithQSMRoleList = await _userService.GetAllUsersWithRoleNameandSectorIDAsync(Roles.QUALITY_SECTION_MANAGER, sectorId);
            var userWithQSMRoleListVM = userWithQSMRoleList.Select(user =>
                                                                    new TeamMemberViewModel
                                                                    {
                                                                        User = user,
                                                                        IsSelected = _qualitySectorManagerRepository
                                                                                     .QuerableData
                                                                                     .Any(a => a.SectorId == sector.Id && a.UserId == user.Id)
                                                                    });

            var sectorVM = new SectorViewModel(sector, userWithQSMRoleListVM.ToList());

            return View(sectorVM);
        }

        [HttpPost]
        public async Task<IActionResult> AssingQualitySectorManager(SectorViewModel sectorVM)
        {
            var sector = await _sectorRepository.GetByIdAsync(sectorVM.Sector.Id);

            if (sector == null)
            {
                return NotFound();
            }

            await _userService.AssingQualitySectorManager(sector.Id, sectorVM.QualitySectorManagerUsers.Where(a => a.IsSelected).Select(a => a.User).ToList());
            return RedirectToAction(nameof(Index));
        }
    }
}