using System;
using System.Collections.Generic;
using Moq;
using PeNet.Structures;
using PeNet.Structures.MetaDataTables;
using Xunit;

namespace PeNet.Test.Structures.MetaDataTables
{
    public class MetaDataTableIndexComputation_Test
    {
        [Fact]
        public void GetTableIndexSize_ThrowsExceptionOnNotEnumType_Test()
        {
            // Given
            var metaDataTableIndexComputation = new MetaDataTableIndexComputation(null);

            // When/Then
            Assert.Throws<ArgumentException>(() => metaDataTableIndexComputation.GetTableIndexSize(typeof(object)));
        }

        [Theory]
        [InlineData(50, 2)]
        [InlineData(0x191, 4)]
        public void GetTableIndexSize_TypeDefOrRef_Test(uint numOfRows, uint expectedIndexSize)
        {
            // Given
            var mock = new Mock<IMETADATATABLESHDR>();
            mock.SetupGet(x => x.TableDefinitions).Returns(new List<METADATATABLESHDR.TableDefinition>()
            {
                new METADATATABLESHDR.TableDefinition("TypeDef", numOfRows),
                new METADATATABLESHDR.TableDefinition("TypeRef", numOfRows - 1),
                new METADATATABLESHDR.TableDefinition("TypeSpec", numOfRows - 2)
            });

            // When
            var metaDataTableIndexComputation = new MetaDataTableIndexComputation(mock.Object);

            // Then
            Assert.Equal(expectedIndexSize, metaDataTableIndexComputation.GetTableIndexSize(typeof(DotNetConstants.TypeDefOrRef)));
        }
    }
}