using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;

namespace Samples.Banks
{
    public class CompanyNameError : Error
    {
        protected CompanyNameError()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="Error"/>
        /// </summary>
        /// <param name="message">Description of the error</param>
        public CompanyNameError(string message)
            : base(message)
        {
            Message = message;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Error"/>
        /// </summary>
        /// <param name="message">Description of the error</param>
        /// <param name="causedBy">The root cause of the <see cref="Error"/></param>
        public CompanyNameError(string message, IError causedBy)
            : base(message, causedBy)
        {

        }

        public static implicit operator Result(CompanyNameError error) => Result.Fail(error);
    }
}
