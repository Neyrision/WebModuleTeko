using Microsoft.EntityFrameworkCore;
using WebModuleTeko.Database.Entities;

namespace WebModuleTeko.Database;

public class WmtContext(DbContextOptions<WmtContext> options) : DbContext(options)
{
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<UserEntity> Users { get; set; }
}