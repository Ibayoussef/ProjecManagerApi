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
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class ProjectsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApiDbContext _context;
       
        public ProjectsController(ApiDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects(string? name)
        {
            IQueryable<Project> query = _context.Projects;

            if (!string.IsNullOrEmpty(name))
            {
                // Convert the search name to lowercase
                string searchName = name.ToLower();

                // Apply the filter based on the lowercase name
                query = query.Where(p => p.name.ToLower().Contains(searchName));
            }

            List<Project> projects = await query.ToListAsync();

            if (string.IsNullOrEmpty(name))
            {
                // Return all projects when name is null or empty
                return await _context.Projects.ToListAsync();
            }

            return projects;
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
        public async Task<ActionResult<Project>> PostProject(ProjectCreateDto projectDto)
        {
            string authorizationHeader = Request?.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                string bearerToken = authorizationHeader.Substring("Bearer ".Length).Trim();
                string userEmail = "";
             
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a very very very very very long string that is at least 32 characters")),

                    ValidateIssuer = true,
                    ValidIssuer = "baseApiIssuer",

                    ValidateAudience = true,
                    ValidAudience = "baseApiIssuer",
                };
                try
                {
                    // Validate the token and extract claims
                    ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(bearerToken, validationParameters, out SecurityToken validatedToken);

                    // Find the user id claim in the token
                    Claim userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim != null)
                    {
                         userEmail = userIdClaim.Value;
                 
                    }
                }
                catch (Exception ex)
                {
                    return Ok(ex?.Message);
                }
                 // implement a method to extract the user id from the token
                if (userEmail == null)
                {
                    return Unauthorized(); 
                }

                var user = await _userManager.FindByEmailAsync(userEmail);

                if (user != null)
                {
                    var project = new Project
                    {
                        name = projectDto.name,
                        description = projectDto.description,
                        User = user,
                        UserId = user.Id,
                        Tickets = new List<Ticket>()
                    };

                    _context.Projects.Add(project);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetProject), new { id = project.id }, project);
                }
                else
                {
                    return BadRequest(user);
                }
            }
            else
            {
                return BadRequest("Bearer token not provided");
            }
        }




        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        
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
