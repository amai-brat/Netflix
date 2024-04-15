using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
	public interface ICommentRepository
	{
		public Task<Comment?> GetCommentByIdAsync(long id);
		public Task SaveChangesAsync();
		public Comment Remove(Comment comment);
	}
}
