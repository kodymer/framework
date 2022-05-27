using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vesta.Auditing.Abstracts;
using Vesta.Core;
using Vesta.Core.Reflection;
using Vesta.Ddd.Domain;
using Vesta.Ddd.Domain.Entities;

namespace Vesta.EntityFrameworkCore.Modeling
{
    public static class EntityTypeBuilderExtensions
    {
        public static void ConfigureCreationAuditedAggregateRoot<T>(this EntityTypeBuilder<T> b)
            where T : class
        {
            b.TryConfigureConcurrencyStamp();
            b.TryConfigureCreationAudited();
        }

        public static void ConfigureAuditedAggregateRoot<T>(this EntityTypeBuilder<T> b)
            where T : class
        {
            b.TryConfigureConcurrencyStamp();
            b.TryConfigureAudited();
        }

        public static void ConfigureFullAuditedAggregateRoot<T>(this EntityTypeBuilder<T> b)
            where T : class
        {
            b.TryConfigureConcurrencyStamp();
            b.TryConfigureFullAudited();
        }

        public static void ConfigureConcurrencyStamp<T>(this EntityTypeBuilder<T> b)
            where T : class, IHasConcurrencyStamp
        {
            b.TryConfigureConcurrencyStamp();
        }

        public static void TryConfigureConcurrencyStamp(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IHasConcurrencyStamp>())
            {
                b.Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                    .IsConcurrencyToken()
                    .HasMaxLength(ConcurrencyStampConsts.MaxLength)
                    .HasColumnName(nameof(IHasConcurrencyStamp.ConcurrencyStamp));
            }
        }

        public static void ConfigureAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, IAuditedObject
        {
            b.TryConfigureAudited();
        }

        public static void TryConfigureAudited(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IAuditedObject>())
            {
                b.TryConfigureCreationAudited();
                b.TryConfigureModificationAudited();
            }
        }

        public static void ConfigureFullAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, IFullAuditedObject
        {
            b.TryConfigureFullAudited();
        }

        public static void TryConfigureFullAudited(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IFullAuditedObject>())
            {
                b.TryConfigureCreationAudited();
                b.TryConfigureModificationAudited();
                b.TryConfigureDeletionAudited();
            }
        }

        public static void ConfigureCreationAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, ICreationAuditedObject
        {
            b.TryConfigureCreationAudited();
        }

        public static void TryConfigureCreationAudited(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<ICreationAuditedObject>())
            {
                b.Property(nameof(ICreationAuditedObject.CreationTime))
                    .IsRequired()
                    .HasColumnName(nameof(ICreationAuditedObject.CreationTime));

                b.Property(nameof(ICreationAuditedObject.CreatorId))
                    .IsRequired(false)
                    .HasColumnName(nameof(ICreationAuditedObject.CreatorId));
            }
        }

        public static void ConfigureModificationAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, IModificationAuditedObject
        {
            b.TryConfigureModificationAudited();
        }

        public static void TryConfigureModificationAudited(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IModificationAuditedObject>())
            {
                b.Property(nameof(IModificationAuditedObject.LastModificationTime))
                    .IsRequired(false)
                    .HasColumnName(nameof(IModificationAuditedObject.LastModificationTime));

                b.Property(nameof(IModificationAuditedObject.LastModifierId))
                    .IsRequired(false)
                    .HasColumnName(nameof(IModificationAuditedObject.LastModifierId));
            }
        }

        public static void ConfigureDeletionAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, IDeletionAuditedObject
        {
            b.TryConfigureDeletionAudited();
        }

        public static void TryConfigureDeletionAudited(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IDeletionAuditedObject>())
            {
                b.TryConfigureSoftDelete();

                b.Property(nameof(IDeletionAuditedObject.DeletionTime))
                    .IsRequired(false)
                    .HasColumnName(nameof(IDeletionAuditedObject.DeletionTime));

                b.Property(nameof(IDeletionAuditedObject.DeleterId))
                    .IsRequired(false)
                    .HasColumnName(nameof(IDeletionAuditedObject.DeleterId));
            }
        }

        public static void ConfigureSoftDelete<T>(this EntityTypeBuilder<T> b)
            where T : class, ISoftDelete
        {
            b.TryConfigureSoftDelete();
        }

        public static void TryConfigureSoftDelete(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<ISoftDelete>())
            {
                b.Property(nameof(ISoftDelete.IsDeleted))
                    .IsRequired()
                    .HasDefaultValue(false)
                    .HasColumnName(nameof(ISoftDelete.IsDeleted));
            }
        }

    }
}
