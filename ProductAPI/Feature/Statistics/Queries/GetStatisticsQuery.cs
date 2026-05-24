using MediatR;
using ProductAPI.DTOs.Statistics;

namespace ProductAPI.Feature.Statistics.Queries
{
    //so here i will be using record as different concept 
    //it is the same as class but way shorter so it is more used 
    public record GetStatisticsQuery : IRequest<StatisticsDto>;
    //A small immutable request object for MediatR.
}
