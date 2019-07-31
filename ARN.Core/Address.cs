using System.Collections.Generic;
using System.Text;

namespace ARN.Core
{
    public class Address : BaseClass
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }

        public Address(string street, string city, string state, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public Address(string line)
        {
            var parts = line.Split(Separator);

            Street = parts[0];
            City = parts[1];
            State = parts[2];
            ZipCode = parts[3];
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Address address))
            {
                return false;
            }

            return address.Street.Equals(Street) && address.City.Equals(City) && address.State.Equals(State) && address.ZipCode.Equals(ZipCode);
        } 

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Street != null ? Street.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (City != null ? City.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (State != null ? State.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ZipCode != null ? ZipCode.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override IEnumerable<byte> ToBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(Encoding.ASCII.GetBytes(Street));
            bytes.AddRange(Encoding.ASCII.GetBytes(City));
            bytes.AddRange(Encoding.ASCII.GetBytes(State));
            bytes.AddRange(Encoding.ASCII.GetBytes(ZipCode));

            return bytes;
        }

        public override string ToString()
        {
            return string.Join(Separator.ToString(), Street, City, State, ZipCode);
        }
    }
}
