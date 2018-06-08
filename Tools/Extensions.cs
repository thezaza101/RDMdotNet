using System;
using System.Collections.Generic;

namespace RDMdotNet
{
    internal static class Extensions
    {
        public static T GetProperty<T> (this Dictionary<string, object> input, string index)
        {
            return (T)input[index];
        }
        public static Type GetPropertyType (this Dictionary<string, object> input, string index)
        {
            return input[index].GetType();
        }
    }
}