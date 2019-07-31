using System.Collections.Generic;

namespace ARN.Core
{
    public abstract class BaseClass
    {

        protected const char Separator = ':';

        public abstract IEnumerable<byte> ToBytes();

    }
}
