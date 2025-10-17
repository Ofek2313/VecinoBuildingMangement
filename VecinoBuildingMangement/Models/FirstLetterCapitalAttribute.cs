using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class FirstLetterCapitalAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {

           
           string[] words = value.ToString().Split(" ");
           foreach(string word in words)
           {
                char firstLetter = word[0];
                if (firstLetter > 'Z' || firstLetter < 'A')
                    return false;
                for (int i = 1; i < word.Length; i++)
                {
                    if (word[i] != ' ')
                    {
                        if (word[i] < 'a' || word[i] > 'z')
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
