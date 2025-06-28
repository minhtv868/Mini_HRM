using Microsoft.AspNetCore.Identity;

namespace WebJob.Areas.Identity
{
	public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
	{
		public override IdentityError DuplicateEmail(string email)
		{
			return new IdentityError
			{
				Code = nameof(DuplicateEmail),
				Description = $"Email {email} đã được sử dụng."
			};
		}

		public override IdentityError DuplicateUserName(string userName)
		{
			return new IdentityError
			{
				Code = nameof(DuplicateUserName),
				Description = $"Tài khoản {userName} đã được sử dụng."
			};
		}

		public override IdentityError InvalidEmail(string email)
		{
			return new IdentityError
			{
				Code = nameof(InvalidEmail),
				Description = $"Định dạng email {email} không hợp lệ."
			};
		}

		public override IdentityError DuplicateRoleName(string role)
		{
			return new IdentityError
			{
				Code = nameof(DuplicateRoleName),
				Description = $"Vai trò {role} đã được sử dụng."
			};
		}

		public override IdentityError InvalidRoleName(string role)
		{
			return new IdentityError
			{
				Code = nameof(InvalidRoleName),
				Description = $"Vai trò ${role} không hợp lệ."
			};
		}

		public override IdentityError InvalidToken()
		{
			return new IdentityError
			{
				Code = nameof(InvalidToken),
				Description = "Mã token không hợp lệ."
			};
		}

		public override IdentityError InvalidUserName(string userName)
		{
			return new IdentityError
			{
				Code = nameof(InvalidUserName),
				Description = $"Tên đăng nhập {userName} không đúng."
			};
		}

		public override IdentityError LoginAlreadyAssociated()
		{
			return new IdentityError
			{
				Code = nameof(LoginAlreadyAssociated),
				Description = "Tên đăng nhập đã được liên kết."
			};
		}

		public override IdentityError PasswordMismatch()
		{
			return new IdentityError
			{
				Code = nameof(PasswordMismatch),
				Description = "Mật khẩu không đúng."
			};
		}

		public override IdentityError PasswordRequiresDigit()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresDigit),
				Description = "Mật khẩu phải có ký tự số."
			};
		}

		public override IdentityError PasswordRequiresLower()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresLower),
				Description = "Mật khẩu phải có ký tự viết thường."
			};
		}

		public override IdentityError PasswordRequiresNonAlphanumeric()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresNonAlphanumeric),
				Description = "Mật khẩu phải có ký tự đặc biệt."
			};
		}

		public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresUniqueChars),
				Description = "Mật khẩu phải chứa ký tự duy nhất."
			};
		}

		public override IdentityError PasswordRequiresUpper()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresUpper),
				Description = "Mật khẩu phải có ký tự viết hoa."
			};
		}

		public override IdentityError PasswordTooShort(int length)
		{
			return new IdentityError
			{
				Code = nameof(PasswordTooShort),
				Description = "Mật khẩu quá ngắn."
			};
		}

		public override IdentityError UserAlreadyHasPassword()
		{
			return new IdentityError
			{
				Code = nameof(UserAlreadyHasPassword),
				Description = "Tài khoản đã có mật khẩu."
			};
		}

		public override IdentityError UserAlreadyInRole(string role)
		{
			return new IdentityError
			{
				Code = nameof(UserAlreadyInRole),
				Description = $"Tài khoản đã có vai trò {role}."
			};
		}

		public override IdentityError UserNotInRole(string role)
		{
			return new IdentityError
			{
				Code = nameof(UserNotInRole),
				Description = $"Tài khoản không có vai trò {role}."
			};
		}

		public override IdentityError UserLockoutNotEnabled()
		{
			return new IdentityError
			{
				Code = nameof(UserLockoutNotEnabled),
				Description = "UserLockoutNotEnabled"
			};
		}

		public override IdentityError RecoveryCodeRedemptionFailed()
		{
			return new IdentityError
			{
				Code = nameof(RecoveryCodeRedemptionFailed),
				Description = "RecoveryCodeRedemptionFailed"
			};
		}

		public override IdentityError ConcurrencyFailure()
		{
			return new IdentityError
			{
				Code = nameof(ConcurrencyFailure),
				Description = "ConcurrencyFailure"
			};
		}

		public override IdentityError DefaultError()
		{
			return new IdentityError
			{
				Code = nameof(DefaultError),
				Description = "Lỗi chưa xác định."
			};
		}
	}
}
