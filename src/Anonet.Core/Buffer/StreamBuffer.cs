using System;

namespace Anonet.Core
{
    class StreamBuffer : IStreamBuffer
    {
        public StreamBuffer(int capacity)
        {
            _Buffer = new byte[capacity + 1];
        }

        public int Capacity { get { return _Buffer.Length; } }

        private byte[] _Buffer = null;

        #region 索引管理

        // 1.读取时，从头索引指向的位置开始依次读取，直到遇到尾索引（并不读取尾索引指向的数据）
        // 2.写入时，从尾索引指向的位置开始依次写入，直到遇到头索引（并不向头索引指向的位置写入数据）

        /// <summary>
        /// 有效数据段头索引
        /// </summary>
        private int _LeadingIndex = 0;

        /// <summary>
        /// 有效数据段尾索引，该索引指向的数据无效
        /// </summary>
        private int _TrailingIndex = 0;

        private bool LeadingIsBeforeTrailing { get { return _LeadingIndex <= _TrailingIndex; } }

        struct IndexRange
        {
            public IndexRange(int from, int to) : this()
            {
                From = from;
                To = to;
            }

            public int From { get; set; }

            public int To { get; set; }
        }

        private IndexRange LeadingRange
        {
            get
            {
                if (LeadingIsBeforeTrailing)
                {
                    return new IndexRange(-(_LeadingIndex + MaxIndexValue - _TrailingIndex), _TrailingIndex - _LeadingIndex);
                }
                else
                {
                    return new IndexRange(-(_LeadingIndex - _TrailingIndex - 1), MaxIndexValue - _LeadingIndex + _TrailingIndex + 1);
                }
            }
        }

        private IndexRange TrailingRange
        {
            get
            {
                if (LeadingIsBeforeTrailing)
                {
                    return new IndexRange(-(_TrailingIndex - _LeadingIndex), MaxIndexValue - _TrailingIndex + _LeadingIndex);
                }
                else
                {
                    return new IndexRange(-(_TrailingIndex + MaxIndexValue - _LeadingIndex), _LeadingIndex - _TrailingIndex - 1);
                }
            }
        }

        private int MaxIndexValue { get { return _Buffer.Length - 1; } }

        private void MoveLeading(int offset)
        {
            offset = Math.Max(Math.Min(offset, LeadingRange.To), LeadingRange.From);

            if ((offset + _LeadingIndex) <= MaxIndexValue)
            {
                _LeadingIndex += offset;
            }
            else
            {
                _LeadingIndex = (_LeadingIndex + offset - _Buffer.Length);
            }
        }

        private void MoveTrailing(int offset)
        {
            offset = Math.Max(Math.Min(offset, TrailingRange.To), TrailingRange.From);

            if ((offset + _TrailingIndex) <= MaxIndexValue)
            {
                _TrailingIndex += offset;
            }
            else
            {
                _TrailingIndex = (_TrailingIndex + offset - _Buffer.Length);
            }
        }

        public int Length
        {
            get
            {
                if (LeadingIsBeforeTrailing)
                {
                    return _TrailingIndex - _LeadingIndex;
                }
                else
                {
                    return MaxIndexValue - _LeadingIndex + 1 + _TrailingIndex;
                }
            }
        }

        #endregion

        public int IndexOf(byte targetByte)
        {
            for (int i = 0; i <= LeadingRange.To; i++)
            {
                var idx = i + _LeadingIndex;

                if (idx > MaxIndexValue)
                {
                    idx = idx - MaxIndexValue - 1;
                }

                if (_Buffer[idx] == targetByte)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Seek(int offset)
        {
            MoveLeading(offset);
        }

        public void Read(byte[] buffer, int offset, int count)
        {
            count = Math.Min(Length, count);
            var hasReadCount = 0;

            if (count > _Buffer.Length - _LeadingIndex)
            {
                hasReadCount = _Buffer.Length - _LeadingIndex;
                ReadFromBuffer(buffer, offset, hasReadCount);
                count -= hasReadCount;
            }

            ReadFromBuffer(buffer, offset + hasReadCount, count - hasReadCount);
        }

        public byte[] ReadAll()
        {
            var buffer = new byte[Length];
            Read(buffer, 0, buffer.Length);
            return buffer;
        }

        private void ReadFromBuffer(byte[] buffer, int offset, int count)
        {
            Array.Copy(_Buffer, _LeadingIndex, buffer, offset, count);
            MoveLeading(count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            count = Math.Min(count, TrailingRange.To);
            var hasWritedCount = 0;

            if (count > (_Buffer.Length - _TrailingIndex))
            {
                hasWritedCount = _Buffer.Length - _TrailingIndex;
                WriteToBuffer(buffer, offset, hasWritedCount);
                count -= hasWritedCount;
            }

            WriteToBuffer(buffer, offset + hasWritedCount, count - hasWritedCount);
        }

        public void Write(byte[] buffer)
        {
            Write(buffer, 0, buffer.Length);
        }

        private void WriteToBuffer(byte[] buffer, int offset, int count)
        {
            Array.Copy(buffer, offset, _Buffer, _TrailingIndex, count);
            MoveTrailing(count);
        }

        public void Reset()
        {
            _LeadingIndex = 0;
            _TrailingIndex = 0;
        }
    }
}