using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface IReviewBusiness
    {
        ReviewEntity AddReview(ReviewModel reviewModel);
        List<ReviewEntity> GetReviews();
    }
}