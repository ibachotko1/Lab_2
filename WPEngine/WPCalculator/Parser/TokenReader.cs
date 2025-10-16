using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPEngine.Parser
{
    public class TokenReader
    {
        private readonly List<Token> _tokens;
        private int _index;

        public TokenReader(List<Token> tokens)
        {
            _tokens = tokens;
            _index = 0;
        }

        public Token Peek()
        {
            return _index < _tokens.Count ? _tokens[_index] : null;
        }

        public Token Read()
        {
            return _index < _tokens.Count ? _tokens[_index++] : null;
        }
    }
}
