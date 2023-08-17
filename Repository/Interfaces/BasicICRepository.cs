using Common.Params.Base;
using Repository.CustomModel;
using Repository.EF;
using System;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IBasicIC_SendEmailRepository<T> : IDisposable where T : class
    {
        Task<T> Create(T obj, M03_BasicEntities dbContext = null);
        Task<T> GetById(object obj, M03_BasicEntities dbContext = null);
        Task<ListResult<T>> GetAll(PagingParam param, M03_BasicEntities dbContext = null);
        Task<T> Update(T obj, M03_BasicEntities dbContext = null);
        Task<bool> Delete(object obj, M03_BasicEntities dbContext = null);
    }
}
