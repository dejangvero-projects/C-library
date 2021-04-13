using System.Collections.Generic;
using System.Threading;

namespace IntermediateProject
{
    public class Author : Person
    {
        public int Id { get; set; }
        public override string Name { get; set; }

        private static int _lastId;
        private static int GenerateId()
        {
            return Interlocked.Increment(ref _lastId);
        }
        public Author(string name)
        {
            Id = GenerateId();
            Name = name;
        }


    }
}