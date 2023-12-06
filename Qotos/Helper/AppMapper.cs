using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Qotos.Models;

namespace Qotos.Helper
{
	public class AppMapper : Profile
	{
		public AppMapper()
		{
			CreateMap<AppUser, IdentityUser>().ReverseMap();
		}
	}
}
