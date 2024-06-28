using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	public static class StaticUserRoles
	{
        public const byte MODERATOR = 4;
        public const byte TUTOR = 3;
		public const byte ADMIN = 2;
		public const byte USER = 1;

		public static string GetRoleName(byte role)
		{
			return role switch
			{
                MODERATOR => nameof(MODERATOR),
                TUTOR => nameof(TUTOR),
				ADMIN => nameof(ADMIN),
				USER => nameof(USER),
				_ => nameof(USER),
			};
		}

		public static byte GetRoleValue(string role)
		{
			return role.ToUpper() switch
			{
                nameof(MODERATOR) => MODERATOR,
                nameof(TUTOR) => TUTOR,
				nameof(ADMIN) => ADMIN,
				nameof(USER) => USER,
				_ => USER,
			};
		}
	}
}
