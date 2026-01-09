using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class IsDigitAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value == null) return false;
            string word = value.ToString();

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] < '0' || word[i] > '9' )
                    return false;

            }
            return true;
        }
    }
}
