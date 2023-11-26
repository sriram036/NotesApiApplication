using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class ReviewBusiness : IReviewBusiness
    {
        private readonly IReviewRepo ReviewRepo;

        public ReviewBusiness(IReviewRepo reviewRepo)
        {
            this.ReviewRepo = reviewRepo;
        }

        public ReviewEntity AddReview(ReviewModel reviewModel)
        {
            return ReviewRepo.AddReview(reviewModel);
        }

        public List<ReviewEntity> GetReviews() 
        {
            return ReviewRepo.GetReviews();
        }
    }
}
