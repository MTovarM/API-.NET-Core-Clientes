namespace APIClientes
{
    using APIClientes.Models;
    using APIClientes.Models.Dto;
    using AutoMapper;

    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() 
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ClientDto, Client>();
                config.CreateMap<Client, ClientDto>();
            });
            return mappingConfig;
        }


    }
}
