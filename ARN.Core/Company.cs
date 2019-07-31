using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARN.Core
{
    public class Company : Entity
    {
        public const string CompanyType = "COMPANY";
        public readonly Address Address;
        public readonly string Name;

        public Company(string name, Address address)
        {
            Type = CompanyType;
            Name = name;
            Address = address;
        }

        public Company(string line) : base(line)
        {
            var parts = line.Split(Separator).Skip(2).ToList();

            Name = parts[0];
            Address = new Address(string.Join(Separator.ToString(), parts.Skip(1).ToArray()));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Company company))
            {
                return false;
            }

            return company.Name.Equals(Name) && company.Address.Equals(Address);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();

                hashCode = (hashCode * 397) ^ (Address != null ? Address.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);

                return hashCode;
            }
        }

        public new static Company Find(string id)
        {
            return Entity.Find(id) as Company;
        }

        public override IEnumerable<byte> ToBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(Encoding.ASCII.GetBytes(Name));
            bytes.AddRange(Address.ToBytes());

            return bytes;
        }

        public override string ToString()
        {
            return string.Join(Separator.ToString(), base.ToString(), Name, Address.ToString());
        }
    }
}
