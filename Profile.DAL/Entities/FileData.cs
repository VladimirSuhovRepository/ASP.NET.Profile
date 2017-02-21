using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.DAL.Entities
{
    public class FileData
    {
        [Key, ForeignKey("File")]
        public int Id { get; set; }
        public byte[] Data { get; set; }

        public virtual File File { get; set; }
    }
}
