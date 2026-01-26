using Ogma.Application.SharedKernel.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogma.Application.Orders.Dtos;

public record CreateOrderLineDto(
    long ItemId,
    decimal OrderedQuantity,
    MoneyDto Price,
    ExchangeRateDto? ExchangeRate,
    string? AdditionalInformation);
