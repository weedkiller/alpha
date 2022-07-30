using Multigroup.Repositories.Shared;
using System;

namespace Multigroup.Services.Shared
{
    public class EntityLogService
    {
        private EntityLogRepository _entityLogRepository;

        public EntityLogService()
        {
            _entityLogRepository = new EntityLogRepository();
        }


        public void Insert(DateTime date, string action, int UserId, string EntityNew, string EntityOld, string AdditionalData)
        {
            _entityLogRepository.Insert(date, action, UserId, EntityNew, EntityOld, AdditionalData);
        }
    }
}