using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsApi.Data;
using ModelsApi.Models.DTOs;
using ModelsApi.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelsApi.Repositories.Implementation;

namespace ModelsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly GenericRepository<EfJobModel> _jobModelRepository;
        private readonly GenericRepository<EfJob> _jobRepository;
        private readonly GenericRepository<EfModel> _modelRepository;

        public JobsController(ApplicationDbContext context,
            IMapper mapper)
        {
            _mapper = mapper;
            _jobModelRepository = new GenericRepository<EfJobModel>(context);
            _jobRepository = new GenericRepository<EfJob>(context);
            _modelRepository = new GenericRepository<EfModel>(context);
        }

        // GET: api/Jobs
        [HttpGet]
        public ActionResult<IEnumerable<Job>> GetJobs()
        {
            var modelStr = User.Claims.First(a => a.Type == "ModelId").Value;
            if (!long.TryParse(modelStr, out var modelId))
                return Unauthorized("ModelId missing");
            if (modelId < 0)
            {
                // Is manager
                var jobs = _jobRepository.GetBy(
                    selector: source => source,
                    predicate: ej => true,
                    include: ej => ej.Include(ej => ej.JobModels)
                        .ThenInclude(jm => jm.Model));
                return _mapper.Map<List<Job>>(jobs);
            }
            else
            {
                var JobModels = _jobModelRepository.GetBy(selector: source => source,
                    predicate: jm => jm.EfModelId == modelId,
                    include: iq => iq.Include(jm => jm.Job)
                        .Include(jm => jm.Model));
                return _mapper.Map<List<Job>>(JobModels.Select(jm => jm.Job));
            }
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public ActionResult<Job> GetJob(long id)
        {
            var job = _jobRepository.GetBy(
                selector: source => source,
                predicate: ej => ej.EfJobId == id,
                include: iq => iq
                    .Include(ej => ej.JobModels)
                        .ThenInclude(jm => jm.Model))
                .FirstOrDefault();

            if (job == null)
                return NotFound();

            return _mapper.Map<Job>(job);
        }

        // PUT: api/Jobs/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "Manager")]
        public IActionResult PutJob(long id, Job newJob)
        {
            if (newJob == null) throw new ArgumentNullException(nameof(newJob));
            var oldJob = _jobRepository.GetBy(
                selector: source => source,
                predicate: ej => ej.EfJobId == id,
                disableTracking: false).FirstOrDefault() ?? throw new ArgumentNullException($"{nameof(newJob)} not found when trying to update job.");

            oldJob.Comments = newJob.Comments;
            oldJob.Customer = newJob.Customer;
            oldJob.Days = newJob.Days;
            oldJob.Location = newJob.Location;
            oldJob.StartDate = newJob.StartDate;

            _jobRepository.Update(oldJob);

            return NoContent();
        }

        // POST: api/Jobs
        /// <summary>
        /// Create a new job
        /// </summary>
        /// <param name="newJob"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult<Job> PostJob(NewJob newJob)
        {
            var job = _mapper.Map<EfJob>(newJob);
            _jobRepository.Create(job);
            return _mapper.Map<Job>(job);
        }

        // POST: api/Jobs
        /// <summary>
        /// Add model to job.
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="modelId">modelId</param>
        /// <returns></returns>
        [HttpPost("{jobId}/model/{modelId}")]
        //[Authorize(Roles = "Manager")]
        public ActionResult<Job> AddModelToJob(long jobId, long modelId)
        {
            var job = _jobRepository.GetBy(selector: source => source, predicate: j => j.EfJobId == jobId, disableTracking: false).FirstOrDefault();
            if (job == null)
            {
                ModelState.AddModelError("jobId", "jobId not found");
                return BadRequest(ModelState);
            }

            var model = _modelRepository.GetBy(selector: source => source, m => m.EfModelId == modelId, disableTracking: false).FirstOrDefault();
            if (model == null)
            {
                ModelState.AddModelError("modelId", "modelId not found");
                return BadRequest(ModelState);
            }

            _jobModelRepository.Create(new EfJobModel
            {
                Job = job,
                Model = model
            });

            var updatedJob = _jobRepository.GetBy(
                selector: source => source,
                predicate: ej => ej.EfJobId == job.EfJobId,
                include: iq => iq.Include(ej => ej.JobModels)
                    .ThenInclude(jm => jm.Model)).FirstOrDefault();

            return _mapper.Map<Job>(updatedJob);
        }

        // DELETE: api/Jobs/1/model/3.
        /// <summary>
        /// Removes the model from the job.
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="modelId">ModelId</param>
        /// <returns></returns>
        [HttpDelete("{jobId}/model/{modelId}")]
        //[Authorize(Roles = "Manager")]
        public ActionResult<EfJob> RemoveModelFromJob(long jobId, long modelId)
        {
            var jobModel = _jobModelRepository.GetBy(
                selector: source => source,
                predicate: jm => jm.EfJobId == jobId && jm.EfModelId == modelId,
                disableTracking: false).FirstOrDefault();

            if (jobModel == null)
            {
                ModelState.AddModelError("jobId", "jobId or modelId not found");
                return BadRequest(ModelState);
            }

            _jobModelRepository.Delete(jobModel);
            return Ok();
        }

        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public ActionResult DeleteJob(long id)
        {
            var job = _jobRepository.GetBy(selector: source => source,
                predicate: ej => ej.EfJobId == id,
                disableTracking: false).FirstOrDefault();

            if (job == null)
            {
                ModelState.AddModelError("jobId", "jobId or modelId not found");
                return BadRequest(ModelState);
            }

            _jobRepository.Delete(job);
            return Ok();
        }
    }
}
