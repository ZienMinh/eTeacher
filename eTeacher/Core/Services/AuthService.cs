﻿using eTeacher.Core.DTO;
using eTeacher.Core.Interfaces;
using eTeacher.Core.OtherObjects;
using eTeacher.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eTeacher.Core.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

		public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
		}

		public async Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto)
		{
			var user = await _userManager.FindByNameAsync(loginDto.UserName);

			if (user is null)
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "Invalid Credentials"
				};

			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

			if (!isPasswordCorrect)
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "Invalid Credentials"
				};

			var userRoles = await _userManager.GetRolesAsync(user);

			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim("JWTID", Guid.NewGuid().ToString()),
				new Claim("FirstName", user.First_name),
				new Claim("LastName", user.Last_name),
			};

			foreach (var userRole in userRoles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, userRole));
			}

			var token = GenerateNewJsonWebToken(authClaims);

			return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = token
			};
		}

		public async Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
		{
			var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

			if (user is null)
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "Invalid User name!"
				};

			await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);

			return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = "User is now an ADMIN"
			};
		}

		public async Task<AuthServiceResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto)
		{
			var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

			if (user is null)
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "Invalid User name!"
				};

			await _userManager.AddToRoleAsync(user, StaticUserRoles.OWNER);

			return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = "User is now an OWNER"
			};
		}

		public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
		{
			var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);

			if (isExistsUser != null)
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "UserName Already Exists"
				};

			User newUser = new User()
			{
				First_name = registerDto.FirstName,
				Last_name = registerDto.LastName,
				Email = registerDto.Email,
				UserName = registerDto.UserName,
				SecurityStamp = Guid.NewGuid().ToString(),
			};

			var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

			if (!createUserResult.Succeeded)
			{
				var errorString = "User Creation Failed Beacause: ";
				foreach (var error in createUserResult.Errors)
				{
					errorString += " # " + error.Description;
				}
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = errorString
				};
			}

			// Add a Default USER Role to all users
			await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);

			return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = "User Created Successfully"
			};
		}

		public async Task<AuthServiceResponseDto> SeedRolesAsync()
		{
			bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
			bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
			bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

			if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists)
				return new AuthServiceResponseDto()
				{
					IsSucceed = true,
					Message = "Roles Seeding is Already Done"
				};

			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));

			return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = "Role Seeding Done Successfully"
			};
		}

		private string GenerateNewJsonWebToken(List<Claim> claims)
		{
			var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

			var tokenObject = new JwtSecurityToken(
					issuer: _configuration["JWT:ValidIssuer"],
					audience: _configuration["JWT:ValidAudience"],
					expires: DateTime.Now.AddHours(1),
					claims: claims,
					signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
				);

			string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

			return token;
		}
	}
}

