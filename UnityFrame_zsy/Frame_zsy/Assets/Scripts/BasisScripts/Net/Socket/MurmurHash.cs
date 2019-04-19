using System;

namespace Share
{
    public sealed class MurmurHash
    {
	    /**
	     * Generates 32 bit hash from byte array of the given length and seed.
	     * 
	     * @param data
	     *            byte array to hash
	     * @param length
	     *            length of the array to hash
	     * @param seed
	     *            initial seed value
	     * @return 32 bit hash of the given array
	     */
	    public static int hash32(byte[] data, int length, int seed)
	    {
		    // 'm' and 'r' are mixing constants generated offline.
		    // They're not really 'magic', they just happen to work well.
		    int m = 0x5bd1e995;
		    int r = 24;
		    // Initialize the hash to a random value
		    int h = seed ^ length;
		    int length4 = length / 4;

		    for (int i = 0; i < length4; i++)
		    {
			    int i4 = i * 4;
			    int k = (data[i4 + 0] & 0xff) + ((data[i4 + 1] & 0xff) << 8) + ((data[i4 + 2] & 0xff) << 16)
					    + ((data[i4 + 3] & 0xff) << 24);
			    k *= m;
			    k ^= rightShiftWithoutSign(k,r);
			    k *= m;
			    h *= m;
			    h ^= k;
		    }

		    // Handle the last few bytes of the input array
		    switch (length % 4)
		    {
                case 3:
			         h ^= (data[(length & ~3) + 2] & 0xff) << 16;
                     h ^= (data[(length & ~3) + 1] & 0xff) << 8;
                     h ^= (data[length & ~3] & 0xff);
			         h *= m;
                     break;
		        case 2:
			        h ^= (data[(length & ~3) + 1] & 0xff) << 8;
                    h ^= (data[length & ~3] & 0xff);
			        h *= m;
                    break;
		        case 1:
			        h ^= (data[length & ~3] & 0xff);
			        h *= m;
                    break;
		    }

		    h ^= rightShiftWithoutSign(h,13);
		    h *= m;
		    h ^= rightShiftWithoutSign(h,15);

		    return h;
	    }

	    /**
	     * Generates 32 bit hash from byte array with default seed value.
	     * 
	     * @param data
	     *            byte array to hash
	     * @param length
	     *            length of the array to hash
	     * @return 32 bit hash of the given array
	     */
	    public static int hash32(byte[] data, int length)
	    {
		    return hash32(data, length, unchecked ((int)0x9747b28c));
	    }

	    /**
	     * Generates 32 bit hash from a string.
	     * 
	     * @param text
	     *            string to hash
	     * @return 32 bit hash of the given string
	     */
	    public static int hash32(String text)
	    {
		    byte[] bytes = System.Text.Encoding.Default.GetBytes(text);
		    return hash32(bytes, bytes.Length);
	    }

	    /**
	     * Generates 32 bit hash from a substring.
	     * 
	     * @param text
	     *            string to hash
	     * @param from
	     *            starting index
	     * @param length
	     *            length of the substring to hash
	     * @return 32 bit hash of the given string
	     */
	    public static int hash32(String text, int from, int length)
	    {
		    return hash32(text.Substring(from, length));
	    }

