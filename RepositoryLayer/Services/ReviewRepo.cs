using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Services
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly FunDooDBContext funDooDBContext;

        public ReviewRepo(FunDooDBContext funDooDBContext)
        {
            this.funDooDBContext = funDooDBContext;
        }

        public ReviewEntity AddReview(ReviewModel Review)
        {
            ReviewEntity reviewEntity = new ReviewEntity();
            reviewEntity.Comment = Review.Comment;
            reviewEntity.Rating = Review.Rating;
            funDooDBContext.Reviews.Add(reviewEntity);
            funDooDBContext.SaveChanges();
            return reviewEntity;
        }

        public List<ReviewEntity> GetReviews()
        {
            List<ReviewEntity> Reviews = new List<ReviewEntity>();
            foreach(ReviewEntity Review in  funDooDBContext.Reviews)
            {
                Reviews.Add(Review);
            }
            return Reviews;
        }
    }
}
