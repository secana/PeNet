using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    public class AbstractStructure_Test
    {
        class SubStructure : AbstractStructure
        {
            public int SubInt { get; } = 11;
            public int SubString { get; } = 12;
            public SubStructure() 
                : base(null, 0)
            {
            }
        }
        class Structure : AbstractStructure
        {
            public int Integer { get; } = 10;
            public string String { get; } = "Hello";
            public long Long { get; } = 20;
            public string[] StringArray { get; } = new string[] { "Hello", "World" }; // Must not be in the results
            public int[] NullArray { get; } = null; // Must not be in the results
            public string NullString { get; } = null;
            public SubStructure[] SubStructure { get; } = new SubStructure[] { new SubStructure(), new SubStructure() };

            private string PrivString { get; } = "ShallNotSee"; // Must not be in the results
            protected int ProtInt { get; } = -1;

            public Structure() 
                : base(null, 0)
            {
            }
        }

        [Fact]
        public void ToString_GivenAnAbstractStructure_ReturnsStringRepresentation()
        {
            var structure = new Structure();
            var expected = "Structure\nInteger: 10\nString: Hello\nLong: 20\nNullString: \nSubStructure\nSubInt: 11\nSubString: 12\nSubStructure\nSubInt: 11\nSubString: 12\n";

            var actual = structure.ToString();

            Assert.Equal(expected, actual);
        }
    }
}