	    /**
	     * Generates 64 bit hash from byte array of the given length and seed.
	     * 
	     * @param data
	     *            byte array to hash
	     * @param length
	     *            length of the array to hash
	     * @param seed
	     *            initial seed value
	     * @return 64 bit hash of the given array
	     */
	    public static long hash64(byte[] data, int length, int seed)
	    {
		    long m = unchecked((long)0xc6a4a7935bd1e995);
		    int r = 47;

		    long h = (seed & 0xffffffffL) ^ (length * m);

		    int length8 = length / 8;

		    for (int i = 0; i < length8; i++)
		    {
			    int i8 = i * 8;
			    long k = ((long) data[i8 + 0] & 0xff) + (((long) data[i8 + 1] & 0xff) << 8)
					    + (((long) data[i8 + 2] & 0xff) << 16) + (((long) data[i8 + 3] & 0xff) << 24)
					    + (((long) data[i8 + 4] & 0xff) << 32) + (((long) data[i8 + 5] & 0xff) << 40)
					    + (((long) data[i8 + 6] & 0xff) << 48) + (((long) data[i8 + 7] & 0xff) << 56);

			    k *= m;
			    k ^= rightShiftWithoutSign(k,r);
			    k *= m;

			    h ^= k;
			    h *= m;
		    }

		    switch (length % 8)
		    {
		    case 7:
			    h ^= (long) (data[(length & ~7) + 6] & 0xff) << 48;
                h ^= (long) (data[(length & ~7) + 5] & 0xff) << 40;
                h ^= (long) (data[(length & ~7) + 4] & 0xff) << 32;
                h ^= (long) (data[(length & ~7) + 3] & 0xff) << 24;
                h ^= (long) (data[(length & ~7) + 2] & 0xff) << 16;
                h ^= (long) (data[(length & ~7) + 1] & 0xff) << 8;
                h ^= (long) (data[length & ~7] & 0xff);
			    h *= m;
                break;
		    case 6:
			    h ^= (long) (data[(length & ~7) + 5] & 0xff) << 40;
                h ^= (long) (data[(length & ~7) + 4] & 0xff) << 32;
                h ^= (long) (data[(length & ~7) + 3] & 0xff) << 24;
                h ^= (long) (data[(length & ~7) + 2] & 0xff) << 16;
                h ^= (long) (data[(length & ~7) + 1] & 0xff) << 8;
                h ^= (long) (data[length & ~7] & 0xff);
			    h *= m;
                break;
		    case 5:
			    h ^= (long) (data[(length & ~7) + 4] & 0xff) << 32;
                h ^= (long) (data[(length & ~7) + 3] & 0xff) << 24;
                h ^= (long) (data[(length & ~7) + 2] & 0xff) << 16;
                h ^= (long) (data[(length & ~7) + 1] & 0xff) << 8;
                h ^= (long) (data[length & ~7] & 0xff);
			    h *= m;
                break;
		    case 4:
			    h ^= (long) (data[(length & ~7) + 3] & 0xff) << 24;
                h ^= (long) (data[(length & ~7) + 2] & 0xff) << 16;
                h ^= (long) (data[(length & ~7) + 1] & 0xff) << 8;
                h ^= (long) (data[length & ~7] & 0xff);
			    h *= m;
                break;
		    case 3:
			    h ^= (long) (data[(length & ~7) + 2] & 0xff) << 16;
                h ^= (long) (data[(length & ~7) + 1] & 0xff) << 8;
                h ^= (long) (data[length & ~7] & 0xff);
			    h *= m;
                break;
		    case 2:
			    h ^= (long) (data[(length & ~7) + 1] & 0xff) << 8;
                h ^= (long) (data[length & ~7] & 0xff);
			    h *= m;
                break;
		    case 1:
			    h ^= (long) (data[length & ~7] & 0xff);
			    h *= m;
                break;
		    }
		    ;

		    h ^= rightShiftWithoutSign(h,r);
		    h *= m;
		    h ^= rightShiftWithoutSign(h,r);

		    return h;
	    }

	    /**
	     * Generates 64 bit hash from byte array with default seed value.
	     * 
	     * @param data
	     *            byte array to hash
	     * @param length
	     *            length of the array to hash
	     * @return 64 bit hash of the given string
	     */
	    public static long hash64(byte[] data, int length)
	    {
		    return hash64(data, length, unchecked((int)0xe17a1465));
	    }

	    /**
	     * Generates 64 bit hash from a string.
	     * 
	     * @param text
	     *            string to hash
	     * @return 64 bit hash of the given string
	     */
	    public static long hash64(String text)
	    {
		    byte[] bytes = System.Text.Encoding.Default.GetBytes(text);
		    return hash64(bytes, bytes.Length);
	    }

	    /**
	     * Generates 64 bit hash from a substring.
	     * 
	     * @param text
	     *            string to hash
	     * @param from
	     *            starting index
	     * @param length
	     *            length of the substring to hash
	     * @return 64 bit hash of the given array
	     */
	    public static long hash64(String text, int from, int length)
	    {
		    return hash64(text.Substring(from,length));
	    }


        private static int rightShiftWithoutSign(int i, int shift)
        {
            if (i >= 0) return i >> shift;

            shift = shift % 32;
            int mask = 0x7fffffff;
            for (int index = 0; index < shift; index++)
            {
                i >>= 1;
                i &= mask;
            }
            return i;
        }

        private static long rightShiftWithoutSign(long i, int shift)
        {
            if (i >= 0) return i >> shift;

            shift = shift % 64;
            long mask = 0x7fffffffffffffff;
            for (int index = 0; index < shift; index++)
            {
                i >>= 1;
                i &= mask;
            }
            return i;
        }
    }

}
