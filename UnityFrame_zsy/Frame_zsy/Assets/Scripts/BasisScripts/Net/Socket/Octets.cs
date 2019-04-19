using System;

namespace Share
{
    public sealed class Octets
    {
        private readonly int capacity;
        private byte[] buffer = null;
        private int start = 0;
        private int size = 0;

        private readonly byte[] tmp = new byte[8];

        public Octets()
        {
            this.capacity = Int32.MaxValue;
            this.buffer = new byte[16];
        }

        public Octets(Octets oc)
        {
            this.buffer = oc.buffer;
            this.capacity = oc.capacity;
            this.start = oc.start;
            this.size = oc.size;
        }

        public Octets(int capacity)
        {
            if (capacity < 16)
                capacity = 16;

            this.capacity = capacity;
            this.buffer = new byte[16];
        }

        public Octets(int capacity, int initialSize)
        {
            if (capacity < 16)
                capacity = 16;

            if (initialSize < 16)
                initialSize = 16;

            if (initialSize > capacity)
                initialSize = capacity;

            this.capacity = capacity;
            this.buffer = new byte[initialSize];
        }

        public Octets(byte[] buffer, int capacity)
        {
            if (capacity < buffer.Length)
                throw new ArgumentException("wrong arg");
            this.capacity = capacity;
            this.buffer = buffer;
            this.size = buffer.Length;
        }

        public int Size
        {
            get { return size; }
        }

        public int Capacity
        {
            get { return capacity; }
        }


        public bool is_empty()
        {
            return this.size == 0;
        }

        public bool is_full()
        {
            return this.size == this.capacity;
        }

        public void clear()
        {
            this.start = 0;
            this.size = 0;
        }

        public Octets roundup()
        {
            if (this.buffer.Length < this.capacity)
            {
                int newLength = this.buffer.Length;
                newLength <<= 1;
                if (newLength > this.capacity)
                    newLength = this.capacity;

                byte[] newBuffer = new byte[newLength];
                this.copy_to(newBuffer, 0);
                this.start = 0;
                this.buffer = newBuffer;
                return this;
            }

            throw new Exception("buffer overflow");
        }

        public void copy_to(byte[] dstBuffer, int destPos)
        {
            int rightSize = this.buffer.Length - this.start;
            if (rightSize < this.size)
            {
                Array.Copy(this.buffer, this.start, dstBuffer, destPos, rightSize);
                Array.Copy(this.buffer, 0, dstBuffer, destPos + rightSize, this.size - rightSize);
            }
            else
            {
                Array.Copy(this.buffer, this.start, dstBuffer, destPos, this.size);
            }
        }

        public void copy_from(Octets x, int size)
        {
            if (size < 0 || size > x.Size)
                throw new Exception("wrong arg");

            if (size == 0)
                return;

            x.arrange(false);
            this.push(x.buffer, x.start, size);
        }

        public void reserve(int size)
        {
            if (size <= this.buffer.Length)
                return;

            if (size > this.capacity)
                throw new Exception("buffer overflow");

            int length = 16;
            while (length < size)
                length <<= 1;

            if (length > this.capacity)
                length = this.capacity;

            byte[] newBuffer = new byte[length];
            this.copy_to(newBuffer, 0);
            this.buffer = newBuffer;
            this.start = 0;
        }

        public byte[] getBytes()
        {
            return this.buffer;
        }

        public byte[] getBytesForWrite(out int offset, out int remainSize)
        {
            reserve(size + 1);

            int next = this.start + this.size;
            if (next < this.buffer.Length)
            {
                offset = next;
                remainSize = this.buffer.Length - next;
            }
            else
            {
                offset = next - this.buffer.Length;
                remainSize = this.buffer.Length - this.size;
            }
            return this.buffer;
        }

        public byte[] getBytesForRead(out int offset, out int remainSize)
        {
            int next = this.start + this.size;
            int length = (next <= this.buffer.Length) ? this.size : this.buffer.Length - this.start;
            offset = start;
            remainSize = length;
            return buffer;
        }

