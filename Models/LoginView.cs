using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
	public class LoginView
	{
		[DataType(DataType.EmailAddress)]
		[Required]
		public string Usuario { get; set; }
		[DataType(DataType.Password)]
		[Required]
		public string Pass { get; set; }

		public string ToString(){
			return Usuario+" "+Pass;
		}
	}
}
