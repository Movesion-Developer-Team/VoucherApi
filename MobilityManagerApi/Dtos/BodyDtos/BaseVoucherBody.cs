﻿using DTOs;

namespace MobilityManagerApi.Dtos.BodyDtos
{
    public class BaseVoucherBody : BaseBody 
    {
        public VoucherDto VoucherDto { get; set; }
    }
}