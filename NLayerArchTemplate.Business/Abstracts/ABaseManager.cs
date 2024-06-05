using AutoMapper;
using NLayerArchTemplate.DataAccess.Repositories.Interfaces;

namespace NLayerArchTemplate.Business.Abstracts;

public abstract class ABaseManager
{
    protected readonly IUnitOfWork _uow;
    protected readonly IMapper _mapper;

    protected ABaseManager(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    protected Exception GetException(string msg)
    {
        return new Exception(msg);
    }
}