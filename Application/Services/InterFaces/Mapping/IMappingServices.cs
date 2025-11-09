using Application.Dtos;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces.Mapping
{
    public interface IMappingServices
    {
        TDestination Map<TSource, TDestination>(TSource source);

        IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source);

        void Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
