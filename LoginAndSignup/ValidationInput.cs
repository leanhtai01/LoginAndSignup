using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LoginAndSignup
{
    public class ValidationInput
    {
        /// <summary>
        /// Check whether input string satisfy a pattern
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public bool IsValid(string inputString, string pattern)
        {
            Regex regex = new Regex(pattern);

            return regex.IsMatch(inputString);
        }
    }
}
