using CompanyName.Auditing.Abstractions;
using CompanyName.Security.Users;
using CompanyName.TestBase;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using Xunit;

namespace CompanyName.Auditing
{
    public class AuditPropertySetterTests
    {

        public AuditPropertySetterTests()
        {

        }

        [Trait("Category", CompanyNameUnitTestCategories.Audit)]
        [Trait("Class", nameof(AuditPropertySetter))]
        [Trait("Method", nameof(AuditPropertySetter.SetCreationProperties))]
        [Fact]
        public void Given_User_When_CreateAnEntity_Then_CreationAuditPropertiesArePopulated()
        {
            Guid EQUAL_USER_ID = Guid.NewGuid();
            var entityMock = new FullAuditedEntity();

            var currentUser = new Mock<ICurrentUser>();
            currentUser.Setup(u => u.GetId<Guid>()).Returns(EQUAL_USER_ID);

            var _auditPropertySetterMock = new AuditPropertySetter(NullLogger<AuditPropertySetter>.Instance, currentUser.Object);
            _auditPropertySetterMock.SetCreationProperties(entityMock);

            entityMock.CreationTime.Should().NotBe(default(DateTime));
            entityMock.CreatorId.Should().Be(EQUAL_USER_ID);

        }

        [Trait("Category", CompanyNameUnitTestCategories.Audit)]
        [Trait("Class", nameof(AuditPropertySetter))]
        [Trait("Method", nameof(AuditPropertySetter.SetCreationProperties))]
        [Fact]
        public void Given_NotUser_When_CreateAnEntity_Then_CreationAuditPropertiesArePopulated()
        {
            var entityMock = new FullAuditedEntity();

            var _auditPropertySetterMock = new AuditPropertySetter(NullLogger<AuditPropertySetter>.Instance, null);
            _auditPropertySetterMock.SetCreationProperties(entityMock);

            entityMock.CreationTime.Should().NotBe(default(DateTime));
            entityMock.CreatorId.Should().Be(null);
        }

        [Trait("Category", CompanyNameUnitTestCategories.Audit)]
        [Trait("Class", nameof(AuditPropertySetter))]
        [Trait("Method", nameof(AuditPropertySetter.SetModificationProperties))]
        [Fact]
        public void Given_User_When_ModifyAnEntity_Then_ModificationAuditPropertiesArePopulated()
        {
            Guid EQUAL_USER_ID = Guid.NewGuid();
            var entityMock = new FullAuditedEntity();

            var currentUser = new Mock<ICurrentUser>();
            currentUser.Setup(u => u.GetId<Guid>()).Returns(EQUAL_USER_ID);

            var _auditPropertySetterMock = new AuditPropertySetter(NullLogger<AuditPropertySetter>.Instance, currentUser.Object);
            _auditPropertySetterMock.SetModificationProperties(entityMock);

            entityMock.LastModificationTime.Should().NotBe(default(DateTime));
            entityMock.LastModifierId.Should().Be(EQUAL_USER_ID);

        }

        [Trait("Category", CompanyNameUnitTestCategories.Audit)]
        [Trait("Class", nameof(AuditPropertySetter))]
        [Trait("Method", nameof(AuditPropertySetter.SetModificationProperties))]
        [Fact]
        public void Given_NotUser_When_ModifyAnEntity_Then_CreationAuditPropertiesArePopulated()
        {
            var entityMock = new FullAuditedEntity();

            var _auditPropertySetterMock = new AuditPropertySetter(NullLogger<AuditPropertySetter>.Instance, null);
            _auditPropertySetterMock.SetModificationProperties(entityMock);

            entityMock.LastModificationTime.Should().NotBe(default(DateTime));
            entityMock.LastModifierId.Should().Be(null);
        }

        [Trait("Category", CompanyNameUnitTestCategories.Audit)]
        [Trait("Class", nameof(AuditPropertySetter))]
        [Trait("Method", nameof(AuditPropertySetter.SetDeletionProperties))]
        [Fact]
        public void Given_User_When_DeleteAnEntity_Then_DeletionAuditPropertiesArePopulated()
        {
            Guid EQUAL_USER_ID = Guid.NewGuid();
            var entityMock = new FullAuditedEntity()
            {
                IsDeleted = true
            };

            var currentUser = new Mock<ICurrentUser>();
            currentUser.Setup(u => u.GetId<Guid>()).Returns(EQUAL_USER_ID);

            var _auditPropertySetterMock = new AuditPropertySetter(NullLogger<AuditPropertySetter>.Instance, currentUser.Object);
            _auditPropertySetterMock.SetDeletionProperties(entityMock);

            entityMock.DeletionTime.Should().NotBe(default(DateTime));
            entityMock.DeleterId.Should().Be(EQUAL_USER_ID);

        }

        [Trait("Category", CompanyNameUnitTestCategories.Audit)]
        [Trait("Class", nameof(AuditPropertySetter))]
        [Trait("Method", nameof(AuditPropertySetter.SetDeletionProperties))]
        [Fact]
        public void Given_NotUser_When_DeleteAnEntity_Then_CreationAuditPropertiesArePopulated()
        {
            var entityMock = new FullAuditedEntity()
            {
                IsDeleted = true
            };

            var _auditPropertySetterMock = new AuditPropertySetter(NullLogger<AuditPropertySetter>.Instance, null);
            _auditPropertySetterMock.SetDeletionProperties(entityMock);

            entityMock.DeletionTime.Should().NotBe(default(DateTime));
            entityMock.DeleterId.Should().Be(null);
        }

        private class FullAuditedEntity :
            ICreationAuditedObject,
            IModificationAuditedObject,
            IDeletionAuditedObject
        {
            public DateTime CreationTime { get; set; }
            public Guid? CreatorId { get; set; }
            public DateTime? LastModificationTime { get; set; }
            public Guid? LastModifierId { get; set; }
            public DateTime? DeletionTime { get; set; }
            public Guid? DeleterId { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
}