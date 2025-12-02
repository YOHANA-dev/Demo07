using AutoMapper;
using GymSystemBLL.ViewModels.BookingViewModels;
using GymSystemBLL.ViewModels.MembershipViewModels;
using GymSystemBLL.ViewModels.SessionViewModels;
using GymSystemDAL.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL
{
    public class MappingProfiles : Profile
    {
        // Profile must be in CTOR
        public MappingProfiles()
        {
            //CreateMap<Session, SessionViewModel>();
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(dest => dest.AvailableSlots, Options => Options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<UpdateSessionViewModel, Session>().ReverseMap();

            CreateMap<Membership, MembershipViewModel>()
                .ForMember(dest => dest.MemberName, Options => Options.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.PlanName, Options => Options.MapFrom(src => src.Plan.Name))
                .ForMember(dest => dest.StartDate, Options => Options.MapFrom(src => src.CreatedAt));
            CreateMap<CreateMembershipViewModel, Membership>();

            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, Options => Options.MapFrom(src => src.CategoryName));

            CreateMap<MemberSession, MemberForSessionViewModel>()
                .ForMember(dest => dest.MemberName, Options => Options.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.BookingDate, Options => Options.MapFrom(src => src.CreatedAt));

            CreateMap<CreateBookingViewModel, MemberSession>();

            CreateMap<Plan, PlanForSelectListViewModel>();
            CreateMap<Member, MemberForSelectListViewModel>();
        }
    }
}
