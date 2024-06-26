﻿using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IContentRepository
    {
        Task<ContentBase?> GetContentByFilterAsync(Expression<Func<ContentBase, bool>> filter);
        Task<List<ContentBase>> GetContentsByFilterAsync(Expression<Func<ContentBase, bool>> filter);
        Task<List<ContentBase>> GetContentsByFilterWithAmountAsync(Expression<Func<ContentBase, bool>> filter, int amount);
        Task<MovieContent?> GetMovieContentByFilterAsync(Expression<Func<MovieContent, bool>> filter);
        Task<SerialContent?> GetSerialContentByFilterAsync(Expression<Func<SerialContent, bool>> filter);
        Task UpdateMovieContent(MovieContent newMovieContent);
        Task UpdateSerialContent(SerialContent serialContent);
        ContentBase DeleteContent(long id);
        void AddMovieContent(MovieContent movieContent);
        void AddSerialContent(SerialContent serialContent);
        Task<ContentBase?> GetContentByIdAsync(long id);
        Task<ContentBase?> GetContentWithAllowedSubscriptionsByIdAsync(long id);
        Task<List<ContentBase>> GetRandomContentsAsync(int amount);
        
        Task SaveChangesAsync();
    }
}
