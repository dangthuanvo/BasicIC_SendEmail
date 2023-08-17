using Repository.EF;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class BasicIC_SendEmailRepository<T> : BaseRepositorySql<T>, IRepositorySql<T> where T : class
    {
        public BasicIC_SendEmailRepository() : base(new M03_BasicEntities())
        {
        }
    }
}
