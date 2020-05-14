using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsApi.Data;
using ModelsApi.Models.DTOs;
using ModelsApi.Models.Entities;
using ModelsApi.Repositories.Implementation;

namespace ModelsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly GenericRepository<EfExpense> _expensesRepository;

        public ExpensesController(ApplicationDbContext context,
            IMapper mapper)
        {
            _mapper = mapper;
            _expensesRepository = new GenericRepository<EfExpense>(context);
        }

        // GET: api/Expenses
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult<IEnumerable<EfExpense>> GetExpenses()
        {
            return _expensesRepository.GetBy(source => source,
                predicate: e => true);
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public ActionResult<EfExpense> GetExpense(long id)
        {
            var expense = _expensesRepository.GetBy(
                selector: source => source,
                predicate: e => e.EfExpenseId == id)
                .FirstOrDefault();

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        [HttpGet("model/{modelId}")]
        public ActionResult<IEnumerable<EfExpense>> GetExpenseForModel(long modelId)
        {            
            return _expensesRepository.GetBy(
                selector: source => source,
                predicate: e => e.ModelId == modelId);
        }

        [HttpGet("model/{modelId}/job/{jobId}")]
        public ActionResult<IEnumerable<EfExpense>> GetExpenseForModelAndJob(long modelId, long jobId)
        {
            return _expensesRepository.GetBy(
                selector: source => source,
                predicate: e => e.ModelId == modelId && e.JobId == jobId);
        }

        // PUT: api/Expenses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutExpense(long id, EfExpense expense)
        {
            if (expense is null)
                return BadRequest("expense is null.");
            if (id != expense.EfExpenseId)
            {
                return BadRequest("Id mismatch.");
            }

            var dbExpense = _expensesRepository.GetBy(
                selector: source => source,
                predicate: e => e.EfExpenseId == id,
                disableTracking: false).FirstOrDefault();

            dbExpense.ModelId = expense.ModelId;
            dbExpense.Date = expense.Date;
            dbExpense.JobId = expense.JobId;
            dbExpense.Text = expense.Text;
            dbExpense.amount = expense.amount;

            _expensesRepository.Update(dbExpense);

            return Ok();
        }

        // POST: api/Expenses
        /// <summary>
        /// Create a new expense for a model.
        /// </summary>
        /// <param name="newExpense"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<EfExpense> PostExpense(NewExpense newExpense)
        {
            if (newExpense is null)
            {
                return BadRequest("Data is missing");
            }

            var expense = _mapper.Map<EfExpense>(newExpense);
            _expensesRepository.Create(expense);

            return _expensesRepository.GetBy(selector: source => source,
                predicate: e => e.EfExpenseId == expense.EfExpenseId).FirstOrDefault();
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public ActionResult<EfExpense> DeleteExpense(long id)
        {
            var expense = _expensesRepository.GetBy(selector: source => source, predicate: e => e.EfExpenseId == id).FirstOrDefault();
            if (expense == null) return NotFound();

            _expensesRepository.Delete(expense);
            return expense;
        }
    }
}
