using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace RepositoryLayer.Interfaces
{
    public interface IReviewRepo
    {
        ReviewEntity AddReview(ReviewModel Review);
        List<ReviewEntity> GetReviews();
    }
}