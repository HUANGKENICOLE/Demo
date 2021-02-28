using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace Demo_One.Controllers
{
    // DbContext依赖注入
    // 在Startup中，PatientDbContext已经注入到容器了
    // 所以现在在PatientController中，通过依赖注入的方式去获取PatientDbContext实例
    // 通过构造函数注入
    
    //  MVC或者API的应用，当你在控制器中这么写的时候，
    // .NET Core会自动把你已注册的服务给注入进来，不需要你再去实例化了new
    
    [ApiController]
   // [Route("[Controller]")]
    [Route("api/patient")]
    public class PatientController:ControllerBase
    {
        
        private readonly PatientDbContext _patientDbContext;
        public PatientController(PatientDbContext patientDbContext) // 构造函数注入
        {
            _patientDbContext = patientDbContext;
        }
        
        //  获取病人列表[支持 模糊查询、分页、排序]
        // keyword模糊查询的关键词、pageIndex第几页、pageSize每页的条数
        
        // 为什么这几个操作不在dg上写
        // List<Patient> list = new List<Patient>(); // 创建泛型集合
        //为什么不写成
        // interface IGetList --  获取病人列表【应该支持 模糊查询，分页，排序】
        // {
        //     select * from platform.patient where name ~ '^李'; -- 找以李开头的字符串
        // }


        /// /patient
        [Route("pat0")]
        [HttpGet]
       // [HttpGet("pat0")]
        public List<Patient> list(string keyword, int pageIndex, int pageSize)
        {
            var skip=pageSize*(pageIndex-1);
            var patient = _patientDbContext.Patients
                .Where(it=>it.Name.Contains(keyword)) // Where属于Linq下的
                // 从两端模糊匹配
                // 相当于pgsql中的
                // select * from platform.patient where name like '%keyword%'
                // offset 40 limit 20; 
               
                // Skip和Take一般放一起用，一般用于分页
                .Skip(skip) // 跳过前skip个元素
                .Take(pageSize) // 获取前pageSize个元素
                
                .ToList();
            return patient;
        }
        
        // 为什么不能用这个？？
        // public List<Patient> patient = new List<Patient>();  
        // public IEnumerable<Patient> GetAll() 
        // {
        //     // int a = null; 不能为null
        //     // int? b = null; 可以为null       
        //     
        //     return Patient;
        // }
        
        [Route("pat1")]
        [HttpGet]
       // [HttpGet("pat1")]
        public Patient GetDetail(int id) // 获取一个病人的详细信息
        {
            return _patientDbContext.Patients.Find(id);
            // Find后面直接写id，不要写it=>it.Id==id
            // 错误写法：return patient.Find(id);  
            // 要有这个 _patientDbContext. ，因为是对数据库进行操作!
        } // = select * from platform.patient;
        
        
        
        //private var countId = 0;   会直接跳了private的原因
        // 不用另外设置id++，因为id自增，看下之前怎么定义的
        [Route("pat2")]
        [HttpPost]
      //  [HttpPost("pat2")]
        public Patient Add(Patient patient)
        {
            // if (patient==null)       // it不会为空，因为id默认是int的初始值，为0
            //     throw new ArgumentNullException("it"); //抛出异常，表示参数为空不可用。

            _patientDbContext.Add(patient); // 对数据库进行操作前面要加_patientDbContext
            _patientDbContext.SaveChanges(); // 这一步的作用是保存到数据库，增删改都要写这句话，查不用
            
            // 执行完后，patient.Id会自动有个值
            
            return patient;
        }
        // interface IAdd 
        // {
        //     IEnglish patient = new Speaker();
        // }

        [Route("pat3")]
        [HttpPut]
        //[HttpPut("pat3")]
        public bool Update(Patient patient) // 修改一个病人的信息
        {
            _patientDbContext.Update(patient); // 不用先删除再添加
            
            // int id=patient.Find(it=>it.Id==patient.Id);  获取patient的id正确写法：var id = patient.Id; 
            // .Find、where是针对一个列表的(patient是一个实例)，比如从某个列中取出某一条数据
            
            // _patientDbContext.Remove(id);
            // _patientDbContext.Add(patient);  // _patientDbContext.Add(patient);
            _patientDbContext.SaveChanges();
            return true; // return patient;
        }
        //     update platform.patient set name='' where id=@id;  对pgsql进行操作，为什么不写sql语句？？

        [Route("pat4")]
        [HttpDelete]
        // [HttpDelete("pat4")]
        public void Delete(int id) // 删除一个病人
        {
            // patient.RemoveAll(it=>it.Id==id); 和下面语句的区别？？ LinQ patient.是对内存上进行操作

            _patientDbContext.Remove(id); // 对数据库进行操作前面要加_patientDbContext
            _patientDbContext.SaveChanges(); // 这一步的作用是保存到数据库，增删改都要写这句话，查不用
        }
        // delete from platform.patient where id=@id;
        // delete from platform.patient where id={userId};
        // delete from platform.patient where id=#{id};
        // #{id}是程序获取的一个参数
    }
}