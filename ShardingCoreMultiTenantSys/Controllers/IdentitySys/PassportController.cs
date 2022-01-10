using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ShardingCore.Helpers;
using ShardingCoreMultiTenantSys.IdentitySys;
using ShardingCoreMultiTenantSys.IdentitySys.Domain.Entities;
using ShardingCoreMultiTenantSys.IdentitySys.ShardingConfigs;

namespace ShardingCoreMultiTenantSys.Controllers.IdentitySys
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class PassportController:ControllerBase
    {
        private readonly IdentityDbContext _identityDbContext;

        public PassportController(IdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (await _identityDbContext.Set<SysUser>().AnyAsync(o => o.Name == request.Name))
                return BadRequest("user not exists");
            var sysUser = new SysUser()
            {
                Id = Guid.NewGuid().ToString("n"),
                Name = request.Name,
                Password = request.Password,
                CreationTime=DateTime.Now
            };
            var shardingTenantOptions = new ShardingTenantOptions()
            {
                ConfigId = sysUser.Id,
                Priority = new Random().Next(1,10),
                DbType = request.DbType,
                DefaultDataSourceName = "ds0",
                DefaultConnectionString = GetDefaultString(request.DbType,sysUser.Id)
            };
            var sysUserTenantConfig = new SysUserTenantConfig()
            {
                Id = Guid.NewGuid().ToString("n"),
                UserId = sysUser.Id,
                CreationTime = DateTime.Now,
                ConfigJson = JsonConvert.SerializeObject(shardingTenantOptions)
            };
            await _identityDbContext.AddAsync(sysUser);
            await _identityDbContext.AddAsync(sysUserTenantConfig);
            await _identityDbContext.SaveChangesAsync();
            //注册完成后进行配置生成
            DynamicShardingHelper.DynamicAppendVirtualDataSourceConfig(new SqlShardingConfiguration(shardingTenantOptions));
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var sysUser = await _identityDbContext.Set<SysUser>().FirstOrDefaultAsync(o=>o.Name==request.Name&&o.Password==request.Password);
            if (sysUser == null)
                return BadRequest("name or password error");

            //秘钥，就是标头，这里用Hmacsha256算法，需要256bit的密钥
            var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("123123!@#!@#123123")), SecurityAlgorithms.HmacSha256);
            //Claim，JwtRegisteredClaimNames中预定义了好多种默认的参数名，也可以像下面的Guid一样自己定义键名.
            //ClaimTypes也预定义了好多类型如role、email、name。Role用于赋予权限，不同的角色可以访问不同的接口
            //相当于有效载荷
            var claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Iss,"https://localhost:5000"),
                new Claim(JwtRegisteredClaimNames.Aud,"api"),
                new Claim("id",Guid.NewGuid().ToString("n")),
                new Claim("uid",sysUser.Id),
            };
            SecurityToken securityToken = new JwtSecurityToken(
                signingCredentials: securityKey,
                expires: DateTime.Now.AddHours(2),//过期时间
                claims: claims
            );
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return Ok(token);
        }

        private string GetDefaultString(DbTypeEnum dbType, string userId)
        {
            switch (dbType)
            {
                case DbTypeEnum.MSSQL: return $"Data Source=localhost;Initial Catalog=DB{userId};Integrated Security=True;";
                case DbTypeEnum.MYSQL: return $"server=127.0.0.1;port=3306;database=DB{userId};userid=root;password=L6yBtV6qNENrwBy7;";
                default: throw new NotImplementedException();
            }
        }
    }
    
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public DbTypeEnum DbType { get; set; }
    }

    public class LoginRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

}
