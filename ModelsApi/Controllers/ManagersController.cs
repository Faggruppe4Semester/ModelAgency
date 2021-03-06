﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ModelsApi.Data;
using ModelsApi.Models.DTOs;
using ModelsApi.Models.Entities;
using ModelsApi.Repositories.Implementation;
using ModelsApi.Utilities;
using static BCrypt.Net.BCrypt;

namespace ModelsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class ManagersController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly GenericRepository<EfManager> _managerRepository;
        private readonly GenericRepository<EfAccount> _accountRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ManagersController(ApplicationDbContext context,
            IOptions<AppSettings> appSettings, IMapper mapper)
        {
            if (appSettings == null) throw new ArgumentNullException(nameof(appSettings));
            _context = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _appSettings = appSettings.Value;
            _managerRepository = new GenericRepository<EfManager>(context);
            _accountRepository = new GenericRepository<EfAccount>(context);
        }

        // GET: api/Managers
        [HttpGet]
        public ActionResult<IEnumerable<EfManager>> GetManagers()
        {
            return _managerRepository.GetBy(selector: source => source,
                predicate: m => true);
        }

        // GET: api/Managers/5
        [HttpGet("{id}")]
        public ActionResult<Manager> GetManager(long id)
        {
            var manager = _managerRepository.GetBy(selector: source => source,
                predicate: m => m.EfManagerId == id).FirstOrDefault();
            return manager == null ? (ActionResult<Manager>) NotFound() : _mapper.Map<Manager>(manager);
        }

        // PUT: api/Managers/5
        [HttpPut("{id}")]
        public IActionResult PutManager(long id, EfManager manager)
        {
            if (manager == null)
            {
                return BadRequest($"Input parameter {typeof(EfManager)} {nameof(manager)} was null.");
            }
            if (id != manager.EfManagerId)
            {
                return BadRequest("Id Mismatch");
            }

            // Check if new email
            var old = _managerRepository.GetBy(selector: source => source,
                predicate: m => m.EfManagerId == id).FirstOrDefault();
            if (old == null) return BadRequest($"Manager not found with id {id}");
            
            // Update account
            var account = _accountRepository.GetBy(
                selector: source => source,
                predicate: a => a.EfAccountId == manager.EfAccountId, 
                disableTracking: false).FirstOrDefault();
            if (account == null) return BadRequest("Account of manager not found."); 
            
            account.Email = manager.Email;
            old.Email = manager.Email;
            old.FirstName = manager.FirstName;
            old.LastName = manager.LastName;

            _accountRepository.Update(account);
            _managerRepository.Update(old);

            return Ok();
        }

        // POST: api/Managers
        [HttpPost]
        public ActionResult<Manager> PostManager(Manager managerDto)
        {
            if (managerDto == null)
                return BadRequest("Data is missing");
            var manager = _mapper.Map<EfManager>(managerDto);
            manager.Email = manager.Email.ToLowerInvariant();

            var emailExist = _managerRepository.GetBy(selector: source => source,
                predicate: u => u.Email == manager.Email).FirstOrDefault();

            if (emailExist != null)
            {
                ModelState.AddModelError("Email", "Email already in use");
                return BadRequest(ModelState);
            }

            var account = new EfAccount()
            {
                Email = manager.Email,
                IsManager = true,
                PwHash = HashPassword(managerDto.Password, _appSettings.BcryptWorkfactor)
            };

            _accountRepository.Create(account);

            manager.Account = _accountRepository.GetBy(
                source => source, 
                predicate: a => a.Email == account.Email, 
                disableTracking: false)
                .FirstOrDefault();

            _managerRepository.Create(manager);

            return _mapper.Map<Manager>(manager);
        }

        // DELETE: api/Managers/5
        [HttpDelete("{id}")]
        public IActionResult DeleteManager(long id)
        {
            var manager = _managerRepository.GetBy(source => source,
                predicate: m => m.EfManagerId == id,
                disableTracking: false).FirstOrDefault();
            if (manager == null)
            {
                return NotFound();
            }
            var account = _accountRepository.GetBy(selector: source => source,
                predicate: a => a.EfAccountId == manager.EfAccountId,
                disableTracking: false).FirstOrDefault();

            _accountRepository.Delete(account);

            return Ok();
        }
    }
}
