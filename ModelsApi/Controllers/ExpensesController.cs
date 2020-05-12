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
using ModelsApi.Services;

namespace ModelsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpensesService _expensesService;

        public ExpensesController(ApplicationDbContext context,
            IMapper mapper)
        {
            _expensesService = new ExpensesService(context, mapper);
        }

        // GET: api/Expenses
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<NewExpense>>> GetExpenses()
        {
            return await _expensesService.GetAll().ConfigureAwait(false);
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public ActionResult<NewExpense> GetExpense(long id)
        {
            try
            {
                return _expensesService.GetBy(e => e.EfExpenseId == id).FirstOrDefault();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("model/{modelId}")]
        public ActionResult<List<NewExpense>> GetExpenseForModel(long modelId)
        {
            return _expensesService.GetBy(e => e.ModelId == modelId);
        }

        // PUT: api/Expenses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(long id, NewExpense expense)
        {
            try
            {
                await _expensesService.Update(id, expense).ConfigureAwait(false);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e);
            }

            return Ok();
        }

        // POST: api/Expenses
        /// <summary>
        /// Create a new expense for a model.
        /// </summary>
        /// <param name="newExpense"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<NewExpense>> PostExpense(NewExpense newExpense)
        {
            try
            {
                return await _expensesService.Create(newExpense).ConfigureAwait(false);
            }
            catch (ArgumentNullException nullException)
            {
                return BadRequest(nullException);
            }
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpense(long id)
        {
            try
            {
                await _expensesService.Delete(id).ConfigureAwait(false);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e);
            }
        }
    }
}
