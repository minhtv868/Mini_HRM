using IC.Domain.Enums.Identity;
using IC.Application.DTOs.DictData;

namespace IC.Application.Common.DictDataHelpers.Identity
{
	public static class UserStatusDtoHelper
	{
		public static List<StatusDto> GetIsEnabledList()
		{
			var statusDtoList = new List<StatusDto>
			{
				new StatusDto 
				{ 
					StatusId = (int)UserIsEnabledEnum.Active, StatusName = "Kích hoạt"
				},
				new StatusDto 
				{ 
					StatusId = (int)UserIsEnabledEnum.Deactive, StatusName = "Hủy kích hoạt"
				}
			};

			return statusDtoList;
		}

		public static List<StatusDto> GetTwoFactorEnabledList()
		{
			var statusDtoList = new List<StatusDto>
			{
				new StatusDto 
				{ 
					StatusId = (int)UserTwoFactorEnabledEnum.Active, StatusName = "Kích hoạt"
				},
				new StatusDto 
				{ 
					StatusId = (int)UserTwoFactorEnabledEnum.Deactive, StatusName = "Hủy kích hoạt"
				}
			};

			return statusDtoList;
		}
	}
}
