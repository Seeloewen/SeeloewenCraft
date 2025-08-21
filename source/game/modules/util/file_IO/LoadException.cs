using System;

namespace SeeloewenCraft.game.util
{
    public class LoadException : Exception
    {

        public LoadException(string message) : base(message) { }
        public LoadException(string message, Exception innerException) : base(message, innerException) { }

    }

    public class JsonUtilException : Exception
    {

        public JsonUtilException(string message) : base(message) { }
        public JsonUtilException(string message, Exception innerException) : base(message, innerException) { }


    }


}
