using System;

namespace JsonStore.Sql.LocalDbTests.Model
{
    public class Person
    {
        private Person()
        {
        }

        public static Person Create(int age, string name)
        {
            if (age <= 0) throw new ArgumentOutOfRangeException(nameof(age));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            return new Person
            {
                Id = Guid.NewGuid().ToString(),
                Age = age,
                Name = name
            };
        }

        public string Id { get; private set; }
        public int Age { get; private set; }
        public string Name { get; private set; }
    }
}