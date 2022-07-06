using FluentAssertions;
using Moq;
using System;
using System.Security.Claims;
using Vesta.Auditing.Abstracts;
using Vesta.Security.Users;
using Vesta.TestBase;
using Xunit;

namespace Vesta.Auditing
{
    public class AuditPropertySetterTests
    {

        public AuditPropertySetterTests()
        {

        }

        [Trait("Category", VestaUnitTestCategories.Audit)]
        [Trait("Class", nameof(AuditPropertySetter))]
        [Trait("Method", nameof(AuditPropertySetter.SetCreationProperties))]
        [Fact]
        public void Given_User_When_CreateAnEntity_Then_CreationAuditPropertiesArePopulated()
        {
            Guid? EQUAL_USER_ID = Guid.NewGuid();
            var entityMock = new FullAuditedEntity();

            var currentUser = new Mock<ICurrentUser>();
            currentUser.SetupGet(u => u.Id).Returns(EQUAL_USER_ID);

            var _auditPropertySetterMock = new AuditPropertySetter(currentUser.Object);
            _auditPropertySetterMock.SetCreationProperties(entityMock);

            entityMock.CreationTime.Should().NotBe(default(DateTime));
            entityMock.CreatorId.Should().Be(EQUAL_USER_ID);

        }

        [Trait("Category", VestaUnitTestCategories.Audit)]
        [Trait("Class", nameof(AuditPropertySetter))]
        [Trait("Method", nameof(AuditPropertySetter.SetCreationProperties))]
        [Fact]
        public void Given_NotUser_When_CreateAnEntity_Then_CreationAuditPropertiesArePopulated()
        {
            var entityMock = new FullAuditedEntity();

            var _auditPropertySetterMock = new AuditPropertySetter(null);
            _auditPropertySetterMock.SetCreationProperties(entityMock);

            entityMock.CreationTime.Should().NotBe(default(DateTime));
            entityMock.CreatorId.Should().Be(null);
        }

        [Trait("Category", VestaUnitTestCategories.Audit)]
        [Trait("Class", nameof(AuditPropertySetter))]
        [Trait("Method", nameof(AuditPropertySetter.SetModificationProperties))]
        [Fact]
        public void Given_User_When_ModifyAnEntity_Then_ModificationAuditPropertiesArePopulated()
        {
            Guid? EQUAL_USER_ID = Guid.NewGuid();
            var entityMock = new FullAuditedEntity();

            var currentUser = new Mock<ICurrentUser>();
            currentUser.SetupGet(u => u.Id).Returns(EQUAL_USER_ID);

            var _auditPropertySetterMock = new AuditPropertySetter(currentUser.Object);
            _auditPropertySetterMock.SetModificationProperties(entityMock);

            entityMock.LastModificationTime.Should().NotBe(default(DateTime));
            entityMock.LastModifierId.Should().Be(EQUAL_USER_ID);

        }

        [Trait("Category", VestaUnitTestCategories.Audit)]
        [Trait("Class", nameof(AuditPropertySetter))]
        [Trait("Method", nameof(AuditPropertySetter.SetModificationProperties))]
        [Fact]
        public void Given_NotUser_When_ModifyAnEntity_Then_CreationAuditPropertiesArePopulated()
        {
            var entityMock = new FullAuditedEntity();

            var _auditPropertySetterMock = new AuditPropertySetter(null);
            _auditPropertySetterMock.SetModificationProperties(entityMock);

            entityMock.LastModificationTime.Should().NotBe(default(DateTime));
            entityMock.LastModifierId.Should().Be(null);
        }

        [Trait("Category", VestaUnitTestCategories.Audit)]
        [Trait("Class", nameof(AuditPropertySetter))]
        [Trait("Method", nameof(AuditPropertySetter.SetDeletionProperties))]
        [Fact]
        public void Given_User_When_DeleteAnEntity_Then_DeletionAuditPropertiesArePopulated()
        {
            Guid? EQUAL_USER_ID = Guid.NewGuid();
            var entityMock = new FullAuditedEntity()
            {
                IsDeleted = true
            };

            var currentUser = new Mock<ICurrentUser>();
            currentUser.SetupGet(u => u.Id).Returns(EQUAL_USER_ID);

            var _auditPropertySetterMock = new AuditPropertySetter(currentUser.Object);
            _auditPropertySetterMock.SetDeletionProperties(entityMock);

            entityMock.DeletionTime.Should().NotBe(default(DateTime));
            entityMock.DeleterId.Should().Be(EQUAL_USER_ID);

        }

        [Trait("Category", VestaUnitTestCategories.Audit)]
        [Trait("Class", nameof(AuditPropertySetter))]
        [Trait("Method", nameof(AuditPropertySetter.SetDeletionProperties))]
        [Fact]
        public void Given_NotUser_When_DeleteAnEntity_Then_CreationAuditPropertiesArePopulated()
        {
            var entityMock = new FullAuditedEntity()
            {
                IsDeleted = true
            };

            var _auditPropertySetterMock = new AuditPropertySetter(null);
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