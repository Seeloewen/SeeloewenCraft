using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SeeloewenCraft
{
    public class KeyConverter
    {
        public string KeyToString(Key key)
        {
            //Convert the key to a string
            return key.ToString();
        }

        public Key StringToKey(string str)
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
