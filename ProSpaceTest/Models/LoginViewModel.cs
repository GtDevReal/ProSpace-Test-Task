﻿using System.ComponentModel.DataAnnotations;

namespace ProSpaceTest.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Введите почту")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "Введите пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Запомнить")]
		public bool RememberMe { get; set; }
	}
}
