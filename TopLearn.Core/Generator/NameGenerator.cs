using System;
using System.Collections.Generic;
using System.Text;

namespace TopLearn.Core.Generator
{
    public class NameGenerator
    {
        public static string GenerateUniqCode() => Guid.NewGuid().ToString().Replace("-", "");


    }
}
