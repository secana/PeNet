using System;

namespace PeNet.Parser
{
    public abstract class SafeParser<T>
    {
        public SafeParser(byte[] buff, uint offset)
        {
            _buff = buff;
            _offset = offset;
        }

        public Exception ParserException { get; protected set; }

        private T _target = default(T);
        private bool _alreadyParsed;
        protected readonly byte[] _buff;
        protected readonly uint _offset;

        protected abstract T ParseTarget();

        public T GetParserTarget()
        {
            if (_alreadyParsed)
                return _target;

            _alreadyParsed = true;

            try
            {
                _target = ParseTarget();
            }
            catch (Exception exception)
            {
                ParserException = exception;
            }

            return _target;
        }
    }
}