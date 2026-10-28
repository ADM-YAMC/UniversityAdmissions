using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityAdmissions.Application.Responses
{
    public class Response<T>
    {
        public bool Success { get; set; } = false;
        public T? SingleData { get; set; }
        public IEnumerable<T> DataList { get; set; }
        public int Identity { get; set; }
        public List<string>? Messages { get; set; } = new List<string>();
        public List<string>? Warnings { get; set; } = new List<string>();
        public List<string>? Errors { get; set; } = new List<string>();
    }
}
