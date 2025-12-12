using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class RequestTypes : Model
    {
        public string requestTypeId;
        public string requestTypeName;
        public RequestTypes() { }

        public string RequestTypeId
        {
            get { return requestTypeId; }
            set { requestTypeId = value; }
        }   
        [Required(ErrorMessage ="Request Type Name can not be empty")]
        [StringLength(30,MinimumLength =3,ErrorMessage ="Request Type name needs to be between 3-30 characters")]
        [FirstLetterCapital()]
        public string RequestTypeName 
        {
            get { return requestTypeName; }
            set { requestTypeName = value; }
        }
    }
}
