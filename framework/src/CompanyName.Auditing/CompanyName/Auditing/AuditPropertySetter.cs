using CompanyName.Security.Users;
using Microsoft.Extensions.Logging;

namespace CompanyName.Auditing
{
    public class AuditPropertySetter : IAuditPropertySetter
    {
        private readonly ILogger<AuditPropertySetter> _logger;
        private readonly ICurrentUser _user;

        public AuditPropertySetter(ILogger<AuditPropertySetter> logger, ICurrentUser user)
        {
            _user = user;
            _logger = logger;
        }

        public void SetCreationProperties(object targetObject)
        {
            // TO-DO: use reflection to reduce code duplication
            switch (targetObject)
            {
                case ICreationAuditedObject<int> i:
                    SetCreationTime(i);
                    SetCreationUser(i);
                    return;
                case ICreationAuditedObject<long> l:
                    SetCreationTime(l);
                    SetCreationUser(l);
                    return;
                case ICreationAuditedObject<Guid> g:
                    SetCreationTime(g);
                    SetCreationUser(g);
                    return;
                default:

                    // Fallback
                    _logger.LogWarning("Creation auditing not supported for type {Type}", targetObject.GetType());
                    return;
            }
        }

        private void SetCreationTime(ICreationAuditedTimeObject auditableEntity)
        {
            if (auditableEntity.CreationTime == default)
            {
                auditableEntity.CreationTime = DateTime.UtcNow;
            }
        }

        private void SetCreationUser<T>(ICreationAuditedObject<T> auditableEntity)
            where T : struct, IParsable<T>
        {
            auditableEntity.CreatorId = _user?.GetId<T>();
        }

        public void SetModificationProperties(object targetObject)
        {

            switch (targetObject)
            {
                case IModificationAuditedObject<int> i:
                    SetModificationTime(i);
                    SetModificationUser(i);
                    return;
                case IModificationAuditedObject<long> l:
                    SetModificationTime(l);
                    SetModificationUser(l);
                    return;
                case IModificationAuditedObject<Guid> g:
                    SetModificationTime(g);
                    SetModificationUser(g);
                    return;
                default:

                    // Fallback
                     _logger.LogWarning("Modification auditing not supported for type {Type}", targetObject.GetType());

                    return;
            }
        }

        private void SetModificationTime(IModificationAuditedTimeObject auditableEntity)
        {
            auditableEntity.LastModificationTime = DateTime.Now;
        }

        private void SetModificationUser<T>(IModificationAuditedObject<T> auditableEntity)
            where T : struct, IParsable<T>
        {
            auditableEntity.LastModifierId = _user?.GetId<T>();
        }

        public void SetDeletionProperties(object targetObject)
        {

            switch (targetObject)
            {
                case IDeletionAuditedObject<int> i when i.IsDeleted:
                    SetDeletionTime(i);
                    SetDeletionUser(i);
                    return;
                case IDeletionAuditedObject<long> l when l.IsDeleted:
                    SetDeletionTime(l);
                    SetDeletionUser(l);
                    return;
                case IDeletionAuditedObject<Guid> g when g.IsDeleted:
                    SetDeletionTime(g);
                    SetDeletionUser(g);
                    return;
                default:

                    // Fallback
                    _logger.LogWarning("Deletion auditing not supported for type {Type}", targetObject.GetType());

                    return;
            }
        }

        private void SetDeletionTime(IDeletionAuditedTimeObject auditableEntity)
        {
            auditableEntity.DeletionTime = DateTime.Now;
        }

        private void SetDeletionUser<T>(IDeletionAuditedObject<T> auditableEntity)
            where T : struct, IParsable<T>
        {
            auditableEntity.DeleterId = _user?.GetId<T>();
        }
    }
}
