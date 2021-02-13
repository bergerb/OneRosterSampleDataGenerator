using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Enrollment : BaseModel
    {
        public Guid sourcedId { get; set; }
        public string status => Status.ToString();
        public DateTime dateLastModified => CreatedAt;
        public Guid classSourcedId { get; set; }
        public Guid schoolSourcedId { get; set; }
        public Guid courseSourcedId { get; set; }
        public Guid userSourcedId { get; set; }
        public RoleType Role { get; set; }
        public string role => Role.ToString();

    }
}
