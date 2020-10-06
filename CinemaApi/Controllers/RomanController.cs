using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RomanController : ControllerBase
    {

        // GET api/<ValuesController>/5
        [HttpGet("{number}")]
     
        public string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
        }
        // returns the value for a roman literal
        private static int romanValue(int index)
        {
            int basefactor = ((index % 2) * 4 + 1); // either 1 or 5...
                                                    // ...multiplied with the exponentation of 10, if the literal is `x` or higher
            return index > 1 ? (int)(basefactor * System.Math.Pow(10.0, index / 2)) : basefactor;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{str}")]
        public  int FromRoman(string str)
        {
            str = str.ToLower();
            string literals = "mdclxvi";
            int value = 0, index = 0;
            foreach (char literal in literals)
            {
                value = romanValue(literals.Length - literals.IndexOf(literal) - 1);
                index = str.IndexOf(literal);
                if (index > -1)
                    return FromRoman(str.Substring(index + 1)) + (index > 0 ? value - FromRoman(str.Substring(0, index)) : value);
            }
            return 0;
        }


    }
}
