using System.Security.Principal;
using Vesta.Auditing.Abstracts;
using Vesta.Security.Claims;
using Vesta.Security.Users;

namespace Vesta.Auditing
{
    public class AuditPropertySetter : IAuditPropertySetter
    {

        private readonly ICurrentUser _user;

        public AuditPropertySetter(ICurrentUser user)
        {
            _user = user;
        }

        public void SetCreationProperties(object targetObject)
        {
            if (targetObject is ICreationAuditedObject auditableEntity)
            {
                SetCreationTime(auditableEntity);
                SetCreationUser(auditableEntity);
            }
        }

        private void SetCreationTime(ICreationAuditedObject auditableEntity)
        {
            auditableEntity.CreationTime = DateTime.Now;
        }

        private void SetCreationUser(ICreationAuditedObject auditableEntity)
        {
            auditableEntity.CreatorId = _user?.Id;
        }

        public void SetModificationProperties(object targetObject)
        {
            if (targetObject is IModificationAuditedObject auditableEntity)
            {
                SetModificationTime(auditableEntity);
                SetModificationUser(auditableEntity);
            }
        }

        private void SetModificationTime(IModificationAuditedObject auditableEntity)
        {
            auditableEntity.LastModificationTime = DateTime.Now;
        }

        private void SetModificationUser(IModificationAuditedObject auditableEntity)
        {
            auditableEntity.LastModifierId = _user?.Id;
        }

        public void SetDeletionProperties(object targetObject)
        {
            if (targetObject is IDeletionAuditedObject auditableEntity
                && auditableEntity.IsDeleted)
            {
                SetDeletionTime(auditableEntity);
                SetDeletionUser(auditableEntity);
            }
        }

        private void SetDeletionTime(IDeletionAuditedObject auditableEntity)
        {
            auditableEntity.DeletionTime = DateTime.Now;
        }

        private void SetDeletionUser(IDeletionAuditedObject auditableEntity)
        {
            auditableEntity.DeleterId = _user?.Id;
        }
    }
}
