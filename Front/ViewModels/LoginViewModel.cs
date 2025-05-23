﻿using System.ComponentModel.DataAnnotations;

namespace Front.ViewModels;

public class LoginViewModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
