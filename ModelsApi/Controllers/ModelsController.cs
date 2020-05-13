using System;
using System.Collections.Generic;
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
    public class ModelsController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly GenericRepository<EfModel> _modelRepository;
        private readonly GenericRepository<EfAccount> _accountRepository;

        public ModelsController(ApplicationDbContext context,
            IOptions<AppSettings> appSettings,
            IMapper mapper)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (appSettings == null) throw new ArgumentNullException(nameof(appSettings));
            _appSettings = appSettings.Value;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _modelRepository = new GenericRepository<EfModel>(context);
            _accountRepository = new GenericRepository<EfAccount>(context);
        }

        // GET: api/Models
        [HttpGet]
        public ActionResult<IEnumerable<Model>> GetModels()
        {
            var models = _modelRepository.GetBy(source => source,
                predicate: m => true);
            return _mapper.Map<List<Model>>(models);
        }

        // GET: api/Models/5
        [HttpGet("{id}")]
        public ActionResult<Model> GetModel(long id)
        {
            var model = _modelRepository.GetBy(source => source,
                predicate: m => m.EfModelId == id).FirstOrDefault();

            if (model == null)
            {
                return NotFound();
            }

            return _mapper.Map<Model>(model);
        }

        // PUT: api/Models/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModel(long id, EfModel model)
        {

            if (id != model.EfModelId)
            {
                return BadRequest();
            }

            var dbModel = _modelRepository.GetBy(source => source,
                predicate: m => m.EfModelId == id,
                disableTracking: false).FirstOrDefault();

            dbModel.Email = model.Email;
            dbModel.AddresLine1 = model.AddresLine1;
            dbModel.AddresLine2 = model.AddresLine2;
            dbModel.BirthDate = model.BirthDate;
            dbModel.City = model.City;
            dbModel.Comments = model.Comments;
            dbModel.EyeColor = model.EyeColor;
            dbModel.Country = model.Country;
            dbModel.FirstName = model.FirstName;
            dbModel.HairColor = model.HairColor;
            dbModel.Height = model.Height;
            dbModel.LastName = model.LastName;
            dbModel.PhoneNo = model.PhoneNo;
            dbModel.Nationality = model.Nationality;
            dbModel.ShoeSize = model.ShoeSize;
            dbModel.Zip = model.Zip;

            _modelRepository.Update(dbModel);

            return Ok();
        }

        // POST: api/Models
        [HttpPost]
        public async Task<ActionResult<Model>> PostModel(ModelDetails modelDto)
        {
            modelDto.Email = modelDto.Email.ToLower();
            var emailExist = _modelRepository.GetBy(selector: source => source,
                predicate: u => u.Email == modelDto.Email).FirstOrDefault();
            if (emailExist != null)
            {
                ModelState.AddModelError("Email", "Email already in use");
                return BadRequest(ModelState);
            }

            var model = _mapper.Map<EfModel>(modelDto);
            
            var account = new EfAccount()
            {
                Email = model.Email,
                IsManager = false,
                PwHash = HashPassword(modelDto.Password, _appSettings.BcryptWorkfactor)
            };

            _accountRepository.Create(account);
            
            model.Account = _accountRepository.GetBy(source => source, a => a.Email == account.Email, disableTracking: false).FirstOrDefault();

            _modelRepository.Create(model);

            return _mapper.Map<Model>(model);
        }

        // DELETE: api/Models/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteModel(long id)
        {
            var model = _modelRepository.GetBy(source => source, m => m.EfModelId == id, disableTracking: false).FirstOrDefault();
            if (model == null)
            {
                return NotFound();
            }

            _modelRepository.Delete(model);

            return Ok();
        }
    }
}
