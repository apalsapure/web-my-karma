using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.ErrorSpace
{
    [Serializable]
    public class BadRequestException : BaseApplicationException
    {
        public BadRequestException()
            : base()
        {
            this.ErrorCode = HttpErrorCode.BadRequest;
            this.Failures = new List<string>();
        }

        public BadRequestException(string message)
            : base(message)
        {
            this.ErrorCode = HttpErrorCode.BadRequest;
            this.Failures = new List<string>();
        }

        public BadRequestException(string message, Exception inner)
            : base(message, inner)
        {
            this.ErrorCode = HttpErrorCode.BadRequest;
            this.Failures = new List<string>();
        }

        public BadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.ErrorCode = HttpErrorCode.BadRequest;
            this.Failures = new List<string>();
        }

        public List<string> Failures { get; private set; }

        public BadRequestException AddFailure(string message)
        {
            this.ErrorCode = HttpErrorCode.BadRequest;
            this.Failures.Add(message);
            return this;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            info.AddValue("Failures", String.Join(", ", this.Failures));

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }
    }
}
