using AssetManager.Application.DTOs.Asset;
using AssetManager.Application.DTOs.AuditLog;
using AssetManager.Application.DTOs.Auth;
using AssetManager.Application.DTOs.Department;
using AssetManager.Application.DTOs.User;
using AssetManager.Core.Entities;
using AutoMapper;

namespace AssetManager.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity -> DTO Dönüşümü
        CreateMap<AppUserEntity, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : "N/A"));


        // Entity -> Response DTO
        CreateMap<AssetEntity, AssetResponseDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Value)) // ÖNEMLİ: Value'yu Price'a eşle
            .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.FullName : "None"))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        // Request DTO -> Entity
        CreateMap<CreateAssetRequestDto, AssetEntity>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Price)); // ÖNEMLİ: Price'ı Value'ya eşle

        // RegisterRequestDto -> AppUserEntity dönüşümü
        // Password alanını bilerek boş bırakıyoruz çünkü onu PasswordHasher ile set edeceğiz.
        CreateMap<RegisterRequestDto, AppUserEntity>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        CreateMap<AuditLogEntity, AuditLogResponseDto>();

        CreateMap<DepartmentEntity, DepartmentResponseDto>()
           .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count));

        CreateMap<CreateDepartmentRequestDto, DepartmentEntity>();
    }
}