        public Octets push_rollback(int count)
        {
            if (count < 0 || count > size)
                throw new Exception("wrong argument");

            this.size -= count;
            return this;
        }

        public Octets push_count(int count)
        {
            if (count < 0)
                throw new Exception("wrong argument");

            this.size += count;
            this.reserve(this.size);
            return this;
        }

        public Octets pop_count(int count)
        {
            if (count < 0)
                throw new Exception("wrong argument");

            if (count > this.size)
                throw new Exception("buffer empty");

            this.start += count;
            if (this.start >= this.buffer.Length)
                this.start -= this.buffer.Length;
            this.size -= count;
            return this;
        }

        public Octets pop_rollback(int count)
        {
            if (count < 0)
                throw new Exception("wrong argument");

            if (count == 0)
                return this;

            this.size += count;
            this.reserve(this.size);
            this.start -= count;
            if (this.start < 0)
            {
                this.start += this.buffer.Length;
            }
            return this;
        }

        public Octets push(bool x)
        {
            return this.push((byte)(x ? 1 : 0));
        }

        public Octets push(byte x)
        {
            reserve(this.size + 1);
            return push_byte(x);
        }

        public Octets push(short x)
        {
            reserve(this.size + 2);
            return this.push_byte((byte)x).push_byte((byte)(x >> 8));
        }

        public Octets push(int x)
        {
            reserve(this.size + 4);
            return this.push_byte((byte)x).push_byte((byte)(x >> 8)).push_byte((byte)(x >> 16)).push_byte(
                    (byte)(x >> 24));
        }

        public Octets push(long x)
        {
            tmp[0] = (byte)x;
            tmp[1] = (byte)(x >> 8);
            tmp[2] = (byte)(x >> 16);
            tmp[3] = (byte)(x >> 24);
            tmp[4] = (byte)(x >> 32);
            tmp[5] = (byte)(x >> 40);
            tmp[6] = (byte)(x >> 48);
            tmp[7] = (byte)(x >> 56);
            return this.push(tmp, 0, 8);
        }

        public Octets push(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            int result = BitConverter.ToInt32(bytes, 0);
            return this.push(result);
        }

        public Octets push(double value)
        {
            return this.push(BitConverter.DoubleToInt64Bits(value));
        }

        public Octets push(String x)
        {
            try
            {
                byte[] buffer = System.Text.UTF8Encoding.UTF8.GetBytes(x);
                this.push(buffer.Length);
                if (buffer.Length > 0)
                {
                    return this.push(buffer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("wrong charset name");
            }

            return this;
        }

        public Octets push(Octets x)
        {
            x.arrange(false);
            this.push(x.size);
            if (x.size > 0)
            {
                return this.push(x.buffer, x.start, x.size);
            }
            return this;
        }

        public Octets push(Marshal x)
        {
            return x.marshal(this);
        }

        public bool pop_bool()
        {
            return this.pop_byte() == 1;
        }

        public bool pop_boolean()
        {
            return this.pop_byte() == 1;
        }

        public byte pop_byte()
        {
            if (this.size <= 0)
            {
                throw new Exception("buffer empty");
            }

            byte r = this.buffer[this.start];

            --this.size;
            ++this.start;
            if (this.start >= this.buffer.Length)
                this.start = 0;
            return r;
        }

        public short pop_short()
        {
            byte b0 = pop_byte();
            byte b1 = pop_byte();
            return (short)((b0 & 0xff) | (b1 << 8));
        }

        public int pop_int()
        {
            byte b0 = pop_byte();
            byte b1 = pop_byte();
            byte b2 = pop_byte();
            byte b3 = pop_byte();
            return (b0 & 0xff) | ((b1 & 0xff) << 8) | ((b2 & 0xff) << 16) | (b3 << 24);
        }

        public long pop_long()
        {
            pop(tmp, 0, 8);
            return (((long)tmp[0]) & 0xff) | ((((long)tmp[1]) & 0xff) << 8) | ((((long)tmp[2]) & 0xff) << 16)
                    | ((((long)tmp[3]) & 0xff) << 24) | ((((long)tmp[4]) & 0xff) << 32)
                    | ((((long)tmp[5]) & 0xff) << 40) | ((((long)tmp[6]) & 0xff) << 48) | (((long)tmp[7]) << 56);
        }

        public float pop_float()
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(this.pop_int()), 0);
        }

