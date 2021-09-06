using System;
using System.Text;

namespace Qnyd.Data.Results
{
    public class Result
    {
        public bool Succeed { get; set; }

        public string Msg { get; set; }

        public static Result CreateResult()
        {
            return new Result { Succeed = true };
        }
    }
}
