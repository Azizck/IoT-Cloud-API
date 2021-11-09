using AutoMapper;
using packagesentinel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Automapper {
    public class MappingProfile:Profile {
        public MappingProfile() {
            CreateMap<ApplicationUser, User>();
        }
    }
}
