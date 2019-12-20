using System;

namespace PeNet.Parser
{
    internal abstract class SafeParser<T>
        where T : class
    {
        protected readonly byte[] Buff;
        protected readonly uint Offset;
        private bool _alreadyParsed;

        private T _target;

        internal SafeParser(byte[] buff, uint offset)
        {
            Buff = buff;
            Offset = offset;
        }

        private bool SanityCheckFailed()
        {
            return Offset > Buff?.Length;
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
                _target = null;
            }

            return _target;
        }
    }
}