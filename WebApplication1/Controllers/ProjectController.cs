using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.DB;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApiDbContext _context;
       
        public ProjectsController(ApiDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
              
                    throw;
                
            }

            return NoContent();
        }

        // POST: api/Projects
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Project>> PostProject(ProjectCreateDto projectDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // get the user id from the token.
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId); // Replace Users with your DbSet name for IdentityUser if different

            if (user != null)
                {
                var project = new Project
                {
                    name = projectDto.name,
                    description = projectDto.description,
                    User = user,
                    UserId = userId,
                    Tickets = new List<Ticket>()
                };
        
                    _context.Projects.Add(project);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetProject), new { id = project.id }, project);
                }
                else
                {
                    return BadRequest("Invalid User Id");
                }
            }
       
        


        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
         [Authorize]
    public async Task<IActionResult> Project(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok(new {message = "Project Deleted"});
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.id == id);
        }
    }
}
