using System;

namespace Anonet.Core
{
    class StreamBuffer : IStreamBuffer
    {
        public StreamBuffer(int capacity)
        {
            _Buffer = new byte[capacity];
        }

        public int Capacity { get { return _Buffer.Length; } }

        private byte[] _Buffer = null;

        private int _LeadingIndex = -1;

        private int _TrailingIndex = -1;

        public int Length
        {
            get
            {
                if (_LeadingIndex == -1 && _TrailingIndex == -1)
                {
                    return 0;
                }
                else if (_LeadingIndex <= _TrailingIndex)
                {
                    return _TrailingIndex - _LeadingIndex + 1;
                }
                else // _TrailingIndex < _LeadingIndex
                {
                    return _Buffer.Length - (_LeadingIndex - _TrailingIndex - 1);
                }
            }
        }

        public int IndexOf(byte targetByte)
        {
            if (_LeadingIndex == -1 && _TrailingIndex == -1)
            {
                return -1;
            }
            else if (_LeadingIndex <= _TrailingIndex)
            {
                for (var i = _LeadingIndex; i <= _TrailingIndex; i++)
                {
                    if (_Buffer[i] == targetByte)
                    {
                        return i - _LeadingIndex;
                    }
                }
            }
            else // _TrailingIndex < _LeadingIndex
            {
                for (var i = _LeadingIndex; i < _Buffer.Length; i++)
                {
                    if (_Buffer[i] == targetByte)
                    {
                        return i - _LeadingIndex;
                    }
                }

                for (var i = 0; i <= _TrailingIndex; i++)
                {
                    if (_Buffer[i] == targetByte)
                    {
                        return _Buffer.Length - _LeadingIndex + i;
                    }
                }
            }

            return -1;
        }

        public void Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null || (offset + count) > buffer.Length)
            {
                throw new ArgumentException();
            }

            if (count > Length)
            {
                throw new Exception("buffer does not have enough bytes to read");
            }

            if (_LeadingIndex <= _TrailingIndex)
            {
                if (count <= (_TrailingIndex - _LeadingIndex + 1))
                {
                    Array.Copy(_Buffer, _LeadingIndex, buffer, offset, count);
                    _LeadingIndex += count;
                }
            }
            else // _TrailingIndex < _LeadingIndex
            {
                var c = _Buffer.Length - _LeadingIndex;
                if (count <= c)
                {
                    Array.Copy(_Buffer, _LeadingIndex, buffer, offset, count);
                    _LeadingIndex += count;
                }
                else // count > c
                {
                    if ((count - c) <= (_TrailingIndex + 1))
                    {
                        Array.Copy(_Buffer, _LeadingIndex, buffer, offset, c);
                        _LeadingIndex = 0;

                        Array.Copy(_Buffer, _LeadingIndex, buffer, offset + c, count - c);
                        _LeadingIndex = count - c;
                    }
                }
            }
        }

        public void Seek(int offset)
        {
            if (_LeadingIndex == -1 && _TrailingIndex == -1)
            {
                Reset();
            }
            else if (_LeadingIndex <= _TrailingIndex)
            {
                if (_LeadingIndex + offset <= _TrailingIndex)
                {
                    _LeadingIndex += offset;
                }
                else
                {
                    Reset();
                }
            }
            else // _TrailingIndex < _LeadingIndex
            {
                var count = _Buffer.Length - _LeadingIndex;
                if (offset < count)
                {
                    _LeadingIndex += count;
                }
                else
                {
                    var c = offset - count;
                    if (c <= _TrailingIndex)
                    {
                        _LeadingIndex = c;
                    }
                    else
                    {
                        Reset();
                    }
                }
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            if (_LeadingIndex == -1 && _TrailingIndex == -1)
            {
                if (count <= _Buffer.Length)
                {
                    Array.Copy(buffer, offset, _Buffer, 0, count);
                    _LeadingIndex = 0;
                    _TrailingIndex = count - 1;
                }
                else
                {
                    Array.Copy(buffer, offset, _Buffer, 0, _Buffer.Length);
                    _LeadingIndex = 0;
                    _TrailingIndex = _Buffer.Length - 1;
                }
            }
            else if (_LeadingIndex <= _TrailingIndex)
            {
                if (_TrailingIndex < _Buffer.Length - 1)
                {
                    var c = _Buffer.Length - _TrailingIndex - 1;
                    if (count <= c) 
                    {
                        Array.Copy(buffer, offset, _Buffer, _TrailingIndex + 1, count);
                        _TrailingIndex += count;
                    }
                    else
                    {
                        Array.Copy(buffer, offset, _Buffer, _TrailingIndex + 1, c);
                        _TrailingIndex += c;
                        
                        Write(buffer, offset + c, count - c);
                    }
                }
                else // _TrailingIndex == Buffer.Length - 1
                {
                    var c = _LeadingIndex;
                    if (c == 0)
                    {
                    }
                    else if (count <= c)
                    {
                        Array.Copy(buffer, offset, _Buffer, 0, count);
                        _TrailingIndex = count - 1;
                    }
                    else
                    {
                        Array.Copy(buffer, offset, _Buffer, 0, c);
                        _TrailingIndex = c - 1;
                    }
                }
            }
            else // _TrailingIndex < _LeadingIndex
            {
                var c = _LeadingIndex - _TrailingIndex - 1;
                if (c == 0)
                {
                }
                else if (count <= c)
                {
                    Array.Copy(buffer, offset, _Buffer, _TrailingIndex + 1, count);
                    _TrailingIndex += count;
                }
                else
                {
                    Array.Copy(buffer, offset, _Buffer, _TrailingIndex + 1, c);
                    _TrailingIndex += c;
                }
            }
        }

        public void Reset()
        {
            _LeadingIndex = -1;
            _TrailingIndex = -1;
        }
    }
}
