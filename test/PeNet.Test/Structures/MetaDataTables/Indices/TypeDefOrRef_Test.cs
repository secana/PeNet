using System;
using PeNet.Structures.MetaDataTables.Indices;
using Xunit;

namespace PeNet.Test.Structures.MetaDataTables.Indices
{
    public class TypeDefOrRef_Test
    {
        [Theory]
        [InlineData(
            new byte[]{0x00, 0x00, 0x00, 0x00}, 
            0, 
            1, 
            2, 
            3, 
            
            2,
            0,
            0,
            true,
            false,
            false)]

        [InlineData(
            new byte[]{0x12, 0x52}, 
            0, 
            5, 
            8, 
            2, 
            
            2,
            0x1484,
            2,
            false,
            false,
            true)]

        [InlineData(
            new byte[]{0x34, 0x35, 0x01, 0x12, 0x45}, 
            1, 
            16384, 
            8, 
            2, 
            
            4,
            0x1144804D,
            1,
            false,
            true,
            false)]
        public void Constructor_ValidValues_SetCorrectValues(
            byte[] buff,
            uint offset, 
            int numRowsTypeDefTable, 
            int numRowsTypeRefTable, 
            int numRowsTypeSpecTable,
            
            int indexSize,
            int index,
            int tag,
            bool isDef,
            bool isRef,
            bool isSpec
            )
        {
            var typeDefOrRef = new TypeDefOrRef(
                buff,
                offset,
                numRowsTypeDefTable,
                numRowsTypeRefTable,
                numRowsTypeSpecTable);

            Assert.Equal(indexSize, typeDefOrRef.IndexSize);
            Assert.Equal(index, typeDefOrRef.Index);
            Assert.Equal(tag, typeDefOrRef.Tag);
            Assert.Equal(isDef, typeDefOrRef.IsTypeDefIndex);
            Assert.Equal(isRef, typeDefOrRef.IsTypeRefIndex);
            Assert.Equal(isSpec, typeDefOrRef.IsTypeSpecIndex);
        }

        [Fact]
        public void Constructor_InvalidTag_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TypeDefOrRef(
                new byte[]{ 0x03, 012},
                0,
                21,
                4,
                5));
        }
    }
}