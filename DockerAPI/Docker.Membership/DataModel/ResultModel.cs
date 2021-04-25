using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.Membership.DataModel
{
    public class ResultModel<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
