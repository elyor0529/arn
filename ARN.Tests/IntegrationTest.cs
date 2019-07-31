using System.IO;
using ARN.Core;
using NUnit.Framework;

namespace ARN.Tests
{
    [TestFixture]
    public class IntegrationTest
    {
        private const string Street = "Tashkent";
        private const string City = "TAS";
        private const string State = "UZB";
        private const string ZipCode = "100115";
        private const string FirstName = "Elyor";
        private const string LastName = "Latipov";
        private const string Name = "ARN";

        [SetUp]
        public void Setup()
        {
            const string file = "db.log";

            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        private static Address CreateAddress()
        {
            return new Address(Street, City, State, ZipCode);
        }

        [Test]
        public void AddressTest()
        {
            var address = CreateAddress();

            Assert.That(address.Street, Is.EqualTo(Street));
            Assert.That(address.City, Is.EqualTo(City));
            Assert.That(address.State, Is.EqualTo(State));
            Assert.That(address.ZipCode, Is.EqualTo(ZipCode));
        }

        [Test]
        public void CustomerTest()
        {
            var address = CreateAddress();

            var customer = new Customer(FirstName, LastName, address);

            Assert.That(customer.FirstName, Is.EqualTo(FirstName));
            Assert.That(customer.LastName, Is.EqualTo(LastName));
            Assert.That(customer.Address, Is.EqualTo(address));
        }

        [Test]
        public void CompanyTest()
        {
            var address = CreateAddress();

            var customer = new Company(Name, address);

            Assert.That(customer.Name, Is.EqualTo(Name));
            Assert.That(customer.Address, Is.EqualTo(address));
        }

        [Test]
        public void ProgrammerTest()
        {
            var address = CreateAddress();
            var customer = new Customer(FirstName, LastName, address);
            var company = new Company(Name, address);

            Assert.IsNull(customer.Id);
            customer.Save();
            Assert.IsNotNull(customer.Id);

            Assert.IsNull(company.Id);
            company.Save();
            Assert.IsNotNull(company.Id);

            var savedCustomer = Customer.Find(customer.Id);
            Assert.IsNotNull(savedCustomer);
            Assert.AreSame(customer.Address, address);
            Assert.AreEqual(savedCustomer.Address, address);
            Assert.AreEqual(customer.Id, savedCustomer.Id);
            Assert.AreEqual(customer.FirstName, savedCustomer.FirstName);
            Assert.AreEqual(customer.LastName, savedCustomer.LastName);
            Assert.AreEqual(customer, savedCustomer);
            Assert.AreNotSame(customer, savedCustomer);

            var savedCompany = Company.Find(company.Id);
            Assert.IsNotNull(savedCompany);
            Assert.AreSame(company.Address, address);
            Assert.AreEqual(savedCompany.Address, address);
            Assert.AreEqual(company.Id, savedCompany.Id);
            Assert.AreEqual(company.Name, savedCompany.Name);
            Assert.AreEqual(company, savedCompany);
            Assert.AreNotSame(company, savedCompany);

            customer.Delete();
            Assert.IsNull(customer.Id);
            Assert.IsNull(Customer.Find(customer.Id));

            company.Delete();
            Assert.IsNull(company.Id);
            Assert.IsNull(Customer.Find(company.Id));
        }
    }
}
