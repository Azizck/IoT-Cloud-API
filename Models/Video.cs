using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime UploadedAt { get; set; }
        public int? PackageId { get; set; }
        public virtual Package Package { get; set; }
    }
}
