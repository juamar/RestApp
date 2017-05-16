using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using RestApp.Models;
using RestApp.Dtos;

namespace RestApp.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Friendship, FriendshipDto>();
            Mapper.CreateMap<FriendshipDto, Friendship>();
            Mapper.CreateMap<User, UserDto>();
            Mapper.CreateMap<UserDto, User>();
            Mapper.CreateMap<Conversation, ConversationDto>();
            Mapper.CreateMap<ConversationDto, Conversation>();
        }
    }
}