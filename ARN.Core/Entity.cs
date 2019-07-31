using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ARN.Core
{
    public abstract class Entity : BaseClass
    {
        private const string DbFile = "db.log";
        private const string EntityType = "ENTITY";

        public string Id { get; private set; }

        protected string Type { private get; set; }

        protected Entity()
        {
            Type = EntityType;
        }

        protected Entity(string line)
        {
            var parts = line.Split(Separator);

            Id = parts[0];
            Type = parts[1];
        }

        public override string ToString()
        {
            return string.Join(Separator.ToString(), Id, Type);
        }

        public override int GetHashCode()
        {
            return ToBytes().Aggregate(0, (current, b) => current + b);
        }

        private void GenerateId()
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(ToBytes().ToArray());

                var id = new StringBuilder();

                id.Append("0x");

                foreach (var b in hash)
                {
                    id.Append(b.ToString("X2"));
                }

                Id = id.ToString();
            }
        }

        public void Save()
        {
            GenerateId();

            if (Find(Id) != null)
            {
                Delete();
            }

            using (var writer = File.AppendText(DbFile))
            {
                writer.WriteLine(ToString());
            }
        }

        public void Delete()
        {
            File.WriteAllLines(DbFile, File.ReadAllLines(DbFile).Where(l => l.Split(Separator).First() != Id).ToArray());

            Id = null;
        }

        protected static Entity Find(string id)
        {
            using (var reader = new StreamReader(File.Open(DbFile, FileMode.OpenOrCreate)))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(Separator);

                    if (parts[0] == id)
                    {
                        switch (parts[1])
                        {
                            case Customer.CustomerType:
                                return new Customer(line);
                            case Company.CompanyType:
                                return new Company(line);
                        }
                    }
                }
            }

            return null;
        }

    }
}
