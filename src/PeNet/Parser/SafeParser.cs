using System;

namespace PeNet.Parser
{
    internal abstract class SafeParser<T>
        where T : class
    {
        protected readonly byte[] _buff;
        protected readonly uint _offset;
        private bool _alreadyParsed;

        private T _target;

        internal SafeParser(byte[] buff, uint offset)
        {
            _buff = buff;
            _offset = offset;
        }

        private bool SanityCheckFailed()
        {
            return _offset > _buff?.Length;
        }


        protected abstract T ParseTarget();

        public T GetParserTarget()
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
                // Silently catch the exception
                // TODO: Add global list with exception
                _target = null;
            }

            return _target;
        }
    }
}