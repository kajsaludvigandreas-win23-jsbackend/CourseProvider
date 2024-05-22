using Infrastructure.Data.Entities;
using Infrastructure.Models;

namespace Infrastructure.Factories
{
    public static class CourseFactory
    {
        public static CourseEntity Create(CourseCreateRequest request)
        {
            return new CourseEntity
            {
                ImageUri = request.ImageUri,
                ImageHeaderUri = request.ImageHeaderUri,
                IsBestSeller = request.IsBestSeller,
                IsDigital = request.IsDigital,
                Categories = request.Categories,
                Title = request.Title,
                Ingress = request.Ingress,
                StarRating = request.StarRating,
                Reviews = request.Reviews,
                LikesInPercent = request.LikesInPercent,
                Likes = request.Likes,
                Hours = request.Hours,
                Authors = request.Authors?.Select(a => new AuthorEntity
                {
                    Name = a.Name
                }).ToList(),
                Prices = request.Prices != null ? new PricesEntity
                {
                    Currency = request.Prices.Currency,
                    Price = request.Prices.Price,
                    Discount = request.Prices.Discount
                } : null,
                Content = request.Content != null ? new ContentEntity
                {
                    Description = request.Content.Description,
                    Includes = request.Content.Includes,
                    ProgramDetails = request.Content.ProgramDetails?.Select(pd => new ProgramDetailItemEntity
                    {
                        Id = pd.Id,
                        Title = pd.Title,
                        Description = pd.Description
                    }).ToList()
                } : null
            };
        }
    }
}
