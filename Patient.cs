using System.ComponentModel.DataAnnotations.Schema;

namespace Demo_One
{
    [Table("patient",Schema = "platform")]
    // 在patient-managent数据库下新建一个platform的schema
    public class Patient
    {
        
        [Column("id")]
        public int Id { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        
    }
}