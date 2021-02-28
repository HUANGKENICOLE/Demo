using Microsoft.EntityFrameworkCore;

namespace Demo_One
{
    // 连接DbContext，而不是创建
    //该cs文件的目的：通过DbContext去连接数据库
    //该类主要用于对 DbContext 做一些必要的参数配置
    public class PatientDbContext:DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> option) : base(option) // 记住
        {
        }
        
        // 新增几个DbSet<TEntity>属性用来表示实体集合 DbSet表示可用于增删改查操作的实体集  
        public DbSet<Patient>Patients { get; set; } // DbContext中每一个表加一个DbSet
    }
}