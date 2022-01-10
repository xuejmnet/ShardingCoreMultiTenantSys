using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShardingCoreMultiTenantSys.DbContexts;
using ShardingCoreMultiTenantSys.TenantSys.Domain.Entities;

namespace ShardingCoreMultiTenantSys.Controllers.TenantSys
{
    [Route("api/tenant/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TenantController : ControllerBase
    {
        private readonly TenantDbContext _tenantDbContext;

        public TenantController(TenantDbContext tenantDbContext)
        {
            _tenantDbContext = tenantDbContext;
        }
        public async Task<IActionResult> AddOrder()
        {
            var order = new Order()
            {
                Id = Guid.NewGuid().ToString("n"),
                CreationTime = DateTime.Now,
                Name = new Random().Next(1,100)+"_name"
            };
            await _tenantDbContext.AddAsync(order);
            await _tenantDbContext.SaveChangesAsync();
            return Ok(order.Id);
        }
        public async Task<IActionResult> UpdateOrder([FromQuery]string id)
        {
            var order =await _tenantDbContext.Set<Order>().FirstOrDefaultAsync(o=>o.Id==id);
            if (order == null) return BadRequest();
            order.Name = new Random().Next(1, 100) + "_name";
            await _tenantDbContext.SaveChangesAsync();
            return Ok(order.Id);
        }
        public async Task<IActionResult> GetOrders()
        {
            var orders =await _tenantDbContext.Set<Order>().ToListAsync();
            return Ok(orders);
        }
    }
}
