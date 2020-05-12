using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ModelsApi.Data;
using ModelsApi.Models.DTOs;
using ModelsApi.Models.Entities;
using ModelsApi.Services.Interfaces;

namespace ModelsApi.Services
{
    public class ExpensesService : IModelService<NewExpense, EfExpense>, IPublicService<NewExpense, EfExpense>
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public ExpensesService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<EfExpense> GetModels(Expression<Func<EfExpense, bool>> predicate, bool disableTracking = true)
        {
            throw new NotImplementedException();
        }

        public async Task<List<NewExpense>> GetAll()
        {
            var allExpenses = await _context.Expenses.ToListAsync().ConfigureAwait(false);
            return _mapper.Map<List<NewExpense>>(allExpenses);
        }

        public List<NewExpense> GetBy(Expression<Func<EfExpense, bool>> predicate)
        {
            var expenses = _context.Expenses.Where(predicate).ToList();
            return _mapper.Map<List<NewExpense>>(expenses);
        }

        public async Task<NewExpense> Create(NewExpense dto)
        {
            if (dto is null)
                throw new ArgumentNullException($"{(NewExpense) null}");

            var expense = _mapper.Map<EfExpense>(dto);
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            var newModel = GetBy(e => e.EfExpenseId == expense.EfExpenseId).FirstOrDefault();
            return _mapper.Map<NewExpense>(newModel);
        }

        public async Task Update(long id, NewExpense dto)
        {
            if (dto is null)
                throw new ArgumentNullException($"{typeof(NewExpense)} in ExpensesService.Update is null.");

            if (id != dto.EfExpenseId)
                throw new ArgumentException($"Id mismatch between {typeof(long)} {id} and {typeof(NewExpense)} {dto}");

            _context.Entry(dto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (GetBy(e => e.EfExpenseId == id).FirstOrDefault() == null)
                    throw new ArgumentException($"ExpenseId: {id} not found.");
                throw;
            }
        }

        public async Task Delete(long id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) throw new ArgumentException($"Expense with ID: {id} wasn't found in the database.");

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}