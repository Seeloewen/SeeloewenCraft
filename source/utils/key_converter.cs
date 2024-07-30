using System;
using System.Windows.Input;

namespace SeeloewenCraft
{
    public static class KeyConverter
    {
        public static string KeyToString(Key key)
        {
            //Convert the key to a string
            return key.ToString();
        }

        public static Key StringToKey(string str)
        {
            //Try to convert the string to a 
            if (Enum.TryParse(str, out Key key))
            {
                return key;
            }
            else
            {
                throw new ArgumentException("Error: An invalid key string was given.");
            }
        }
    }
}
