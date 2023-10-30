// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("IJIRMiAdFhk6lliW5x0REREVEBOSER8QIJIRGhKSEREQiY7oJB+IIPZQwGjzvIf1T5FiBnEFX8OjZGuTpmx05u74TCde4/cuGmR2LFj9lOyLN55CKQo83RTGPZJp9ux/Bcgh8iGhEOfdCFIMNY/O+eUBXyvEYNY5gH8BhrOuoNq0cpe+kmotWxG500YiuPFXzUqjPrnB2A8iop448HTAajC7Mbw0AlK44sVR9fTyaQsOxATonW4arzcm/KZwEOqxiVZtMwfMsYoVx1gMn8cv7ZxRYYsZtb/OZFXMiepz8LSlD1aALkAFJDE4XGM3R/uZXlz8q28/nWBtHWuWGpLWF/86rfIbvn9OMSsfDdEOOxd7hhz8MRUfNlooKAY/16iq+xITERAR");
        private static int[] order = new int[] { 0,1,8,10,11,7,6,11,9,10,13,11,12,13,14 };
        private static int key = 16;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
