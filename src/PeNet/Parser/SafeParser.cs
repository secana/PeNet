using System;
using System.IO;

namespace PeNet.Parser
{
    internal abstract class SafeParser<T>
        where T : class
    {
        protected readonly Stream PeFile;
        protected readonly uint Offset;
        private bool _alreadyParsed;

        private T? _target;

        internal SafeParser(Stream peFile, uint offset)
        {
            PeFile = peFile;
            Offset = offset;
        }

        private bool SanityCheckFailed()
        {
            return Offset > PeFile?.Length;
        }


        protected abstract T? ParseTarget();

        public T? GetParserTarget()
        {
            if (_alreadyParsed)
                return _target;

            _alreadyParsed = true;

            if (SanityCheckFailed())
                return null;

            try
            {
                _target = ParseTarget();
            }
            catch (Exception)
            {
                _target = null;
            }

            return _target;
        }
    }
}