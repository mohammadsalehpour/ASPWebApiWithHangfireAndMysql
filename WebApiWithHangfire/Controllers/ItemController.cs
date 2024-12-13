using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiWithHangfire.Data;
using WebApiWithHangfire.Models.Entities;

namespace WebApiWithHangfire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        // Background Jobs
        //Display message(object) As soon as  post is made.

        [HttpPost("BackgroundJob-Enqueue")]
        public async Task<IActionResult> Post([FromBody] Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            BackgroundJob.Enqueue(() => DisplayBackground(item));
            return Ok(true);
        }
        public static void DisplayBackground(Item item)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("This is the Item you just added");
            Console.WriteLine("Name: " + item.Name);
            Console.WriteLine("Quantity: " + item.Quantity + Environment.NewLine);
        }


        //Schedule Jobs
        // Display list of items 8 seconds past after calling this method. 

        [HttpGet("BackgroundJob-Schedule")]
        public async Task<IActionResult> Get()
        {
            var items = await _context.Items.ToListAsync();
            BackgroundJob.Schedule(() => DoConsoleStuff(items), new DateTimeOffset(DateTime.UtcNow.AddSeconds(2)));
            return Ok(items);
        }
        public static void DoConsoleStuff(List<Item> items)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"[Name  Quantity]");
            foreach (var item in items)
            {
                Console.WriteLine($"[{item.Name} ----- {item.Quantity}]");
            }
            Console.WriteLine(Environment.NewLine);
        }



        //Continuation Job
        // Display items total quantity 10 times, it first needs started job's Id to check if it has finished then it continues.

        [HttpGet("BackgroundJob-ContinueJobWith")]
        public async Task<IActionResult> TotalQuantity()
        {
            var items = await _context.Items.ToListAsync();
            var DefaultJobId = BackgroundJob.Schedule(() => DoConsoleStuff(items), new DateTimeOffset(DateTime.UtcNow.AddSeconds(5)));
            await Task.Delay(5000);
            var newjob1 = BackgroundJob.ContinueJobWith(DefaultJobId, () => GetItemQuantity(items));
            await Task.Delay(7000);
            var newjob2 = BackgroundJob.ContinueJobWith(newjob1, () => DoConsoleStuff(items));
            await Task.Delay(8000);
            var newjob3 = BackgroundJob.ContinueJobWith(newjob2, () => GetItemQuantity(items));
            await Task.Delay(9000);
            var newjob4 = BackgroundJob.ContinueJobWith(newjob3, () => DoConsoleStuff(items));
            await Task.Delay(1000);
            var newjob5 = BackgroundJob.ContinueJobWith(newjob4, () => GetItemQuantity(items));
            return Ok(items.Sum(_ => _.Quantity));
        }
        public static void GetItemQuantity(List<Item> items)
        {
            Console.WriteLine(Environment.NewLine + "Total Quantities: " + items.Sum(_ => _.Quantity) + Environment.NewLine);
        }

        //Recurring Job
        // Display the names in List and quantities

        [HttpGet("Recurring-Jobs")]
        public async Task<IActionResult> GetItems()
        {
            var items = await _context.Items.ToListAsync();
            RecurringJob.AddOrUpdate("RecurrigJobId", () => DoConsoleStuff(items), "* * * * * *");
            return Ok(items);
        }


        //###############################################################################

        // GET: api/Item
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        //{
        //    return await _context.Items.ToListAsync();
        //}

        // GET: api/Item/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Item/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Item
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Item/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