        public double pop_double()
        {
            return BitConverter.Int64BitsToDouble(this.pop_long());
        }

        public String pop_string()
        {
            int length = this.pop_int();
            if (length == 0)
                return "";

            if (length > 1048576)
            {
                throw new Exception("wrong string length : " + length);
            }

            byte[] buffer = new byte[length];
            this.pop(buffer);
            try
            {
                return System.Text.Encoding.UTF8.GetString(buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("wrong charset name");
            }
        }

        public Octets pop_octets()
        {
            int length = this.pop_int();
            if (length == 0)
            {
                return new Octets();
            }

            byte[] buffer = new byte[length];
            this.pop(buffer);
            return new Octets(buffer, Int32.MaxValue);
        }

        public Octets pop(Marshal x)
        {
            return x.unmarshal(this);
        }

        public Octets arrange(bool strict)
        {
            if (this.start == 0)
                return this;

            int next = this.start + this.size;
            if (!strict && (next <= this.buffer.Length))
                return this;

            byte[] tmpBuffer = new byte[this.size < 16 ? 16 : this.size];
            this.copy_to(tmpBuffer, 0);
            this.buffer = tmpBuffer;
            this.start = 0;
            return this;
        }

        public Octets push(byte[] src, int offset, int length)
        {
            this.reserve(this.size + length);

            int next = this.start + this.size;
            if (next >= this.buffer.Length)
                next -= this.buffer.Length;

            int right = this.buffer.Length - next;
            if (right < length)
            {
                Array.Copy(src, offset, this.buffer, next, right);
                Array.Copy(src, offset + right, this.buffer, 0, length - right);
            }
            else
            {
                Array.Copy(src, offset, this.buffer, next, length);
            }

            return this.push_count(length);
        }

        public Octets copy()
        {
            Octets copy = new Octets(this);
            copy.buffer = new byte[buffer.Length];
            Array.Copy(buffer, 0, copy.buffer, 0, buffer.Length);
            return copy;
        }

        private Octets push_byte(byte b)
        {
            int next = this.start + this.size;
            if (next >= this.buffer.Length)
                next -= this.buffer.Length;
            this.buffer[next] = b;
            ++this.size;
            return this;
        }

        private Octets push(byte[] src)
        {
            return this.push(src, 0, src.Length);
        }

        private Octets pop(byte[] dst, int offset, int length)
        {
            if (length > this.size)
            {
                throw new Exception("buffer underflow");
            }

            int right = this.buffer.Length - this.start;
            if (length > right)
            {
                Array.Copy(this.buffer, this.start, dst, offset, right);
                Array.Copy(this.buffer, 0, dst, offset + right, length - right);
            }
            else
            {
                Array.Copy(this.buffer, this.start, dst, offset, length);
            }

            return this.pop_count(length);
        }

        public Octets pop(byte[] dst)
        {
            return this.pop(dst, 0, dst.Length);
        }

        public override int GetHashCode()
        {
            return (int)hash();
        }

        public long hash()
        {
            arrange(true);

            long hash = MurmurHash.hash64(this.buffer, this.size);
            if (hash < 0)
                hash = -hash;
            return hash;
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
                return true;

            if (obj == null)
                return false;

            if (!GetType().Equals(obj.GetType()))
                return false;

            Octets other = (Octets)obj;
            if (this.size != other.size)
                return false;

            int index = 0;
            int otherIndex = 0;
            for (int i = 0; i < size; ++i)
            {
                index = start + i;
                otherIndex = other.start + i;

                if (index >= this.buffer.Length)
                    index = 0;

                if (otherIndex >= other.buffer.Length)
                    otherIndex = 0;

                if (this.buffer[index] != other.buffer[otherIndex])
                    return false;
            }

            return true;
        }

    }
}

