using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2_Server.Model
{
    [Serializable]
    public class RequestModel
    {
        public List<Subject> Subjects{ get; set; }
    }
}
