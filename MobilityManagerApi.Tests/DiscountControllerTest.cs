using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using CsvHelper;
using CsvHelper.Configuration;
using DTOs.BodyDtos;
using DTOs.ResponseDtos;
using Enum;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.Common;
using MobilityManagerApi.Controllers;
using NUnit.Framework;

namespace MobilityManagerApi.Tests
{
    [TestFixture(TestOf = typeof(DiscountController))]
    public class DiscountControllerTest : BaseTest
    {
        private List<DiscountCode> _records;
        private string _filePath;
        private int? _playerId;

        [OneTimeSetUp]
        public async Task TestSetup()
        {
            var categoryIdObjectResult = await _categoryController
                .CreateNewCategory(new CreateNewCategoryBodyDto
            {
                Name = "Public Transport",
                Description = null
            }) as ObjectResult;
            var categoryId = (categoryIdObjectResult.Value as CreateNewEntityResponseDto).Id;
            var playerIdObjectResult = await _playerController.CreateNewPlayer(new CreateNewPlayerBodyDto
            {
                ShortName = "Share",
                FullName = "CarShare",
                PlayStoreLink = null,
                AppStoreLink = null,
                LinkDescription = null,
                Color = "Yellow"
            }) as ObjectResult;
            var _playerId = (playerIdObjectResult.Value as CreateNewEntityResponseDto).Id;

            _records = new List<DiscountCode>()
            {
                new DiscountCode
                {
                    Code = "sjdh63"
                },
                new DiscountCode
                {
                    Code = "sjdf53"
                },
                new DiscountCode
                {
                    Code = "sadh60"
                }
            };

            _filePath = @"\tempDiscounts.csv";
            await using var writer = new StreamWriter(_filePath);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            await using var csv = new CsvWriter(writer, config);
            await csv.WriteRecordsAsync(_records);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            _records.Clear();
            await _context.DisposeAsync();
            File.Delete(_filePath);
        }

        //[Test]
        //[Author(nameof(Authors.Arif))]
        //public async Task UploadCsvTest()
        //{
        //    var requestBody = new UploadCsvBodyDto
        //    {
        //        PlayerId = _playerId,
        //        DiscountType = DiscountType.SingleUse
        //    };
        //    var app = new WebApplicationFactory 
        //    var result = await _discountController.UploadCsv(requestBody) as ObjectResult;
        //    var responseMessage = (result!
        //        .Value as BaseResponse)!.Message;

        //    //Assert.AreEqual(responseMessage, "Discounts are saved in Db");
        //    Assert.AreEqual(result.StatusCode, StatusCodes.Status400BadRequest);

        //}
    }
}
