using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.sf.jabref.imports 
{
    public static class ExtensionMethods
    {
        public static StringBuilder reverse(this StringBuilder sb) 
        {
            var cs = sb.ToString().ToCharArray();
            Array.Reverse(cs);
            return new StringBuilder(new string(cs));
        }
    }
}
