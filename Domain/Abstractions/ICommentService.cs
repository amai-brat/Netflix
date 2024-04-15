using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
	public interface ICommentService
	{
		public Task<Comment> DeleteCommentByIdAsync(long id);
	}
}
