﻿using API.Common;
using Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpagWallet.Application.DTOs.CardDtoBranch;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : BaseController
    {
        private readonly ICardService _cardVault;

        public CardController(ICardService cardVault)
        {
            _cardVault = cardVault;
        }

        [HttpGet("get-card-by-id/{cardId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<CardDto>>> GetCardById(Guid cardId)
        {
            var cardRecord = await _cardVault.GetCardByIdAsync(cardId);
            if (cardRecord == null)
            {
                return NotFoundResponse<CardDto>(
                  new List<string> { "Error getting card" },
                  "Card not fetched successfully");
            }
            return Success(cardRecord, "Card record fetched successfully");
        }

        [HttpGet("get-all-cards")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CardDto?>>>> GetAllCards()
        {
            var cardRecords = await _cardVault.GetAllCardsAsync();
            if (cardRecords == null || !cardRecords.Any())
                return NotFoundResponse<IEnumerable<CardDto?>>(
                    new List<string> { "Error getting cards" },
                    "Cards not fetched successfully"
                 );
            return Success(cardRecords, "Cards fetched successfully.");
        }

        [HttpPatch("deactivate-card-by-id/{cardId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeactivateCard(Guid cardId)
        {
            var success = await _cardVault.DeactivateCard(cardId);
            if (!success)
            {
                return Failure<bool>(
                    new List<string> { "Error deactivating card" },
                    "Card not deactivated successfully"
                );
            }
            return Success(success, "Card deactivated successfully");
        }

        [HttpPatch("activate-card-by-id/{cardId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivateCard(Guid cardId)
        {
            var success = await _cardVault.ActivateCard(cardId);
            if (!success)
            {
                return Failure<bool>(
                    new List<string> { "Error activating card" },
                    "Card not activated successfully"
                );
            }
            return Success(success, "Card activated successfully");
        }

        [HttpPatch("block-card-by-id/{cardId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> BlockCard(Guid cardId)
        {
            var success = await _cardVault.BlockCard(cardId);
            if (!success)
            {
                return Failure<bool>(
                    new List<string> { "Error blocking card" },
                    "Card not blocked successfully"
                );
            }
            return Success(success, "Card blocked successfully");
        }
    }
}
