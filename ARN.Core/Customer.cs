
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARN.Core
{
    public class Customer : Entity
    {
        public const string CustomerType = "CUSTOMER";
        public readonly Address Address;
        public readonly string FirstName;
        public readonly string LastName;

        public Customer(string firstName, string lastName, Address address)
        {
            Type = CustomerType;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
        }

        public Customer(string line) : base(line)
        {
            var parts = line.Split(Separator).Skip(2).ToList();

            FirstName = parts[0];
            LastName = parts[1];
            Address = new Address(string.Join(Separator.ToString(), parts.Skip(2).ToArray()));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Customer customer))
            {
                return false;
            }

            return customer.FirstName.Equals(FirstName) && customer.LastName.Equals(LastName) && customer.Address.Equals(Address);
        }
         
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();

                hashCode = (hashCode * 397) ^ (Address != null ? Address.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);

                return hashCode;
            }
        }

        public new static Customer Find(string id)
        {
            return Entity.Find(id) as Customer;
        }

        public override IEnumerable<byte> ToBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(Encoding.ASCII.GetBytes(FirstName));
            bytes.AddRange(Encoding.ASCII.GetBytes(LastName));
            bytes.AddRange(Address.ToBytes());

            return bytes;
        }

        public override string ToString()
        {
            return string.Join(Separator.ToString(), base.ToString(), FirstName, LastName, Address.ToString());
        }

    }
}
