using AutoMapper;
using Purchasing.Domain.DTOs;
using Purchasing.Domain.DTOs.PurchaseOrder;
using Purchasing.Domain.DTOs.PurchaseOrderItems;
using Purchasing.Domain.Models;

namespace Purchasing.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<PurchaseOrderItem, PurchaseOrderItemDTO>().ReverseMap();
            CreateMap<PurchaseOrderItem, PurchaseOrderItemUpdateDTO>().ReverseMap();
            CreateMap<PurchaseOrderItem, PurchaseOrderItemCreateDTO>().ReverseMap();
            CreateMap<PurchaseOrderItem, PurchaseOrderItemPagination>().ReverseMap();

            CreateMap<PurchaseOrderDTO, PurchaseOrder>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<PurchaseOrderItem, PurchaseOrderItemReadDto>();
            //CreateMap<PurchaseOrder, PurchaseOrderReadDTO>()
            //    .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString())) // Map enum to string
            //    .ForMember(dest => dest.ActivationState, opt => opt.MapFrom(src => src.ActivationState.ToString())) // Map enum to string
            //    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));



            // Mapping for PurchaseOrderItem to PurchaseOrderItemReadDto
            CreateMap<PurchaseOrderItemMapping, PurchaseOrderItemReadDto>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.PurchaseOrderItem.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PurchaseOrderItem.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PurchaseOrderItem.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            // Mapping for PurchaseOrder to PurchaseOrderReadDTO
            CreateMap<PurchaseOrder, PurchaseOrderReadDTO>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString())) // Map enum to string
                .ForMember(dest => dest.ActivationState, opt => opt.MapFrom(src => src.ActivationState.ToString())) // Map enum to string
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.PurchaseOrderItemMappings));



        }
    }
}
