using AssetManager.Application.DTOs.Asset;
using AssetManager.Application.DTOs.AuditLog;
using AssetManager.Application.DTOs.Department;
using AssetManager.Application.DTOs.User;
using AssetManager.Application.Features.Asset.Commands.CreateAsset;
using AssetManager.Application.Features.Asset.Commands.UpdateAsset;
using AssetManager.Application.Features.Auth.Commands.Register;
using AssetManager.Application.Features.Department.Commands.CreateDepartment;
using AssetManager.Application.Features.Department.Commands.UpdateDepartment;
using AssetManager.Application.Features.User.Commands.UpdateUserAccess;
using AssetManager.Domain.Entities;
using AssetManager.Domain.Enums;
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
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.FullName : null))
            .ForMember(dest => dest.AssignedUserId, opt => opt.MapFrom(src => src.AssignedUserId))
       // DİNAMİK STATÜ KONTROLÜ:
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                (src.Status == AssetStatus.Assigned && src.AssignedUserId == null)
                ? AssetStatus.InStock // Eğer atanmış görünüyor ama kullanıcı yoksa 'InStock' (Boşta) yap
                : src.Status));

        CreateMap<AuditLogEntity, AuditLogResponseDto>();

        CreateMap<DepartmentEntity, DepartmentResponseDto>()
           .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count));

       
        // --- 2. COMMAND -> ENTITY (RequestDto'ların yerini alan yeni yapı) ---

        // CreateAssetRequestDto GİTTİ -> CreateAssetCommand GELDİ
        CreateMap<CreateAssetCommand, AssetEntity>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Price)); // ÖNEMLİ: Price'ı Value'ya eşle

        // RegisterRequestDto GİTTİ -> RegisterCommand GELDİ
        // Password alanını bilerek boş bırakıyoruz çünkü onu PasswordHasher ile set edeceğiz.
        CreateMap<RegisterCommand, AppUserEntity>()
             .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // CreateDepartmentRequestDto GİTTİ -> CreateDepartmentCommand GELDİ
        CreateMap<CreateDepartmentCommand, DepartmentEntity>();

        
        // Command -> Entity
        CreateMap<UpdateAssetCommand, AssetEntity>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.AssignedUserId, opt => opt.MapFrom(src => src.AssignedUserId));

        CreateMap<UpdateDepartmentCommand, DepartmentEntity>();

        CreateMap<UpdateUserAccessCommand, AppUserEntity>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<Roles>(src.NewRole, true)))
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // ID'yi ezmemek için ignore ediyoruz
    }
}