using Application.Services.InterFaces.Mapping;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.Services.Implementaions.Mapping
{
    public class MappingService : IMappingServices
    {
        private readonly IMapper _Mapper;
        public MappingService(IMapper _Mapper)
        {
            this._Mapper = _Mapper;
            
        }
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _Mapper.Map<TDestination>(source);        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            _Mapper.Map(source, destination);
        }

        public IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return _Mapper.Map<IEnumerable<TDestination>>(source);
        }
    }
}
