// GetPropertiesByLandlordQueryableTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using System.Collections.Generic;
using System.Linq;

namespace RealEstateManagement.UnitTests.OwnerTest.OwnerPropertyTest
{
    [TestClass]
    public class GetPropertiesByLandlordQueryableTests : OwnerPropertyTestBase_InMemory
    {
        [TestMethod]
        public void Projects_DTO_And_Calls_Repo()
        {
            int landlordId = 7;

            var data = new List<Property>
            {
                new Property
                {
                    Id = 1, Title="A", Description="D", Price=1000, Area=50, Bedrooms=2,
                    IsPromoted=true, IsVerified=false, Location="HCMC",
                    PropertyType = new PropertyType { Name = "Apartment" },
                    Address = new Address
                    {
                        Province = new Province{ Name="HCM"},
                        Ward = new Ward{ Name="W1"},
                        Street = new Street{ Name="Nguyen Trai"},
                        DetailedAddress = "S1"
                    },
                    Images = new List<PropertyImage>
                    {
                        new PropertyImage{ Id=11, Url="u1", IsPrimary=true },
                        new PropertyImage{ Id=12, Url="u2", IsPrimary=false },
                    },
                    Posts = new List<PropertyPost>
                    {
                        new PropertyPost{ Id=101, Status=PropertyPost.PropertyPostStatus.Draft }
                    }
                }
            }.AsQueryable();

            // Seed 1 interest để Count = 1
            Db.InterestedProperties.Add(new InterestedProperty
            {
                PropertyId = 1,
                Status = InterestedStatus.WaitingForLandlordReply
            });
            Db.SaveChanges();

            Repo.Setup(r => r.GetByLandlordQueryable(landlordId)).Returns(data);

            var list = Svc.GetPropertiesByLandlordQueryable(landlordId).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(1, list[0].Id);
            Assert.AreEqual("HCM", list[0].Province);
            Assert.AreEqual("W1", list[0].Ward);
            Assert.AreEqual("Nguyen Trai", list[0].Street);
            Assert.AreEqual(1, list[0].Posts.Count);
            // Assert.AreEqual(1, list[0].InterestNo);

            Repo.Verify(r => r.GetByLandlordQueryable(landlordId), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Sets_RenterContract_Flags_And_RenterContractId()
        {
            int landlordId = 8;

            var postWithContract = new PropertyPost
            {
                Id = 201,
                Status = PropertyPost.PropertyPostStatus.Approved,
                RentalContract = new RentalContract { Id = 999, PropertyPostId = 201 }
            };

            var data = new List<Property>
            {
                new Property
                {
                    Id = 2, Title="HasContract", Description="X", Price=123, Area=33, Bedrooms=1,
                    IsPromoted=false, IsVerified=true, Location="HN",
                    PropertyType = new PropertyType { Name = "Condo" },
                    Address = new Address
                    {
                        Province = new Province{ Name="HN"},
                        Ward = new Ward{ Name="W2"},
                        Street = new Street{ Name="Kim Ma"},
                        DetailedAddress = "S2"
                    },
                    Images = new List<PropertyImage>(),
                    Posts = new List<PropertyPost> { postWithContract } // phần tử đầu tiên có contract
                }
            }.AsQueryable();

            Repo.Setup(r => r.GetByLandlordQueryable(landlordId)).Returns(data);

            var result = Svc.GetPropertiesByLandlordQueryable(landlordId).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result[0].IsExistRenterContract);
            Assert.AreEqual(999, result[0].RenterContractId);
            Assert.AreEqual(1, result[0].Posts.Count);
        }

        [TestMethod]
        public void Maps_Images_Address_And_Basic_Fields_Correctly()
        {
            int landlordId = 9;

            var data = new List<Property>
            {
                new Property
                {
                    Id = 3, Title="ImgMap", Description="Desc", Price=777, Area=70, Bedrooms=3,
                    IsPromoted=false, IsVerified=false, Location="Da Nang",
                    PropertyType = new PropertyType { Name = "Villa" },
                    Address = new Address
                    {
                        Province = new Province{ Name="DN"},
                        Ward = new Ward{ Name="W3"},
                        Street = new Street{ Name="Vo Nguyen Giap"},
                        DetailedAddress = "S3"
                    },
                    Images = new List<PropertyImage>
                    {
                        new PropertyImage{ Id=31, Url="cover.jpg", IsPrimary=true },
                        new PropertyImage{ Id=32, Url="side.jpg",  IsPrimary=false },
                        new PropertyImage{ Id=33, Url="back.jpg",  IsPrimary=false },
                    },
                    Posts = new List<PropertyPost>()
                }
            }.AsQueryable();

            Repo.Setup(r => r.GetByLandlordQueryable(landlordId)).Returns(data);

            var dto = Svc.GetPropertiesByLandlordQueryable(landlordId).Single();

            Assert.AreEqual("DN", dto.Province);
            Assert.AreEqual("W3", dto.Ward);
            Assert.AreEqual("Vo Nguyen Giap", dto.Street);
            Assert.AreEqual("S3", dto.DetailedAddress);
            Assert.AreEqual("ImgMap", dto.Title);
            Assert.AreEqual(777, dto.Price);
            Assert.AreEqual(70, dto.Area);
            Assert.AreEqual(3, dto.Bedrooms);
            Assert.AreEqual("Villa", dto.Type);
            Assert.AreEqual("Da Nang", dto.Location);
            Assert.AreEqual(3, dto.Images.Count);
            Assert.IsTrue(dto.Images.Single(i => i.Id == 31).IsPrimary);
            Assert.AreEqual("cover.jpg", dto.Images.Single(i => i.Id == 31).Url);
        }
    }
}
