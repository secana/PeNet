using PeNet.FileParser;
using PeNet.Header;
using Xunit;

namespace PeNet.Test.Header
{
    public class AbstractStructureTest
    {
        class SubStructure : AbstractStructure
        {
            public int SubInt { get; } = 11;
            public int SubString { get; } = 12;
            public SubStructure() 
                : base(new BufferFile(new byte[0]), 0)
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

            #pragma warning disable IDE0051 // Remove unused private members
            private string PrivString { get; } = "ShallNotSee"; // Must not be in the results
            #pragma warning restore IDE0051 // Remove unused private members
            protected int ProtInt { get; } = -1;

            public Structure() 
                : base(new BufferFile(new byte[0]), 0)
            {
            }
        }
    }
}
