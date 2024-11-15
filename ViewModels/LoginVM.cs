﻿using Microsoft.Build.Framework;

namespace Projeto.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public bool RememberMe { get; set; }
    }
}
