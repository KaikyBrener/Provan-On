using ApiProvaOnline.Application.DTOs;
using ApiProvaOnline.Application.DTOs.Responses;
using ApiProvaOnline.Models;
using AutoMapper;

namespace ApiProvaOnline.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Maps from Entities to DTOs and vice-versa
            CreateMap<Professor, ProfessorDTO>().ReverseMap();
            CreateMap<Administrativo, AdministrativoDTO>().ReverseMap();
            CreateMap<Aluno, AlunoDTO>().ReverseMap();
            CreateMap<Prova, ProvaDTO>().ReverseMap();
        }
    }
}
