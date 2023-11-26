using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace FunDooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewBusiness reviewBusiness;

        public ReviewController(IReviewBusiness reviewBusiness)
        {
            this.reviewBusiness = reviewBusiness;
        }

        [HttpPost]
        [Route("AddReview")]
        public ActionResult AddReview(ReviewModel reviewModel)
        {
            ReviewEntity reviewEntity = reviewBusiness.AddReview(reviewModel);

            if(reviewEntity != null)
            {
                return Ok(new ResponseModel<ReviewEntity> { IsSuccess = true, Message = "Review Added", Data = reviewEntity });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Review Not Added", Data = "Failed to add Review" });
            }
        }

        [HttpGet]
        [Route("ShowReviews")]
        public List<ReviewEntity> GetReview()
        {
            List<ReviewEntity> Reviews = reviewBusiness.GetReviews();
            if(Reviews != null)
            {
                return Reviews;
            }
            else
            {
                return null;
            }
        }
    }
}
